namespace TollManagement.API.App_Start;

public static class WebApiConfig
{
    public static void Register(WebApplication app)
    {
        app.MapControllers();
    }
}
