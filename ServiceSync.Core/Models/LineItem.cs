using System.ComponentModel.DataAnnotations;

namespace ServiceSync.Core.Models;
public class LineItem
{
    [Key]
    public Guid Id { get; set; }
    public Guid CompanyId { get; set; }
    public virtual Company? Company { get; set; }
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; } = 0;
    public Guid InvoiceLineItemId {  get; set; }
    public virtual InvoiceLineItem? InvoiceLineItem { get; set; }
    public Guid LineItemTypeId { get; set; }
    public virtual LineItemType? LineItemType { get; set; }
}
