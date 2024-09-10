using Microsoft.WindowsAzure.Storage.Table;

namespace ABCRetailPOC.Models
{
    public class Product : TableEntity
    {
        public Product(string partitionKey, string rowKey)
        {
            this.PartitionKey = partitionKey;
            this.RowKey = rowKey;
        }

        public Product() { }

        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }

}
