using ABCRetailPOC.Models;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;

namespace ABCRetailPOC.Services
{
    public class ProfileTableStorageService
    {
        private CloudTableClient tableClient;
        private CloudTable table;

        public ProfileTableStorageService(string connectionString, string tableName)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            tableClient = storageAccount.CreateCloudTableClient();
            table = tableClient.GetTableReference(tableName);
            table.CreateIfNotExistsAsync();
        }

        public async Task InsertOrMergeProfileAsync(Profile profile)
        {
            var insertOrMergeOperation = TableOperation.InsertOrMerge(profile);
            await table.ExecuteAsync(insertOrMergeOperation);
        }
        public async Task<List<Profile>> GetAllProfilesAsync()
        {
            var profiles = new List<Profile>();

            TableQuery<Profile> query = new TableQuery<Profile>();
            TableContinuationToken token = null;

            do
            {
                TableQuerySegment<Profile> resultSegment = await table.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;

                profiles.AddRange(resultSegment.Results);
            } while (token != null);

            return profiles;
        }


        public async Task<Profile> RetrieveProfileAsync(string partitionKey, string rowKey)
        {
            var retrieveOperation = TableOperation.Retrieve<Profile>(partitionKey, rowKey);
            var result = await table.ExecuteAsync(retrieveOperation);
            return (Profile)result.Result;
        }

        public async Task DeleteProfileAsync(Profile profile)
        {
            var deleteOperation = TableOperation.Delete(profile);
            await table.ExecuteAsync(deleteOperation);
        }

        // Additional CRUD operations as needed
    }
}

