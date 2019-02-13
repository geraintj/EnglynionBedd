using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnglynionBedd.Endidau;
using EnglynionBedd.Gwasanaethau.Configuration;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace EnglynionBedd.Gwasanaethau
{
    public class CronfaEnglynion : ICronfaEnglynion
    {
        private readonly IOptions<Gosodiadau> _gosodiadau;
        private readonly IOptions<GosodiadauAllweddgell> _gosodiadauAllweddgell;
        private readonly DocumentClient _client;

        public CronfaEnglynion(IOptions<Gosodiadau> gosodiadau, IOptions<GosodiadauAllweddgell> gosodiadauAllweddgell)
        {
            _gosodiadau = gosodiadau;
            _gosodiadauAllweddgell = gosodiadauAllweddgell;
            _client = new DocumentClient(new Uri(_gosodiadau.Value.CyfeiriadBasDdata), _gosodiadauAllweddgell.Value.BasDdataCosmos.ToString());
        }

        public async Task<Englyn> ArbedEnglyn(Englyn englyn)
        {
            var canlyniad =
                await _client.CreateDocumentAsync(
                    UriFactory.CreateDocumentCollectionUri(_gosodiadau.Value.EnwBasDdata, _gosodiadau.Value.Casgliad),
                    englyn);
            return (Englyn)canlyniad.Resource;
        }

        public async Task<Englyn> AdalwEnglyn(string id)
        {
            var opsiynau = new FeedOptions() {EnableCrossPartitionQuery = true};
            IDocumentQuery<Englyn> query = _client.CreateDocumentQuery<Englyn>(
                    UriFactory.CreateDocumentCollectionUri(_gosodiadau.Value.EnwBasDdata, _gosodiadau.Value.Casgliad), opsiynau)
                .Where(d => d.Id == id)
                .AsDocumentQuery();

            var result = await query.ExecuteNextAsync<Englyn>();

            return result.FirstOrDefault();
        }

        public async Task<List<Englyn>> AdalwEnglynion()
        {
            IDocumentQuery<Englyn> query = _client.CreateDocumentQuery<Englyn>(
                    UriFactory.CreateDocumentCollectionUri(_gosodiadau.Value.EnwBasDdata, _gosodiadau.Value.Casgliad))
                .AsDocumentQuery();

            var results = new List<Englyn>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<Englyn>());
            }

            return results;
        }

        public async Task<string> ArbedDelwedd(byte[] delwedd)
        {
            CloudStorageAccount storageAccount = null;
            CloudStorageAccount.TryParse(string.Format(_gosodiadau.Value.LlinynCysylltuStorfa, _gosodiadauAllweddgell.Value.CyfrifStorfa), out storageAccount);
            CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(_gosodiadau.Value.EnwAmlwyth);
            var enwBlob = Guid.NewGuid().ToString();
            CloudBlockBlob blob = cloudBlobContainer.GetBlockBlobReference(enwBlob);
            await blob.UploadFromByteArrayAsync(delwedd, 0, delwedd.Length);
            return blob.Uri.AbsoluteUri;
        }
    }
}