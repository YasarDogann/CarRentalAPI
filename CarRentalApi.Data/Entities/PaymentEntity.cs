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
    // Ödeme bilgilerini tutan entity sınıfı
    // Entity class that holds payment information
    public class PaymentEntity : BaseEntity
    {
        public decimal Amount { get; set; } // ücret
        public DateTime PaymentDate { get; set; } // ödeme yapılan tarih

        public PaymentType PaymentType { get; set; } // ödeme türü

        // Navigation Property - Foreign Key
        public int ReservationId { get; set; }
        public ReservationEntity Reservation { get; set; }
    }

    public class PaymentConfiguration : BaseConfiguration<PaymentEntity>
    {
        public override void Configure(EntityTypeBuilder<PaymentEntity> builder)
        {
            builder.Property(p => p.Amount)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(p => p.PaymentDate)
                   .IsRequired();
            base.Configure(builder);
        }
    }
}
