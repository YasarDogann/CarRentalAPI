using CarRentalApi.Business.Operations.Car.CarDtos;
using CarRentalApi.Business.Operations.User.Dtos;
using CarRentalApi.Business.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApi.Business.Operations.User
{
    public interface IUserService
    {
        Task<ServiceMessage> AddUser(AddUserDto user); // async çünkü unit of work kullanılacak
        ServiceMessage<UserInfoDto> LoginUser(LoginUserDto user);
        Task<ServiceMessage> UpdateUser(UpdateUserDto user);
        Task<List<UserDto>> GetAllUsers();
    }
}
