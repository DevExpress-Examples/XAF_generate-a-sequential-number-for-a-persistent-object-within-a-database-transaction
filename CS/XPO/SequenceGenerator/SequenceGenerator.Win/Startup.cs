using System.Configuration;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ApplicationBuilder;
using DevExpress.ExpressApp.Win.ApplicationBuilder;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Win;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.XtraEditors;
using DevExpress.ExpressApp.Design;
using GenerateUserFriendlyId.Module;
namespace SequenceGenerator.Win;
using DevExpress.ExpressApp.ApplicationBuilder.Internal;
using DevExpress.ExpressApp.Xpo;
using Microsoft.Extensions.DependencyInjection;

public class ApplicationBuilder : IDesignTimeApplicationFactory {
    public static WinApplication BuildApplication(string connectionString) {
        var builder = WinApplication.CreateBuilder();
        builder.UseApplication<SequenceGeneratorWindowsFormsApplication>();
        builder.Modules
            .Add<SequenceGenerator.Module.SequenceGeneratorModule>()
            .Add<SequenceGeneratorWinModule>();

        builder.Services.AddScoped<SequenceGeneratorProvider>();
        builder.Services.Configure<SequenceGeneratorOptions>(opt => {
            opt.GetConnectionString = (serviceProvider) => {
                //return serviceProvider.GetRequiredService<IConnectionStringProvider>().GetConnectionString();
                return connectionString;
            };
        });

        builder.ObjectSpaceProviders
             .AddXpo((application, options) => {
                 options.ConnectionString = connectionString;
             })
            .AddNonPersistent();
        builder.AddBuildStep(application => {
            application.ConnectionString = connectionString;
#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached && application.CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema) {
                application.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
            }
#endif
        });
        var winApplication = builder.Build();
        return winApplication;
    }

    XafApplication IDesignTimeApplicationFactory.Create()
        => BuildApplication(XafApplication.DesignTimeConnectionString);
}
