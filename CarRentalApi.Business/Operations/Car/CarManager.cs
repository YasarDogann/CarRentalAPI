using CarRentalApi.Business.Operations.Car.CarDtos;
using CarRentalApi.Business.Types;
using CarRentalApi.Data.Entities;
using CarRentalApi.Data.Repositories;
using CarRentalApi.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApi.Business.Operations.Car
{
    public class CarManager : ICarService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<CarEntity> _carRepository;
        private readonly IRepository<CarFeatureEntity> _carFeatureRepository;

        public CarManager(IUnitOfWork unitOfWork, IRepository<CarEntity> carRepository, IRepository<CarFeatureEntity> carFeatureRepository)
        {
            _unitOfWork = unitOfWork;
            _carRepository = carRepository;
            _carFeatureRepository = carFeatureRepository;
        }

        public async Task<ServiceMessage> AddCar(AddCarDto car)
        {
            var hasCar = _carRepository.GetAll(c => c.Make.ToLower() == car.Make.ToLower()).Any();

            if (hasCar)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Bu araç zaten sistemde mevcut",
                };
            }

            await _unitOfWork.BeginTransaction();

            var carEntity = new CarEntity
            {
                Make = car.Make,
                Model = car.Model,
                Year = car.Year,
                VehicleType = car.VehicleType,
                PricePerDay = car.PricePerDay,
                StockQuantity = car.StockQuantity,
            };

            _carRepository.Add(carEntity);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw new Exception("Araç kaydı sırasında bir hata oluştu");
            }

            foreach (var featureId in car.FeatureIds)
            {
                var carFeature = new CarFeatureEntity
                {
                    CarId = carEntity.Id,
                    FeatureId = featureId,
                };

                _carFeatureRepository.Add(carFeature);
            }

            try
            {
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackTransction();
                throw new Exception("Araç özellikleri eklenirken bir hata oluştu, süreç başa alındı.");
            }

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Araç özelliği ekleme işlemi başarılı oldu"
            };
        }

        public async Task<ServiceMessage> AdjustCarPrice(int id, decimal changeBy)
        {
            var car = _carRepository.GetById(id);

            if (car is null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Bu id ile eşlesen bir araç bulunamadı."
                };
            }

            car.PricePerDay = changeBy;

            _carRepository.Update(car);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new Exception("Günlük ücret değiştirilirken bir hata oluştu");
            }

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Fiyat değişimi başarıyla gerçekleştirildi"
            };
        }

        public async Task<ServiceMessage> DeleteCar(int id)
        {
            var car = _carRepository.GetById(id);

            if (car is null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Silinmek istenen araba bulunamadı."
                };
            }

            _carRepository.Delete(id);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw new Exception("Silme işlemi sırasında bir hata oluştu.");
            }

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Silme işlemi başarıyla gerçekleşti"
            };
        }

        public async Task<List<CarDto>> GetAllCars()
        {
            var cars = await _carRepository.GetAll()
              .Select(x => new CarDto
              {
                  Id = x.Id,
                  Make = x.Make,
                  Model = x.Model,
                  Year = x.Year,
                  PricePerDay = x.PricePerDay,
                  StockQuantity = x.StockQuantity,
                  VehicleType = x.VehicleType,
                  Features = x.CarFeatures.Select(f => new CarFeaturesDto // arabanın her bir featuresi için yeni new'leme yap böylelikle özellikleri yazdırıcaz 
                  {
                      Id = f.Id,
                      Title = f.Feature.Title
                  }).ToList()
              }).ToListAsync();

            return cars;
        }

        public async Task<CarDto> GetCar(int id)
        {
            var car = await _carRepository.GetAll(x => x.Id == id)
               .Select(x => new CarDto
               {
                   Id = x.Id,
                   Make = x.Make,
                   Model = x.Model,
                   Year = x.Year,
                   PricePerDay = x.PricePerDay,
                   StockQuantity = x.StockQuantity,
                   VehicleType = x.VehicleType,
                   Features = x.CarFeatures.Select(f => new CarFeaturesDto // arabanın her bir featuresi için yeni new'leme yap böylelikle özellikleri yazdırıcaz 
                   {
                       Id = f.Id,
                       Title = f.Feature.Title
                   }).ToList()
               }).FirstOrDefaultAsync();

            return car;
        }

        public async Task<ServiceMessage> UpdateCar(UpdateCarDto car)
        {
            var carEntity = _carRepository.GetById(car.Id);

            if(carEntity is null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Araba bulunamadı."
                };
            }

            // önce car tablosunda daha sonra carFeature tablosunda güncelleme yapıcam.
            // 2 ayrı tablo üzerinde işlem yapacağımdan dolayı Transaction açıyorum 
            await _unitOfWork.BeginTransaction();

            carEntity.Make = car.Make;
            carEntity.Model = car.Model;
            carEntity.Year = (int)car.Year;
            carEntity.PricePerDay = car.PricePerDay;
            carEntity.StockQuantity = car.StockQuantity;
            carEntity.VehicleType = car.VehicleType;

            _carRepository.Update(carEntity);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackTransction();
                throw new Exception("Araba bilgileri güncellenirken bir hata ile karşılaşıldı");
            }

            var carFeatures = _carFeatureRepository.GetAll(c => c.CarId == c.CarId).ToList();

            foreach(var carFeature in carFeatures)
            {
                _carFeatureRepository.Delete(carFeature, false); // HArd Delete
            }

            foreach (var featureId in car.FeatureIds)
            {
                var carFeature = new CarFeatureEntity
                {
                    CarId = carEntity.Id,
                    FeatureId = featureId,
                };
                _carFeatureRepository.Add(carFeature);
            }

            try
            {
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransaction();
            }
            catch (Exception)
            {
                await _unitOfWork.RollBackTransction();
                throw new Exception("Araba bilgileri güncellenirken bir hata oluştu. İşlemler başa alınıyor");
            }

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Araba bilgileri güncelleme işlemi başarıyla gerçekleşti."
            };
        }
    }
}
 