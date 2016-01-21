using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitio_Privado.Models
{
    public class AzureFolder
    {
        public string Name { get; set; }
        public List<Blob> Blobs { get; set; }

        public AzureFolder(string name)
        {
            Name = name;
            Blobs = new List<Blob>();
        }
    }
}