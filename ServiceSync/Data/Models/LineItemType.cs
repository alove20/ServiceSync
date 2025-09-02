using System.ComponentModel.DataAnnotations;

namespace ServiceSync.Data.Models;
public class LineItemType
{
    [Key]
    public Guid Id { get; set; }
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;
    public virtual ICollection<LineItem> LineItems { get; set; } = [];
}
