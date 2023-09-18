using MonoTorrent;
using API.DTOs.Torrent;
using API.Services.UserService;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Diagnostics;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using Microsoft.Extensions.Configuration;

namespace API.Services.TorrentService
{
    public class TorrentService : ITorrentService
    {
        // Fields
        private readonly DataContext _context;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;

        public TorrentService(DataContext context, IUserService userService, IConfiguration configuration)
        {
            _context = context;
            _userService = userService;
            _configuration = configuration;
            _configuration = configuration;
        }

        // Methods
        public async Task<string> GetScrapedTorrentsAsync(
            [Required] string searchQuery,
            [Required] Enums.TorrentCategory category,
            [Required] int limit
        )
        {
            // Input validation
            if (searchQuery == null || limit == 0)
            {
                throw new Exception("Search query, category and limit are required!");
            }

            // Get scraped torrents from Node.js project
            if (_configuration["NodeScripts:NodePath"] == null || _configuration["NodeScripts:ScriptsPath"] == null)
            {
                throw new Exception("Cannot find internal script paths!");
            }
            string? nodePath = _configuration["NodeScripts:NodePath"];
            string? scriptPath = _configuration["NodeScripts:ScriptsPath"];
            string args = string.Format("{0} {1} {2} {3}", "app.js", searchQuery, category, limit);
            if (nodePath == null || scriptPath == null)
            {
                throw new Exception("Cannot find internal script paths!");
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
                return output;
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

        public async Task<Models.Main.Torrent> UploadTorrentAsync(UploadTorrentDto request)
        {
            return null;
            // Input validation
            if (request.Title == null || request.TorrentFile == null || request.TorrentCategory == null)
            {
                throw new ArgumentException("Naslov, torrent datoteka in kategorija so obvezni");
            }

            // Check if user exists
            if (!await _userService.UserExists(request.UserId))
            {
                throw new ConflictExceptionDto("Uporabnik ne obstaja!");
            }

            // Check if file is .torrent
            if (!request.TorrentFile.FileName.EndsWith(".torrent") || request.TorrentFile == null || request.TorrentFile.Length == 0)
            {
                throw new ArgumentException("Napaka pri nalaganju datoteke! Naloži velajvno .torrent datoteko");
            }

            // Check if user has right to upload
            if (!await _userService.CanUpload(request.UserId))
            {
                throw new ConflictExceptionDto("Uporabnik nima pravic za nalaganje torrent datotek!");
            }

            // Check if torrent category exists
            if (!await _context.TorrentCategory.AnyAsync(tc => tc.CategoryName == request.TorrentCategory))
            {
                throw new ConflictExceptionDto("Kategorija ne obstaja!");
            }

            // Check if all torrent tags exists (if there are any)
            if (request.TorrentTags != null && request.TorrentTags.Any())
            {
                // Tags are present
                foreach (var tag in request.TorrentTags)
                {
                    if (!await _context.TorrentTag.AnyAsync(tt => tt.TagValue == tag))
                    {
                        throw new ConflictExceptionDto("Torrent tag ne obstaja!");
                    }
                }
            }

            // Check if torrent already exists - Torrents can have the same title, but not by the same uploader

            // Check if torrent with the same GUID already exists in file storage and database

            // Read the torrent file
            string magnetLink = "";
            try
            {
                using (var torrentStram = request.TorrentFile.OpenReadStream())
                {
                    MonoTorrent.Torrent torrent = MonoTorrent.Torrent.Load(torrentStram);

                    // Get magnet link from torrent file
                    string infoHash = torrent.InfoHash.ToString() ?? throw new Exception("Napaka pri branju torrent datoteke!"); ;
                    magnetLink = string.Format("magnet:?xt=urn:btih:{0}", infoHash);
                    

                    
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }


            // Create torrent
            Guid torrentGuid = Guid.NewGuid();
            var newTorrent = new Models.Main.Torrent
            {
                TorrentGuid = torrentGuid,
                Title = request.Title,
                TorrentFilePath = string.Format("/torrent/{0}.torrent", torrentGuid.ToString()),
                DescriptionFilePath = string.Format("/torrent/{0}.torrent", torrentGuid.ToString()),
                ImageFilePath = string.Format("/img/{0}.torrent", torrentGuid.ToString()),
                SizeBytes = request.TorrentFile.Length,
                UploadedDate = DateTime.Now,
                MagnetLink = magnetLink,
                UserId = request.UserId,
                TorrentCategoryId = await _context.TorrentCategory.FirstOrDefaultAsync(tc => tc.CategoryName == request.TorrentCategory).ContinueWith(tc => tc.Result.TorrentCategoryId),
                DownloadAmount = 0,
                LikeAmount = 0
            };

            // Add torrent to database
            await _context.Torrent.AddAsync(newTorrent);

            // Relate torrent to user


        }
        
        public async Task<Models.Main.Torrent> CreateTorrentAsync(UploadTorrentDto addTorrentDto)
        {
            // TODO
            throw new NotImplementedException();
        }

        public async Task<Models.Main.Torrent> GetTorrentByIdAsync(int torrentId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Models.Main.Torrent>> GetTorrentByQueryAsync(string searchQuery)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Models.Main.Torrent>> GetTorrentByCategoryAsync(int torrentCategoryId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Models.Main.Torrent>> GetTorrentByTagAsync(int torrentTagId)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Models.Main.Torrent>> GetAllTorrentsAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<List<Models.Main.Torrent>> UpdateTorrentAsync(UpdateTorrentDto request)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Models.Main.Torrent>> DeleteTorrentAsync(int torrentId)
        {
            throw new NotImplementedException();
        }
    }
}