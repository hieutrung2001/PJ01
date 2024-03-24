using PJ01.Core.ViewModels.Requests.Classes;
using PJ01.Domain.Entities;

namespace PJ01.Core.ViewModels.Requests.Students
{
    public class IndexModel
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime Dob { get; set; }
        public int PhoneNumber { get; set; }
        //public List<StudentClass> StudentClasses { get; set; }
        public string Address { get; set; }
        public List<Class> Classes { get; set; }
    }
}
