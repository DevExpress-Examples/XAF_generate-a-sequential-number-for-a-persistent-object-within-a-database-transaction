using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Xpo;
using DevExpress.ExpressApp.ApplicationBuilder;
using Microsoft.Extensions.DependencyInjection;
using DevExpress.ExpressApp.ApplicationBuilder.Internal;
using DevExpress.ExpressApp;

namespace GenerateUserFriendlyId.Module {
    public static class MyExtension {
        public static IObjectSpaceProviderServiceBasedBuilder<TContext> AddMyXpo<TContext>(this IObjectSpaceProviderServiceBasedBuilder<TContext> builder, Action<IServiceProvider, XPObjectSpaceProviderOptions> configureOptions)
                    where TContext : IXafApplicationBuilder<TContext>, IAccessor<IServiceCollection> {
            var dataStoreProviderManager = new DataStoreProviderManager();
            builder.Add(serviceProvider => {
                var options = new XPObjectSpaceProviderOptions();
                configureOptions(serviceProvider, options);
                var dataStoreProvider = options.GetDataStoreProvider(dataStoreProviderManager);
                GenerateUserFriendlyId.Module.SequenceGenerator.Initialize(dataStoreProvider);
                return new XPObjectSpaceProvider(
                    dataStoreProvider,
                    serviceProvider.GetRequiredService<ITypesInfo>(),
                    null,
                    options.ThreadSafe,
                    options.UseSeparateDataLayers);
            });
            return builder;
        }

        public static IObjectSpaceProviderBuilder<TContext> AddMyXpo<TContext>(this IObjectSpaceProviderBuilder<TContext> builder, Action<XafApplication, XPObjectSpaceProviderOptions> configureOptions)
            where TContext : IXafApplicationBuilder<TContext> {
            var dataStoreProviderManager = new DataStoreProviderManager();
            builder.Add((application, _) => {
                var options = new XPObjectSpaceProviderOptions();
                configureOptions(application, options);
                var dataStoreProvider = options.GetDataStoreProvider(dataStoreProviderManager);
                GenerateUserFriendlyId.Module.SequenceGenerator.Initialize(dataStoreProvider);
                return new XPObjectSpaceProvider(
                    dataStoreProvider,
                    application.TypesInfo,
                    null,
                    options.ThreadSafe,
                    options.UseSeparateDataLayers);
            });
            return builder;
        }
    }
}