const TorrentSearchApi = require('torrent-search-api');
const providers = ['ThePirateBay', '1337x', 'Yts'];

// // Define your search parameters
const searchQuery = process.argv[2] || undefined;
const category = process.argv[3] || 'All';
const resultsLimit = parseInt(process.argv[4]) || 10;

(async () => {
  try {
    if (!searchQuery || typeof searchQuery !== 'string') return console.log('ERR');
    if (!category || typeof category !== 'string') return console.log('ERR');
    if (!resultsLimit || typeof resultsLimit !== 'number') return console.log('ERR');

    let torrents = [];

    // Loop through all the available providers
    for (const provider of providers) {
      TorrentSearchApi.enableProvider(provider);

      if (provider === 'ThePirateBay') {
        const foundTorrents = await TorrentSearchApi.search([provider], searchQuery, 'All', resultsLimit);

        // Format
        let formattedTorrents = [];
        foundTorrents.forEach(torrent => {
          formattedTorrents.push({
            provider: torrent.provider,
            title: torrent.title,
            time: torrent.time,
            size: torrent.size,
            magnet: torrent.magnet,
            seeds: torrent.seeds,
            peers: torrent.peers,
            imdb: torrent.imdb !== "" ? torrent.imdb : undefined
          });
        });
        torrents.push({ provider: provider, results: formattedTorrents });
      }
      if (provider === '1337x') {
        const foundTorrents = await TorrentSearchApi.search([provider], searchQuery, category, resultsLimit);

        // Format
        let formattedTorrents = [];
        foundTorrents.forEach(torrent => {
          formattedTorrents.push({
            provider: torrent.provider,
            title: torrent.title,
            time: torrent.time,
            size: torrent.size,
            pageUrl: torrent.desc,
            seeds: torrent.seeds,
            peers: torrent.peers
          });
        });
        torrents.push({ provider: provider, results: formattedTorrents });
      }
      if (provider === 'Yts') {
        const foundTorrents = await TorrentSearchApi.search([provider], searchQuery, category, resultsLimit);

        // Format
        let formattedTorrents = [];
        foundTorrents.forEach(torrent => {
          formattedTorrents.push({
            provider: torrent.provider,
            title: torrent.title,
            time: torrent.time,
            size: torrent.size,
            magnet: torrent.link,
            seeds: torrent.seeds,
            peers: torrent.peers
          });
        });
        torrents.push({ provider: provider, results: formattedTorrents });
      }

      TorrentSearchApi.disableProvider(provider);
    }
    console.log(JSON.stringify(torrents, null, 2));
  } catch (error) {

    console.log('ERR dol');
  }
})();