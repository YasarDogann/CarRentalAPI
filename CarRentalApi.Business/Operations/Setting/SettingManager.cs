using CarRentalApi.Data.Entities;
using CarRentalApi.Data.Repositories;
using CarRentalApi.Data.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApi.Business.Operations.Setting
{
    public class SettingManager : ISettingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<SettingEntity> _repository;

        public SettingManager(IUnitOfWork unitOfWork, IRepository<SettingEntity> repository)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
        }

        public bool GetMaintenenceState()
        {
            var maintenenceState = _repository.GetById(1).MaintenenceMode;
            return maintenenceState;
        }

        public async Task ToggleMaintenence()
        {
            var setting = _repository.GetById(1);

            // her defasında tersine çevirce (T --> F) veya (F --> T)
            setting.MaintenenceMode = !setting.MaintenenceMode;

            _repository.Update(setting);

            try
            {
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw new Exception("Bakım durumu güncellenirken bir hata oluştu");
            }
        }
    }
}
