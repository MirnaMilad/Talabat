namespace Talabat.APIS.Extensions
{
    public static class AddSwagarExtension
    {
        public static WebApplication UseSwaggarMiddleware (this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
    }
}
