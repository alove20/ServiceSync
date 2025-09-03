namespace ServiceSync.Core.Models;

public class InvoiceLineItem
{
    public Guid InvoiceId { get; set; }
    public virtual Invoice? Invoice { get; set; }
    public Guid LineItemId { get; set; }
    public virtual LineItem? LineItem { get; set; }
    public int Quantity { get; set; }
    public decimal? PriceOverride { get; set; }
    public string? Notes { get; set; }
}