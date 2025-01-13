using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApi.Data.Entities
{
    public class CarFeatureEntity : BaseEntity
    {
        // hangi arabada hangi özellik var ?
        public int CarId { get; set; } // hangi araba dediğimize göre?
        public int FeatureId { get; set; }  // hangi özellik dediğimize göre?

        // Relational Property
        public CarEntity Car { get; set; } // 1 arabaya 1 özellik ait olacak dedim
        public FeatureEntity Feature { get; set; }
    }

    public class CarFeatureConfiguration : BaseConfiguration<CarFeatureEntity>
    {
        public override void Configure(EntityTypeBuilder<CarFeatureEntity> builder)
        {
            builder.Ignore(x => x.Id); // Id property görmezden gelindi tabloya aktarılmayacak
            builder.HasKey("CarId", "FeatureId"); // Composite key oluşturup PK olarak atandı


            base.Configure(builder);
        }
    }
}
