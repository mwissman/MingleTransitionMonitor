namespace MingleTransitionMonitor.Domain
{
    public interface IMingleProjectConnectionInformation
    {
        string Username { get; }
        string Password { get; }
        string EventFeedUrl { get; }
    }
}