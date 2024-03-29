﻿using DAL.Models;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using PettyCashNeve_ServerSide.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Repositories.MonthlyBudgetRepository
{
    public class MonthlyBudgetRepository: IMonthlyBudgetRepository
    {
        private readonly PettyCashNeveDbContext _context;
        public MonthlyBudgetRepository(PettyCashNeveDbContext context)
        {
            _context = context;
        }
        public async Task<MonthlyBudget> CreateMonthlyBudgetAsync(MonthlyBudget monthlyBudget)
        {

            try
            {
                var departmentId  = monthlyBudget.DepartmentId;

                bool isDuplicate = await checkDuplicateMonthlyBudgetByDate(monthlyBudget);

                if (!isDuplicate)
                {
                    throw new Exception("Cannot create monthly budget for the same month and year.");
                }

                var activeNonthlyBudget = await _context.MonthlyBudgets
                    .FirstOrDefaultAsync(ab => ab.DepartmentId == departmentId && ab.IsActive == true);
                if(activeNonthlyBudget != null)
                {
                    activeNonthlyBudget.IsActive = false;
                }


                _context.MonthlyBudgets.Add(monthlyBudget);
                await _context.SaveChangesAsync();
                return monthlyBudget;
            }
            catch (Exception ex)
            {
                // Log or handle the exception if needed
                throw;
            }
        }

        public async Task<List<MonthlyBudget>> GetMonthlyBudgetsByDepartmentIdAsync(int departmentId)
        {
            try
            {
                var monthlyBudgets = await _context.MonthlyBudgets
                    .Where(ab => ab.DepartmentId == departmentId)
                    .ToListAsync();

                return monthlyBudgets;
            }
            catch (Exception ex)
            {
                // Log or handle the exception if needed
                throw;
            }
        }

        public async Task<MonthlyBudget> GetMonthlyBudgetsByDepartmentIdAndIsActiveAsync(int departmentId)
        {
            try
            {
                var monthlyBudget = await _context.MonthlyBudgets
                    .FirstOrDefaultAsync(ab => ab.DepartmentId == departmentId && ab.IsActive == true);
                if (monthlyBudget == null)
                {
                    return null;
                }
                return monthlyBudget;
            }
            catch (Exception ex)
            {
                // Log or handle the exception if needed
                throw;
            }
        }

        public async Task<bool> AddSumToMonthlyBudgetCeilingAsync(int monthlyBudgetId, int additionalAmount)
        {
            try
            {
                var monthlyBudget = await _context.MonthlyBudgets.FindAsync(monthlyBudgetId);

                if (monthlyBudget == null || !monthlyBudget.IsActive == true)
                {
                    throw new NotFoundException("Monthly budget not found or not active");
                }

                monthlyBudget.MonthlyBudgetCeiling += additionalAmount;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log or handle the exception if needed
                throw;
            }
        }

        public async Task<bool> deactivateMonthlyBudget(int departmentId)
        {
            try
            {
                var monthlyBudget = await GetMonthlyBudgetsByDepartmentIdAndIsActiveAsync(departmentId);
                if (monthlyBudget != null)
                {
                    monthlyBudget.IsActive = false;
                    await _context.SaveChangesAsync();
                }
                return true;

            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public async Task<bool> DeleteMonthlyBudgetAsync(int monthlyBudgetId)
        {
            try
            {
                var monthlyBudget = await _context.MonthlyBudgets.FindAsync(monthlyBudgetId);

                if (monthlyBudget == null || !monthlyBudget.IsActive == true)
                {
                    throw new NotFoundException("Monthly budget not found or not active");
                }

                monthlyBudget.IsActive = false;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log or handle the exception if needed
                throw;
            }
        }

        public async Task<bool> resettingMonthlyBudget(int departmentId)
        {
            try
            {
                var activeBudget = GetMonthlyBudgetsByDepartmentIdAndIsActiveAsync(departmentId).Result;
                if (activeBudget == null)
                {
                    throw new NotFoundException("Monthly budget not found or not active");
                }
                activeBudget.MonthlyBudgetCeiling = 0;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public async Task<bool> addAmountToMonthlyBudget(int departmentId, decimal amountToAdd)
        {
            try
            {
                var activeBudget = GetMonthlyBudgetsByDepartmentIdAndIsActiveAsync(departmentId).Result;
                if (activeBudget == null)
                {
                    throw new NotFoundException("Monthly budget not found or not active");
                }
                activeBudget.MonthlyBudgetCeiling += amountToAdd;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        private async Task<bool> checkDuplicateMonthlyBudgetByDate(MonthlyBudget monthlyBudget)
        {
            var monthlyBudgetList = await GetMonthlyBudgetsByDepartmentIdAsync(monthlyBudget.DepartmentId);

            foreach (var budget in monthlyBudgetList)
            {
                if (monthlyBudgetList != null && budget.MonthlyBudgetMonth == monthlyBudget.MonthlyBudgetMonth && budget.MonthlyBudgetYear == monthlyBudget.MonthlyBudgetYear)
                {
                    return false;
                }
            }

            return true;
        }


    }
}
