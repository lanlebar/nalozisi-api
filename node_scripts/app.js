const TorrentSearchApi = require('torrent-search-api');
const providers = ['All', 'ThePirateBay', '1337x', 'Yts'];
const allProviders = TorrentSearchApi.getProviders();

// Enable providers
TorrentSearchApi.enableProvider('ThePirateBay');
TorrentSearchApi.enableProvider('1337x');
TorrentSearchApi.enableProvider('Yts');

// // Define your search parameters
const searchQuery = process.argv[2] || undefined;
let category = process.argv[3] || 'All';
let source = process.argv[4];
const resultsLimit = parseInt(process.argv[5]);

(async () => {
  // try {

    if (!searchQuery || typeof searchQuery !== 'string') return console.log('ERR');
    if (!category || typeof category !== 'string') return console.log('ERR c');
    if (!providers.includes(source)) return console.log('ERR s');
    if (!resultsLimit || typeof resultsLimit !== 'number') return console.log('ERR');

    let globalTorrents = [];


    if (source === 'All') {
      for (const provider of providers) {
        console.log(provider)
        // Skip 'All' provider
        if (provider === 'All') continue;

        // Check if category exists in the given provider
        if (!categoryExists(provider, 'All')) {
          console.log('ERR cat');
          continue
        };

        // Search
        globalTorrents = await search(provider, searchQuery, category, resultsLimit, globalTorrents);
      }
    } else {
      console.log("specific provider")
      if (providerExists(source) && source !== 'All') {
        // Check if category exists in the given provider
        if (!categoryExists(source, 'All')) return console.log('ERR');

        // Search
        globalTorrents = await search(source, searchQuery, category, resultsLimit, globalTorrents);
      }
    }

    console.log(JSON.stringify(globalTorrents, null, 2));


  // } catch (error) {
  //   console.log('ERR dol');
  // }

})();

async function search(provider, searchQuery, category, resultsLimit, globalTorrents) {
  const foundTorrents = await TorrentSearchApi.search([provider], searchQuery, 'All', resultsLimit);
  console.log("\n\n\nfoundTorrents");
  // console.log("\n\n\nfoundTorrents");
  // console.log(foundTorrents);
  // console.log("\n\n\n");
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

function providerExists(providerName) {
  return allProviders.some(provider => provider.name === providerName);
}

function categoryExists(providerName, categoryName) {
  return true;
  allProviders.forEach(provider => {
    if (provider.name === providerName) {

      if (!provider.categories.includes(categoryName)) {
        return false;
      }
    }
  });
  return true;
}