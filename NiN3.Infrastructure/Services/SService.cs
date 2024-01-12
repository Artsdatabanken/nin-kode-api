using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace NiN3.Infrastructure.Services
{
    public class SService : ISService
    {
        private string? _admintoken;  // the name field
        public string? Admintoken => _admintoken;



        /*
                 public async void Startup()
        {
            throw new NotImplementedException();
            Console.WriteLine("hepp");
            const string secretName = "admintoken";
            var keyVaultName = Environment.GetEnvironmentVariable("nin3kodeKV");
            var kvUri = "https://" + keyVaultName + ".vault.azure.net";
            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
            Console.WriteLine($"Retrieving your secret from {keyVaultName}.");
            var sec = await client.GetSecretAsync(secretName);
            AdminToken = sec.Value.Value;
            Console.WriteLine($"Your secret is '{AdminToken}'.");
        }
         
         */
        public async void Startup()
        {
            const string secretName = "admintoken";
            //var keyVaultName = Environment.GetEnvironmentVariable("nin3kodeKV");
            //var kvUri = "https://" + keyVaultName + ".vault.azure.net";
            var kvUri = "https://adb-test-nin3-kv.vault.azure.net";
            var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());
            //Console.WriteLine($"Retrieving your secret from {keyVaultName}.");
            var sec = await client.GetSecretAsync(secretName);
            _admintoken = sec.Value.Value;
            //Console.WriteLine($"Your secret is '{Admintoken}'.");
        }

    }
}
