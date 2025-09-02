namespace ServiceSync.Data.Models;

public class UserJobRequest
{
    public Guid UserId { get; set; }
    public virtual Contact? User { get; set; }
    public Guid JobRequestId { get; set; }
    public virtual JobRequest? JobRequest { get; set; }
}