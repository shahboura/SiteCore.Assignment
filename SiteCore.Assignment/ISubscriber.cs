namespace SiteCore.Assignment
{
    public interface ISubscriber
    {
        string Name { get; set; }
        bool IsLive { get; set; }
        void Subscribe();
        void Unsubscribe();
    }
}