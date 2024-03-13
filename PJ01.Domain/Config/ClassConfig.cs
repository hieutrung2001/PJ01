using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PJ01.Domain.Entities;

namespace PJ01.Domain.Config
{
    public class ClassConfig : IEntityTypeConfiguration<Class>
    {
        void IEntityTypeConfiguration<Class>.Configure(EntityTypeBuilder<Class> builder)
        {
            builder.ToTable(nameof(Class));
            builder.HasKey(x => x.Id);
        }
    }
}
