using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitio_Privado.Models
{
    public class AzureFolder
    {
        public string Name { get; set; }
        public List<AzureBlob> Blobs { get; set; }
        public List<AzureFolder> Folders { get; set; }

        public AzureFolder(string name)
        {
            Name = name;
            Blobs = new List<AzureBlob>();
            Folders = new List<AzureFolder>();
        }
    }
}