using System.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;

using System.Web;


namespace Sitio_Privado
{
    public class BlBlobs
    {
        public CloudBlobContainer GetBLOBRef()
        {
            String aux = ConfigurationManager.ConnectionStrings["StorageConnection"].ConnectionString;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(aux);

            CloudBlobClient blobclient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer blobcontainer = blobclient.GetContainerReference("test-container");
            if (blobcontainer.CreateIfNotExists())
                blobcontainer.SetPermissions(new BlobContainerPermissions
                { PublicAccess = BlobContainerPublicAccessType.Blob });
            return blobcontainer;
        }
        public List<string> GetBlobList()
        {
            CloudBlobContainer blobcontainer = GetBLOBRef();
            List<string> blobs = new List<string>();
            foreach (var item in blobcontainer.ListBlobs())
            {
                blobs.Add(item.Uri.ToString());
            }
            return blobs;
        }
        public void AddBlob(HttpPostedFileBase pic)
        {
            if (pic.ContentLength > 0)
            {
                CloudBlobContainer blobcontainer = GetBLOBRef();
                CloudBlockBlob blob = blobcontainer.GetBlockBlobReference(pic.FileName);
                blob.UploadFromStream(pic.InputStream);
            }
        }
        internal void DeleteBlob(string name)
        {
            Uri ur = new Uri(name);
            string fname = Path.GetFileName(ur.LocalPath);
            CloudBlobContainer blobcontainer = GetBLOBRef();
            CloudBlockBlob blob = blobcontainer.GetBlockBlobReference(fname);
            blob.Delete();
        }
    }
}