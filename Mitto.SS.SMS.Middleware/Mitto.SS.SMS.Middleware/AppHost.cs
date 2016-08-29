using Funq;
using Microsoft.Practices.Unity;
using Mitto.SS.SMS.Business;
using Mitto.SS.SMS.Data;
using Mitto.SS.SMS.Middleware.Core;
using Mitto.SS.SMS.Middleware.Core.Abstractions;
using Mitto.SS.SMS.Middleware.ServiceInterface;
using ServiceStack;
using ServiceStack.Data;
using ServiceStack.OrmLite;
using System.Configuration;

namespace Mitto.SS.SMS.Middleware
{
    public class AppHost : AppHostBase
    {
        /// <summary>
        /// Database name constant
        /// </summary>
        private static readonly string SmsDatabaseName = "mittosmsEntities";

        /// <summary>
        /// Base constructor requires a Name and Assembly where web service implementation is located
        /// </summary>
        public AppHost()
            : base("Mitto.SS.SMS.Middleware", typeof(ShortMessageServices).Assembly)
        { }

        /// <summary>
        /// Application specific configuration
        /// This method should initialize any IoC resources utilized by your web service classes.
        /// </summary>
        public override void Configure(Container container)
        {
            //Create Unity IoC container
            var unityContainer = new UnityContainer();
            
            //Data provider
            unityContainer.RegisterType<ISmsEntitiesDataProvider, SmsEntitiesDataProvider>();

            //Business operations manager
            unityContainer.RegisterType<IMessageServicesManager, MessageServicesManager>();
            
            //DB Factory provider
            unityContainer.RegisterInstance<IDbConnectionFactory>(new OrmLiteConnectionFactory(ConfigurationManager.ConnectionStrings[SmsDatabaseName].ConnectionString, MySqlDialect.Provider));

            //Register SimpleInjector IoC container, so ServiceStack can use it
            container.Adapter = new UnityIocAdapter(unityContainer);
        }
    }
}