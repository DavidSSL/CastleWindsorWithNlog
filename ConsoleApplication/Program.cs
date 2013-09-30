using Castle.Core.Logging;
using Castle.Facilities.Logging;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;

namespace ConsoleApplication
{
    class Program
    {
        static void Main()
        {
            BootStrapContainer();
        }

        private static void BootStrapContainer()
        {
            using (var container = new WindsorContainer())
            {
                container.Install(new MyWindsorInstaller())
                    .Resolve<IObjectGraphRoot>()
                    .Log();
            }
        }
    }

    internal interface IObjectGraphRoot
    {
        void Log();
    }

    internal class MyWindsorInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.AddFacility<LoggingFacility>(f => f.LogUsing(LoggerImplementation.NLog)
                                                         .WithConfig("NLog.config"));

            container.Register(
                Component
                    .For<IObjectGraphRoot>()
                    .ImplementedBy<ObjectGraphRoot>());
        }
    }

    internal class ObjectGraphRoot : IObjectGraphRoot
    {
        private ILogger _logger = NullLogger.Instance;

        public ILogger Logger
        {
            get { return _logger; }
            set { _logger = value; }
        }

        public void Log()
        {
            _logger.Info("Log something");
        }
    }
}
