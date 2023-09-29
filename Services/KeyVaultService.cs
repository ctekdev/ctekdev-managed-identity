using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace CtekDev_ManagedIdentity.Services
{
    public interface IKeyVaultService
    {
        Task<string> GetKeyVaultValue(string key);
    }

    public class KeyVaultService : IKeyVaultService
    {
        private SecretClient _client;
        private readonly IConfiguration _config;
        private readonly string _kvUrl;

        public KeyVaultService(IConfiguration config)
        {
            _config = config;
            _kvUrl = _config["Azure:KvUrl"];

            SecretClientOptions options = new SecretClientOptions()
            {
                Retry =
                {
                    Delay= TimeSpan.FromSeconds(2),
                    MaxDelay = TimeSpan.FromSeconds(16),
                    MaxRetries = 5,
                    Mode = RetryMode.Exponential
                 }
            };

            _client = new SecretClient(new Uri(_kvUrl),
                 new DefaultAzureCredential(), options);
        }

        public async Task<string> GetKeyVaultValue(string key)
        {
            var secret = await _client.GetSecretAsync(key);
            var secretValue = secret.Value;
            return secretValue.Value;
        }
    }
}
