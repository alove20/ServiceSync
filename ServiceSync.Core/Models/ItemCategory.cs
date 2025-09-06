using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceSync.Core.Models
{
    public class ItemCategory
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public virtual Company Company { get; set; }
        public string Name { get; set; }
        public virtual ICollection<CatalogItem> CatalogItems { get; set; } = new List<CatalogItem>();
    }
}
