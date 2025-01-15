using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApi.Data.Entities
{
    // Araç özelliklerini tutan entity sınıfı
    // Entity class that holds car features
    public class FeatureEntity : BaseEntity
    {
        public string Title { get; set; }

        // Relational Property
        // Bu özelliğe sahip araçlar (çoka-çok ilişki)
        // Cars that have this feature (many-to-many relationship)
        public ICollection<CarFeatureEntity> CarFeatures { get; set; }
    }

    public class FeatureConfiguration : BaseConfiguration<FeatureEntity>
    {
        public override void Configure(EntityTypeBuilder<FeatureEntity> builder)
        {
            base.Configure(builder);
        }
    }
}
