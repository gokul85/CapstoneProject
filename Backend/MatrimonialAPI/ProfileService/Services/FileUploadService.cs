using Azure.Storage.Blobs;

namespace ProfileService.Services
{
    public class FileUploadService
    {
        private readonly BlobServiceClient _blobServiceClient;

        public FileUploadService(BlobServiceClient blobServiceClient)
        {
            _blobServiceClient = blobServiceClient;
        }

        public async Task<string> UploadFileAsync(IFormFile file, string containerName)
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await blobContainerClient.CreateIfNotExistsAsync();
            var blobClient = blobContainerClient.GetBlobClient(Guid.NewGuid() + Path.GetExtension(file.FileName));

            using (var stream = file.OpenReadStream())
            {
                await blobClient.UploadAsync(stream);
            }

            return blobClient.Uri.ToString();
        }

        public async Task<IEnumerable<string>> UploadFilesAsync(IEnumerable<IFormFile> files, string containerName)
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
            await blobContainerClient.CreateIfNotExistsAsync();

            var urls = new List<string>();

            foreach (var file in files)
            {
                var blobClient = blobContainerClient.GetBlobClient(Guid.NewGuid() + Path.GetExtension(file.FileName));

                using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream);
                }

                urls.Add(blobClient.Uri.ToString());
            }

            return urls;
        }
    }
}
