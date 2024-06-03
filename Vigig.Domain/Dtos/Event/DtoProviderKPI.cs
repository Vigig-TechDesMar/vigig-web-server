namespace Vigig.Domain.Dtos.Event;

public class DtoProviderKPI
{
    public Guid Id;
    public float Progress { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid ProviderId { get; set; }
    public Guid LeaderBoardId { get; set; }
}