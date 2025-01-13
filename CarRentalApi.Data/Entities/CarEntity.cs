using CarRentalApi.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApi.Data.Entities
{
    public class CarEntity : BaseEntity
    {
        public string Make { get; set; } // üreten
        public string Model { get; set; }
        public int Year { get; set; }
        public VehicleType VehicleType { get; set; } // araç tipi 
        public decimal PricePerDay { get; set; } // günlük ücrt

        public int StockQuantity { get; set; } // stok adedi
        public bool IsStock => StockQuantity > 0; // 0'dan büyükse Stok durumu

        // Relational Property
        public ICollection<CarFeatureEntity> CarFeatures { get; set; } // çoklu bağlantı 
        public ICollection<ReservationEntity> Reservations { get; set; }
    }

    public class CarConfiguration : BaseConfiguration<CarEntity>
    {
        public override void Configure(EntityTypeBuilder<CarEntity> builder)
        {
            builder.Property(x => x.Make)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(x => x.Year)
                .IsRequired()
                .HasDefaultValue(DateTime.Now.Year)
                .HasComment("Araç Üretim Yılı");  // SQL'de açıklama

            builder.Property(x => x.PricePerDay)
                .IsRequired()
                .HasPrecision(18, 2)
                .HasDefaultValue(0.0m);

            base.Configure(builder);
        }
    }
}
