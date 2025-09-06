using ServiceSync.Core.Models;
using System.ComponentModel.DataAnnotations.Schema;

public class EstimateTemplateItem
{
    public Guid EstimateTemplateId { get; set; }
    [ForeignKey("EstimateTemplateId")]
    public virtual EstimateTemplate EstimateTemplate { get; set; }
    public Guid CatalogItemId { get; set; }
    [ForeignKey("CatalogItemId")]
    public virtual CatalogItem CatalogItem { get; set; }
    public int Quantity { get; set; }
}
