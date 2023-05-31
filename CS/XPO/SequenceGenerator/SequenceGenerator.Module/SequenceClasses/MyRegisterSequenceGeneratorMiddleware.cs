using DevExpress.ExpressApp.Xpo;

namespace SequenceGenerator.Blazor.Server;

public class MyRegisterSequenceGeneratorMiddleware {
    private readonly RequestDelegate next;
    public MyRegisterSequenceGeneratorMiddleware(RequestDelegate next) {
        this.next = next;
    }
    bool myLock = false;
    public async Task InvokeAsync(HttpContext httpContext, IConfiguration Configuration) {
        if(!myLock) {

            if (Configuration.GetConnectionString("ConnectionString") != null) {
                var connectionString = Configuration.GetConnectionString("ConnectionString");
                IXpoDataStoreProvider dataStoreProvider = XPObjectSpaceProvider.GetDataStoreProvider(connectionString, null, true);
                GenerateUserFriendlyId.Module.SequenceGenerator.Initialize(dataStoreProvider);
            }
          
            myLock = true;
        }

        await next(httpContext);
    }
}
