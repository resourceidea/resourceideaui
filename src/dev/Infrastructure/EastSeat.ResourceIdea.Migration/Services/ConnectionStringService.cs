using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using EastSeat.ResourceIdea.Migration.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EastSeat.ResourceIdea.Migration.Services;

/// <summary>
/// Service for managing connection strings and secrets from Azure Key Vault.
/// </summary>
public sealed class ConnectionStringService : IConnectionStringService
{
    private readonly SecretClient _secretClient;
    private readonly ILogger<ConnectionStringService> _logger;
    private readonly KeyVaultOptions _keyVaultOptions;    /// <summary>
                                                          /// Initializes a new instance of the <see cref="ConnectionStringService"/> class.
                                                          /// </summary>
                                                          /// <param name="keyVaultOptions">Key Vault configuration options.</param>
                                                          /// <param name="logger">Logger instance.</param>
    public ConnectionStringService(
        IOptions<KeyVaultOptions> keyVaultOptions,
        ILogger<ConnectionStringService> logger)
    {
        ArgumentNullException.ThrowIfNull(keyVaultOptions);
        ArgumentNullException.ThrowIfNull(logger);

        _keyVaultOptions = keyVaultOptions.Value;
        _logger = logger;

        if (string.IsNullOrWhiteSpace(_keyVaultOptions.VaultUri))
        {
            throw new ArgumentException("Key Vault URI cannot be null or empty.", nameof(keyVaultOptions));
        }

        // Use DefaultAzureCredential for authentication (supports Managed Identity, Azure CLI, etc.)
        var credential = new DefaultAzureCredential();
        _secretClient = new SecretClient(new Uri(_keyVaultOptions.VaultUri), credential);

        _logger.LogInformation("ConnectionStringService initialized with Key Vault: {VaultUri}",
            _keyVaultOptions.VaultUri);
    }

    /// <inheritdoc />
    public async Task<string> GetSourceConnectionStringAsync(CancellationToken cancellationToken = default)
    {
        return await GetSecretAsync("source-database-connection-string", cancellationToken);
    }

    /// <inheritdoc />
    public async Task<string> GetDestinationConnectionStringAsync(CancellationToken cancellationToken = default)
    {
        return await GetSecretAsync("destination-database-connection-string", cancellationToken);
    }

    /// <summary>
    /// Retrieves a secret from Azure Key Vault.
    /// </summary>
    /// <param name="secretName">The name of the secret to retrieve.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The secret value.</returns>
    private async Task<string> GetSecretAsync(string secretName, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogDebug("Retrieving secret: {SecretName}", secretName);

            var secret = await _secretClient.GetSecretAsync(secretName, cancellationToken: cancellationToken);

            _logger.LogInformation("Successfully retrieved secret: {SecretName}", secretName);
            return secret.Value.Value;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to retrieve secret: {SecretName}", secretName);
            throw;
        }
    }
}
