namespace API.DTOs.Recommend
{
    public class DiscoverMovieRequestDto
    {
        public bool IncludeAdult { get; set; }
        public string Language { get; set; } = "en-US";
        public string Region { get; set; }
        public int Year { get; set; }
    }
}
