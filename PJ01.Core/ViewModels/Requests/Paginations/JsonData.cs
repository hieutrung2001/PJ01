
namespace PJ01.Core.ViewModels.Paginations
{
    public class JsonData<T> where T : class
    {
        public int Draw { get; set; }
        public int RecordsFiltered { get; set; }
        public int RecordsTotal { get; set; }
        public List<T> Data { get; set; }
    }
}
