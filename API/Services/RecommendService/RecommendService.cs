using API.DTOs.Recommend;
using Newtonsoft.Json;
using System;
using System.Net.Http;

namespace API.Services.RecommendService  
{
    public class RecommendService : IRecommnedService
    {
        // Fields
        private readonly IConfiguration _configuration;

        // Constructor
        public RecommendService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<List<TmdbMovieResponse>> DiscoverMovie(DiscoverMovieRequestDto request)
        {
            throw new NotImplementedException();
        }

        public Task<List<TmdbMovieResponse>> DiscoverTv(DiscoverTvRequestDto request)
        {
            throw new NotImplementedException();
        }

        public async Task<List<TmdbMovieResponse>> NowPlaying(string language, int page, string region)
        {
            // Get TMDB API key from configugration
            string? tmdbKey = _configuration["ApiKeys:Tmdb"] ?? throw new Exception("Cannot find internal API keys");
            string tmdbUrl = "https://api.themoviedb.org/3/movie/now_playing";
            tmdbUrl += $"?api_key={tmdbKey}&language={language}&page={page}&region={region}";

            using HttpClient httpClient = new HttpClient();

            HttpResponseMessage response = await httpClient.GetAsync(tmdbUrl);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var serializedJsonResponse = JsonConvert.DeserializeObject<TmdbMovieApiResponse>(jsonResponse);

            if (serializedJsonResponse?.Results == null) return new List<TmdbMovieResponse>();

            var recommendResponseDtoList = serializedJsonResponse.Results.Select(movie => new TmdbMovieResponse
            {
                Title = movie.Title,
                Description = movie.Overview,
                ReleaseDate = movie.Release_Date,
                ImageUrl = movie.Poster_Path,
                Genres = movie.Genre_Ids.Select(id => GetGenreName(id)).ToList()
            }).ToList();

            return recommendResponseDtoList;
        }

        public async Task<List<TmdbMovieResponse>> Popular(string language, int page, string region)
        {
            // Get TMDB API key from configugration
            string? tmdbKey = _configuration["ApiKeys:Tmdb"] ?? throw new Exception("Cannot find internal API keys");
            string tmdbUrl = "https://api.themoviedb.org/3/movie/popular";
            tmdbUrl += $"?api_key={tmdbKey}&language={language}&page={page}&region={region}";

            using HttpClient httpClient = new HttpClient();

            HttpResponseMessage response = await httpClient.GetAsync(tmdbUrl);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var serializedJsonResponse = JsonConvert.DeserializeObject<TmdbMovieApiResponse>(jsonResponse);

            if (serializedJsonResponse?.Results == null) return new List<TmdbMovieResponse>();

            var recommendResponseDtoList = serializedJsonResponse.Results.Select(movie => new TmdbMovieResponse
            {
                Title = movie.Title,
                Description = movie.Overview,
                ReleaseDate = movie.Release_Date,
                ImageUrl = movie.Poster_Path,
                Genres = movie.Genre_Ids.Select(id => GetGenreName(id)).ToList()
            }).ToList();

            return recommendResponseDtoList;
        }

        public async Task<List<TmdbMovieResponse>> TopRated(string language, int page, string region)
        {
            // Get TMDB API key from configugration
            string? tmdbKey = _configuration["ApiKeys:Tmdb"] ?? throw new Exception("Cannot find internal API keys");
            string tmdbUrl = "https://api.themoviedb.org/3/movie/top_rated";
            tmdbUrl += $"?api_key={tmdbKey}&language={language}&page={page}&region={region}";

            using HttpClient httpClient = new HttpClient();

            HttpResponseMessage response = await httpClient.GetAsync(tmdbUrl);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var serializedJsonResponse = JsonConvert.DeserializeObject<TmdbMovieApiResponse>(jsonResponse);

            if (serializedJsonResponse?.Results == null) return new List<TmdbMovieResponse>();

            var recommendResponseDtoList = serializedJsonResponse.Results.Select(movie => new TmdbMovieResponse
            {
                Title = movie.Title,
                Description = movie.Overview,
                ReleaseDate = movie.Release_Date,
                ImageUrl = movie.Poster_Path,
                Genres = movie.Genre_Ids.Select(id => GetGenreName(id)).ToList()
            }).ToList();

            return recommendResponseDtoList;
        }

        public async Task<List<TmdbMovieResponse>> Upcoming(string language, int page, string region)
        {
            // Get TMDB API key from configugration
            string? tmdbKey = _configuration["ApiKeys:Tmdb"] ?? throw new Exception("Cannot find internal API keys");
            string tmdbUrl = "https://api.themoviedb.org/3/movie/upcoming";
            tmdbUrl += $"?api_key={tmdbKey}&language={language}&page={page}&region={region}";

            using HttpClient httpClient = new HttpClient();

            HttpResponseMessage response = await httpClient.GetAsync(tmdbUrl);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var serializedJsonResponse = JsonConvert.DeserializeObject<TmdbMovieApiResponse>(jsonResponse);

            if (serializedJsonResponse?.Results == null) return new List<TmdbMovieResponse>();

            var recommendResponseDtoList = serializedJsonResponse.Results.Select(movie => new TmdbMovieResponse
            {
                Title = movie.Title,
                Description = movie.Overview,
                ReleaseDate = movie.Release_Date,
                ImageUrl = movie.Poster_Path,
                Genres = movie.Genre_Ids.Select(id => GetGenreName(id)).ToList()
            }).ToList();

            return recommendResponseDtoList;
        }

