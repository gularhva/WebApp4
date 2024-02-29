using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebApp4.Entities;

namespace WebApp4.Configurations
{
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.Property(x => x.School_Id).IsRequired();
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
        }
    }
}
