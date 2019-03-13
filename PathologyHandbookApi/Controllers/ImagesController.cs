//Pathology Handbook API
//Copyright (C) 2019  University Hospitals Plymouth NHS Trust 
//
//You should have received a copy of the GNU Affero General Public License
//along with this program.  If not, see <http://www.gnu.org/licenses/>. 
// 
// See LICENSE in the project root for license information.
﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PathologyHandbookApi.Models;
using ImageMagick;
using Microsoft.AspNetCore.Authorization;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;


namespace PathologyHandbookApi.Controllers
{
    [Authorize]
    [Route("api/Images")]
    public class ImagesController : Controller
    {
        private readonly IHostingEnvironment _host;
        private readonly PathologyHandbookContext _context;
        private readonly ImageSettings _imageSettings;
        private readonly StorageAccountOptions _storageAccountOptions;
        private static CloudBlobClient _blobClient;
        private static CloudBlobContainer _blobContainer;

        public ImagesController(IHostingEnvironment host, PathologyHandbookContext context, IOptionsSnapshot<ImageSettings> imageOptions, IOptionsSnapshot<StorageAccountOptions> storageOptions)
        {
            _imageSettings = imageOptions.Value;
            _storageAccountOptions = storageOptions.Value;
            _host = host;
            _context = context;

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse($"DefaultEndpointsProtocol=https;AccountName={_storageAccountOptions.StorageAccountNameOption};AccountKey={_storageAccountOptions.StorageAccountKeyOption}");

            // Gain Access to the containers and blobs in your Azure storage account
            _blobClient = storageAccount.CreateCloudBlobClient();
        }

