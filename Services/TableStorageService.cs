using ABCRetailPOC.Models;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage;

namespace ABCRetailPOC.Services
{
    public class TableStorageService
    {
        private CloudTableClient tableClient;
        private CloudTable table;

        public TableStorageService(string connectionString, string tableName)
        {
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            tableClient = storageAccount.CreateCloudTableClient();
            table = tableClient.GetTableReference(tableName);
            table.CreateIfNotExistsAsync();
        }

        public async Task InsertOrMergeProductAsync(Product product)
        {
            var insertOrMergeOperation = TableOperation.InsertOrMerge(product);
            await table.ExecuteAsync(insertOrMergeOperation);
        }
        public async Task<List<Product>> GetAllProductsAsync()
        {
            var products = new List<Product>();

            TableQuery<Product> query = new TableQuery<Product>();
            TableContinuationToken token = null;

            do
            {
                TableQuerySegment<Product> resultSegment = await table.ExecuteQuerySegmentedAsync(query, token);
                token = resultSegment.ContinuationToken;

                products.AddRange(resultSegment.Results);
            } while (token != null);

            return products;
        }


        public async Task<Product> RetrieveProductAsync(string partitionKey, string rowKey)
        {
            var retrieveOperation = TableOperation.Retrieve<Product>(partitionKey, rowKey);
            var result = await table.ExecuteAsync(retrieveOperation);
            return (Product)result.Result;
        }

        public async Task DeleteProductAsync(Product product)
        {
            var deleteOperation = TableOperation.Delete(product);
            await table.ExecuteAsync(deleteOperation);
        }

        // Additional CRUD operations as needed
    }
}

