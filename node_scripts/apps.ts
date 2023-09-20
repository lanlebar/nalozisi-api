const TorrentSearchApi = require('torrent-search-api');
const providers = ['All', 'ThePirateBay', '1337x', 'Yts'];
const allProviders = TorrentSearchApi.getProviders();

// Define your search parameters
const searchQuery = process.argv[2] || undefined;
const category = process.argv[3] || 'All';
const source = process.argv[4] || 'All';
const resultsLimit = parseInt(process.argv[5]) || 10;

(async () => {
  const megalol = await TorrentSearchApi.search(['ThePirateBay'], 'fast', 'All', 1);
  console.log(megalol);
})();



(async () => {
  // try {
    if (!searchQuery || typeof searchQuery !== 'string') return console.log('ERR');
    if (!category || typeof category !== 'string') return console.log('ERR');
    if (!providers.includes(source)) return console.log('ERR source');
    if (!resultsLimit || typeof resultsLimit !== 'number') return console.log('ERR');

    let torrents = [];

    if (source === 'All') {
      for (const provider of providers) {
        if (provider === 'All') continue; // Skip 'All' provider
        if (!categoryExists(provider, category)) {
          console.log(`Category "${category}" does not exist in provider "${provider}". Skipping.`);
          continue;
        }
        torrents = await searchProvider(provider, searchQuery, category, resultsLimit, torrents);
      }
    } else {
      console.log('ELSE');
      if (providerExists(source) && source !== 'All') {
        console.log('proivder exists');
        if (!categoryExists(source, category)) {
          console.log(`Category "${category}" does not exist in provider "${source}". Skipping.`);
        } else {
          console.log('category exists and searching occurs');
          console.log(source);
          console.log(searchQuery);
          console.log(category);
          console.log(resultsLimit);
          console.log(torrents);
          torrents = await searchProvider(source, searchQuery, category, resultsLimit);
        }
      }
    }

    console.log(JSON.stringify(torrents, null, 2));
  // } catch (error) {
  //   console.log('ERR catch');
  // }
})();

async function searchProvider(provider, searchQuery, category, resultsLimit) {
  let torrents = [];
  TorrentSearchApi.enableProvider(provider);

  console.log('provider enabled');
  const foundTorrents = await TorrentSearchApi.search([provider], searchQuery, 'All', resultsLimit);
  // TorrentSearchApi.search([provider], searchQuery, 'All', resultsLimit);
  console.log('foundTorrents');

  // Format and add to the torrents array
  let formattedTorrents = foundTorrents.map(torrent => ({
    provider: torrent.provider,
    title: torrent.title,
    time: torrent.time,
    size: torrent.size,
    magnet: torrent.magnet || torrent.link || '',
    seeds: torrent.seeds,
    peers: torrent.peers,
    imdb: torrent.imdb !== '' ? torrent.imdb : '',
    pageUrl: torrent.desc || '',
  }));

  console.log('formattedTorrents');
  torrents.push({ provider: provider, results: formattedTorrents });
  console.log('torrents hereee');
  console.log(torrents);

  TorrentSearchApi.disableProvider(provider);

  return torrents;
}

function providerExists(providerName) {
  return allProviders.some(provider => provider.name === providerName);
}

function categoryExists(providerName, categoryName) {
  const provider = allProviders.find(provider => provider.name === providerName);
  return provider && provider.categories.includes(categoryName);
}
