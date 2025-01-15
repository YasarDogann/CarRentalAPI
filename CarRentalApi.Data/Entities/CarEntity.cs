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
    // Araç bilgilerini tutan entity sınıfı
    // Entity class that holds car information
    public class CarEntity : BaseEntity
    {
        public string Make { get; set; } // Marka - make
        public string Model { get; set; }
        public int Year { get; set; }

        // Araç tipi (enum)
        // Vehicle type (enum)
        public VehicleType VehicleType { get; set; } 
        public decimal PricePerDay { get; set; } 
        public int StockQuantity { get; set; } 
        public bool IsStock => StockQuantity > 0; // 0'dan büyükse Stok durumu

        // Relational Property
        public ICollection<CarFeatureEntity> CarFeatures { get; set; } // çoklu bağlantı 
        public ICollection<ReservationEntity> Reservations { get; set; }
    }


    // Araç entity'sinin veritabanı yapılandırması
    // Database configuration for car entity
    public class CarConfiguration : BaseConfiguration<CarEntity>
    {
        public override void Configure(EntityTypeBuilder<CarEntity> builder)
        {
            builder.Property(x => x.Make)
                .IsRequired()
                .HasMaxLength(30);

            // Yıl alanı zorunlu ve varsayılan değeri şu anki yıl
            // Year field is required and defaults to current year
            builder.Property(x => x.Year)
                .IsRequired()
                .HasDefaultValue(DateTime.Now.Year)
                .HasComment("Araç Üretim Yılı");

            // Günlük ücret zorunlu, 18,2 formatında ve varsayılan 0
            // Daily price is required, in 18,2 format and defaults to 0
            builder.Property(x => x.PricePerDay)
                .IsRequired()
                .HasPrecision(18, 2)
                .HasDefaultValue(0.0m);

            base.Configure(builder);
        }
    }
}
