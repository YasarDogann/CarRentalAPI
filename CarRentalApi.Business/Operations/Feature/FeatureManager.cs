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
            var hasFeature = _featureRepository.GetAll(x => x.Title.ToLower() == feature.Title.ToLower()).Any();

            if (hasFeature)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Özellik zaten bulunuyor"
                };
            }

            var featureEntity = new FeatureEntity
            {
                Title = feature.Title,
            };

            _featureRepository.Add(featureEntity);

            try
            {
                await  _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw new Exception("Özellik kaydı sırasında bir hata oluştu.");
            }

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Özellik Başarıyla Eklendi."
            };
        }

        public async Task<ServiceMessage> DeleteFeature(int id)
        {
            var feture = _featureRepository.GetById(id);

            if (feture is null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Silinmek istenen özellik bulunamadı."
                };
            }

            _featureRepository.Delete(id);

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
            var featureEntity = _featureRepository.GetById(feature.Id);

            if (featureEntity is null)
            {
                return new ServiceMessage
                {
                    IsSucceed = false,
                    Message = "Özellik Bulunamadı."
                };
            }

            featureEntity.Title = feature.Title;    

            _featureRepository.Update(featureEntity);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw new Exception("Özellik güncellenirken hata oluştu.");
            }

            return new ServiceMessage
            {
                IsSucceed = true,
                Message = "Özellik güncelleme işlemi başarıyla tamamlandı"
            };
        }
    }
}
