namespace MovieBooking.Model
{
    public class DatabaseSetting : IDatabaseSetting
    {
        public string CollectionName { get; set; } = String.Empty;
        public string ConnectionString { get; set; } = String.Empty;
        public string DatabaseName { get; set; } = String.Empty;
        public string MovieCollectionName { get; set; } = String.Empty;
        public string TicketCollection { get; set; } = String.Empty;

    }
}
