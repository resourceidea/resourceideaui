using EastSeat.ResourceIdea.Migration.Configuration;
using EastSeat.ResourceIdea.Migration.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.CommandLine;

namespace EastSeat.ResourceIdea.Migration;

/// <summary>
/// Main program entry point for the database migration console application.
/// </summary>
public sealed class Program
{
    /// <summary>
    /// Main entry point for the application.
    /// </summary>
    /// <param name="args">Command line arguments.</param>
    /// <returns>Exit code.</returns>
    public static async Task<int> Main(string[] args)
    {
        // Create host builder with configuration and services
        var host = CreateHostBuilder(args).Build();

        // Configure command line interface
        var rootCommand = CreateRootCommand(host);

        // Execute the command
        return await rootCommand.InvokeAsync(args);
    }    /// <summary>
         /// Creates the host builder with configuration and dependency injection.
         /// </summary>
         /// <param name="args">Command line arguments.</param>
         /// <returns>Host builder.</returns>
    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location) ?? Environment.CurrentDirectory;
                config.SetBasePath(basePath)
                      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                      .AddEnvironmentVariables("MIGRATION_")
                      .AddCommandLine(args);
            })
            .ConfigureServices((context, services) =>
            {
                // Configure options
                services.Configure<MigrationOptions>(
                    context.Configuration.GetSection(MigrationOptions.SectionName));
                services.Configure<KeyVaultOptions>(
                    context.Configuration.GetSection(KeyVaultOptions.SectionName));

                // Register services
                services.AddSingleton<IConnectionStringService, ConnectionStringService>();
                services.AddSingleton<IDatabaseMigrationService, DatabaseMigrationService>();
            })
            .ConfigureLogging((context, logging) =>
            {
                logging.ClearProviders();
                logging.AddConsole();
                logging.SetMinimumLevel(LogLevel.Information);
            });

    /// <summary>
    /// Creates the root command for the CLI interface.
    /// </summary>
    /// <param name="host">The application host.</param>
    /// <returns>Root command.</returns>
    private static RootCommand CreateRootCommand(IHost host)
    {
        var rootCommand = new RootCommand("Azure SQL Database Migration Tool")
        {
            Description = "Migrates data from a source Azure SQL database to a destination Azure SQL database."
        };

        // Add migrate-all command
        var migrateAllCommand = new Command("migrate-all", "Migrate all tables from source to destination database");
        migrateAllCommand.SetHandler(async () =>
        {
            await ExecuteMigrationAsync(host, async (service, cancellationToken) =>
                await service.MigrateAllTablesAsync(cancellationToken));
        });
        rootCommand.AddCommand(migrateAllCommand);

        // Add migrate-table command
        var migrateTableCommand = new Command("migrate-table", "Migrate a specific table from source to destination database");
        var tableNameArgument = new Argument<string>("tableName", "Name of the table to migrate");
        migrateTableCommand.AddArgument(tableNameArgument);
        migrateTableCommand.SetHandler(async (string tableName) =>
        {
            await ExecuteMigrationAsync(host, async (service, cancellationToken) =>
                await service.MigrateTableAsync(tableName, cancellationToken));
        }, tableNameArgument);
        rootCommand.AddCommand(migrateTableCommand);

        return rootCommand;
    }

    /// <summary>
    /// Executes the migration operation with proper error handling and logging.
    /// </summary>
    /// <param name="host">The application host.</param>
    /// <param name="migrationAction">The migration action to execute.</param>
    private static async Task ExecuteMigrationAsync(
        IHost host,
        Func<IDatabaseMigrationService, CancellationToken, Task<Models.MigrationResult>> migrationAction)
    {
        using var scope = host.Services.CreateScope();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        var migrationService = scope.ServiceProvider.GetRequiredService<IDatabaseMigrationService>();

        using var cts = new CancellationTokenSource();

        // Handle Ctrl+C gracefully
        Console.CancelKeyPress += (_, e) =>
        {
            e.Cancel = true;
            logger.LogWarning("Cancellation requested. Stopping migration...");
            cts.Cancel();
        };

        try
        {
            logger.LogInformation("Starting database migration...");

            var result = await migrationAction(migrationService, cts.Token);

            if (result.Success)
            {
                logger.LogInformation("Migration completed successfully: {Result}", result);
                Environment.Exit(0);
            }
            else
            {
                logger.LogError("Migration failed: {Result}", result);
                Environment.Exit(1);
            }
        }
        catch (OperationCanceledException) when (cts.Token.IsCancellationRequested)
        {
            logger.LogWarning("Migration was cancelled by user request");
            Environment.Exit(130); // Standard exit code for process terminated by Ctrl+C
        }
        catch (InvalidOperationException ex)
        {
            logger.LogError(ex, "An invalid operation occurred during migration");
            Environment.Exit(1);
        }
        catch (TimeoutException ex)
        {
            logger.LogError(ex, "The migration process timed out");
            Environment.Exit(1);
        }
        catch (Exception ex)
        {
            logger.LogCritical(ex, "An unexpected error occurred during migration");
            Environment.Exit(1);
        }
    }
}
