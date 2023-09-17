const TorrentSearchApi = require('torrent-search-api');

// Search for torrents
(async () => {
  try {
    TorrentSearchApi.enableProvider('ThePirateBay');
    const torrents = await TorrentSearchApi.search(['ThePirateBay'], '1080');
    console.log(torrents);
  } catch (error) {
    console.error('Error:', error);
  }
})();
