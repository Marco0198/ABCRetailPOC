using Microsoft.WindowsAzure.Storage.Table;
using System.ComponentModel.DataAnnotations;


namespace ABCRetailPOC.Models
{
    public class Profile : TableEntity
    {
        public Profile(string partitionKey, string rowKey)
        {
            this.PartitionKey = partitionKey;
            this.RowKey = rowKey;
        }

        public Profile() { }

        public string UserName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public int CustomerNumber { get; set; }

    }
}

