using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Portfolio.Domain.Entity;

namespace Portfolio.Infrastructure.Configurations
{
    public class TechnologyConfiguration : IEntityTypeConfiguration<Technology>
    {
        //techonogies table konfigurálása, requestek
        public void Configure(EntityTypeBuilder<Technology> builder)
        {
            builder.ToTable("Technologies");
            builder.HasKey(t => t.TechnologyId);
            builder.Property(t => t.TechnologyName).IsRequired().HasMaxLength(50);

        }
    }
}