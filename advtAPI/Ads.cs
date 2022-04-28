namespace advtAPI
{
    public class Ads //Main reprs data in database
    {
        public int Id { get; set; }
        public string AdName { get; set; }
        public double AdPrice { get; set; }
        public string AdDescription { get; set; }
        public string AdMainLink { get; set; }
        public string AdLinks { get; set; }    
        public DateTime CreationTime { get; set; } 

    }
    public class ShortAds //Short data class for list
    {
        public string AdName { get; set; }
        public string AdMainLink { get; set; }
        public double AdPrice { get; set; }

    }
    public class AdsResponse //Response class with pagination
    {
        public int page { get; set; }    
        public int per_page { get; set; }
        public int total_pages { get; set; }
        public List<ShortAds> AdsList { get; set; }

    }
}
