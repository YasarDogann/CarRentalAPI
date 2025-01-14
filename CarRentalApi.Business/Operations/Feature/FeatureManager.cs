using CarRentalApi.Business.Excepions;
using CarRentalApi.Business.Operations.Feature.Dtos;
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

namespace CarRentalApi.Business.Operations.Feature
{
    public class FeatureManager : IFeatureService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<FeatureEntity> _featureRepository;

        public FeatureManager(IUnitOfWork unitOfWork, IRepository<FeatureEntity> featureRepository)
        {
            _unitOfWork = unitOfWork;
            _featureRepository = featureRepository;
        }

        public async Task<ServiceMessage> AddFeature(AddFeatureDto feature)
        {
            // 1. Özellik Kontrolü
            var hasFeature = _featureRepository.GetAll(x => x.Title.ToLower() == feature.Title.ToLower()).Any();

            if (hasFeature)
            {
                throw new BadRequestException("Bu özellik zaten sistemde mevcut");
            }

            // 2. Entity Oluşturma
            var featureEntity = new FeatureEntity
            {
                Title = feature.Title,
            };

            // 3. Veritabanına Ekleme
            _featureRepository.Add(featureEntity);

            try
            {
                // 4. Değişiklikleri Kaydetme
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                // 5. Hata Yönetimi
                throw new CustomException("Özellik kaydı sırasında bir hata oluştu", 500);
            }

            // 6. Başarılı Sonuç
            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Özellik Başarıyla Eklendi."
            };
        }

        public async Task<ServiceMessage> DeleteFeature(int id)
        {
            // 1. Özellik Kontrolü
            var feture = _featureRepository.GetById(id);

            if (feture is null)
            {
                throw new NotFoundException($"{id} numaralı özellik bulunamadı");
            }

            // 2. Silme İşlemi
            _featureRepository.Delete(id);

            try
            {
                // 3. Değişiklikleri Kaydetme
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                // 4. Hata Yönetimi
                throw new CustomException("Silme işlemi sırasında bir hata oluştu", 500);
            }

            // 5. Başarılı Sonuç
            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Silme işlemi başarıyla gerçekleşti"
            };
        }

        public async Task<List<FeatureDto>> GetAllFeatures()
        {
            var features = await _featureRepository.GetAll()
                .Select(f => new FeatureDto
                {
                    Id = f.Id,
                    Title = f.Title,
                }).ToListAsync();

            return features;
        }

        public async Task<ServiceMessage> UpdateFeature(UpdateFeatureDto feature)
        {
            // 1. Özellik Kontrolü
            var featureEntity = _featureRepository.GetById(feature.Id);

            if (featureEntity is null)
            {
                throw new NotFoundException($"{feature.Id} numaralı özellik bulunamadı");
            }

            // 2. Bilgileri Güncelleme
            featureEntity.Title = feature.Title;    

            _featureRepository.Update(featureEntity);

            try
            {
                // 3. Değişiklikleri Kaydetme
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {
                // 4. Hata Yönetimi
                throw new CustomException("Özellik güncellenirken bir hata oluştu", 500);
            }

            // 5. Başarılı Sonuç
            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Özellik güncelleme işlemi başarıyla tamamlandı"
            };
        }
    }
}
