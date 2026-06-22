//using GymManagementSystem.DAL.Models;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Metadata.Builders;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace GymManagementSystem.DAL.Configurations
//{
//    public class MemberConfiguration : GymUserConfiguration<Member>, IEntityTypeConfiguration<Member>
//    {
//        public new void Configure(EntityTypeBuilder<Member> builder)
//        {
//            builder.Property(M => M.CreatedAt)
//                .HasColumnName("JoinDate")
//                .HasDefaultValueSql("GETDATE()");

//            base.Configure(builder);


//        }
//    }
//}


using GymManagementSystem.DAL.Configurations;
using GymManagementSystem.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace GymManagement.DAL.Configurations
{
    public class MemberConfiguration : GymUserConfiguration<Member>, IEntityTypeConfiguration<Member>
    {
        public new void Configure(EntityTypeBuilder<Member> builder)
        {
            builder.Property(M => M.CreatedAt)
                   .HasColumnName("JoinDate")
                   .HasDefaultValueSql("GETDATE()");

            base.Configure(builder);
        }
    }
}
