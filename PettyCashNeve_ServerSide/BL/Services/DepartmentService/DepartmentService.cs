using AutoMapper;
using DAL.Models;
using BL.Services;
using Entities.Models_Dto;
using PettyCashNeve_ServerSide.Exceptions;
using PettyCashNeve_ServerSide.Repositories.DepartmentRepository;
using PettyCashNeve_ServerSide.Dto;
using Entities.Models_Dto.UserDto;

namespace PettyCashNeve_ServerSide.Services.DepartmentService
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IMapper _mapper;
        public DepartmentService(IDepartmentRepository departmentRepository, IMapper mapper)
        {
            _departmentRepository = departmentRepository;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<List<DepartmentDto>>> GetDepartmentsAsync()
        {
            var serviceResponse = new ServiceResponse<List<DepartmentDto>>();
            try
            {
                var departments = await _departmentRepository.GetDepartmentsAsync();
                var departmentDtos = _mapper.Map<List<DepartmentDto>>(departments);
                if (departmentDtos == null)
                {
                    serviceResponse.Message = "no departments found";
                }
                serviceResponse.Data = departmentDtos;
                return serviceResponse;

            }
            catch (Exception ex)
            {
                return new ServiceResponse<List<DepartmentDto>> { Success = false, Message = ex.Message };
            }
        }

        public async Task<ServiceResponse<DepartmentDto>> GetDepartmentByIdAsync(int departmentId)
        {
            try
            {
                var department = await _departmentRepository.GetDepartmentByIdAsync(departmentId);
                var departmentDto = _mapper.Map<DepartmentDto>(department);
                return new ServiceResponse<DepartmentDto> { Data = departmentDto };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<DepartmentDto> { Success = false, Message = ex.Message };
            }
        }

        public async Task<ServiceResponse<bool>> DeleteDepartmentAsync(int departmentId)
        {
            try
            {
                var result = await _departmentRepository.DeleteDepartmentAsync(departmentId);
                return new ServiceResponse<bool> { Data = result };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool> { Success = false, Message = ex.Message };
            }
        }

        public async Task<ServiceResponse<bool>> UpdateDepartmentAsync(DepartmentDto updatedDepartment)
        {
            var serviceResponse = new ServiceResponse<bool>();
            try
            {
                var existingDepartment = await _departmentRepository.GetDepartmentByIdAsync(updatedDepartment.DepartmentId);
                if (existingDepartment == null)
                {
                    throw new NotFoundException("Department not found or not active");
                }

                _mapper.Map(updatedDepartment, existingDepartment);
                await _departmentRepository.UpdateDepartmentAsync(existingDepartment);
                serviceResponse.Data = true;
                serviceResponse.Success = true;
            }
            catch (NotFoundException ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            catch (Exception ex)
            {
                serviceResponse.Success = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<bool>> CreateDepartmentAsync(DepartmentDto departmentDto)
        {
            var serviceResponse = new ServiceResponse<bool>();
            try
            {
                var newDepartment = _mapper.Map<Department>(departmentDto);
                var result = await _departmentRepository.CreateDepartment(newDepartment);
                serviceResponse.Data = result;
                serviceResponse.Success = result;
                if (!result)
                {
                    serviceResponse.Message = "Department creation failed";
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success= false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<UserInfoModel>>> GetUsersByDepartmentId(int departmentId)
        {
            var serviceResponse = new ServiceResponse<List<UserInfoModel>>();
            try
            {
                var usersList = await _departmentRepository.GetUsersByDepartmentId(departmentId);

                if (usersList == null || !usersList.Any())
                {
                    serviceResponse.Message = "No users found";
                    serviceResponse.Success = false;
                    return serviceResponse;
                }
                var usersInfo = new List<UserInfoModel>();

                foreach (var user in usersList)
                {
                    var userInfo = new UserInfoModel
                    {
                        Username = user.UserName,
                        DepartmentId = user.DepartmentId,
                        Id = user.Id,
                        email = user.Email,
                        phoneNumber = user.PhoneNumber
                    };

                    // Add UserInfoModel to the list
                    usersInfo.Add(userInfo);
                }

                serviceResponse.Data = usersInfo;
                serviceResponse.Success = true;
            }
            catch (Exception ex)
            {
                serviceResponse.Message = "An error occurred while retrieving users.";
                serviceResponse.Success = false;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<int>> GetYearByDepartmentId(int departmentId)
        {
            var serviceResponse = new ServiceResponse<int>();
            try
            {
                var year = await _departmentRepository.GetYearByDepartmentId(departmentId);
                if(year != 0)
                {
                    serviceResponse.Success = true;
                    serviceResponse.Data = year;
                }
            }
            catch (Exception ex)
            {
                serviceResponse.Success=false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}
