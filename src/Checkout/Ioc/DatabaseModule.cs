using Autofac;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace Checkout.Ioc
{
    public class DatabaseModule : Module
    {
        private readonly string _connectionString;

        public DatabaseModule(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void Load(ContainerBuilder builder)
        {
            var persistenceConfigurer = GetPersistentConfigurer(_connectionString);
            var sessionFactory = BuildISessionFactory(persistenceConfigurer);
            builder.RegisterInstance(sessionFactory).As<ISessionFactory>();
            builder.Register(s => s.Resolve<ISessionFactory>().OpenSession())
                .As<ISession>()
                .InstancePerLifetimeScope();
        }

        private static IPersistenceConfigurer GetPersistentConfigurer(string connectionString)
        {
            return MsSqlConfiguration.MsSql2005.ShowSql().ConnectionString(connectionString);
        }

        private static ISessionFactory BuildISessionFactory(IPersistenceConfigurer msSqlConfiguration)
        {
            return Fluently.Configure()
                .Database(msSqlConfiguration)
                .Mappings(m => m.FluentMappings.AddFromAssemblyOf<DatabaseModule>())
                .BuildSessionFactory();
        }
    }
}