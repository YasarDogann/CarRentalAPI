using CarRentalApi.Business.DataProtection;
using CarRentalApi.Business.Operations.User.Dtos;
using CarRentalApi.Business.Types;
using CarRentalApi.Data.Entities;
using CarRentalApi.Data.Enums;
using CarRentalApi.Data.Repositories;
using CarRentalApi.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApi.Business.Operations.User
{
    public class UserManager : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<UserEntity> _userRepository;
        private readonly IDataProtection _protector;

        public UserManager(IUnitOfWork unitOfWork, IRepository<UserEntity> userRepository, IDataProtection protector)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _protector = protector;
        }

        public async Task<ServiceMessage> AddUser(AddUserDto user)
        {
            var hasMail = _userRepository.GetAll(x => x.Email.ToLower() == user.Email.ToLower());

            if (hasMail.Any())
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Email adresi zaten mevcut"
                };
            }

            var userEntity = new UserEntity()
            {
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Password = _protector.Protect(user.Password),
                BirthDate = user.BirthDate,
                UserType = UserType.Customer 
            };

            _userRepository.Add(userEntity);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw new Exception("Kullanıcı kaydı sırasında bir hata oluştu");
            }

            return new ServiceMessage
            {
                IsSucceed = true
            };
        }

        public async Task<List<UserDto>> GetAllUsers()
        {
            var users = await _userRepository.GetAll()
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Password = u.Password,
                    BirthDate = u.BirthDate,
                    UserType = u.UserType == UserType.Admin ? "Admin" : "Customer"  // Enum değerini metne dönüştürüyoruz
                }).ToListAsync();
            return users;
        }

        public ServiceMessage<UserInfoDto> LoginUser(LoginUserDto user)
        {
            var userEntity = _userRepository.Get(x => x.Email.ToLower() == user.Email.ToLower());

            if (userEntity is null)
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceed = false,
                    Message = "Kullanıcı adı veya şifre hatalı."
                };
            }

            // şifrelenmiş mesaj geliyor unprotect
            var unprotectedPass = _protector.UnProtect(userEntity.Password);

            if (unprotectedPass == user.Password)
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceed = true,
                    Data = new UserInfoDto
                    {
                        Email = userEntity.Email,
                        FirstName = userEntity.FirstName,
                        LastName = userEntity.LastName,
                        UserType = userEntity.UserType
                    }
                };
            }
            else
            {
                return new ServiceMessage<UserInfoDto>
                {
                    IsSucceed = false,
                    Message = "Kullanıcı adı veya şifre hatalı."
                };
            }
        }

        public async Task<ServiceMessage> UpdateUser(UpdateUserDto user)
        {
            var userEntity = _userRepository.GetById(user.Id);
            
            if(userEntity is null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Kullanıcı Bulunamadı"
                };
            }

            userEntity.Email = user.Email;
            userEntity.Password = _protector.Protect(user.Password);
            userEntity.FirstName = user.FirstName;
            userEntity.LastName = user.LastName;
            userEntity.BirthDate = user.BirthDate;
            userEntity.UserType = user.UserType;

            _userRepository.Update(userEntity);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw new Exception("Kullanıcı bilgileri güncellenirken bir hata oluştu.");
            }

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Kullanıcı bilgileri güncelleme işlemi başarıyla gerçekleşti."
            };
        }
    }
}
