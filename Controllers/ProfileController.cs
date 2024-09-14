using ABCRetailPOC.Models;
using ABCRetailPOC.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using static System.Net.Mime.MediaTypeNames;

namespace ABCRetailPOC.Controllers
{
    public class ProfileController : Controller
    {
        private ProfileTableStorageService storageService;
        private readonly BlobStorageService _blobStorageService;
        private const string BlobConnectionString = "DefaultEndpointsProtocol=https;AccountName=st10458148;AccountKey=BvcUpjKGDaJgfPh9Qm2VNyS4WFkDCy/PQxrl8VR3YDXXBqm5iRS+vHCqSMV3Xd2+QKMRbftyttML+AStzRuwAw==;EndpointSuffix=core.windows.net;BlobEndpoint=https://st10458148.blob.core.windows.net/;";
        private const string BlobContainerName = "images";





        public ProfileController(BlobStorageService blobStorageService)
        {
            _blobStorageService = new BlobStorageService(BlobConnectionString, BlobContainerName);
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=st10458148;AccountKey=BvcUpjKGDaJgfPh9Qm2VNyS4WFkDCy/PQxrl8VR3YDXXBqm5iRS+vHCqSMV3Xd2+QKMRbftyttML+AStzRuwAw==;EndpointSuffix=core.windows.net;BlobEndpoint=https://st10458148.blob.core.windows.net/;FileEndpoint=https://st10458148.file.core.windows.net/;QueueEndpoint=https://st10458148.queue.core.windows.net/;TableEndpoint=https://st10458148.table.core.windows.net/";
            string tableName = "Profiles";
            storageService = new ProfileTableStorageService(connectionString, tableName);
        }

        // GET: Profile
        public async Task<IActionResult> Index()
        {
            var profiles = await storageService.GetAllProfilesAsync(); // Implement this method in your ProfileTableStorageService

            // Pass the list of profiles to the view
            return View(profiles);
        }

        // GET: Profile/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Profile/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(Profile profile)
        {
            if (ModelState.IsValid)
            {
                // Set partition key and row key
                profile.PartitionKey = "profiles";
                profile.RowKey = Guid.NewGuid().ToString();

                await storageService.InsertOrMergeProfileAsync(profile);
                return RedirectToAction(nameof(Index));
            }
            return View(profile);
        }



        // GET: Profile/Edit/5
        public async Task<IActionResult> Edit(string partitionKey, string rowKey)
        {
            var Profile = await storageService.RetrieveProfileAsync(partitionKey, rowKey);
            if (Profile == null)
            {
                return NotFound();
            }
            return View(Profile);
        }

        // POST: Profile/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string partitionKey, string rowKey, Profile Profile)
        {
            if (ModelState.IsValid)
            {
                Profile.PartitionKey = partitionKey;
                Profile.RowKey = rowKey;

                await storageService.InsertOrMergeProfileAsync(Profile);
                return RedirectToAction(nameof(Index));
            }
            return View(Profile);
        }

        // GET: Profile/Details/5
        public async Task<IActionResult> Details(string partitionKey, string rowKey)
        {
            var Profile = await storageService.RetrieveProfileAsync(partitionKey, rowKey);
            if (Profile == null)
            {
                return NotFound();
            }
            return View(Profile);
        }

        // GET: Profile/Delete/5
        public async Task<IActionResult> Delete(string partitionKey, string rowKey)
        {
            var Profile = await storageService.RetrieveProfileAsync(partitionKey, rowKey);
            if (Profile == null)
            {
                return NotFound();
            }
            return View(Profile);
        }

        // POST: Profile/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string partitionKey, string rowKey)
        {
            var Profile = await storageService.RetrieveProfileAsync(partitionKey, rowKey);
            if (Profile != null)
            {
                await storageService.DeleteProfileAsync(Profile);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
