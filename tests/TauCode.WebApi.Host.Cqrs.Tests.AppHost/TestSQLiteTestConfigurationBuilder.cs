using NHibernate.Cfg;
using TauCode.Db;

namespace TauCode.WebApi.Host.Cqrs.Tests.AppHost
{
    public class TestSQLiteTestConfigurationBuilder
    {
        public TestSQLiteTestConfigurationBuilder()
        {
            var tuple = DbUtils.CreateSQLiteConnectionString();
            this.TempFilePath = tuple.Item1;
            this.ConnectionString = tuple.Item2;

            var configuration = new Configuration();
            configuration.Properties.Add("connection.connection_string", this.ConnectionString);
            configuration.Properties.Add("connection.driver_class", "NHibernate.Driver.SQLite20Driver");
            configuration.Properties.Add("connection.provider", "NHibernate.Connection.DriverConnectionProvider");
            configuration.Properties.Add("dialect", "NHibernate.Dialect.SQLiteDialect");

            this.Configuration = configuration;
        }

        public string TempFilePath { get; }
        public string ConnectionString { get; }
        public Configuration Configuration { get; }
    }
}
