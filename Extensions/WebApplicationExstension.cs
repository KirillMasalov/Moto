namespace Moto.Extensions
{
    public static class WebApplicationExstension
    {
        public static WebApplication UseCookieWithPolicy(this WebApplication app)
        {
            app.UseCookiePolicy(new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.None,
            });

            return app;
        }

    }
}
