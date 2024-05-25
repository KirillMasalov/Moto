using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Moto.DB;
using Moto.Services;
using Moto.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Moto.Extensions;
using Microsoft.AspNetCore.Builder;
using Moto.DB.Models;

var builder = WebApplication.CreateBuilder(args);
var policyName = "Policy";


builder.Services.AddCustomizedCors(policyName);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMvc();
builder.Services.AddServicesDependencies();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

var str = builder.Configuration.GetConnectionString("DefaultConnection");
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

builder.Services.AddIdentityCore<User>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddUserManager<ApplicationUserManager>()
    .AddApiEndpoints();

builder.Services.AddAuthentication().AddCookie("Identity.Application");
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminRole", policy => policy.RequireUserName("admin"));
});

builder.Services.ConfigureIdentity();
builder.Services.ConfigureCookie();




var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseHttpsRedirection();

app.UseCookieWithPolicy();

app.UseAuthentication();
app.UseAuthorization();

app.UseCors(policyName);

app.MapGroup("/user").MapAuthEndpoints<User>();

app.MapControllers();


app.Run();
