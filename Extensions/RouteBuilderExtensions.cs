using Microsoft.AspNetCore.Authentication.BearerToken;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Moto.Controllers.ControllersInputModels;
using Moto.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Moto.DB.Models;
using Moto.Controllers.DTO;

namespace Moto.Extensions
{
    public static class RouteBuilderExtensions
    {
        private static readonly EmailAddressAttribute _emailAddressAttribute = new();
        public static IEndpointConventionBuilder MapAuthEndpoints<TUser>(this IEndpointRouteBuilder endpoints)
        where TUser : class, new()
        {
            ArgumentNullException.ThrowIfNull(endpoints);

            var timeProvider = endpoints.ServiceProvider.GetRequiredService<TimeProvider>();
            var bearerTokenOptions = endpoints.ServiceProvider.GetRequiredService<IOptionsMonitor<BearerTokenOptions>>();
            var emailSender = endpoints.ServiceProvider.GetRequiredService<IEmailSender<TUser>>();
            var linkGenerator = endpoints.ServiceProvider.GetRequiredService<LinkGenerator>();

            // We'll figure out a unique endpoint name based on the final route pattern during endpoint generation.
            string? confirmEmailEndpointName = null;

            var routeGroup = endpoints.MapGroup("");

            // NOTE: We cannot inject UserManager<TUser> directly because the TUser generic parameter is currently unsupported by RDG.
            // https://github.com/dotnet/aspnetcore/issues/47338
            routeGroup.MapPost("/create", async Task<Results<Ok, Ok<SignUpResponse>>>
                ([FromBody] UserCreateInputModel registration, HttpContext context, [FromServices] IServiceProvider sp) =>
            {
                var userManager = sp.GetRequiredService<UserManager<TUser>>();

                if (!userManager.SupportsUserEmail)
                {
                    throw new NotSupportedException($"{nameof(MapAuthEndpoints)} requires a user store with email support.");
                }

                var userStore = sp.GetRequiredService<IUserStore<TUser>>();
                var emailStore = (IUserEmailStore<TUser>)userStore;
                var email = registration.Email;

                var userService = sp.GetRequiredService<IUserService>();
                var sameEmailUser = await userService.GetUserByEmail(email);
                if (sameEmailUser != null)
                {
                    return TypedResults.Ok(new SignUpResponse() { Code = 409, Message = "Email exists" });
                }

                if (string.IsNullOrEmpty(email) || !_emailAddressAttribute.IsValid(email))
                {
                    return TypedResults.Ok(new SignUpResponse() { Code = 400 , Message = "Email validation error"});
                }

                var user = new TUser();
                await userStore.SetUserNameAsync(user, registration.Login, CancellationToken.None);
                await emailStore.SetEmailAsync(user, email, CancellationToken.None);
                var result = await userManager.CreateAsync(user, registration.Password);

                if (!result.Succeeded)
                {
                    return TypedResults.Ok(new SignUpResponse() { Code = 409, Message = "Login exists" });
                }

                return TypedResults.Ok(new SignUpResponse() { Code = 200, Message = "" });
            });

            routeGroup.MapPost("/signin", async Task<Results<Ok<AccessTokenResponse>, EmptyHttpResult, ProblemHttpResult>>
                ([FromBody] UserLoginModel login, [FromServices] IServiceProvider sp) =>
            {
                var signInManager = sp.GetRequiredService<SignInManager<TUser>>();

                signInManager.AuthenticationScheme = IdentityConstants.ApplicationScheme;

                var result = await signInManager.PasswordSignInAsync(login.Login, login.Password, true, lockoutOnFailure: false);

                if (!result.Succeeded)
                {
                    return TypedResults.Problem(result.ToString(), statusCode: StatusCodes.Status401Unauthorized);
                }

                await signInManager.Context.Response.WriteAsync(login.Login);
                // The signInManager already produced the needed response in the form of a cookie or bearer token.
                return TypedResults.Empty;
            });

            routeGroup.MapGet("/checkadmin", async Task<Results<Ok, ValidationProblem>>
                (HttpContext context, [FromServices] IServiceProvider sp) =>
            {
                return TypedResults.Ok();
            }).RequireAuthorization("AdminRole");

            routeGroup.MapGet("/signout", async Task<Results<SignOutHttpResult, ValidationProblem>>
                (HttpContext context, [FromServices] IServiceProvider sp) =>
            {
                //var signInManager = sp.GetRequiredService<SignInManager<TUser>>();
                //await signInManager.SignOutAsync();
                await context.SignOutAsync();
                return TypedResults.SignOut();
            }).RequireAuthorization();

            routeGroup.MapGet("/getinfo", async Task<Results<Ok<User>, NotFound>>
                (HttpContext context, [FromServices] IServiceProvider sp) =>
            {
                var userService = sp.GetRequiredService<IUserService>();
                var user = await userService.GetUserByName(context.User?.Identity?.Name);
                if (user is null)
                    return TypedResults.NotFound();
                return TypedResults.Ok(user);
            }).RequireAuthorization();

            routeGroup.MapPost("/refresh", async Task<Results<Ok<AccessTokenResponse>, UnauthorizedHttpResult, SignInHttpResult, ChallengeHttpResult>>
                ([FromBody] RefreshRequest refreshRequest, [FromServices] IServiceProvider sp) =>
            {
                var signInManager = sp.GetRequiredService<SignInManager<TUser>>();
                var refreshTokenProtector = bearerTokenOptions.Get(IdentityConstants.BearerScheme).RefreshTokenProtector;
                var refreshTicket = refreshTokenProtector.Unprotect(refreshRequest.RefreshToken);

                // Reject the /refresh attempt with a 401 if the token expired or the security stamp validation fails
                if (refreshTicket?.Properties?.ExpiresUtc is not { } expiresUtc ||
                    timeProvider.GetUtcNow() >= expiresUtc ||
                    await signInManager.ValidateSecurityStampAsync(refreshTicket.Principal) is not TUser user)

                {
                    return TypedResults.Challenge();
                }

                var newPrincipal = await signInManager.CreateUserPrincipalAsync(user);
                return TypedResults.SignIn(newPrincipal, authenticationScheme: IdentityConstants.BearerScheme);
            });

            

         
            var accountGroup = routeGroup.MapGroup("/manage").RequireAuthorization();

            accountGroup.MapPost("/2fa", async Task<Results<Ok<TwoFactorResponse>, ValidationProblem, NotFound>>
                (ClaimsPrincipal claimsPrincipal, [FromBody] TwoFactorRequest tfaRequest, [FromServices] IServiceProvider sp) =>
            {
                var signInManager = sp.GetRequiredService<SignInManager<TUser>>();
                var userManager = signInManager.UserManager;
                if (await userManager.GetUserAsync(claimsPrincipal) is not { } user)
                {
                    return TypedResults.NotFound();
                }

                if (tfaRequest.Enable == true)
                {
                    if (tfaRequest.ResetSharedKey)
                    {
                        return CreateValidationProblem("CannotResetSharedKeyAndEnable",
                            "Resetting the 2fa shared key must disable 2fa until a 2fa token based on the new shared key is validated.");
                    }
                    else if (string.IsNullOrEmpty(tfaRequest.TwoFactorCode))
                    {
                        return CreateValidationProblem("RequiresTwoFactor",
                            "No 2fa token was provided by the request. A valid 2fa token is required to enable 2fa.");
                    }
                    else if (!await userManager.VerifyTwoFactorTokenAsync(user, userManager.Options.Tokens.AuthenticatorTokenProvider, tfaRequest.TwoFactorCode))
                    {
                        return CreateValidationProblem("InvalidTwoFactorCode",
                            "The 2fa token provided by the request was invalid. A valid 2fa token is required to enable 2fa.");
                    }

                    await userManager.SetTwoFactorEnabledAsync(user, true);
                }
                else if (tfaRequest.Enable == false || tfaRequest.ResetSharedKey)
                {
                    await userManager.SetTwoFactorEnabledAsync(user, false);
                }

                if (tfaRequest.ResetSharedKey)
                {
                    await userManager.ResetAuthenticatorKeyAsync(user);
                }

                string[]? recoveryCodes = null;
                if (tfaRequest.ResetRecoveryCodes || (tfaRequest.Enable == true && await userManager.CountRecoveryCodesAsync(user) == 0))
                {
                    var recoveryCodesEnumerable = await userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
                    recoveryCodes = recoveryCodesEnumerable?.ToArray();
                }

                if (tfaRequest.ForgetMachine)
                {
                    await signInManager.ForgetTwoFactorClientAsync();
                }

                var key = await userManager.GetAuthenticatorKeyAsync(user);
                if (string.IsNullOrEmpty(key))
                {
                    await userManager.ResetAuthenticatorKeyAsync(user);
                    key = await userManager.GetAuthenticatorKeyAsync(user);

                    if (string.IsNullOrEmpty(key))
                    {
                        throw new NotSupportedException("The user manager must produce an authenticator key after reset.");
                    }
                }

                return TypedResults.Ok(new TwoFactorResponse
                {
                    SharedKey = key,
                    RecoveryCodes = recoveryCodes,
                    RecoveryCodesLeft = recoveryCodes?.Length ?? await userManager.CountRecoveryCodesAsync(user),
                    IsTwoFactorEnabled = await userManager.GetTwoFactorEnabledAsync(user),
                    IsMachineRemembered = await signInManager.IsTwoFactorClientRememberedAsync(user),
                });
            });

            accountGroup.MapGet("/info", async Task<Results<Ok<InfoResponse>, ValidationProblem, NotFound>>
                (ClaimsPrincipal claimsPrincipal, [FromServices] IServiceProvider sp) =>
            {
                var userManager = sp.GetRequiredService<UserManager<TUser>>();
                if (await userManager.GetUserAsync(claimsPrincipal) is not { } user)
                {
                    return TypedResults.NotFound();
                }

                return TypedResults.Ok(await CreateInfoResponseAsync(user, userManager));
            });



            async Task SendConfirmationEmailAsync(TUser user, UserManager<TUser> userManager, HttpContext context, string email, bool isChange = false)
            {
                if (confirmEmailEndpointName is null)
                {
                    throw new NotSupportedException("No email confirmation endpoint was registered!");
                }

                var code = isChange
                    ? await userManager.GenerateChangeEmailTokenAsync(user, email)
                    : await userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var userId = await userManager.GetUserIdAsync(user);
                var routeValues = new RouteValueDictionary()
                {
                    ["userId"] = userId,
                    ["code"] = code,
                };

                if (isChange)
                {
                    // This is validated by the /confirmEmail endpoint on change.
                    routeValues.Add("changedEmail", email);
                }

                var confirmEmailUrl = linkGenerator.GetUriByName(context, confirmEmailEndpointName, routeValues)
                    ?? throw new NotSupportedException($"Could not find endpoint named '{confirmEmailEndpointName}'.");

                await emailSender.SendConfirmationLinkAsync(user, email, HtmlEncoder.Default.Encode(confirmEmailUrl));
            }

            return new IdentityEndpointsConventionBuilder(routeGroup);
        }

