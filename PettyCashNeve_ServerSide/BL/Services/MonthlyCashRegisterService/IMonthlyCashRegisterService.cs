using BL.Services;
using Entities.Models_Dto;
using PettyCashNeve_ServerSide.Dto;
namespace PettyCashNeve_ServerSide.Services.MonthlyCashRegisterService
{
    public interface IMonthlyCashRegisterService
    {
        //Task<ServiceResponse<MonthlyCashRegisterDto>> GetMonthlyCashRegistersByUserOfDepartmentIdAsync(int userOfDepartmentId);
        //Task<ServiceResponse<List<MonthlyCashRegisterDto>>> GetMonthlyCashRegistersByDepartmentIdAsync(int departmentId);
        Task<ServiceResponse<List<MonthlyCashRegisterDepartmentDto>>> GetCurrentMonthlyCashRegistersByDepartmentIdAsync(int departmentId);
        Task<ServiceResponse<MonthlyCashRegisterDto>> GetCurrentMonthlyCashRegistersByUserIdAsync(string userId);
        Task<ServiceResponse<bool>> CreateNewMonthlyCashRegisterAsync(MonthlyCashRegisterDto newMonthlyCashRegisterDto);
        Task<ServiceResponse<bool>> UpdateMonthlyCashRegisterAsync(MonthlyCashRegisterDto updateMonthlyCashRegisterDto);
        Task<int> GetCurrentMonthByUserIdAsync(string  userId);
        Task<ServiceResponse<bool>> DeactivateMonthlyCashRegister(string userId);
        Task<ServiceResponse<bool>> InsertRefundAmount(decimal refundAmount, string userId);
        Task<ServiceResponse<bool>> CheckAllMonthlyCashRegistersInactiveForYearAsync(int departmentId, int academicYear);
        //Task<ServiceResponse<bool>> DeleteMonthlyCashRegisterAsync(int monthlyCashRegisterId);


    }
}
