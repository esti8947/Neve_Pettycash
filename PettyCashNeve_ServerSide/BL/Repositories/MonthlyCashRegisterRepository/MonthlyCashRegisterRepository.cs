using Microsoft.EntityFrameworkCore;
using PettyCashNeve_ServerSide.Exceptions;
using DAL.Models;
using DAL.Data;
using BL.Repositories.ExpenseCategoryRepository;
using BL.Repositories.ExpenseRepository;
using Entities.Models_Dto;
using BL.Repositories.UserRepository;


namespace PettyCashNeve_ServerSide.Repositories.MonthlyCashRegisterRepository
{
    public class MonthlyCashRegisterRepository : IMonthlyCashRegisterRepository
    {
        private readonly PettyCashNeveDbContext _context;
        private readonly IExpenseRepository _expenseRepository;
        private readonly IUserRepository _userRepository;
        public MonthlyCashRegisterRepository(PettyCashNeveDbContext context, IExpenseRepository expenseRepository, IUserRepository userRepository)
        {
            _context = context;
            _expenseRepository = expenseRepository;
            _userRepository = userRepository;
        }

        public async Task<MonthlyCashRegister> GetMonthlyCashRegisterByIdAsync(int monthlyCashRegisterId)
        {
            var monthlyCashRegister = await _context.MonthlyCashRegisters
               .FirstOrDefaultAsync(m => m.MonthlyCashRegisterId == monthlyCashRegisterId);

            if (monthlyCashRegister == null)
            {

                return null;
            }

            return monthlyCashRegister;
        }

        public async Task<MonthlyCashRegister> GetCurrentMonthlyCashRegistersByUserIdAsync(string userId)
        {
            try
            {
                var monthlyRegister = await _context.MonthlyCashRegisters.FirstOrDefaultAsync(m => m.UpdatedBy == userId && m.IsActive);
                if (monthlyRegister != null)
                {
                    decimal totalAmount = await _expenseRepository.GetExpensesAmountForMonth(monthlyRegister.MonthlyCashRegisterMonth, monthlyRegister.MonthlyCashRegisterYear, userId);
                    monthlyRegister.AmountInCashRegister = totalAmount;
                    _context.SaveChanges();
                }
                return monthlyRegister;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public async Task<bool> CreateNewMonthlyCashRegisterAsync(MonthlyCashRegister monthlyCashRegister)
        {
            try
            {
                var existingMonthlyRegister = await _context.MonthlyCashRegisters
                    .FirstOrDefaultAsync(m =>
                        m.UpdatedBy == monthlyCashRegister.UpdatedBy &&
                        m.MonthlyCashRegisterYear == monthlyCashRegister.MonthlyCashRegisterYear &&
                        m.MonthlyCashRegisterMonth == monthlyCashRegister.MonthlyCashRegisterMonth);
                if (existingMonthlyRegister != null)
                {
                    return false;
                }

                var existingMonthlyRegisters = await _context.MonthlyCashRegisters
                    .Where(m => m.UpdatedBy == monthlyCashRegister.UpdatedBy && m.IsActive)
                    .ToListAsync();

                foreach (var existingMonthlyReg in existingMonthlyRegisters)
                {
                    existingMonthlyReg.IsActive = false;
                }

                _context.MonthlyCashRegisters.Add(monthlyCashRegister);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> UpdateMonthlyCashRegisterAsync(MonthlyCashRegister updatedMonthlyCashRegister)
        {
            try
            {
                var existingMontlyCashRegister = await _context.MonthlyCashRegisters
                    .FirstOrDefaultAsync(m => m.MonthlyCashRegisterId == updatedMonthlyCashRegister.MonthlyCashRegisterId);

                if (existingMontlyCashRegister == null)
                {
                    throw new NotFoundException("MonthlyCashRegister not found or not active");
                }

                _context.Entry(existingMontlyCashRegister).CurrentValues.SetValues(updatedMonthlyCashRegister);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<bool> DeactivateMonthlyCashRegister(string userId)
        {
            try
            {
                // Get the current monthly cash register
                var currentMonthlyCashRegister = await _context.MonthlyCashRegisters.FirstOrDefaultAsync(m => m.UpdatedBy == userId);

                if (currentMonthlyCashRegister != null)
                {
                    // Deactivate the current monthly cash register
                    currentMonthlyCashRegister.IsActive = false;

                    // Save changes to the database
                    await _context.SaveChangesAsync();
                    return true;
                }

                return false; // Or throw an exception if needed
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> InsertRefundAmount(decimal refunatedAmount, string userId)
        {
            try
            {
                var currentMonthlyCashRegister = await _context.MonthlyCashRegisters.FirstOrDefaultAsync(m => m.UpdatedBy == userId && m.IsActive);
                if (currentMonthlyCashRegister != null)
                {
                    currentMonthlyCashRegister.RefundAmount += refunatedAmount;
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {

                throw;
            }

        }

        public async Task<List<MonthlyCashRegister>> GetMonthlyCashRegisterByDepartmenId(int departmentId)
        {
            try
            {
                var usersList = await _userRepository.GetUsersOfDepartment(departmentId);

                var monthlyRegistersList = new List<MonthlyCashRegister>();

                foreach (var user in usersList)
                {
                    var monthlyRegisters = GetCurrentMonthlyCashRegistersByUserIdAsync(user.Id).Result;
                    if (monthlyRegisters != null)
                        monthlyRegistersList.Add(monthlyRegisters);
                }
                return monthlyRegistersList;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> CheckAllMonthlyCashRegistersInactiveForYearAsync(int departmentId, int academicYear)
        {
            try
            {
                var startYear = academicYear / 10000;
                var endYear = academicYear % 10000;

                var monthlyRegisterForDepartment = GetMonthlyCashRegisterByDepartmenId(departmentId).Result;
                var activeMonthlyCashRegister = monthlyRegisterForDepartment.FirstOrDefault(mcr => (mcr.MonthlyCashRegisterYear == startYear && mcr.MonthlyCashRegisterMonth >= 10) || (mcr.MonthlyCashRegisterYear == endYear && mcr.MonthlyCashRegisterMonth < 10) && mcr.IsActive);  
                
                if(activeMonthlyCashRegister != null)
                {
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
