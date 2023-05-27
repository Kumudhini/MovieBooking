namespace MovieBooking.Model
{
    public interface IDatabaseSetting
    {
        public string CollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string MovieCollectionName { get; set; }
        public string TicketCollection { get; set; }


    }
}
