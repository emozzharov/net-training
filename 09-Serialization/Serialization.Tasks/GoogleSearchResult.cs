
namespace Serialization.Tasks
{

    // TODO: Implement GoogleSearchResult class to be deserialized from Google Search API response
    // Specification is available at: https://developers.google.com/custom-search/v1/using_rest#WorkingResults
    // The test json file is at Serialization.Tests\Resources\GoogleSearchJson.txt



    public class GoogleSearchResult
    {
    }

    public class Rootobject
    {
        public string kind { get; set; }
        public Url url { get; set; }
        public Queries queries { get; set; }
        public Context context { get; set; }
        public Item[] items { get; set; }
    }

    public class Url
    {
        public string type { get; set; }
        public string template { get; set; }
    }

    public class Queries
    {
        public Nextpage[] nextPage { get; set; }
        public Request[] request { get; set; }
    }

    public class Nextpage
    {
        public string title { get; set; }
        public int totalResults { get; set; }
        public string searchTerms { get; set; }
        public int count { get; set; }
        public int startIndex { get; set; }
        public string inputEncoding { get; set; }
        public string outputEncoding { get; set; }
        public string cx { get; set; }
    }

    public class Request
    {
        public string title { get; set; }
        public int totalResults { get; set; }
        public string searchTerms { get; set; }
        public int count { get; set; }
        public int startIndex { get; set; }
        public string inputEncoding { get; set; }
        public string outputEncoding { get; set; }
        public string cx { get; set; }
    }

    public class Context
    {
        public string title { get; set; }
    }

    public class Item
    {
        public string kind { get; set; }
        public string title { get; set; }
        public string htmlTitle { get; set; }
        public string link { get; set; }
        public string displayLink { get; set; }
        public string snippet { get; set; }
        public string htmlSnippet { get; set; }
        public Pagemap pagemap { get; set; }
    }

    public class Pagemap
    {
        public RTO[] RTO { get; set; }
    }

    public class RTO
    {
        public string format { get; set; }
        public string group_impression_tag { get; set; }
        public string Optmax_rank_top { get; set; }
        public string Optthreshold_override { get; set; }
        public string Optdisallow_same_domain { get; set; }
        public string Outputtitle { get; set; }
        public string Outputwant_title_on_right { get; set; }
        public string Outputnum_lines1 { get; set; }
        public string Outputtext1 { get; set; }
        public string Outputgray1b { get; set; }
        public string Outputno_clip1b { get; set; }
        public string UrlOutputurl2 { get; set; }
        public string Outputlink2 { get; set; }
        public string Outputtext2b { get; set; }
        public string UrlOutputurl2c { get; set; }
        public string Outputlink2c { get; set; }
        public string result_group_header { get; set; }
        public string Outputimage_url { get; set; }
        public string image_size { get; set; }
        public string Outputinline_image_width { get; set; }
        public string Outputinline_image_height { get; set; }
        public string Outputimage_border { get; set; }
    }

}