        private void DeleteAllImagesByContainerTypeId(int containerTypeId)
        {
            var images = _context.Images.Where(img => img.CollectionContainerTypeId == containerTypeId);

            if (images.Any())
            {
                try
                {
                    _context.Images.RemoveRange(images);
                    _context.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        private async Task DeleteAllBlobsByContainerTypeId(int containerTypeId)
        {

            var images = _context.Images.Where(img => img.CollectionContainerTypeId == containerTypeId);
            var container = _blobClient.GetContainerReference(_storageAccountOptions.FullSizeContainerNameOption);

            if (images.Any())
            {
                foreach (var image in images)
                {
                    var blockBlob = container.GetBlobReference(image.FileName);
                    await blockBlob.DeleteIfExistsAsync();
                }
            }
        }

        private async Task DeleteAllThumbnailBlobsByContainerTypeId(int containerTypeId)
        {

            var images = _context.Images.Where(img => img.CollectionContainerTypeId == containerTypeId);
            var container = _blobClient.GetContainerReference(_storageAccountOptions.ThumbnailContainerNameOption);

            if (images.Any())
            {
                foreach (var image in images)
                {
                    var blockBlob = container.GetBlobReference(image.ThumbnailFileName);
                    await blockBlob.DeleteIfExistsAsync();
                }
            }
        }
        [HttpGet]
        [Route("{containerTypeId}")]
        public async Task<IEnumerable<Image>> GetImages(int containerTypeId)
        {
            var images = await _context.Images.Where(img => img.CollectionContainerTypeId == containerTypeId).ToListAsync();

            return images;
        }

        [HttpGet]
        [Route("/api/Images/download/{imageFileName}")]
        public async Task<IActionResult> GetImage(string imageFileName)
        {
            if (string.IsNullOrWhiteSpace(imageFileName))
                return BadRequest();

            var uploadsFolderPath = Path.Combine(_host.ContentRootPath, "uploads");
            var filePath = Path.Combine(uploadsFolderPath, imageFileName);

            var extType = Path.GetExtension(imageFileName).Remove(0, 1);

            var contentType = $"image/{extType}";
     
            return PhysicalFile(filePath, "image/png");
        }

        [HttpGet]
        [Route("/api/Images/DownloadBlob/{imageFileName}")]
        public async Task<IActionResult> GetImageBlobs(string imageFileName)
        {
            if (string.IsNullOrWhiteSpace(imageFileName))
                return null;

            var container = _blobClient.GetContainerReference(_storageAccountOptions.FullSizeContainerNameOption);
            await container.CreateIfNotExistsAsync();

            await container.SetPermissionsAsync(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });

            var uploadsFolderPath = Path.Combine(_host.ContentRootPath, "uploads");
            if (!Directory.Exists(uploadsFolderPath))
                Directory.CreateDirectory(uploadsFolderPath);

            var filePath = Path.Combine(uploadsFolderPath, imageFileName);

            var extType = Path.GetExtension(imageFileName).Remove(0, 1);

            var contentType = $"image/{extType}";

            var blockBlob = container.GetBlobReference(imageFileName);

            using (var fileStream = System.IO.File.Create(filePath))
            {
                await blockBlob.DownloadToStreamAsync(fileStream);
            }

            return PhysicalFile(filePath, "image/png");
        }

        [HttpPost]
        [Route("/api/Images/UploadBlob/{containerTypeId}")]
        public async Task<IActionResult> UploadBlob(int containerTypeId, IFormFile file)
        {
            var containerType = await _context.CollectionContainerTypes.FirstOrDefaultAsync(ct => ct.Id == containerTypeId);

            if (Equals(containerType, null))
                return NotFound();

            if (Equals(file, null))
                return BadRequest("null file");

            if (file.Length == 0)
                return BadRequest("Empty File");

            if (file.Length > _imageSettings.MaxBytes)
                return BadRequest("Max file size exceeded");

            if (!_imageSettings.AcceptedFileTypes.Any(s => s == Path.GetExtension(file.FileName)))
                return BadRequest("File type not allowed");

            await DeleteAllBlobsByContainerTypeId(containerTypeId);
            await DeleteAllThumbnailBlobsByContainerTypeId(containerTypeId);
            DeleteAllImagesByContainerTypeId(containerTypeId);

            var fileExtension = Path.GetExtension(file.FileName).ToLower();

            var fullSizeFolderPath = Path.Combine(_host.ContentRootPath, "fullsize");
            var fullsizeFileName = Guid.NewGuid().ToString() + fileExtension;
            var fullsizeFilePath = Path.Combine(fullSizeFolderPath, fullsizeFileName);

            await CreateFullsizeImageInLocalFolder(fullSizeFolderPath, file, fullsizeFilePath);
            await UploadFullsizeBlobToBlobStorage(fullsizeFileName, fullsizeFilePath);

            var thumbnailFolderPath = Path.Combine(_host.ContentRootPath, "thumbnail");
            var thumbnailFileName = Guid.NewGuid().ToString() + fileExtension;
            var thumbnailFilePath = Path.Combine(thumbnailFolderPath, thumbnailFileName);
            
            await CreateMagickThumbnailInLocalFolder(fullsizeFilePath, thumbnailFolderPath, thumbnailFilePath);
            await UploadThumbnailBlobStorage(thumbnailFileName, thumbnailFilePath);
 
            var image = new Image
            {
                FileName = fullsizeFileName,
                FullsizeFileName = fullsizeFileName,
                ThumbnailFileName = thumbnailFileName
            };
            containerType.Images.Add(image);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            Directory.Delete(fullSizeFolderPath, true);
            Directory.Delete(thumbnailFolderPath, true);

            return Ok(image);

        }

        private async Task CreateMagickThumbnailInLocalFolder(string file, string thumbnailUploadsFolderPath, string thumbnailFilePath)
        {
            if (!Directory.Exists(thumbnailUploadsFolderPath))
                Directory.CreateDirectory(thumbnailUploadsFolderPath);

            using (var image = new MagickImage(file))
            {
                var size = new MagickGeometry(250, 125);
                image.Resize(size);

                image.Write(thumbnailFilePath);
            }
        }

        private async Task CreateFullsizeImageInLocalFolder(string uploadsFolderPath, IFormFile file, string filePath)
        {
            if (!Directory.Exists(uploadsFolderPath))
                Directory.CreateDirectory(uploadsFolderPath);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
        }

        private async Task UploadFullsizeBlobToBlobStorage(string fileName, string filePath)
        {
            var container =
                _blobClient.GetContainerReference(_storageAccountOptions.FullSizeContainerNameOption);

            var blockBlob = container.GetBlockBlobReference(fileName);

            using (var fileStream = System.IO.File.OpenRead(filePath))
            {
                await blockBlob.UploadFromStreamAsync(fileStream);
            }

        }

        private async Task UploadThumbnailBlobStorage(string fileName, string filePath)
        {
            var container =
                _blobClient.GetContainerReference(_storageAccountOptions.ThumbnailContainerNameOption);

            var blockBlob = container.GetBlockBlobReference(fileName);

            using (var fileStream = System.IO.File.OpenRead(filePath))
            {
                await blockBlob.UploadFromStreamAsync(fileStream);
            }
        }

        [HttpPost]
        [Route("/api/Images/{containerTypeId}")]
        public async Task<IActionResult> Upload(int containerTypeId, IFormFile file)
        {
            var containerType = await _context.CollectionContainerTypes.FirstOrDefaultAsync(ct => ct.Id == containerTypeId);

            if (Equals(containerType, null))
                return NotFound();

            if (Equals(file, null))
                return BadRequest("null file");

            if (file.Length == 0)
                return BadRequest("Empty File");

            if (file.Length > _imageSettings.MaxBytes)
                return BadRequest("Max file size exceeded");

            if (!_imageSettings.AcceptedFileTypes.Any(s => s == Path.GetExtension(file.FileName)))
                return BadRequest("File type not allowed");
            
            var uploadsFolderPath = Path.Combine(_host.ContentRootPath, "uploads");
            if (!Directory.Exists(uploadsFolderPath))
                Directory.CreateDirectory(uploadsFolderPath);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var image = new Image {FileName = fileName};
            containerType.Images.Add(image);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return Ok(image);
        }
    }
}
