using System.Collections.Generic;
using System.Threading.Tasks;
using VATToolBoxApiApp.Models;

namespace VATToolBoxApiApp.Repository.Interfaces
{
    public interface IGetRatesRepository
    {
        Task<List<CountryRates>> GetReturnRates();
    }
}