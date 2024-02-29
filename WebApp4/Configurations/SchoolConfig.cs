using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp4.Entities;

namespace WebApp4.Configurations
{
    public class SchoolConfig : IEntityTypeConfiguration<School>
    {
        public void Configure(EntityTypeBuilder<School> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
        }
    }
}
