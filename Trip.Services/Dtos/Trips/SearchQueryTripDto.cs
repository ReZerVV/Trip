namespace Trip.Services.Dtos.Trips
{
    public class SearchQueryTripDto
    {
        public DateTime? Date { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
