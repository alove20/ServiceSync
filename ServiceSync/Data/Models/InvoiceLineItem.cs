using System.ComponentModel.DataAnnotations;

namespace ServiceSync.Data.Models;
public class InvoiceLineItem
{
    [Key]
    public Guid Id { get; set; }
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; } = 0;
    public Guid InvoiceLineItemTypeId {  get; set; }
}
