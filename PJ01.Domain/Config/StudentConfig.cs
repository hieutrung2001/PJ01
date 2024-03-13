using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PJ01.Domain.Entities;

namespace PJ01.Domain.Config
{
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {

        void IEntityTypeConfiguration<Student>.Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable(nameof(Student));
            builder.HasKey(x => x.Id);
        }
    }
}
