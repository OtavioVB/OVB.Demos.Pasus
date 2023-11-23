
namespace OVB.Demos.Eschody.WebApi;

public static partial class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();

        var app = builder.Build();

        app.MapControllers();
        app.Run();
    }
}
