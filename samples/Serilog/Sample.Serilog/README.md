## Replace log configuration

Remove section ```Logging``` from ```appsettings.Development.json```

Replace section ```Logging``` from ```appsettings.json``` with Serilog configuration:
```json
{
  ...
  "Serilog": {
    "Using": [
      "Serilog.Sinks.Console"
    ],
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "Console"
      }
    ]
  }     
  ...
}
```

Exchange AspNetCore logger with Serilog (in Program.cs):

```C#
public static void Main(string[] args)
{
    var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
    var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
    
    var config = new ConfigurationBuilder()
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .AddJsonFile($"appsettings.{environment}.json", optional: true)
        .AddEnvironmentVariables()
        .Build();

    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(config)
        .CreateLogger();
        
    try
    {
        Log.Information($"Application {assemblyName} starting.");
        CreateHostBuilder(args).Build().Run();
    }
    catch (Exception ex)
    {
        Log.Fatal(ex, $"Application {assemblyName} failed to start.");
    }
    finally
    {
        Log.CloseAndFlush();
    }
}

public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .UseSerilog()
        .ConfigureWebHostDefaults(webBuilder =>
        {
            webBuilder.UseStartup<Startup>();
        });
```