using Dapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UdemyMicroservice.Discount
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;

                var configuration = serviceProvider.GetRequiredService<IConfiguration>();

                var _dbConnection = new NpgsqlConnection(configuration.GetConnectionString("PostgreSql"));

                _dbConnection.Execute("create table Discount(Id serial primary key, UserId varchar(200) not null, Rate smallint not null, Code varchar(50) not null, CreatedDate timestamp not null default CURRENT_TIMESTAMP)");
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
