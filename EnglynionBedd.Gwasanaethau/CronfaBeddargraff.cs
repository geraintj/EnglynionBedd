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
    public class CronfaBeddargraff : ICronfaBeddargraff
    {
        private readonly IOptions<Gosodiadau> _gosodiadau;
        private readonly DocumentClient _client;

        public CronfaBeddargraff(IOptions<Gosodiadau> gosodiadau)
        {
            _gosodiadau = gosodiadau;
            _client = new DocumentClient(new Uri(_gosodiadau.Value.CyfeiriadBasDdata),
                _gosodiadau.Value.AllweddBasDdata);
        }

        public async Task<string> ArbedBeddargraff(Beddargraff beddargraff)
        {
            var canlyniad =
                await _client.CreateDocumentAsync(
                    UriFactory.CreateDocumentCollectionUri(_gosodiadau.Value.EnwBasDdata, _gosodiadau.Value.Casgliad),
                    beddargraff);
            return canlyniad.Resource.Id;
        }

        public async Task<Beddargraff> AdalwBeddargraff(string id)
        {
            IDocumentQuery<Beddargraff> query = _client.CreateDocumentQuery<Beddargraff>(
                    UriFactory.CreateDocumentCollectionUri(_gosodiadau.Value.EnwBasDdata, _gosodiadau.Value.Casgliad))
                .Where(d => d.Id == id)
                .AsDocumentQuery();

            var result = await query.ExecuteNextAsync<Beddargraff>();
            
            return result.FirstOrDefault();
        }

        public async Task<List<Beddargraff>> AdalwBeddargraffiadau()
        {
            IDocumentQuery<Beddargraff> query = _client.CreateDocumentQuery<Beddargraff>(
                    UriFactory.CreateDocumentCollectionUri(_gosodiadau.Value.EnwBasDdata, _gosodiadau.Value.Casgliad))
                .AsDocumentQuery();

            var results = new List<Beddargraff>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<Beddargraff>());
            }

            return results;
        }

        public async Task<string> ArbedDelwedd(byte[] delwedd)
        {
            CloudStorageAccount storageAccount = null;
            CloudStorageAccount.TryParse(_gosodiadau.Value.LlinynCysylltuStorfa, out storageAccount);
            CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudBlobContainer = cloudBlobClient.GetContainerReference(_gosodiadau.Value.EnwAmlwyth);
            var enwBlob = Guid.NewGuid().ToString();
            CloudBlockBlob blob = cloudBlobContainer.GetBlockBlobReference(enwBlob);
            await blob.UploadFromByteArrayAsync(delwedd, 0, delwedd.Length);
            return blob.Uri.AbsoluteUri;
        }
    }
}