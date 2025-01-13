using CarRentalApi.Data.Enums;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApi.Data.Entities
{
    public class UserEntity : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime BirthDate { get; set; }
        public UserType UserType { get; set; }

        // relational property 
        // bir müşteri farklı farklı zamnlarda araç kiralamış olabilri
        public ICollection<ReservationEntity> Reservations { get; set; }
    }

    public class UserConfiguration : BaseConfiguration<UserEntity>
    {
        public override void Configure(EntityTypeBuilder<UserEntity> builder)
        {
            builder.Property(x => x.FirstName)
                .IsRequired() // Zorunlu
                .HasMaxLength(50); // Maksimum 50 karakter

            builder.Property(x => x.LastName)
                .IsRequired() // Zorunlu
                .HasMaxLength(50); // Maksimum 50 karakter

            builder.Property(x => x.Email)
                .IsRequired() // Zorunlu
                .HasMaxLength(100); // Maksimum 100 karakter

            builder.Property(x => x.Password)
                .IsRequired();// Zorunlu

            builder.Property(u => u.BirthDate)
                   .IsRequired();

            base.Configure(builder);
        }
    }
}
