using System.ComponentModel.DataAnnotations.Schema;

namespace ServiceSync.Core.Models;

public class EstimateLineItem
{
    public Guid EstimateId { get; set; }
    [ForeignKey("EstimateId")]
    public virtual Estimate Estimate { get; set; }
    public Guid CatalogItemId { get; set; }
    [ForeignKey("CatalogItemId")]
    public virtual CatalogItem CatalogItem { get; set; }
    public int Quantity { get; set; }
    public decimal? PriceOverride { get; set; }
    public string? Notes { get; set; }
    public string? Description { get; set; }
    public decimal UnitPrice { get; set; }
}