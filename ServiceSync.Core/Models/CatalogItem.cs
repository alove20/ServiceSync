using ServiceSync.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceSync.Core.Models;
public class CatalogItem
{
    [Key]
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    [ForeignKey("CompanyId")]
    public virtual Company Company { get; set; }
    public Guid? ItemCategoryId { get; set; }
    [ForeignKey("ItemCategoryId")]
    public virtual ItemCategory? ItemCategory { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public UnitType UnitType { get; set; }
}