        private static ValidationProblem CreateValidationProblem(string errorCode, string errorDescription) =>
            TypedResults.ValidationProblem(new Dictionary<string, string[]> {
            { errorCode, [errorDescription] }
            });

        private static ValidationProblem CreateValidationProblem(IdentityResult result)
        {
            // We expect a single error code and description in the normal case.
            // This could be golfed with GroupBy and ToDictionary, but perf! :P
            Debug.Assert(!result.Succeeded);
            var errorDictionary = new Dictionary<string, string[]>(1);

            foreach (var error in result.Errors)
            {
                string[] newDescriptions;

                if (errorDictionary.TryGetValue(error.Code, out var descriptions))
                {
                    newDescriptions = new string[descriptions.Length + 1];
                    Array.Copy(descriptions, newDescriptions, descriptions.Length);
                    newDescriptions[descriptions.Length] = error.Description;
                }
                else
                {
                    newDescriptions = [error.Description];
                }

                errorDictionary[error.Code] = newDescriptions;
            }

            return TypedResults.ValidationProblem(errorDictionary);
        }

        private static async Task<InfoResponse> CreateInfoResponseAsync<TUser>(TUser user, UserManager<TUser> userManager)
            where TUser : class
        {
            return new()
            {
                Email = await userManager.GetEmailAsync(user) ?? throw new NotSupportedException("Users must have an email."),
                IsEmailConfirmed = await userManager.IsEmailConfirmedAsync(user),
            };
        }

        // Wrap RouteGroupBuilder with a non-public type to avoid a potential future behavioral breaking change.
        private sealed class IdentityEndpointsConventionBuilder(RouteGroupBuilder inner) : IEndpointConventionBuilder
        {
            private IEndpointConventionBuilder InnerAsConventionBuilder => inner;

            public void Add(Action<EndpointBuilder> convention) => InnerAsConventionBuilder.Add(convention);
            public void Finally(Action<EndpointBuilder> finallyConvention) => InnerAsConventionBuilder.Finally(finallyConvention);
        }

        [AttributeUsage(AttributeTargets.Parameter)]
        private sealed class FromBodyAttribute : Attribute, IFromBodyMetadata
        {
        }

        [AttributeUsage(AttributeTargets.Parameter)]
        private sealed class FromServicesAttribute : Attribute, IFromServiceMetadata
        {
        }

        [AttributeUsage(AttributeTargets.Parameter)]
        private sealed class FromQueryAttribute : Attribute, IFromQueryMetadata
        {
            public string? Name => null;
        }
    }
}
