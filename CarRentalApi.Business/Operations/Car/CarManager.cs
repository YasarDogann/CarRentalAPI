using CarRentalApi.Business.Operations.Car.CarDtos;
using CarRentalApi.Business.Types;
using CarRentalApi.Data.Entities;
using CarRentalApi.Data.Repositories;
using CarRentalApi.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using CarRentalApi.Business.Excepions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CarRentalApi.Business.Operations.Car
{
    // Araç işlemlerini yöneten servis sınıfı
    // Service class that manages car operations
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

        // Yeni araç ekleme
        // Add new car
        public async Task<ServiceMessage> AddCar(AddCarDto car)
        {
            var hasCar = _carRepository.GetAll(c => c.Model.ToLower() == car.Model.ToLower()).Any();

            if (hasCar)
            {
                // Global Exception Handling
                throw new BadRequestException("Bu araç zaten sistemde mevcut");
            }

            await _unitOfWork.BeginTransaction();


            // Araç entity'sini oluşturma
            // Create car entity
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

                throw new CustomException("Araç kaydı sırasında bir hata oluştu");
            }

            // Araç özelliklerini ekleme
            // Add car features
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
                throw new CustomException("Araç özellikleri eklenirken bir hata oluştu, süreç başa alındı.",500);
            }

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Araç özelliği ekleme işlemi başarılı oldu"
            };
        }


        // Araç fiyatını güncelleme
        // Update car price
        public async Task<ServiceMessage> AdjustCarPrice(int id, decimal changeBy)
        {
            var car = _carRepository.GetById(id);

            if (car is null)
            {
                throw new NotFoundException($"{id} numaralı araç bulunamadı");
            }

            car.PricePerDay = changeBy;

            _carRepository.Update(car);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw new CustomException("Günlük ücret değiştirilirken bir hata oluştu",500);
            }

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Fiyat değişimi başarıyla gerçekleştirildi"
            };
        }


        // Araç silme
        // Delete car
        public async Task<ServiceMessage> DeleteCar(int id)
        {
            var car = _carRepository.GetById(id);

            if (car is null)
            {
                throw new NotFoundException($"{id} numaralı silinmek istenen araç bulunamadı");
            }

            _carRepository.Delete(id);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw new CustomException("Silme işlemi sırasında bir hata oluştu.",500);
            }

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Silme işlemi başarıyla gerçekleşti"
            };
        }

        // Tüm araçları listeleme
        // List all cars
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

        // ID'ye göre araç getirme
        // Get car by ID
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


        // Araç bilgilerini güncelleme
        // Update car information
        public async Task<ServiceMessage> UpdateCar(UpdateCarDto car)
        {
            var carEntity = _carRepository.GetById(car.Id);

            if(carEntity is null)
            {
                throw new NotFoundException($"{car.Id} numaralı güncellenecek araç bulunamadı");
            }

            // önce car tablosunda daha sonra carFeature tablosunda güncelleme yapıcam.
            // 2 ayrı tablo üzerinde işlem yapacağımdan dolayı Transaction açıyorum 
            await _unitOfWork.BeginTransaction();

            // Araç bilgilerini güncelleme
            // Update car information
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
                throw new CustomException("Araba bilgileri güncellenirken bir hata ile karşılaşıldı",500);
            }

            var carFeatures = _carFeatureRepository.GetAll(c => c.CarId == car.Id).ToList();

            foreach (var carFeature in carFeatures)
            {
                _carFeatureRepository.Delete(carFeature, false); // Mevcut özellikleri -> Hard Delete
            }

            // Yeni özellikleri ekleme
            // Add new features
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
                throw new CustomException("Araba bilgileri güncellenirken bir hata oluştu. İşlemler başa alınıyor", 500);
            }

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Araba bilgileri güncelleme işlemi başarıyla gerçekleşti."
            };
        }
    }
}
 