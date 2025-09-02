using System.ComponentModel.DataAnnotations;

namespace ServiceSync.Data.Models;
public class LineItem
{
    [Key]
    public Guid Id { get; set; }
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; } = 0;
    public Guid InvoiceLineItemId {  get; set; }
    public virtual InvoiceLineItem? InvoiceLineItem { get; set; }
    public Guid LineItemTypeId { get; set; }
    public virtual LineItemType? LineItemType { get; set; }
}
