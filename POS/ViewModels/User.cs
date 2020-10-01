using POS.Models.Enums;

namespace POS.ViewModels
{
    public class SearchViewModel
    {
        public string Title { get; set; }

        public int Id { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string ResolvePropertyNames { get; set; }
    }
}
