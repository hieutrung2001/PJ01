
namespace PJ01.Domain.Entities
{
    public class Class : BaseEntity
    {
        public string Name { get; set; }
        
        public IList<StudentClass> StudentClasses { get; set; }
    }
}