        public async Task<List<TmdbTrendingResponse>> TrendingMovie(string timeWindow, string language)
        {
            // Get TMDB API key from configugration
            string? tmdbKey = _configuration["ApiKeys:Tmdb"] ?? throw new Exception("Cannot find internal API keys");
            string tmdbUrl = $"https://api.themoviedb.org/3/trending/movie/{timeWindow}";
            tmdbUrl += $"?api_key={tmdbKey}&language={language}";

            using HttpClient httpClient = new HttpClient();

            HttpResponseMessage response = await httpClient.GetAsync(tmdbUrl);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var serializedJsonResponse = JsonConvert.DeserializeObject<TmdbMovieApiResponse>(jsonResponse);

            if (serializedJsonResponse?.Results == null) return new List<TmdbTrendingResponse>();

            var recommendResponseDtoList = serializedJsonResponse.Results.Select(movie => new TmdbTrendingResponse
            {
                Title = movie.Title,
                Description = movie.Overview,
                ImageUrl = movie.Poster_Path,
                Genres = movie.Genre_Ids.Select(id => GetGenreName(id)).ToList()
            }).ToList();

            return recommendResponseDtoList;
        }

        public async Task<List<TmdbTrendingResponse>> TrendingTv(string timeWindow, string language)
        {
            // Get TMDB API key from configugration
            string? tmdbKey = _configuration["ApiKeys:Tmdb"] ?? throw new Exception("Cannot find internal API keys");
            string tmdbUrl = $"https://api.themoviedb.org/3/trending/tv/{timeWindow}";
            tmdbUrl += $"?api_key={tmdbKey}&language={language}";

            using HttpClient httpClient = new HttpClient();

            HttpResponseMessage response = await httpClient.GetAsync(tmdbUrl);
            response.EnsureSuccessStatusCode();
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var serializedJsonResponse = JsonConvert.DeserializeObject<TmdbTrendingApiResponse>(jsonResponse);

            if (serializedJsonResponse?.Results == null) return new List<TmdbTrendingResponse>();

            var recommendResponseDtoList = serializedJsonResponse.Results.Select(movie => new TmdbTrendingResponse
            {
                Title = movie.Name,
                Description = movie.Overview,
                ImageUrl = movie.Poster_Path,
                Genres = movie.Genre_Ids.Select(id => GetGenreName(id)).ToList()
            }).ToList();

            return recommendResponseDtoList;
        }

        // Helper methods
        private static string GetGenreName(int genreId)
        {
            // Updated data to include both movie and TV genres
            IDictionary<int, string> genreDict = new Dictionary<int, string>
            {
                { 28, "Action" },
                { 12, "Adventure" },
                { 16, "Animation" },
                { 35, "Comedy" },
                { 80, "Crime" },
                { 99, "Documentary" },
                { 18, "Drama" },
                { 10751, "Family" },
                { 14, "Fantasy" },
                { 36, "History" },
                { 27, "Horror" },
                { 10402, "Music" },
                { 9648, "Mystery" },
                { 10749, "Romance" },
                { 878, "Science Fiction" },
                { 10770, "TV Movie" },
                { 53, "Thriller" },
                { 10752, "War" },
                { 37, "Western" },
                { 10759, "Action & Adventure" },
                { 10762, "Kids" },
                { 10763, "News" },
                { 10764, "Reality" },
                { 10765, "Sci-Fi & Fantasy" },
                { 10766, "Soap" },
                { 10767, "Talk" },
                { 10768, "War & Politics" }
            };

            if (genreDict.TryGetValue(genreId, out string genreName))
            {
                return genreName;
            }
            return "Unknown category";
        }

    }



    // TMDB movie formats for MOVIE LISTS
    public class TmdbMovieApiResponse
    {
        public required List<TmdbMovie> Results { get; set; }
    }

    public class TmdbMovie
    {
        public required string Title { get; set; }
        public required string Overview { get; set; }
        public required string Release_Date { get; set; }
        public required string Poster_Path { get; set; }
        public required List<int> Genre_Ids { get; set; }
    }

    // TMDB trending formats for TRENDING
    public class TmdbTrendingApiResponse
    {
        public required List<TmdbTrending> Results { get; set; }
    }

    public class TmdbTrending
    {
        public required string Name { get; set; }
        public required string Overview { get; set; }
        public required string Release_Date { get; set; }
        public required string Poster_Path { get; set; }
        public required List<int> Genre_Ids { get; set; }
    }
}
