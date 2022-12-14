using mauiBlazor.Data;
using Radzen;

namespace mauiBlazor;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");

            });

		builder.Services.AddMauiBlazorWebView();
		#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		#endif
		builder.Services.AddSingleton<WeatherForecastService>();
        builder.Services.AddSingleton<CustomerService>();
        builder.Services.AddSingleton<ReportService>();
        builder.Services.AddSingleton<DataService>();
        builder.Services.AddDevExpressBlazor(configure => configure.BootstrapVersion = DevExpress.Blazor.BootstrapVersion.v5);
		builder.Services.AddScoped<DialogService>();
        builder.Services.AddScoped<NotificationService>();
        builder.Services.AddScoped<TooltipService>();
        builder.Services.AddScoped<ContextMenuService>();
        return builder.Build();
		
        //app.UseDevExpressServerSideBlazorReportViewer();
		
    }
}
