using System.Collections.Generic;
using VATToolBoxApiApp.Models;

namespace VATToolBoxApiApp.Repository.Interfaces
{
    public interface IGetRatesRepository
    {
        List<CountryRates> GetReturnRates();
    }
}