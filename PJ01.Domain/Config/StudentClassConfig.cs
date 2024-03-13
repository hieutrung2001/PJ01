using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PJ01.Domain.Entities;

namespace PJ01.Domain.Config
{
    public class StudentClassConfig : IEntityTypeConfiguration<StudentClass>
    {
        void IEntityTypeConfiguration<StudentClass>.Configure(EntityTypeBuilder<StudentClass> builder)
        {
            builder.ToTable(nameof(StudentClass));
        }
    }
}
