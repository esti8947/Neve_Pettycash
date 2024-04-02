using DAL.Models;
using Entities.Models_Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories.BuyerRepository
{
    public interface IBuyerRepository
    {
        Task<Buyer> GetBuyeByIdrAsync(int id);
        Task<List<Buyer>> GetBuyersAsync();
        Task<bool> CreateBuyerAsync(Buyer buyer);
        Task<bool> DeleteBuyerAsync(int buyerId);
        Task<string> GetBuyerNameByIdAsync(int id);
        Task<string> GetBuyerNameHebByIdAsync(int id);


    }
}
