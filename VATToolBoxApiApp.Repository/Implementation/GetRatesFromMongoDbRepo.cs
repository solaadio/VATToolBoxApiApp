using System.Collections.Generic;
using System.Security.Authentication;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using VATToolBoxApiApp.Models;
using VATToolBoxApiApp.Repository.Interfaces;

namespace VATToolBoxApiApp.Repository.Implementation
{
    public class GetRatesFromMongoDbRepo : IGetRatesRepository
    {
        public  async Task<List<CountryRates>> GetReturnRates()
        {


            // const string connectionString = @"mongodb://vatdb:xEsrRuBN5duupDWV6L0f3tTGPKSmNvOcepkNqnGAGypWOikSYbOKlKyGMus2TLxW4poosuDBlUMr15w86qUA6Q==@vatdb.documents.azure.com:10255/?ssl=true&replicaSet=globaldb";
            const string connectionString =
                @"mongodb://logiondb:GdGcQ0ZDoV3KuW3wmfdJwAt1zwiKW0D3WLxwVOwXAvzrgKrNQZ8qDIDMUBSBTiRjzzLz4jgoKqYxhZ5m51bx2Q==@logiondb.documents.azure.com:10250/?ssl=true&sslverifycertificate=false";

            var settings = MongoClientSettings.FromUrl(
              new MongoUrl(connectionString)
            );
            settings.SslSettings =
              new SslSettings { EnabledSslProtocols = SslProtocols.Tls12 };
            var mongoClient = new MongoClient(settings);

            var db = mongoClient.GetDatabase("vat");
            
            var col = db.GetCollection<RatesBson>("rates");

            //            var res = await col.Find(new BsonDocument("version",3.51)).SortByDescending(x => x.Version).Limit(1).FirstAsync();
            var res = await col.Find(new BsonDocument("version", 3.51)).FirstAsync();

            return res.Rates;
        }
    }
}