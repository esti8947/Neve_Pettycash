using BL.Services;
using DAL.Models;

namespace PettyCashNeve_ServerSide.Repositories.MonthlyCashRegisterRepository
{
    public interface IMonthlyCashRegisterRepository
    {
        Task<MonthlyCashRegister> GetMonthlyCashRegisterByIdAsync(int monthlyCashRegisterId);
        Task<MonthlyCashRegister> GetCurrentMonthlyCashRegistersByUserIdAsync(string userId);
        //Task<MonthlyCashRegister> GetMonthlyCashRegisterByUserOfDepartmentIdAsync(int userOfDepartmentId);
        //Task<List<MonthlyCashRegister>> GetMonthlyCashRegistersByDepartmentIdAsync(int departmentId);
        Task<bool> CreateNewMonthlyCashRegisterAsync(MonthlyCashRegister newMonthlyCashRegisterDto);
        Task<bool> UpdateMonthlyCashRegisterAsync(MonthlyCashRegister updatedMonthlyCashRegister);
        Task<bool> DeactivateMonthlyCashRegister(string userId);
        Task<bool> InsertRefundAmount(decimal refunatedAmount, string userId);
        Task<List<MonthlyCashRegister>> GetMonthlyCashRegisterByDepartmenId(int departmenId);
        Task<bool> CheckAllMonthlyCashRegistersInactiveForYearAsync(int departmentId, int academicYear);

        //Task<bool> DeleteMonthlyCashRegisterAsync(int monthlyCashRegisterId);
    }

}
