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
const searchQuery = process.argv[2] || '';
var globalCategory = process.argv[3] || 'All';
var source = process.argv[4];
const resultsLimit = parseInt(process.argv[5]);

(async () => {
  // try {

    if (!searchQuery || typeof searchQuery !== 'string') return console.log('ERR');
    if (!globalCategory || typeof globalCategory !== 'string') return console.log('ERR c');
    if (!providers.includes(source)) return console.log('ERR source');
    if (!resultsLimit || typeof resultsLimit !== 'number') return console.log('ERR');

    let globalTorrents = [];

    if (source === 'All') {
      for (const provider of providers) {
        // Skip 'All' provider
        if (provider === 'All') continue;

        // TODO Check if category exists in the given provider
        if (!categoryExists(provider, 'All')) {
          console.log('ERR');
          continue;
        }

        // Search
        globalTorrents = await search(provider, searchQuery, globalCategory, resultsLimit, globalTorrents);
      }
    } else {
      if (providerExists(source) && source !== 'All') {
        // Check if category exists in the given provider
        if (!categoryExists(source, 'All')) return console.log('ERR');

        // Search
        globalTorrents = await search(source, searchQuery, globalCategory, resultsLimit, globalTorrents);
      }
    }

    console.log(JSON.stringify(globalTorrents, null, 2));


  // } catch (error) {
  //   console.log('ERR dol');
  // }

})();

async function search(provider, searchQuery, globalCategory, resultsLimit, globalTorrents) {
  const foundTorrents = await TorrentSearchApi.search([provider], searchQuery, globalCategory, resultsLimit);
  let formattedTorrents = foundTorrents.map(torrent => ({
    provider: torrent.provider,
    title: torrent.title,
    time: torrent.time,
    size: torrent.size,
    url: torrent.magnet,
    seeds: torrent.seeds,
    peers: torrent.peers,
    imdb: torrent.imdb !== "" ? torrent.imdb : ""
  }));

  globalTorrents.push({ provider: provider, results: formattedTorrents });
  return globalTorrents;
}

async function providerExists(providerName) {
  const allProviders = await TorrentSearchApi.getProviders();
  return allProviders.some(provider => provider.name === providerName);
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