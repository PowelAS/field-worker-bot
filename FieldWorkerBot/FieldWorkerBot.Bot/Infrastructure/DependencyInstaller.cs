using System.Web.Http;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using FieldWorkerBot.DataAccess;
using FieldWorkerBot.DataAccess.Interfaces;
using FieldWorkerBot.Services;
using FieldWorkerBot.Services.Interfaces;
using Microsoft.Bot.Builder.Luis;

namespace FieldWorkerBot.Bot.Infrastructure
{
    public class DependencyInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var settings = new SettingsReader();

            container.Register(Classes.FromThisAssembly().BasedOn<ApiController>().LifestylePerWebRequest());

            ConfigureLuis(container, settings);
            ConfigureServices(container);
        }

        private void ConfigureLuis(IWindsorContainer container, ISettingsReader settings)
        {
            var appId = settings["LUIS:AppId"];
            var appKey = settings["LUIS:AppKey"];
            var model = new LuisModelAttribute(appId, appKey);

            container.Register(Component.For<ILuisService>().ImplementedBy<LuisService>()
                .DependsOn(Dependency.OnValue("model", model))
                .LifestyleSingleton());
        }

        private void ConfigureServices(IWindsorContainer container)
        {
            container.Register(Component.For<IAssetRepository>().ImplementedBy<AssetRepository>());
            container.Register(Component.For<IDiscrepancyTypeRepository>().ImplementedBy<DiscrepancyTypeRepository>());
            container.Register(Component.For<IInspectionRepository>().ImplementedBy<InspectionRepository>());
            container.Register(Component.For<ISnapshotRepository>().ImplementedBy<SnapshotRepository>());

            container.Register(Component.For<IAssetService>().ImplementedBy<AssetService>());
            container.Register(Component.For<IDiscrepancyTypeService>().ImplementedBy<DiscrepancyTypeService>());
            container.Register(Component.For<IInspectionService>().ImplementedBy<InspectionService>());
            container.Register(Component.For<ISnapshotService>().ImplementedBy<SnapshotService>());
        }
    }
}