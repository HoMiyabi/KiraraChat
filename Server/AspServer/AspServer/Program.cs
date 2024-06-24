using AspServer.Models;
using Microsoft.AspNetCore.Hosting;
using SqlSugar;

namespace AspServer
{
    public class DbConnectionOptions
    {
        public const string key = "DbConnection";
        public string Server { get; set; }
        public int Port { get; set; }
        public string Uid {  get; set; }
        public string Pwd { get; set; }
        public string Database { get; set; }
    }

    public class Program
    {
        public static SqlSugarScope db;

        public static void Main(string[] args)
        {
            // Config
            var builder = WebApplication.CreateBuilder(args);

            // Database
            var dbConnectionOptions = builder.Configuration
                .GetSection(DbConnectionOptions.key).Get<DbConnectionOptions>();

            if (dbConnectionOptions == null)
            {
                Console.WriteLine("dbConnectionOptions == null");
                return;
            }

            string connectionString =
                $"server={dbConnectionOptions.Server};" +
                $"port={dbConnectionOptions.Port};" +
                $"uid={dbConnectionOptions.Uid};" +
                $"pwd={dbConnectionOptions.Pwd};" +
                $"database={dbConnectionOptions.Database}";

            //Console.WriteLine($"Connection String: " + connectionString);

            db = new(new ConnectionConfig()
            {
                ConnectionString = connectionString,
                DbType = DbType.MySql,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute,
                
            });


            // Database Log
            //db.Aop.OnLogExecuted = (sql, pars) =>
            //{
            //    Console.WriteLine(sql + "\r\n" +
            //    db.Utilities.SerializeObject(pars.ToString()));
            //    Console.WriteLine();
            //};

            db.DbMaintenance.CreateDatabase();
            db.CodeFirst.InitTables(typeof(UserTable));
            db.CodeFirst.InitTables(typeof(ConversationTable));
            db.CodeFirst.InitTables(typeof(FriendRelationshipDO));
            db.CodeFirst.InitTables(typeof(FriendRequestDO));

            // Server
            builder.Services.AddControllersWithViews();
            //builder.Services.AddControllers()
            //    .ConfigureApiBehaviorOptions(options =>
            //    {
            //        options.SuppressModelStateInvalidFilter = false;
            //    });
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.IncludeFields = true;
            });

            // Url Config
            string? url = builder.Configuration["UseUrls:Url"];
            if (url == null)
            {
                Console.WriteLine("url == null");
                return;
            }
            Console.WriteLine("url: " + url);
            builder.WebHost.UseUrls(url);

            var app = builder.Build();
            app.MapControllers();
            app.Run();
        }
    }
}
