using CarRentalApi.Business.Operations.Feature.Dtos;
using CarRentalApi.Business.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApi.Business.Operations.Feature
{
    public interface IFeatureService
    {
        Task<ServiceMessage> AddFeature(AddFeatureDto feature);
    }
}
