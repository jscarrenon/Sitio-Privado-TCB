using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitio_Privado.Models
{
    public class AzureFolderModel
    {
        public string Name { get; set; }
        public List<AzureBlobModel> Blobs { get; set; }
        public List<AzureFolderModel> Folders { get; set; }

        public AzureFolderModel(string name)
        {
            Name = name;
            Blobs = new List<AzureBlobModel>();
            Folders = new List<AzureFolderModel>();
        }
    }
}