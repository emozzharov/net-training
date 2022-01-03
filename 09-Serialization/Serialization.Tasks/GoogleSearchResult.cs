﻿
using System.Runtime.Serialization;

namespace Serialization.Tasks
{

    // TODO: Implement GoogleSearchResult class to be deserialized from Google Search API response
    // Specification is available at: https://developers.google.com/custom-search/v1/using_rest#WorkingResults
    // The test json file is at Serialization.Tests\Resources\GoogleSearchJson.txt


    [DataContract]
    public class GoogleSearchResult
    {
        [DataMember(Name = "kind")]
        public string Kind { get; set; }

        [DataMember(Name = "context")]
        public Context Context { get; set; }

        [DataMember(Name = "url")]
        public Url Url { get; set; }

        [DataMember(Name = "queries")]
        public Queries Queries { get; set; }

        [DataMember(Name = "items")]
        public Item[] Items { get; set; }
    }

    [DataContract]
    public class Context
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }
    }

    [DataContract]
    public class Url
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "template")]
        public string Template { get; set; }
    }

    [DataContract]
    public class Queries
    {
        [DataMember(Name = "request")]//?
        public Request[] Request { get; set; }

        [DataMember(Name = "nextPage")]//?
        public NextPage[] NextPage { get; set; }

        [DataMember(Name = "previousPage")]
        public PreviousPage PreviousPage { get; set; }
    }

    [DataContract]
    public class Request
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "totalResults")]
        public long TotalResults { get; set; }

        [DataMember(Name = "searchTerms")]
        public string SearchTerms { get; set; }

        [DataMember(Name = "count")]
        public int Count { get; set; }

        [DataMember(Name = "startIndex")]
        public int StartIndex { get; set; }

        [DataMember(Name = "inputEncoding")]
        public string InputEncoding { get; set; }

        [DataMember(Name = "outputEncoding")]
        public string OutputEncoding { get; set; }

        [DataMember(Name = "cx")]
        public string Cx { get; set; }
    }

    [DataContract]
    public class NextPage : Request
    {
    }

    [DataContract]
    public class PreviousPage
    {
    }

    [DataContract]
    public class Item
    {
        [DataMember(Name = "kind")]
        public string Kind { get; set; }

        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "htmlTitle")]
        public string HtmlTitle { get; set; }

        [DataMember(Name = "link")]
        public string Link { get; set; }

        [DataMember(Name = "displayLink")]
        public string DisplayLink { get; set; }

        [DataMember(Name = "snippet")]
        public string Snippet { get; set; }

        [DataMember(Name = "htmlSnippet")]
        public string HtmlSnippet { get; set; }
    }
}
