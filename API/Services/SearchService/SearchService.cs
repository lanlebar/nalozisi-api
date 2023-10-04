using API.DTOs.Search;
using API.DTOs.TorrentScrape;
using Newtonsoft.Json;
using System.Diagnostics;

namespace API.Services.SearchService
{
    public class SearchService : ISearchService
    {
        // Fields
        private readonly IConfiguration _configuration;

        // Constructor
        public SearchService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // Methods
        public async Task<ScrapedTorrentsResponseDto> GetScrapedTorrentsAsync(SearchRequestDto request)
        {
            // Input validation
            if (request.Query == null)
            {
                throw new Exception("Search query parameter is required!");
            }

            // Get scraped torrents from Node.js project
            if (_configuration["NodeScripts:NodePath"] == null || _configuration["NodeScripts:ScriptsPath"] == null)
            {
                throw new Exception("Cannot find internal script paths!");
            }

            string? constLimit = _configuration["InternalApiSettings:BaseScrapeSearchLimit"];
            string? nodePath = _configuration["NodeScripts:NodePath"];
            string? scriptPath = _configuration["NodeScripts:ScriptsPath"];
            string args = $"\"{request.Query}\" \"{request.Category}\" \"{request.Source}\" \"{constLimit}\"";
            if (nodePath == null || scriptPath == null)
            {
                throw new Exception("Cannot find internal script paths");
            }

            // Start a new process for running Node.js
            ProcessStartInfo startInfo = new ProcessStartInfo(nodePath, scriptPath + " " + args);
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;

            // Create new process
            Process process = new Process();
            process.StartInfo = startInfo;

            try
            {
                process.Start();
                string output = process.StandardOutput.ReadToEnd();
                await process.WaitForExitAsync();

                // Check if scraping was successful
                if (output != null && output.ToUpper().Contains("ERR"))
                {
                    throw new Exception("Error scraping torrents!");
                }

                // Deserialize output with any type
                var jsonResponse = JsonConvert.DeserializeObject<ScrapedTorrentsResponseDto>(output);

                // Check if there are any torrents
                if (
                    output == "[]" ||
                    output == null ||
                    jsonResponse.ThePirateBay.Count == 0 && jsonResponse._1337x.Count == 0 && jsonResponse.Yts.Count == 0
                )
                {
                    throw new NotFoundExceptionDto("No torrents found!");
                }

                return jsonResponse;
            }
            catch (NotFoundExceptionDto)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
            finally
            {
                process.Close();
            }
        }
    }
}
