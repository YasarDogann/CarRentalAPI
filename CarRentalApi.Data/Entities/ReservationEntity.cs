using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApi.Data.Entities
{
    // Araç rezervasyon bilgilerini tutan entity sınıfı
    // Entity class that holds car reservation information
    public class ReservationEntity : BaseEntity
    {
        public int CarId { get; set; }
        public int UserId { get; set; }
        public int PaymentId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public decimal TotalAmount { get; set; }

        // Relational Property
        public CarEntity Car { get; set; }
        public UserEntity User { get; set; }
        public PaymentEntity Payment { get; set; }
    }

    public class ReservationConfiguration : BaseConfiguration<ReservationEntity>
    {
        public override void Configure(EntityTypeBuilder<ReservationEntity> builder)
        {
            builder.Ignore(x => x.Id);

            builder.Property(r => r.StartDate)
                .IsRequired();

            builder.Property(r => r.EndDate)
                .IsRequired();

            builder.Property(r => r.TotalAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.HasKey("CarId", "UserId", "PaymentId");
            base.Configure(builder);
        }
    }
}
