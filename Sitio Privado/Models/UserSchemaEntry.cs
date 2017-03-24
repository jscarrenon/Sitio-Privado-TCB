using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Sitio_Privado.Models
{
    public class UserSchemaEntry
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        [Index("IX_ExternalFieldName", 1, IsUnique = true)]
        public string ExternalFieldName { get; set; }

        [Required]
        [MaxLength(255)]
        [Index("IX_PropertyName", 1, IsUnique = true)]
        public string PropertyName { get; set; }

        [Required]
        public bool IsEditable { get; set; }
    }
}