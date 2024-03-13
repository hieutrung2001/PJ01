
namespace PJ01.Domain.Entities
{
    public class Student : BaseEntity
    {
        public string FullName { get; set; }
        public DateOnly Dob { get; set; }
        public int PhoneNumber { get; set; }
        public string Address { get; set; }

        public IList<StudentClass> StudentClasses { get; set; }
    }
}
