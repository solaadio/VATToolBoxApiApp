using System.Collections.Generic;
using MongoDB.Bson;
using VATToolBoxApiApp.Models;

namespace VATToolBoxApiApp.Repository.Implementation
{
    public class RatesBson
    {
        public ObjectId Id { get; set; }

        public List<CountryRates> Rates { get; set; } 

        public double Version { get; set; }
    }
}