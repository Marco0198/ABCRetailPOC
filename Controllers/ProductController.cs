using ABCRetailPOC.Models;
using ABCRetailPOC.Services;
using Microsoft.AspNetCore.Mvc;

namespace ABCRetailPOC.Controllers
{
    public class ProductController : Controller
    {
        private TableStorageService storageService;

        public ProductController()
        {
            string connectionString = "";
            //DefaultEndpointsProtocol=https;AccountName=st10458148;AccountKey=BvcUpjKGDaJgfPh9Qm2VNyS4WFkDCy/PQxrl8VR3YDXXBqm5iRS+vHCqSMV3Xd2+QKMRbftyttML+AStzRuwAw==;EndpointSuffix=core.windows.net;BlobEndpoint=https://st10458148.blob.core.windows.net/;FileEndpoint=https://st10458148.file.core.windows.net/;QueueEndpoint=https://st10458148.queue.core.windows.net/;TableEndpoint=https://st10458148.table.core.windows.net/
            string tableName = "Products";
            storageService = new TableStorageService(connectionString, tableName);
        }

        // GET: Product
        public async Task<IActionResult> Index()
        {
            var products = await storageService.GetAllProductsAsync(); // Implement this method in your TableStorageService

            // Pass the list of products to the view
            return View(products);
        }

        // GET: Product/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            if (ModelState.IsValid)
            {
                // Set partition key and row key
                product.PartitionKey = "Products";
                product.RowKey = Guid.NewGuid().ToString();

                await storageService.InsertOrMergeProductAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(string partitionKey, string rowKey)
        {
            var product = await storageService.RetrieveProductAsync(partitionKey, rowKey);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string partitionKey, string rowKey, Product product)
        {
            if (ModelState.IsValid)
            {
                product.PartitionKey = partitionKey;
                product.RowKey = rowKey;

                await storageService.InsertOrMergeProductAsync(product);
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Product/Details/5
        public async Task<IActionResult> Details(string partitionKey, string rowKey)
        {
            var product = await storageService.RetrieveProductAsync(partitionKey, rowKey);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(string partitionKey, string rowKey)
        {
            var product = await storageService.RetrieveProductAsync(partitionKey, rowKey);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string partitionKey, string rowKey)
        {
            var product = await storageService.RetrieveProductAsync(partitionKey, rowKey);
            if (product != null)
            {
                await storageService.DeleteProductAsync(product);
            }
            return RedirectToAction(nameof(Index));
        }
    }

}
