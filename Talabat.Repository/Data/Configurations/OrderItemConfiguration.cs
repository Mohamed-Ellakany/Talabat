using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Repository.Data.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<Orderitem>
    {
        public void Configure(EntityTypeBuilder<Orderitem> builder)
        {
            builder.Property(O => O.Price).HasColumnType("decimal(18,2)");

            builder.OwnsOne(O => O.Product, P => P.WithOwner());

        }
    }
}
