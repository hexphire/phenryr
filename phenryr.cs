using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Discord;
using Discord.Net;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Phenryr.Services;
using Phenryr.Database;


namespace Phenryr
{
    public class Program
    {

        private readonly IConfiguration _config;
        private DiscordSocketClient _client;
        private static string _logLevel;

        public static void Main(string[] args)
        {
            if(args.Count() != 0)
            {
                _logLevel = args[0];
            }
            Log.Logger = new LoggerConfiguration()
                .WriteTo.File("logs/phenryr.log", rollingInterval: RollingInterval.Day)
                .WriteTo.Console()
                .CreateLogger();

            new Program().MainAsync().GetAwaiter().GetResult();
        }

        public Program()
        {
           
            var _builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile(path: "config.json");

            _config = _builder.Build();
        }


        public async Task MainAsync()
        {

            using (var services = ConfigureServices())
            {

                var client = services.GetRequiredService<DiscordSocketClient>();
                _client = client;

                services.GetRequiredService<LoggingService>();

                await client.LoginAsync(TokenType.Bot, _config["Token"]);
                await client.StartAsync();


                await services.GetRequiredService<CommandHandler>().InitializeAsync();

                await Task.Delay(-1);
            }

        }


        private ServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection()
                .AddSingleton(_config)
                .AddSingleton<DiscordSocketClient>()
                .AddSingleton<CommandService>()
                .AddSingleton<CommandHandler>()
                .AddSingleton<LoggingService>()
                .AddDbContext<PhenryrEntities>()
                .AddLogging(configure => configure.AddSerilog());
                

            if (!string.IsNullOrEmpty(_logLevel))
            {
                switch (_logLevel.ToLower())
                {
                    case "info":
                        {
                            services.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
                            break;
                        }
                    case "error":
                        {
                            services.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Error);
                            break;
                        }
                    case "debug":
                        {
                            services.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Debug);
                            break;
                        }
                    default:
                        {
                            services.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Error);
                            break;
                        }
                }
            }
            else
            {
                services.Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information);
            }

            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider;
        }
        
    }
}
