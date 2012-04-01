using System;
using System.Configuration;
using System.IO;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using FluentNHibernate.Conventions.Helpers;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace Scrumee.Infrastructure
{
    public class NHibernateBootstrapper
    {
        #region Public Properties

        public static ISessionFactory SessionFactory
        {
            get { return _sessionFactory ?? ( _sessionFactory = CreateSessionFactory() ); }
        }

        public static NHibernate.Cfg.Configuration Configuration { get; set; }

        #endregion Public Properties
        
        #region Private Fields

        private static ISessionFactory _sessionFactory;

        private const string SqLiteConnectionStringName = "SqliteProjects";

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Open a new NHibenate Session
        /// </summary>
        /// <returns>A new ISession</returns>
        public static ISession OpenSession()
        {
            var session = SessionFactory.OpenSession();

            session.SessionFactory.Statistics.IsStatisticsEnabled = true;

            return session;
        }
        
        #endregion Public Methods

        #region Private Methods

        private static ISessionFactory CreateSessionFactory()
        {
            if ( Configuration == null )
            {
                // FluentNHibernate Configuration API for configuring NHibernate
                Configuration = Fluently.Configure()
                    .Database( SQLiteConfiguration.Standard.ConnectionString( c => c.FromConnectionStringWithKey( SqLiteConnectionStringName ) ) )
                    .Mappings( m => m.FluentMappings.AddFromAssemblyOf<Scrumee.Data.Entities.Project>()
                                        .Conventions.Add( PrimaryKey.Name.Is( x => x.EntityType.Name + "Id" ), ForeignKey.EndsWith( "Id" ) ) )
                    .BuildConfiguration();

                // Loquacious ( NHibernate v3.0+ ) Configuration API for configuring NHibernate
                /*
                Configuration = new NHibernate.Cfg.Configuration()
                    .Proxy( p => p.ProxyFactoryFactory<ProxyFactoryFactory>() )
                    .DataBaseIntegration( db =>
                                              {
                                                  db.Dialect<SQLiteDialect>();
                                                  db.ConnectionStringName = SqLiteConnectionStringName;
                                                  db.Driver<NHibernate.Driver.SQLite20Driver>();
                                              } )
                    .AddAssembly( typeof( Scrumee.Data.Entities.Entity ).Assembly );
                */

                DropAndRecreateSqliteDatabase();
            }

            var sessionFactory = Configuration.BuildSessionFactory();

            return sessionFactory;
        }

        private static void UpdateSchema()
        {
            new SchemaUpdate( Configuration )
                .Execute( true, true );
        }

        private static void DropAndRecreateSqliteDatabase()
        {
            string path = PathToDatabase( SqLiteConnectionStringName );

            if ( File.Exists( path ) )
                File.Delete( path );

            UpdateSchema();
        }

        private static string PathToDatabase( string connectionStringName )
        {
            string connectionString = ConfigurationManager.ConnectionStrings[ connectionStringName ].ConnectionString;
            
            string pathToAppData = AppDomain.CurrentDomain.GetData( "DataDirectory" ).ToString();

            // Example: "Data Source=|DataDirectory|Projects.db" => "Projects.db"
            string databaseName = connectionString.Replace( "Data Source=|DataDirectory|", "" );

            string fullPath =  Path.Combine( pathToAppData, databaseName );

            return fullPath;
        }

        #endregion Private Methods
    }
}
