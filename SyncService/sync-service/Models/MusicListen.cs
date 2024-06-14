namespace sync_service.Models
{
    public class MusicListen
    {
        public Guid Id { get; set; }
        public Guid MusicId { get; set; }
        public Music Music { get; set; }
        public DateTime ListenDate { get; set; }
        public int ListenCount { get; set; }
    }
}
