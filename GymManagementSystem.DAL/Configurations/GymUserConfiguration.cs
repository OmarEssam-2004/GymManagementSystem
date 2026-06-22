//using GymManagementSystem.DAL.Models;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using Microsoft.Extensions.Configuration;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace GymManagementSystem.DAL.Configurations
//{
//    public class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
//    {
//        public void Configure(EntityTypeBuilder<T> builder)
//        {
//            builder.Property(U => U.Name)
//                .HasColumnType("varchar")
//                .HasMaxLength(50);

//            builder.Property(U => U.Email)
//                   .HasColumnType("varchar")
//                   .HasMaxLength(50);

//            builder.Property(U => U.Phone)
//                   .HasColumnType("varchar")
//                   .HasMaxLength(11);

//            builder.HasIndex(U => U.Phone).IsUnique();
//            builder.HasIndex(U => U.Email).IsUnique();

//            builder.OwnsOne(U => U.Address, address =>
//            {
//                address.Property(A => A.Street)
//                       .HasColumnName("Street")
//                       .HasColumnType("varchar")
//                       .HasMaxLength(30);

//                address.Property(A => A.City)
//                       .HasColumnName("City")
//                       .HasColumnType("varchar")
//                       .HasMaxLength(50);

//                address.Property(A => A.BuildingNumber)
//                       .HasColumnName("BuildingNumber");

//            });

//            builder.ToTable(b => b.HasCheckConstraint("EmailCheck", "Email Like '_%@_%._%'"));
//            builder.ToTable(b => b.HasCheckConstraint("PhoneCheck", "Phone Like '010%' or Phone Like '011%' or Phone Like '012%' or Phone Like '015%'"));


//        }
//    }
//}


using GymManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagementSystem.DAL.Configurations
{
    public class GymUserConfiguration<T> : IEntityTypeConfiguration<T> where T : GymUser
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.Property(U => U.Name)
                   .HasColumnType("varchar")
                   .HasMaxLength(50);
            builder.Property(U => U.Email)
                 .HasColumnType("varchar")
                 .HasMaxLength(100);

            builder.Property(U => U.Phone)
               .HasColumnType("varchar")
               .HasMaxLength(11);

            builder.HasIndex(U => U.Phone).IsUnique();
            builder.HasIndex(U => U.Email).IsUnique();


            builder.OwnsOne(U => U.Address, address =>
            {
                address.Property(A => A.Street).HasColumnName("Street").HasColumnType("varchar(30)");
                address.Property(A => A.City).HasColumnName("City").HasColumnType("varchar(30)");
                address.Property(A => A.BuildingNumber).HasColumnName("BuildingNumber");
            });

            builder.ToTable(tb =>
            {
                tb.HasCheckConstraint("EmailCheck", "Email like '_%@_%._%'");
                tb.HasCheckConstraint("PhoneCheck", "Phone like '010%' or Phone like '011%' or Phone like '012%' or Phone like '015%'");
            });

        }
    }
}
