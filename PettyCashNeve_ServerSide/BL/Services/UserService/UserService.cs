//using AutoMapper;
//using BL.Repositories.UserRepository;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using BCrypt.Net;
//using Entities.Models_Dto.UserDto;

//namespace BL.Services.UserService
//{
//    public class UserService : IUserService
//    {
//        private readonly IUserRepository _userRepository;
//        private readonly IMapper _mapper;
//        public UserService(IUserRepository userRepository, IMapper mapper)
//        {
//            _userRepository = userRepository;
//            _mapper = mapper;
//        }

//        public async Task<ServiceResponse<string>> GetUserNameById(int id)
//        {
//            var serviceResponse = new ServiceResponse<string>();
//            try
//            {
//                var user = await _userRepository.GetUserByIdAsync(id);
//                if(user != null)
//                {
//                    serviceResponse.Data = user.Name;
//                    serviceResponse.Success = true;
//                }
//            }
//            catch (Exception ex)
//            {

//                serviceResponse.Success = false;
//                serviceResponse.Message = ex.Message;
//            }
//            return serviceResponse;
//        }

//        public async Task<ServiceResponse<UserDto>> LoginAsync(string username, string password)
//        {
//            var user = await _userRepository.GetUserByUsernameAsync(username);
//            if (user == null)
//                //|| BCrypt.Net.BCrypt.Verify(password, user.PasswordHash)
//            {
//                return new ServiceResponse<UserDto> { Success = false, Message = "Invalid username or password" };
//            }

//            var userDto = _mapper.Map<UserDto>(user);
//            return new ServiceResponse<UserDto> { Data = userDto, Success = true, Message = "Login successful" };
//        }
//    }
//}
