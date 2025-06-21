namespace EastSeat.ResourceIdea.Migration.Configuration;

/// <summary>
/// Configuration options for Azure Key Vault.
/// </summary>
public sealed class KeyVaultOptions
{
    /// <summary>
    /// Configuration section name.
    /// </summary>
    public const string SectionName = "KeyVault";

    /// <summary>
    /// Gets or sets the Azure Key Vault URI.
    /// </summary>
    public string VaultUri { get; set; } = string.Empty;
}
