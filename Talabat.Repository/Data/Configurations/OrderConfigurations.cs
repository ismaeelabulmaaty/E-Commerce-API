using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Orders;

namespace Talabat.Repository.Data.Configurations
{
    internal class OrderConfigurations : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(o => o.ShippingAddress, shippingAddress => shippingAddress.WithOwner());

            builder.Property(o => o.Status)
                .HasConversion(
                ostatus => ostatus.ToString(),
                ostatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), ostatus));


            builder.Property(o => o.SubTotal)
                   .HasColumnType("decimal(18,2)");

            builder.HasOne(o => o.DeliveryMethod)
                   .WithMany()
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
