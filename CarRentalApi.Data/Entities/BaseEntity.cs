using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApi.Data.Entities
{
    // Tüm entity'lerin temel aldığı ana sınıf
    // Base class that all entities inherit from
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        // Kaydın silinip silinmediği (yumuşak silme için)
        // Whether the record is deleted (for soft delete)
        public bool IsDeleted { get; set; } 
    }


    // Entity yapılandırmalarının temel sınıfı
    // Base class for entity configurations
    public abstract class BaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            // Tüm sorgularda silinmemiş kayıtları filtreleme
            // Filter out deleted records in all queries
            builder.HasQueryFilter(x => x.IsDeleted == false);


            // ModifiedDate alanının zorunlu olmadığını belirtme
            // Specify that ModifiedDate field is not required
            builder.Property(x => x.ModifiedDate)
                .IsRequired(false);
        }
    }
}
