using PJ01.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace PJ01.Core.ViewModels.Requests.Classes
{
    public class IndexModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public IList<StudentClass> StudentClasses { get; set; }
    }
}
