using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Statistics.Infrastructure.Entities;

namespace Statistics.Infrastructure.Configuration
{
    public class CallConfiguration : IEntityTypeConfiguration<CallInfo>
    {
        public void Configure(EntityTypeBuilder<CallInfo> builder)
        {
            builder
                 .Property(c => c.Id)
                 .ValueGeneratedOnAdd();

            builder
               .Property(c => c.Count)
               .IsRequired();

            builder.
                Property(c => c.CreatedAt)
                .IsRequired();

            builder.
                Property(c => c.BeginDateTime)
                .IsRequired();

            builder.
                Property(c => c.EndDateTime)
                .IsRequired();           
        }
    }
}
