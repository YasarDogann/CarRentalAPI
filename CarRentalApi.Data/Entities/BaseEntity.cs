using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalApi.Data.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; } // Soft Delete İşlemlerinde Kullanılacak
    }

    public abstract class BaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<TEntity> builder)
        {
            // bu veritabanı üzerinde yapılacak bütün sorgulamalarda ve diğer linq işlemlerinde geçerli olacak bir filtreleme yazdım. Bu sayede soft delete atılmış verilerle uğraşmıcaz
            builder.HasQueryFilter(x => x.IsDeleted == false);

            builder.Property(x => x.ModifiedDate)
                .IsRequired(false);
        }
    }
}
