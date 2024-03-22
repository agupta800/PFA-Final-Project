using PagedList;
using PFA.BlogModel;


namespace PFA.BlogVM
{
    public class HomeVM
    {
        public string? Title { get; set; }
        public string? ShortDescription { get; set; }
        public string? ThumbnailUrl { get; set; }
        //public IPagedList<Post>? Posts { get; set; }
        public X.PagedList.IPagedList<BlogModel.Post> Posts { get; set; }

    }
}
