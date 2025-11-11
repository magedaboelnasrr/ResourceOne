namespace ResourceOne.Models
{
    public class InsightsHubPageViewModel
    {
        public List<BlogViewModel> News { get; set; }
        public List<BlogViewModel> Blogs { get; set; }

        public int NewsPageNumber { get; set; }
        public int NewsTotalPages { get; set; }

        public int BlogsPageNumber { get; set; }
        public int BlogsTotalPages { get; set; }
    }
}
