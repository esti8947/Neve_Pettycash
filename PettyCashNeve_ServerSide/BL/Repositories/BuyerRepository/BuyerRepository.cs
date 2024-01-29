// BuyerRepository.cs
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DAL.Models;
using PettyCashNeve_ServerSide.Exceptions;
using BL.Repositories.BuyerRepository;
using DAL.Data;

namespace PettyCashNeve_ServerSide.Repositories
{
    public class BuyerRepository : IBuyerRepository
    {
        private readonly PettyCashNeveDbContext _context;

        public BuyerRepository(PettyCashNeveDbContext context)
        {
            _context = context;
        }

        public async Task<List<Buyer>> GetBuyersAsync()
        {
            try
            {
                return await _context.Buyers.ToListAsync();
            }
            catch (Exception ex)
            {
                // Log or handle the exception if needed
                throw;
            }
        }

        public async Task<bool> CreateBuyerAsync(Buyer buyer)
        {
            try
            {
                _context.Buyers.Add(buyer);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log or handle the exception if needed
                throw;
            }
        }

        public async Task<bool> DeleteBuyerAsync(int buyerId)
        {
            try
            {
                var buyer = await _context.Buyers.FirstOrDefaultAsync(b => b.BuyerId == buyerId);
                if (buyer == null)
                {
                    throw new NotFoundException("Buyer not found");
                }

                _context.Buyers.Remove(buyer);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log or handle the exception if needed
                throw;
            }
        }

        public async Task<Buyer> GetBuyeByIdrAsync(int id)
        {
            try
            {
                var buyer = await _context.Buyers.FirstOrDefaultAsync(b => b.BuyerId == id);
                return buyer;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> GetBuyerNameByIdAsync(int id)
        {
            try
            {
                var buyerName = await _context.Buyers
                    .Where(b => b.BuyerId == id)
                    .Select(b => b.BuyerName)
                    .FirstOrDefaultAsync();

                if (buyerName == null)
                {
                    return null;
                }

                return buyerName;
            }
            catch (Exception ex)
            {
                // Log or handle the exception if needed
                throw;
            }
        }
    }
}
