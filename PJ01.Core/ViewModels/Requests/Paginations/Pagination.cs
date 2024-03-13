
namespace PJ01.Core.ViewModels.Paginations
{
    public class Pagination
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public Search Search { get; set; }
        public List<Order> Order { get; set; }
        public List<Columns> Columns { get; set; }

    }

    public class Order
    {
        public int Column { get; set; }
        public string Dir { get; set; }
    }

    public class Search {
        public string Value {  get; set; }
    }

    public class Columns
    {
        public string Name { get; set; }
        public bool Searchable { get; set; }
        public bool Orderable { get; set; }
        public string Search { get; set; }
        public string Value { get; set; }
        public bool Regex { get; set; }
        public string Data { get; set; }

    }
}
