const TorrentSearchApi = require('torrent-search-api');
const providers = ['All', 'ThePirateBay', '1337x', 'Yts'];
const providerCateogries = {
  "ThePirateBay": [
    "All",
    "Audio",
    "Video",
    "Applications",
    "Games",
    "Porn",
    "Other",
    "Top100"
  ],
  "1337x": [
    "All",
    "Movies",
    "TV",
    "Games",
    "Music",
    "Anime",
    "Applications",
    "Documentaries",
    "Xxx",
    "Other",
    "Top100"
  ],
  "Yts": [
    "All",
    "Movies"
  ]
};
const supportedCategories = ['All', 'Movies', 'Series', 'Cartoons', 'XXX', 'Programs', 'Other'];

// Enable supported providers
TorrentSearchApi.enableProvider('ThePirateBay');
TorrentSearchApi.enableProvider('1337x');
TorrentSearchApi.enableProvider('Yts');

// // Define your search parameters
const searchQuery = process.argv[3] || '';
var globalCategory = process.argv[4] || 'All';
var source = process.argv[5];
const resultsLimit = process.argv[6];

(async () => {
  try {
    if (!searchQuery || typeof searchQuery !== 'string') return console.log('ERR-query');
    if (!globalCategory || typeof globalCategory !== 'string') return console.log('ERR-category');
    if (!providers.includes(source)) return console.log('ERR-provider');
    if (!resultsLimit) return console.log('ERR-limit');

    let globalTorrents = {};

    if (source === 'All') {
      for (const provider of providers) {
        // Skip 'All' provider
        if (provider === 'All') continue;

        // TODO Check if category exists in the given provider
        if (!categoryExists(provider, 'All')) {
          console.log('ERR-categoryExists');
          continue;
        }

        // Search
        globalTorrents = await search(provider, searchQuery, globalCategory, resultsLimit, globalTorrents);
      }
    } else {
      if (providerExists(source) && source !== 'All') {
        // Check if category exists in the given provider
        if (!categoryExists(source, 'All')) return console.log('ERR-categoryExists');

        // Search
        globalTorrents = await search(source, searchQuery, globalCategory, resultsLimit, globalTorrents);
      }
    }

    console.log(JSON.stringify(globalTorrents));


  } catch (error) {
    console.log('ERR-unknown');
  }

})();

async function search(provider, searchQuery, globalCategory, resultsLimit, globalTorrents) {
  const foundTorrents = await TorrentSearchApi.search([provider], searchQuery, globalCategory, resultsLimit);

  if (provider === 'ThePirateBay') {
    // Check if any torrents were found
    const noResultsTorrent = foundTorrents.find(torrent => torrent.title === 'No results returned');
    if (noResultsTorrent) {
      globalTorrents[provider] = [];
      return globalTorrents;
    }
  }

  let formattedTorrents = foundTorrents.map(torrent => ({
    provider: torrent.provider,
    title: torrent.title,
    time: torrent.time,
    size: torrent.size,
    url: getMagnetLink(provider, torrent),
    seeds: torrent.seeds,
    peers: torrent.peers,
    imdb: getImdb(torrent)
  }));

  globalTorrents[provider] = formattedTorrents;
  return globalTorrents;
}

function providerExists(providerName) {
  const allProviders = TorrentSearchApi.getProviders();
  return allProviders.some(provider => provider.name === providerName);
}

function getMagnetLink(providerName, torrent) {
  if (providerName === 'ThePirateBay') return torrent.magnet ||'';
  else if (providerName === '1337x') return torrent.desc ||'';
  else if (providerName === 'Yts') return torrent.link ||'';
  else return '';
}

function getImdb(torrent) {
  // TODO - IMDB API call
  if (torrent.imdb) return torrent.imdb
  else return '';
}

function categoryExists(providerName, categoryName) {
  // Not supported yet
  globalCategory = 'All';
  return true;

  /*
  // Check if given category is supported
  if (!supportedCategories.includes(categoryName)) {
    return false;
  }

  // // Check if given category exists in the given provider
  // if (!providerCateogries[providerName].includes(categoryName)) {
  //   return false;
  // }
  
  // Specific provider category checking
  if (source === 'ThePirateBay') {
    if (categoryName === 'Movies' || categoryName === 'Series' || categoryName === 'Cartoons' || categoryName === 'XXX') {
      globalCategory = 'Video';
    }
  }
  else if (source === '1337x') {

  } 
  else if (source === 'Yts') {

  }
    
  return true;
  */
}