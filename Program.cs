using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Net.Mail;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;

namespace Brawler
{
    /*
    #region Subscriber Class Definition
    [Serializable()]
    public class Subscriber
    {
        public String email { get; set; }
        public List<Post> sentBlogs;
        public List<String> genreList;
        public List<String> blogList;
        public List<String> genreBlackList;
        public List<String> blogBlackList;

        public Subscriber(String email)
        {
            this.email = email;
            this.sentBlogs = new List<Post>;
            this.genreList = new List<String>;
            this.blogList = new List<String>;
            this.genreBlackList = new List<String>;
            this.blogBlackList = new List<String>;
        }

        //Deserialization constructor.
        public Post(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            email = (String)info.GetValue("email", typeof(String));
            author = (String)info.GetValue("author", typeof(String));
            publishDate = (DateTime)info.GetValue("publishDate", typeof(DateTime));
            textPreview = (String)info.GetValue("textPreview", typeof(String));
            weblink = (String)info.GetValue("weblink", typeof(String));
            blogName = (String)info.GetValue("blogName", typeof(String));
            blogCategory = (String)info.GetValue("blogCategory", typeof(String));
            sendDate = (DateTime)info.GetValue("sendDate", typeof(DateTime));
        }

        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("title", title);
            info.AddValue("author", author);
            info.AddValue("publishDate", publishDate);
            info.AddValue("textPreview", textPreview);
            info.AddValue("weblink", weblink);
            info.AddValue("blogName", blogName);
            info.AddValue("blogCategory", blogCategory);
            info.AddValue("sendDate", sendDate);
        }

        public override string ToString()
        {
            return String.Format("TITLE: {0}\nAUTHOR: {1}\nPUBLISH DATE: {2}\nTEXT PREVIEW: {3}\nWEB LINK: {4}\nBLOG NAME: {5}\nBLOG CATEGORY: {6}\nSEND DATE: {7}\n\n"
                , this.title, this.author, this.publishDate.ToString("MM/dd/yyyy"), this.textPreview, this.weblink, this.blogName, this.blogCategory, this.sendDate);
        }

        public override bool Equals(object obj)
        {
            Post p = obj as Post;
            return (p.email == email);
        }
    }
    #endregion
    */

    #region Post Class Definition
    [Serializable()]
    public class Post
    {
        public String title { get; set; }
        public String author { get; set; }
        public DateTime publishDate { get; set; }
        public String textPreview { get; set; }
        public String weblink { get; set; }
        public String blogName { get; set; }
        public String blogCategory { get; set; }
        public DateTime sendDate { get; set; }

        public Post()
        {
            this.title = "UNDEFINED";
        }

        public Post(String title, String author, DateTime publishDate, String textPreview, String weblink, String blogName, String blogCategory)
        {
            this.title = title;
            this.author = author;
            this.publishDate = publishDate;
            this.textPreview = textPreview;
            this.weblink = weblink;
            this.blogName = blogName;
            this.blogCategory = blogCategory;
            this.sendDate = sendDate;
        }

        //Deserialization constructor.
        public Post(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            title = (String)info.GetValue("title", typeof(String));
            author = (String)info.GetValue("author", typeof(String));
            publishDate = (DateTime)info.GetValue("publishDate", typeof(DateTime));
            textPreview = (String)info.GetValue("textPreview", typeof(String));
            weblink = (String)info.GetValue("weblink", typeof(String));
            blogName = (String)info.GetValue("blogName", typeof(String));
            blogCategory = (String)info.GetValue("blogCategory", typeof(String));
            sendDate = (DateTime)info.GetValue("sendDate", typeof(DateTime));
        }

        //Serialization function.
        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            info.AddValue("title", title);
            info.AddValue("author", author);
            info.AddValue("publishDate", publishDate);
            info.AddValue("textPreview", textPreview);
            info.AddValue("weblink", weblink);
            info.AddValue("blogName", blogName);
            info.AddValue("blogCategory", blogCategory);
            info.AddValue("sendDate", sendDate);
        }

        public override string ToString()
        {
            return String.Format("TITLE: {0}\nAUTHOR: {1}\nPUBLISH DATE: {2}\nTEXT PREVIEW: {3}\nWEB LINK: {4}\nBLOG NAME: {5}\nBLOG CATEGORY: {6}\nSEND DATE: {7}\n\n"
                , this.title, this.author, this.publishDate.ToString("MM/dd/yyyy"), this.textPreview
                , this.weblink, this.blogName, this.blogCategory, this.sendDate);
        }

        public string ToHTMLString()
        {
            return String.Format("<h2>{0}</h2><i>by {1} &#8226; {2}</i> &#8226; {3} &#8226; {4}<br><br>{5} {6}<br><br><br>"
                , this.title, this.author, this.publishDate.ToString("MM/dd/yyyy"), this.blogName
                , this.blogCategory, this.textPreview, this.weblink);
        }

        public override bool Equals(object obj)
        {
            Post p = obj as Post;
            return ((p.title == title || p.weblink == weblink) && p.blogName == blogName);
        }
    }
    #endregion

    public class Program
    {
        public static List<Post> blogList = new List<Post>();
        public static List<Post> sentBlogList = new List<Post>();
        public static bool TESTING = true;

        static void Main(string[] args)
        {
            string msg = "";
            gatherAllBlogs();

            if (!TESTING)
            {
                readSentEntries();

                //REMOVE ANY PREVIOUSLY SENT BLOGS
                foreach (Post p in sentBlogList)
                    blogList.Remove(p);

                foreach (Post p in blogList)
                    msg += p.ToHTMLString();

                sendEmail("schmittycommittee@gmail.com", "Blog Catch Up As Of "
                        + DateTime.Now.ToString("yyyy/MM/dd"), msg);

                //LOG ALL SENT ENTRIES
                writeSentEntries(blogList.Union(sentBlogList).ToList());
            }
            else
            {
                foreach (Post p in blogList)
                    Console.WriteLine(p.ToString());
            }
            Console.WriteLine("\nFINISHED ... BLOG COUNT: " + blogList.Count);
            Console.ReadKey();
        }

        public static void gatherAllBlogs()
        {
            if (TESTING)
            {
                gatherEntriesGeneric(blogAddress: "http://ben-evans.com/"
                                                        , beginReadAllIndicator: "<div class=\"main-content\">"
                                                        , endReadAllIndicator: "<!--PAGINATION-->"
                                                        , endCurrentEntryIndicator: "<!--POST FOOTER-->"
                                                        , beginReadAuthorIndicator: "DEFAULT"
                                                        , beginReadAuthorOffset: 0
                                                        , endReadAuthorIndicator: "DEFAULT"
                                                        , endReadAuthorOffset: 0

                                                        , beginReadTitleWeblinkIndicator: "        <a href=\"/benedictevans/"
                                                        , beginReadTitleIndicator: "\">"
                                                        , beginReadTitleOffset: 2
                                                        , endReadTitleIndicator: "<"
                                                        , endReadTitleOffset: 0

                                                        , beginReadWeblinkIndicator: "href=\""
                                                        , beginReadWeblinkOffset: 6
                                                        , endReadWeblinkIndicator: "\""
                                                        , endReadWeblinkOffset: 0

                                                        , beginReadDateIndicator: "<time datetime="
                                                        , beginReadDateOffset: 16
                                                        , endReadDateIndicator: "\""
                                                        , endReadDateOffset: 0
                                                        , dateFormat: "yyyy-MM-dd"

                                                        , beginReadPreviewIndicator: "<!--POST BODY-->"
                                                        , beginReadPreviewOffset: 0
                                                        , endReadPreviewIndicator: "<!--POST FOOTER-->"
                                                        , endReadPreviewOffset: 0

                                                        , defaultTitle: ""
                                                        , defaultAuthor: "Ben Evans"
                                                        , defaultTextPreview: ""
                                                        , defaultWeblink: ""
                                                        , defaultBlogName: "Benedict Evans Blog"
                                                        , defaultBlogCategory: "BUSINESS"
                                                        , weblinkPrefix: "http://ben-evans.com"
                                                        );
            }
            else
            {
                #region Poorly Design Gatherer Calls
                gatherRBloggersEntries();
                gatherRevolutionsEntries();
                gatherWalkingRandomlyEntries();
                gatherThoughtCatalogEntries();
                gatherLanguageLogEntries();
                gatherSimplyStatisticsEntries();
                gatherHilaryMasonEntries();
                gatherIdibonEntries();
                gatherGapMinderEntries();
                gatherApaxoEntries();
                gatherBrainPickingsEntries();
                gatherTheRumpusEntries();
                gatherThatJeffSmithEntries();
                gatherFlowingDataEntries();
                #endregion

                #region Generic Procedure Gatherer Calls

                gatherEntriesGeneric(blogAddress: "http://ben-evans.com/"
                                                        , beginReadAllIndicator: "<div class=\"main-content\">"
                                                        , endReadAllIndicator: "<!--PAGINATION-->"
                                                        , endCurrentEntryIndicator: "<!--POST FOOTER-->"
                                                        , beginReadAuthorIndicator: "DEFAULT"
                                                        , beginReadAuthorOffset: 0
                                                        , endReadAuthorIndicator: "DEFAULT"
                                                        , endReadAuthorOffset: 0

                                                        , beginReadTitleWeblinkIndicator: "        <a href=\"/benedictevans/"
                                                        , beginReadTitleIndicator: "\">"
                                                        , beginReadTitleOffset: 2
                                                        , endReadTitleIndicator: "<"
                                                        , endReadTitleOffset: 0

                                                        , beginReadWeblinkIndicator: "href=\""
                                                        , beginReadWeblinkOffset: 6
                                                        , endReadWeblinkIndicator: "\""
                                                        , endReadWeblinkOffset: 0

                                                        , beginReadDateIndicator: "<time datetime="
                                                        , beginReadDateOffset: 16
                                                        , endReadDateIndicator: "\""
                                                        , endReadDateOffset: 0
                                                        , dateFormat: "yyyy-MM-dd"

                                                        , beginReadPreviewIndicator: "<!--POST BODY-->"
                                                        , beginReadPreviewOffset: 0
                                                        , endReadPreviewIndicator: "<!--POST FOOTER-->"
                                                        , endReadPreviewOffset: 0

                                                        , defaultTitle: ""
                                                        , defaultAuthor: "Ben Evans"
                                                        , defaultTextPreview: ""
                                                        , defaultWeblink: ""
                                                        , defaultBlogName: "Benedict Evans Blog"
                                                        , defaultBlogCategory: "BUSINESS"
                                                        , weblinkPrefix: "http://ben-evans.com"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://thebillfold.com/"
                                                        , beginReadAllIndicator: "<!-- header-wrap -->"
                                                        , endReadAllIndicator: "<!-- /#content -->"
                                                        , endCurrentEntryIndicator: "<!-- /post -->"
                                                        , beginReadAuthorIndicator: "rel=\"author\">"
                                                        , beginReadAuthorOffset: 13
                                                        , endReadAuthorIndicator: "</a>"
                                                        , endReadAuthorOffset: 0

                                                        , beginReadTitleWeblinkIndicator: "<h2>"
                                                        , beginReadTitleIndicator: "\">"
                                                        , beginReadTitleOffset: 2
                                                        , endReadTitleIndicator: "<"
                                                        , endReadTitleOffset: 0

                                                        , beginReadWeblinkIndicator: "href=\""
                                                        , beginReadWeblinkOffset: 6
                                                        , endReadWeblinkIndicator: "\""
                                                        , endReadWeblinkOffset: 0

                                                        , beginReadDateIndicator: "DEFAULT"
                                                        , beginReadDateOffset: 0
                                                        , endReadDateIndicator: "DEFAULT"
                                                        , endReadDateOffset: 0
                                                        , dateFormat: "yyyy/MM/dd"

                                                        , beginReadPreviewIndicator: "<div class=\"entry clearfix\">"
                                                        , beginReadPreviewOffset: 0
                                                        , endReadPreviewIndicator: "<!-- /entry -->"
                                                        , endReadPreviewOffset: 0

                                                        , defaultTitle: ""
                                                        , defaultAuthor: ""
                                                        , defaultTextPreview: ""
                                                        , defaultWeblink: ""
                                                        , defaultBlogName: "The Billfold"
                                                        , defaultBlogCategory: "FINANCE"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://splitsider.com/"
                                                        , beginReadAllIndicator: "<div id=\"main\">"
                                                        , endReadAllIndicator: "<!-- /main -->"
                                                        , endCurrentEntryIndicator: "<!-- /post -->"
                                                        , beginReadAuthorIndicator: "rel=\"author\">"
                                                        , beginReadAuthorOffset: 13
                                                        , endReadAuthorIndicator: "</a>"
                                                        , endReadAuthorOffset: 0

                                                        , beginReadTitleWeblinkIndicator: "<h2>"
                                                        , beginReadTitleIndicator: "\">"
                                                        , beginReadTitleOffset: 2
                                                        , endReadTitleIndicator: "<"
                                                        , endReadTitleOffset: 0

                                                        , beginReadWeblinkIndicator: "href=\""
                                                        , beginReadWeblinkOffset: 6
                                                        , endReadWeblinkIndicator: "\""
                                                        , endReadWeblinkOffset: 0

                                                        , beginReadDateIndicator: "DEFAULT"
                                                        , beginReadDateOffset: 0
                                                        , endReadDateIndicator: "DEFAULT"
                                                        , endReadDateOffset: 0
                                                        , dateFormat: "yyyy/MM/dd"

                                                        , beginReadPreviewIndicator: "<div class=\"entry clearfix\">"
                                                        , beginReadPreviewOffset: 0
                                                        , endReadPreviewIndicator: "<!-- /entry -->"
                                                        , endReadPreviewOffset: 0

                                                        , defaultTitle: ""
                                                        , defaultAuthor: ""
                                                        , defaultTextPreview: ""
                                                        , defaultWeblink: ""
                                                        , defaultBlogName: "The Splitsider"
                                                        , defaultBlogCategory: "COMEDY"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://www.theawl.com/"
                                                        , beginReadAllIndicator: "<!-- header -->"
                                                        , endReadAllIndicator: "<!-- /content -->"
                                                        , endCurrentEntryIndicator: "<!-- /post -->"
                                                        , beginReadAuthorIndicator: "rel=\"author\">"
                                                        , beginReadAuthorOffset: 13
                                                        , endReadAuthorIndicator: "</a>"
                                                        , endReadAuthorOffset: 0

                                                        , beginReadTitleWeblinkIndicator: "<h2>"
                                                        , beginReadTitleIndicator: "\">"
                                                        , beginReadTitleOffset: 2
                                                        , endReadTitleIndicator: "<"
                                                        , endReadTitleOffset: 0

                                                        , beginReadWeblinkIndicator: "href=\""
                                                        , beginReadWeblinkOffset: 6
                                                        , endReadWeblinkIndicator: "\""
                                                        , endReadWeblinkOffset: 0

                                                        , beginReadDateIndicator: "DEFAULT"
                                                        , beginReadDateOffset: 0
                                                        , endReadDateIndicator: "DEFAULT"
                                                        , endReadDateOffset: 0
                                                        , dateFormat: "yyyy/MM/dd"

                                                        , beginReadPreviewIndicator: "<div class=\"entry clearfix\">"
                                                        , beginReadPreviewOffset: 0
                                                        , endReadPreviewIndicator: "<!-- /entry -->"
                                                        , endReadPreviewOffset: 0

                                                        , defaultTitle: ""
                                                        , defaultAuthor: ""
                                                        , defaultTextPreview: ""
                                                        , defaultWeblink: ""
                                                        , defaultBlogName: "The Awl"
                                                        , defaultBlogCategory: "GENERAL"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://thehairpin.com/"
                                                        , beginReadAllIndicator: "<!-- #header -->"
                                                        , endReadAllIndicator: "<!-- #content -->"
                                                        , endCurrentEntryIndicator: "<!-- .post -->"
                                                        , beginReadAuthorIndicator: "<a title="
                                                        , beginReadAuthorOffset: 13
                                                        , endReadAuthorIndicator: "\""
                                                        , endReadAuthorOffset: 0

                                                        , beginReadTitleWeblinkIndicator: "<h2>"
                                                        , beginReadTitleIndicator: "\">"
                                                        , beginReadTitleOffset: 2
                                                        , endReadTitleIndicator: "<"
                                                        , endReadTitleOffset: 0

                                                        , beginReadWeblinkIndicator: "href=\""
                                                        , beginReadWeblinkOffset: 6
                                                        , endReadWeblinkIndicator: "\""
                                                        , endReadWeblinkOffset: 0

                                                        , beginReadDateIndicator: "<h3 class=\"date\">"
                                                        , beginReadDateOffset: 17
                                                        , endReadDateIndicator: "</h3>"
                                                        , endReadDateOffset: 0
                                                        , dateFormat: "yyyy/MM/dd"

                                                        , beginReadPreviewIndicator: "<div class=\"entry clearfix\">"
                                                        , beginReadPreviewOffset: 0
                                                        , endReadPreviewIndicator: "<!-- .post -->"
                                                        , endReadPreviewOffset: 0

                                                        , defaultTitle: ""
                                                        , defaultAuthor: ""
                                                        , defaultTextPreview: ""
                                                        , defaultWeblink: ""
                                                        , defaultBlogName: "The Hairpin"
                                                        , defaultBlogCategory: "GENERAL"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://planet.python.org/"
                                                        , beginReadAllIndicator: "<h1 class=\"pageheading\">Planet Python</h1>"
                                                        , endReadAllIndicator: "<div id=\"left-hand-navigation\">"
                                                        , endCurrentEntryIndicator: "</em>"
                                                        , beginReadAuthorIndicator: "DEFAULT"
                                                        , beginReadAuthorOffset: 0
                                                        , endReadAuthorIndicator: "DEFAULT"
                                                        , endReadAuthorOffset: 0

                                                        , beginReadTitleWeblinkIndicator: "<h4>"
                                                        , beginReadTitleIndicator: "\">"
                                                        , beginReadTitleOffset: 2
                                                        , endReadTitleIndicator: "<"
                                                        , endReadTitleOffset: 0

                                                        , beginReadWeblinkIndicator: "<a href=\""
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , endReadWeblinkOffset: 0

                                                        , beginReadDateIndicator: "DEFAULT"
                                                        , beginReadDateOffset: 0
                                                        , endReadDateIndicator: "DEFAULT"
                                                        , endReadDateOffset: 0
                                                        , dateFormat: "yyyy/MM/dd"

                                                        , beginReadPreviewIndicator: "<p>"
                                                        , beginReadPreviewOffset: 0
                                                        , endReadPreviewIndicator: "<em>"
                                                        , endReadPreviewOffset: 0

                                                        , defaultTitle: ""
                                                        , defaultAuthor: ""
                                                        , defaultTextPreview: ""
                                                        , defaultWeblink: ""
                                                        , defaultBlogName: "Planet Python"
                                                        , defaultBlogCategory: "PYTHON"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://inventwithpython.com/blog/"
                                                        , beginReadAllIndicator: "<h2 class=\"pagetitle\">Latest posts.</h2>"
                                                        , endReadAllIndicator: "<div class=\"navigation\">"
                                                        , endCurrentEntryIndicator: "Categories: <a href=\"http://"
                                                        , beginReadAuthorIndicator: "title=\"Posts by "
                                                        , beginReadAuthorOffset: 16
                                                        , endReadAuthorIndicator: "\""
                                                        , endReadAuthorOffset: 0

                                                        , beginReadTitleWeblinkIndicator: "<h3>"
                                                        , beginReadTitleIndicator: "title=\"Permanent Link to "
                                                        , beginReadTitleOffset: 25
                                                        , endReadTitleIndicator: "\""
                                                        , endReadTitleOffset: 0

                                                        , beginReadWeblinkIndicator: "<a href=\""
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , endReadWeblinkOffset: 0

                                                        , beginReadDateIndicator: "<h3><a href=\"http://inventwithpython.com/blog/"
                                                        , beginReadDateOffset: 46
                                                        , endReadDateIndicator: "/"
                                                        , endReadDateOffset: 8
                                                        , dateFormat: "yyyy/MM/dd"

                                                        , beginReadPreviewIndicator: "<div class=\"entry\">"
                                                        , beginReadPreviewOffset: 0
                                                        , endReadPreviewIndicator: "<p class=\"postmetadata\">"
                                                        , endReadPreviewOffset: 0

                                                        , defaultTitle: ""
                                                        , defaultAuthor: ""
                                                        , defaultTextPreview: ""
                                                        , defaultWeblink: ""
                                                        , defaultBlogName: "Invent With Python"
                                                        , defaultBlogCategory: "PYTHON"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://programming.oreilly.com/"
                                                        , beginReadAllIndicator: "div id=\"content-inner\">"
                                                        , endReadAllIndicator: "<!-- #nav-below -->"
                                                        , endCurrentEntryIndicator: "<!-- .entry-content -->"
                                                        , beginReadAuthorIndicator: "title=\"View all posts by "
                                                        , beginReadAuthorOffset: 25
                                                        , endReadAuthorIndicator: "\">"
                                                        , beginReadTitleWeblinkIndicator: "<h2 class"
                                                        , beginReadTitleIndicator: "target=\"_self\">"
                                                        , beginReadTitleOffset: 15
                                                        , endReadTitleIndicator: "</a>"
                                                        , beginReadWeblinkIndicator: "<a href=\""
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , beginReadDateIndicator: "<span class=\"entry-date\">"
                                                        , beginReadDateOffset: 25
                                                        , endReadDateIndicator: "</span>"
                                                        , dateFormat: "MMMM d, yyyy"
                                                        , beginReadPreviewIndicator: "<div class=\"entry-summary entry-blurb\">"
                                                        , endReadPreviewIndicator: "</div>"
                                                        , defaultBlogName: "O'Reilly Programming"
                                                        , defaultBlogCategory: "PROGRAMMING"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://strata.oreilly.com/"
                                                        , beginReadAllIndicator: "div id=\"content-inner\">"
                                                        , endReadAllIndicator: "<!-- #nav-below -->"
                                                        , endCurrentEntryIndicator: "<!-- .entry-content -->"
                                                        , beginReadAuthorIndicator: "title=\"View all posts by "
                                                        , beginReadAuthorOffset: 25
                                                        , endReadAuthorIndicator: "\">"
                                                        , beginReadTitleWeblinkIndicator: "<h2 class"
                                                        , beginReadTitleIndicator: "target=\"_self\">"
                                                        , beginReadTitleOffset: 15
                                                        , endReadTitleIndicator: "</a>"
                                                        , beginReadWeblinkIndicator: "<a href=\""
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , beginReadDateIndicator: "<span class=\"entry-date\">"
                                                        , beginReadDateOffset: 25
                                                        , endReadDateIndicator: "</span>"
                                                        , dateFormat: "MMMM d, yyyy"
                                                        , beginReadPreviewIndicator: "<div class=\"entry-summary entry-blurb\">"
                                                        , endReadPreviewIndicator: "</div>"
                                                        , defaultBlogName: "O'Reilly Data"
                                                        , defaultBlogCategory: "DATA"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://creativeandlive.com/categories/design/articles"
                                                        , beginReadAllIndicator: "<div id='content'>"
                                                        , endReadAllIndicator: "<div class=\"pagination\">"
                                                        , endCurrentEntryIndicator: "<div class='spacer'></div>"
                                                        , beginReadAuthorIndicator: "DEFAULTED"
                                                        , beginReadTitleWeblinkIndicator: "<img alt="
                                                        , beginReadTitleIndicator: "<img alt="
                                                        , beginReadTitleOffset: 10
                                                        , endReadTitleIndicator: "\""
                                                        , beginReadWeblinkIndicator: "<a href=\""
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , beginReadDateIndicator: "http://creativeandlive.com/archives/"
                                                        , beginReadDateOffset: 36
                                                        , endReadDateIndicator: "/"
                                                        , endReadDateOffset: 10
                                                        , dateFormat: "yyyy/MM/dd"
                                                        , beginReadPreviewIndicator: "<div class='desc'>"
                                                        , endReadPreviewIndicator: "</div>"
                                                        , defaultAuthor: "C&L"
                                                        , defaultBlogName: "Creative & Live"
                                                        , defaultBlogCategory: "DESIGN"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://www.johndcook.com/blog/"
                                                        , beginReadAllIndicator: "<!-- end of #header -->"
                                                        , endReadAllIndicator: "<!-- end of #content-blog -->"
                                                        , endCurrentEntryIndicator: "<div class=\"post-data\">"
                                                        , beginReadAuthorIndicator: "DEFAULTED"
                                                        , beginReadTitleWeblinkIndicator: "<h2 class=\"entry-title post-title\">"
                                                        , beginReadTitleIndicator: "rel=\"bookmark\">"
                                                        , beginReadTitleOffset: 15
                                                        , endReadTitleIndicator: "</a>"
                                                        , beginReadWeblinkIndicator: "<a href=\""
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , beginReadDateIndicator: "<span class=\"timestamp updated\">"
                                                        , beginReadDateOffset: 32
                                                        , endReadDateIndicator: "</span>"
                                                        , dateFormat: "d MMMM yyyy"
                                                        , beginReadPreviewIndicator: "<div class=\"post-entry\">"
                                                        , endReadPreviewIndicator: "<!-- end of .post-entry -->"
                                                        , defaultAuthor: "John Cook"
                                                        , defaultBlogName: "The Endeavour"
                                                        , defaultBlogCategory: "MATH"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://art-nerd.com/chicago/category/art-nerd-reviews/"
                                                        , beginReadAllIndicator: "<div class=\"postarea\">"
                                                        , endReadAllIndicator: "<div id=\"sidebar_right\">"
                                                        , endCurrentEntryIndicator: "<div class=\"postmeta\">"
                                                        , beginReadAuthorIndicator: "rel=\"author\">"
                                                        , beginReadAuthorOffset: 13
                                                        , endReadAuthorIndicator: "</a>"
                                                        , beginReadTitleWeblinkIndicator: "<h2>"
                                                        , beginReadTitleIndicator: "rel=\"bookmark\">"
                                                        , beginReadTitleOffset: 15
                                                        , endReadTitleIndicator: "</a>"
                                                        , beginReadWeblinkIndicator: "<a href=\""
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , beginReadDateIndicator: "</a> on "
                                                        , beginReadDateOffset: 8
                                                        , endReadDateIndicator: " &middot;"
                                                        , dateFormat: "MMMM d, yyyy"
                                                        , beginReadPreviewIndicator: "class=\"attachment-category-thumbnail wp-post-image"
                                                        , endReadPreviewIndicator: "[&hellip;]</p>"
                                                        , defaultBlogName: "Art Nerd: Reviews"
                                                        , defaultBlogCategory: "ART"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://art-nerd.com/chicago/category/get-it/"
                                                        , beginReadAllIndicator: "<div class=\"postarea\">"
                                                        , endReadAllIndicator: "<div id=\"sidebar_right\">"
                                                        , endCurrentEntryIndicator: "<div class=\"postmeta\">"
                                                        , beginReadAuthorIndicator: "rel=\"author\">"
                                                        , beginReadAuthorOffset: 13
                                                        , endReadAuthorIndicator: "</a>"
                                                        , beginReadTitleWeblinkIndicator: "<h2>"
                                                        , beginReadTitleIndicator: "rel=\"bookmark\">"
                                                        , beginReadTitleOffset: 15
                                                        , endReadTitleIndicator: "</a>"
                                                        , beginReadWeblinkIndicator: "<a href=\""
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , beginReadDateIndicator: "</a> on "
                                                        , beginReadDateOffset: 8
                                                        , endReadDateIndicator: " &middot;"
                                                        , dateFormat: "MMMM d, yyyy"
                                                        , beginReadPreviewIndicator: "class=\"attachment-category-thumbnail wp-post-image"
                                                        , endReadPreviewIndicator: "[&hellip;]</p>"
                                                        , defaultBlogName: "Art Nerd: #GETIT"
                                                        , defaultBlogCategory: "ART"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://art-nerd.com/chicago/category/obsessions/"
                                                        , beginReadAllIndicator: "<div class=\"postarea\">"
                                                        , endReadAllIndicator: "<div id=\"sidebar_right\">"
                                                        , endCurrentEntryIndicator: "<div class=\"postmeta\">"
                                                        , beginReadAuthorIndicator: "rel=\"author\">"
                                                        , beginReadAuthorOffset: 13
                                                        , endReadAuthorIndicator: "</a>"
                                                        , beginReadTitleWeblinkIndicator: "<h2>"
                                                        , beginReadTitleIndicator: "rel=\"bookmark\">"
                                                        , beginReadTitleOffset: 15
                                                        , endReadTitleIndicator: "</a>"
                                                        , beginReadWeblinkIndicator: "<a href=\""
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , beginReadDateIndicator: "</a> on "
                                                        , beginReadDateOffset: 8
                                                        , endReadDateIndicator: " &middot;"
                                                        , dateFormat: "MMMM d, yyyy"
                                                        , beginReadPreviewIndicator: "class=\"attachment-category-thumbnail wp-post-image"
                                                        , endReadPreviewIndicator: "[&hellip;]</p>"
                                                        , defaultBlogName: "Art Nerd: Obsession"
                                                        , defaultBlogCategory: "ART"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://art-nerd.com/chicago/category/whats-up/"
                                                        , beginReadAllIndicator: "<div class=\"postarea\">"
                                                        , endReadAllIndicator: "<div id=\"sidebar_right\">"
                                                        , endCurrentEntryIndicator: "<div class=\"postmeta\">"
                                                        , beginReadAuthorIndicator: "rel=\"author\">"
                                                        , beginReadAuthorOffset: 13
                                                        , endReadAuthorIndicator: "</a>"
                                                        , beginReadTitleWeblinkIndicator: "<h2>"
                                                        , beginReadTitleIndicator: "rel=\"bookmark\">"
                                                        , beginReadTitleOffset: 15
                                                        , endReadTitleIndicator: "</a>"
                                                        , beginReadWeblinkIndicator: "<a href=\""
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , beginReadDateIndicator: "</a> on "
                                                        , beginReadDateOffset: 8
                                                        , endReadDateIndicator: " &middot;"
                                                        , dateFormat: "MMMM d, yyyy"
                                                        , beginReadPreviewIndicator: "class=\"attachment-category-thumbnail wp-post-image"
                                                        , endReadPreviewIndicator: "[&hellip;]</p>"
                                                        , defaultBlogName: "Art Nerd: What's Up In Chicago?"
                                                        , defaultBlogCategory: "ART"
                                                        );

                #region Generic "52 Weeks of UX" Gathering
                gatherEntriesGeneric(blogAddress: "http://52weeksofux.com/"
                                                        , beginReadAllIndicator: "<!-- search results heading-->"
                                                        , endReadAllIndicator: "<!-- end of content -->"
                                                        , endCurrentEntryIndicator: "<!-- end hentry -->"
                                                        , beginReadAuthorIndicator: "<span class=\"hcard\">"
                                                        , beginReadAuthorOffset: 40
                                                        , endReadAuthorIndicator: "</span>"
                                                        , beginReadTitleWeblinkIndicator: "title=\"permanent link to this post\">"
                                                        , beginReadTitleIndicator: "title=\"permanent link to this post\">"
                                                        , beginReadTitleOffset: 36
                                                        , endReadTitleIndicator: "</a>"
                                                        , beginReadWeblinkIndicator: "<a href=\""
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , beginReadDateIndicator: "<span class=\"date\""
                                                        , beginReadDateOffset: 118
                                                        , endReadDateIndicator: "</span>"
                                                        , dateFormat: "MMMM d, yyyy"
                                                        , beginReadPreviewIndicator: "<div class=\"entry-content\">"
                                                        , endReadPreviewIndicator: "<div class=\"entry-info\">"
                                                        , defaultBlogName: "52 Weeks of UX"
                                                        , defaultBlogCategory: "UX"
                                                        );
                #endregion

                #region Generic "Innovation Playground" Gathering
                gatherEntriesGeneric(blogAddress: "http://mootee.typepad.com/"
                                                        , beginReadAllIndicator: "<!-- entry list sticky -->"
                                                        , endReadAllIndicator: "<!-- sidebar2 -->"
                                                        , endCurrentEntryIndicator: "<!-- post footer links -->"
                                                        , beginReadAuthorIndicator: "AUTHOR DEFAULTED"
                                                        , beginReadTitleWeblinkIndicator: "<h3 class=\"entry-header\">"
                                                        , beginReadTitleIndicator: ".html\">"
                                                        , beginReadTitleOffset: 7
                                                        , endReadTitleIndicator: "</a>"
                                                        , beginReadWeblinkIndicator: "<a href=\""
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , beginReadDateIndicator: "<h2 class=\"date-header\">"
                                                        , beginReadDateOffset: 24
                                                        , endReadDateIndicator: "</h2>"
                                                        , dateFormat: "MMMM dd, yyyy"
                                                        , beginReadPreviewIndicator: "<div class=\"entry-body\">"
                                                        , endReadPreviewIndicator: "<div class=\"entry-footer\">"
                                                        , defaultAuthor: "Idris Mootee"
                                                        , defaultBlogName: "Innovation Playground"
                                                        , defaultBlogCategory: "INNOVATION"
                                                        );
                #endregion

                #region Generic "xkcd" Gathering
                gatherEntriesGeneric(blogAddress: "http://blag.xkcd.com/"
                                                        , beginReadAllIndicator: "<!-- #nav-above -->"
                                                        , endReadAllIndicator: "<!-- #primary -->"
                                                        , endCurrentEntryIndicator: "</article>"
                                                        , beginReadAuthorIndicator: "AUTHOR DEFAULTED"
                                                        , beginReadTitleWeblinkIndicator: "<h1 class=\"entry-title\">"
                                                        , beginReadTitleIndicator: "rel=\"bookmark\">"
                                                        , beginReadTitleOffset: 15
                                                        , endReadTitleIndicator: "</a>"
                                                        , beginReadWeblinkIndicator: "<a href=\""
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , beginReadDateIndicator: "<time class=\"entry-date"
                                                        , beginReadDateOffset: 62
                                                        , endReadDateIndicator: "</time>"
                                                        , dateFormat: "MMMM d, yyyy"
                                                        , beginReadPreviewIndicator: "<div class=\"entry-content\">"
                                                        , endReadPreviewIndicator: "<!-- .entry-content -->"
                                                        , defaultAuthor: "Randall Munroe"
                                                        , defaultBlogName: "xkcd"
                                                        , defaultBlogCategory: "FUN"
                                                        );
                #endregion

                gatherEntriesGeneric(blogAddress: "http://cssanalytics.wordpress.com/"
                                                        , beginReadAllIndicator: "<!-- #masthead -->"
                                                        , endReadAllIndicator: "<!-- #content -->"
                                                        , endCurrentEntryIndicator: "<!-- #post-## -->"
                                                        , beginReadAuthorIndicator: "rel=\"author\">"
                                                        , beginReadAuthorOffset: 13
                                                        , endReadAuthorIndicator: "</a>"
                                                        , beginReadTitleWeblinkIndicator: "<h1 class=\"entry-title\">"
                                                        , beginReadTitleIndicator: "rel=\"bookmark\">"
                                                        , beginReadTitleOffset: 15
                                                        , endReadTitleIndicator: "</a>"
                                                        , beginReadWeblinkIndicator: "<a href=\""
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , beginReadDateIndicator: "<time class=\"entry-date\" datetime="
                                                        , beginReadDateOffset: 35
                                                        , endReadDateIndicator: "T"
                                                        , dateFormat: "yyyy-MM-dd"
                                                        , beginReadPreviewIndicator: "<div class=\"entry-content\">"
                                                        , endReadPreviewIndicator: "<!-- .entry-content -->"
                                                        , defaultBlogName: "CSSA - New Concepts in Quantitative Research"
                                                        , defaultBlogCategory: "QUANT"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://fastml.com/"
                                                        , beginReadAllIndicator: "<div id=\"main\">"
                                                        , endReadAllIndicator: "<aside class=\"sidebar\">"
                                                        , endCurrentEntryIndicator: "</article>"
                                                        , beginReadAuthorIndicator: "AUTHOR DEFAULTED"
                                                        , beginReadTitleWeblinkIndicator: "<h1 class=\"entry-title\">"
                                                        , beginReadTitleIndicator: "/\">"
                                                        , beginReadTitleOffset: 3
                                                        , endReadTitleIndicator: "<"
                                                        , beginReadWeblinkIndicator: "<a href=\""
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , beginReadDateIndicator: "<time datetime="
                                                        , beginReadDateOffset: 16
                                                        , endReadDateIndicator: "T"
                                                        , dateFormat: "yyyy-MM-dd"
                                                        , beginReadPreviewIndicator: "<div class=\"entry-content\">"
                                                        , endReadPreviewIndicator: "</div>"
                                                        , defaultAuthor: "Zygmunt Zajac"
                                                        , defaultBlogName: "FastML: Machine learning made easy"
                                                        , defaultBlogCategory: "MACHINE LEARNING"
                                                        , weblinkPrefix: "http://fastml.com"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://experiencecurve.com"
                                                        , beginReadAllIndicator: "<div id=\"main\">"
                                                        , endReadAllIndicator: "<div id=\"nav-below\" class=\"navigation\">"
                                                        , endCurrentEntryIndicator: "<!-- #post -->"
                                                        , beginReadAuthorIndicator: "title=\"View all posts by "
                                                        , beginReadAuthorOffset: 25
                                                        , endReadAuthorIndicator: "\""
                                                        , beginReadTitleWeblinkIndicator: "<h2 class=\"entry-title\">"
                                                        , beginReadTitleIndicator: "title=\"Permalink to "
                                                        , beginReadTitleOffset: 20
                                                        , endReadTitleIndicator: "\""
                                                        , beginReadWeblinkIndicator: "<a href=\""
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , beginReadDateIndicator: "<abbr class=\"published\" title=\""
                                                        , beginReadDateOffset: 31
                                                        , endReadDateIndicator: "T"
                                                        , dateFormat: "yyyy-MM-dd"
                                                        , beginReadPreviewIndicator: "<div class=\"entry-content\">"
                                                        , endReadPreviewIndicator: "<!-- .entry-content -->"
                                                        , defaultBlogName: "Experience Curve: Creative Experience Design & Social Business"
                                                        , defaultBlogCategory: "DESIGN"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://davidsimon.com/"
                                                        , beginReadAllIndicator: "<div id='main'>"
                                                        , endReadAllIndicator: "<!-- ####### END MAIN CONTAINER ####### -->"
                                                        , endCurrentEntryIndicator: "<!--end post-entry-->"
                                                        , beginReadAuthorIndicator: "AUTHOR DEFAULTED"
                                                        , beginReadTitleWeblinkIndicator: "rel=\"bookmark\" title="
                                                        , beginReadTitleIndicator: "title="
                                                        , beginReadTitleOffset: 23
                                                        , endReadTitleIndicator: "\""
                                                        , beginReadWeblinkIndicator: "<a href=\""
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , beginReadDateIndicator: "<span class='date-container minor-meta meta-color'>"
                                                        , beginReadDateOffset: 51
                                                        , endReadDateIndicator: "</span>"
                                                        , dateFormat: "MMMM d, yyyy"
                                                        , beginReadPreviewIndicator: "<p>"
                                                        , endReadPreviewIndicator: "<h3 class=\"sd-title"
                                                        , defaultAuthor: "David Simon"
                                                        , defaultBlogName: "The Audacity of Dispair"
                                                        , defaultBlogCategory: "WRITING"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://ejohn.org/blog/"
                                                        , beginReadAllIndicator: "<div class=\"post\">"
                                                        , endReadAllIndicator: "Previous entries</a>"
                                                        , endCurrentEntryIndicator: "class=\"commentslink"
                                                        , beginReadAuthorIndicator: "AUTHOR DEFAULTED"
                                                        , beginReadTitleWeblinkIndicator: "<h3"
                                                        , beginReadTitleIndicator: "title=\"Permanent link to "
                                                        , beginReadTitleOffset: 25
                                                        , endReadTitleIndicator: "\""
                                                        , beginReadWeblinkIndicator: "<a href=\""
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , beginReadDateIndicator: "<strong>Posted:</strong> "
                                                        , beginReadDateOffset: 25
                                                        , endReadDateIndicator: "</small>"
                                                        , dateFormat: "MMMM dcc, yyyy"
                                                        , beginReadPreviewIndicator: "</h3>"
                                                        , endReadPreviewIndicator: "</p>"
                                                        , defaultAuthor: "John Resig"
                                                        , defaultBlogName: "John Resig's Blog"
                                                        , defaultBlogCategory: "PROGRAMMING"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://decor8blog.com/"
                                                        , beginReadAllIndicator: "mainContent"
                                                        , endReadAllIndicator: "older posts</a></div>"
                                                        , endCurrentEntryIndicator: "<div class=\"socialLinks\">"
                                                        , beginReadAuthorIndicator: "AUTHOR DEFAULTED"
                                                        , beginReadTitleWeblinkIndicator: "<h2>"
                                                        , beginReadTitleIndicator: "title="
                                                        , beginReadTitleOffset: 7
                                                        , endReadTitleIndicator: "\""
                                                        , beginReadWeblinkIndicator: "<a href=\""
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , beginReadDateIndicator: "</a> on "
                                                        , beginReadDateOffset: 8
                                                        , endReadDateIndicator: "</div>"
                                                        , dateFormat: "MMMM dd, yyyy"
                                                        , beginReadPreviewIndicator: "<div class=\"entry clear\">"
                                                        , endReadPreviewIndicator: "<div class=\"socialLinks\">"
                                                        , defaultAuthor: "Holly Becker"
                                                        , defaultBlogName: "decor8"
                                                        , defaultBlogCategory: "DESIGN"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://decisionstats.com/"
                                                        , beginReadAllIndicator: "<!-- end .column-narrow -->"
                                                        , endReadAllIndicator: "<span class=\"nav-previous\">"
                                                        , endCurrentEntryIndicator: "<!-- end #post-## -->"
                                                        , beginReadAuthorIndicator: "AUTHOR DEFAULTED"
                                                        , beginReadTitleWeblinkIndicator: "<h2>"
                                                        , beginReadTitleIndicator: "rel=\"bookmark\">"
                                                        , beginReadTitleOffset: 15
                                                        , endReadTitleIndicator: "</a>"
                                                        , beginReadWeblinkIndicator: "<a href=\""
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , beginReadDateIndicator: "<a href=\"http://decisionstats.com/20"
                                                        , beginReadDateOffset: 34
                                                        , endReadDateIndicator: "/"
                                                        , endReadDateOffset: 8
                                                        , dateFormat: "yyyy/MM/dd"
                                                        , beginReadPreviewIndicator: "<div class=\"entry-summary\">"
                                                        , endReadPreviewIndicator: "<div id=\"geo-post"
                                                        , defaultAuthor: "Ajay Ohri"
                                                        , defaultBlogName: "DECISION STATS"
                                                        , defaultBlogCategory: "STATISTICS"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://www.conversionvoodoo.com/blog/"
                                                        , beginReadAllIndicator: "<div id=\"content"
                                                        , endReadAllIndicator: "<ul id=\"prev-next\">"
                                                        , endCurrentEntryIndicator: "<div class=\"sharedaddy"
                                                        , beginReadAuthorIndicator: "rel=\"author\">"
                                                        , beginReadAuthorOffset: 13
                                                        , endReadAuthorIndicator: "</a>"
                                                        , beginReadTitleWeblinkIndicator: "<h2>"
                                                        , beginReadTitleIndicator: "rel=\"bookmark\">"
                                                        , beginReadTitleOffset: 15
                                                        , endReadTitleIndicator: "</a>"
                                                        , beginReadWeblinkIndicator: "<a href=\""
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , beginReadDateIndicator: "NO DATE"
                                                        , dateFormat: "MMMM d, yyyy"
                                                        , beginReadPreviewIndicator: "<p>"
                                                        , endReadPreviewIndicator: "Read the rest of this entry"
                                                        , defaultBlogName: "Converson Voodoo"
                                                        , defaultBlogCategory: "OPTIMIZATION"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://blog.zawodny.com/"
                                                        , beginReadAllIndicator: "<!-- #nav-above -->"
                                                        , endReadAllIndicator: "<div class=\"nav-previous\">"
                                                        , endCurrentEntryIndicator: "<!-- #post-## -->"
                                                        , beginReadAuthorIndicator: "rel=\"author\">"
                                                        , beginReadAuthorOffset: 13
                                                        , endReadAuthorIndicator: "</a>"
                                                        , beginReadTitleWeblinkIndicator: "<h2 class=\"entry-title\">"
                                                        , beginReadTitleIndicator: "rel=\"bookmark\">"
                                                        , beginReadTitleOffset: 15
                                                        , endReadTitleIndicator: "</a>"
                                                        , beginReadWeblinkIndicator: "<a href=\""
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , beginReadDateIndicator: "<span class=\"entry-date\">"
                                                        , beginReadDateOffset: 25
                                                        , endReadDateIndicator: "</span>"
                                                        , dateFormat: "MMMM d, yyyy"
                                                        , beginReadPreviewIndicator: "<div class=\"entry-content\">"
                                                        , endReadPreviewIndicator: "<!-- .entry-content -->"
                                                        , defaultBlogName: "Jeremy Zawodny's Blog"
                                                        , defaultBlogCategory: "SOFTWARE"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://eli.thegreenplace.net/"
                                                        , beginReadAllIndicator: "Below, you will find the most recent posts."
                                                        , endReadAllIndicator: "<div id=\"sidebar\">"
                                                        , endCurrentEntryIndicator: "<p class=\"postmetadata\">"
                                                        , beginReadAuthorIndicator: "AUTHOR DEFAULTED"
                                                        , beginReadTitleWeblinkIndicator: "<h2>"
                                                        , beginReadTitleIndicator: "title=\"Permanent Link to "
                                                        , beginReadTitleOffset: 25
                                                        , endReadTitleIndicator: "\""
                                                        , beginReadWeblinkIndicator: "<a href=\""
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , beginReadDateIndicator: "<small>"
                                                        , beginReadDateOffset: 7
                                                        , endReadDateIndicator: ", 2"
                                                        , endReadDateOffset: 6
                                                        , dateFormat: "MMMM dcc, yyyy"
                                                        , beginReadPreviewIndicator: "<div class=\"entry\">"
                                                        , endReadPreviewIndicator: "<p class=\"postmetadata\">"
                                                        , defaultAuthor: "Eli Bendersky"
                                                        , defaultBlogName: "Eli Bendersky's Blog"
                                                        , defaultBlogCategory: "PROGRAMMING"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://bly.com/blog/"
                                                        , beginReadAllIndicator: "<div id=\"content\">"
                                                        , endReadAllIndicator: "<!-- Close id=\"content\" -->"
                                                        , endCurrentEntryIndicator: "<p class=\"suffix\">"
                                                        , beginReadAuthorIndicator: "title=\"View all posts by "
                                                        , beginReadAuthorOffset: 25
                                                        , endReadAuthorIndicator: "\""
                                                        , beginReadTitleWeblinkIndicator: "<h2>"
                                                        , beginReadTitleIndicator: "title=\"Permanent Link to "
                                                        , beginReadTitleOffset: 25
                                                        , endReadTitleIndicator: "\""
                                                        , beginReadWeblinkIndicator: "<a href=\""
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , beginReadDateIndicator: "<p class=\"prefix\">"
                                                        , beginReadDateOffset: 18
                                                        , endReadDateIndicator: " by"
                                                        , dateFormat: "MMMM dcc, yyyy"
                                                        , beginReadPreviewIndicator: "<div class=\"entry\">"
                                                        , endReadPreviewIndicator: "<div class=\"addtoany"
                                                        , defaultAuthor: "Bob Bly"
                                                        , defaultBlogName: "Bob Bly Copywriter"
                                                        , defaultBlogCategory: "MARKETING"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://blog.catchpoint.com/"
                                                        , beginReadAllIndicator: "<!-- #header -->"
                                                        , endReadAllIndicator: "<!-- #content -->"
                                                        , endCurrentEntryIndicator: "<!-- #post-## -->"
                                                        , beginReadAuthorIndicator: "title=\"View all posts by "
                                                        , beginReadAuthorOffset: 25
                                                        , endReadAuthorIndicator: "\""
                                                        , beginReadTitleWeblinkIndicator: "<h2 class=\"entry-title\">"
                                                        , beginReadTitleIndicator: "rel=\"bookmark\">"
                                                        , beginReadTitleOffset: 15
                                                        , endReadTitleIndicator: "</a>"
                                                        , beginReadWeblinkIndicator: "<a href=\""
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , beginReadDateIndicator: "<span class=\"entry-date\">"
                                                        , beginReadDateOffset: 94
                                                        , endReadDateIndicator: "DATE DEFAULTED"
                                                        , dateFormat: "MMMM d, yyyy"
                                                        , beginReadPreviewIndicator: "<div class=\"entry-content\">"
                                                        , endReadPreviewIndicator: "<!-- .entry-content -->"
                                                        , defaultBlogName: "Catchpoint"
                                                        , defaultBlogCategory: "WEB PROGRAMMING"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://blog.echen.me/"
                                                        , beginReadAllIndicator: "<div id=\"main\">"
                                                        , endReadAllIndicator: "<aside class=\"sidebar\">"
                                                        , endCurrentEntryIndicator: "</article>"
                                                        , beginReadAuthorIndicator: "AUTHOR DEFAULTED"
                                                        , beginReadTitleWeblinkIndicator: "<h1 class=\"entry-title\">"
                                                        , beginReadTitleIndicator: "/\">"
                                                        , beginReadTitleOffset: 3
                                                        , endReadTitleIndicator: "</a>"
                                                        , beginReadWeblinkIndicator: "<a href=\""
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , beginReadDateIndicator: "<time datetime="
                                                        , beginReadDateOffset: 3
                                                        , endReadDateIndicator: "T"
                                                        , dateFormat: "yyyy-MM-dd"
                                                        , beginReadPreviewIndicator: "<div class=\"entry-content\">"
                                                        , endReadPreviewIndicator: "</article>"
                                                        , defaultAuthor: "Edwin Chen"
                                                        , defaultBlogName: "Edwin Chen's Blog"
                                                        , defaultBlogCategory: "QUANTATIVE ANALYSIS"
                                                        , weblinkPrefix: "http://blog.echen.me"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://www.usabilitypost.com"
                                                        , beginReadAllIndicator: "<div class=\"recent_post\">"
                                                        , endReadAllIndicator: "<div id=\"recent_archive_link\">"
                                                        , endCurrentEntryIndicator: "<hr/>"
                                                        , beginReadAuthorIndicator: "AUTHOR DEFAULTED"
                                                        , beginReadTitleWeblinkIndicator: "<h2>"
                                                        , beginReadTitleIndicator: "\">"
                                                        , beginReadTitleOffset: 2
                                                        , endReadTitleIndicator: "</a>"
                                                        , beginReadWeblinkIndicator: "<a href=\""
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , beginReadDateIndicator: "<div class=\"date\">"
                                                        , beginReadDateOffset: 18
                                                        , endReadDateIndicator: "</div>"
                                                        , dateFormat: "MMMM d, yyyy"
                                                        , beginReadPreviewIndicator: "<p>"
                                                        , endReadPreviewIndicator: "<hr/>"
                                                        , defaultAuthor: "Dmitry Fadeyev"
                                                        , defaultBlogName: "Usability Post"
                                                        , defaultBlogCategory: "UX"
                                                        , weblinkPrefix: "http://www.usabilitypost.com"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://adam.heroku.com/"
                                                        , beginReadAllIndicator: "<div id=\"content\">"
                                                        , endReadAllIndicator: "<div id=\"older_posts\">"
                                                        , endCurrentEntryIndicator: "Continue reading &raquo;</a>"
                                                        , beginReadAuthorIndicator: "AUTHOR DEFAULTED"
                                                        , beginReadTitleWeblinkIndicator: "<h2 class=\"title\">"
                                                        , beginReadTitleIndicator: "/\">"
                                                        , beginReadTitleOffset: 3
                                                        , endReadTitleIndicator: "</a>"
                                                        , beginReadWeblinkIndicator: "<a href=\""
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , beginReadDateIndicator: "DATE DEFAULTED"
                                                        , dateFormat: "MMMM d, yyyy"
                                                        , beginReadPreviewIndicator: "<div class=\"entry\">"
                                                        , endReadPreviewIndicator: "</p>"
                                                        , defaultAuthor: "Adam Wiggins"
                                                        , defaultBlogName: "A Tornado of Razorblades"
                                                        , defaultBlogCategory: "PROGRAMMING"
                                                        , weblinkPrefix: "http://adam.heroku.com"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://www.uxmatters.com"
                                                        , beginReadAllIndicator: "<!-- MT info_start -->"
                                                        , endReadAllIndicator: "<!-- MT info_end -->"
                                                        , endCurrentEntryIndicator: "<!-- End pullquote -->"
                                                        , beginReadAuthorIndicator: "title=\""
                                                        , beginReadAuthorOffset: 7
                                                        , endReadAuthorIndicator: "\""
                                                        , beginReadTitleWeblinkIndicator: "<h3 class=\"permalink\">"
                                                        , beginReadTitleIndicator: "title=\"Link\">"
                                                        , beginReadTitleOffset: 13
                                                        , endReadTitleIndicator: "</a>"
                                                        , beginReadWeblinkIndicator: "<a href=\""
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , beginReadDateIndicator: "<p class=\"date\">Published: "
                                                        , beginReadDateOffset: 27
                                                        , endReadDateIndicator: "</p>"
                                                        , dateFormat: "MMMM d, yyyy"
                                                        , beginReadPreviewIndicator: "<div class=\"pullquote-wide\">"
                                                        , endReadPreviewIndicator: "<!-- End pullquote -->"
                                                        , defaultBlogName: "UX Matters :: Insights and Inspiration for the User Experience Community"
                                                        , defaultBlogCategory: "UX"
                                                        , weblinkPrefix: "http://www.uxmatters.com"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://andrewgelman.com/"
                                                        , beginReadAllIndicator: "<div id=\"content\">"
                                                        , endReadAllIndicator: "<div class=\"navigation\">"
                                                        , endCurrentEntryIndicator: "<span class=\"postcomment\">"
                                                        , beginReadAuthorIndicator: "AUTHOR DEFAULTED"
                                                        , beginReadTitleWeblinkIndicator: "<h2 class=\"posttitle\">"
                                                        , beginReadTitleIndicator: "title=\""
                                                        , beginReadTitleOffset: 7
                                                        , endReadTitleIndicator: "\""
                                                        , beginReadWeblinkIndicator: "<a href=\""
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , beginReadDateIndicator: "<h2 class=\"posttitle\"><a href=\"http://andrewgelman.com/"
                                                        , beginReadDateOffset: 56
                                                        , endReadDateIndicator: "2"
                                                        , endReadDateOffset: 10
                                                        , dateFormat: "yyyy/MM/dd"
                                                        , beginReadPreviewIndicator: "<div class=\"postentry\">"
                                                        , endReadPreviewIndicator: "<p class=\"to_comments\">"
                                                        , defaultAuthor: "Andrew Gelman"
                                                        , defaultBlogName: "Statistical Modeling, Causal Inference, and Social Science"
                                                        , defaultBlogCategory: "STATISTICS"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://bartoszmilewski.com/"
                                                        , beginReadAllIndicator: "<div id=\"content\">"
                                                        , endReadAllIndicator: "<div class=\"navigation\">"
                                                        , endCurrentEntryIndicator: "<div class=\"post-footer\">"
                                                        , beginReadAuthorIndicator: "AUTHOR DEFAULTED"
                                                        , beginReadTitleWeblinkIndicator: "rel=\"bookmark\" title=\""
                                                        , beginReadTitleIndicator: "Permanent Link: "
                                                        , beginReadTitleOffset: 16
                                                        , endReadTitleIndicator: "\""
                                                        , beginReadWeblinkIndicator: "<a href=\""
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , beginReadDateIndicator: "<p class=\"post-date\">"
                                                        , beginReadDateOffset: 21
                                                        , endReadDateIndicator: "</p>"
                                                        , dateFormat: "MMMM d, yyyy"
                                                        , beginReadPreviewIndicator: "<div class=\"post-content\">"
                                                        , endReadPreviewIndicator: "<div class=\"post-footer\">"
                                                        , defaultAuthor: "Bartosz Milewski"
                                                        , defaultBlogName: "Bartosz Milewski's Programming Cafe"
                                                        , defaultBlogCategory: "PROGRAMMING"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://bokardo.com/"
                                                        , beginReadAllIndicator: "LATEST BLOG POSTS"
                                                        , endReadAllIndicator: "<a href=\"/archives/\">"
                                                        , endCurrentEntryIndicator: "</div>"
                                                        , beginReadAuthorIndicator: "<p class=\"author\">by <a href=\"/about/\">"
                                                        , beginReadAuthorOffset: 39
                                                        , endReadAuthorIndicator: "</a>"
                                                        , beginReadTitleWeblinkIndicator: "rel=\"bookmark\""
                                                        , beginReadTitleIndicator: "title=\"Permanent Link to "
                                                        , beginReadTitleOffset: 25
                                                        , endReadTitleIndicator: "\""
                                                        , beginReadWeblinkIndicator: "<a href=\""
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , beginReadDateIndicator: "<p class=\"post-date\">"
                                                        , beginReadDateOffset: 21
                                                        , endReadDateIndicator: "</p>"
                                                        , dateFormat: "MMMM d, yyyy"
                                                        , beginReadPreviewIndicator: "<p>"
                                                        , endReadPreviewIndicator: "</div>"
                                                        , defaultBlogName: "Bokardo"
                                                        , defaultBlogCategory: "DESIGN"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://blog.teamtreehouse.com/"
                                                        , beginReadAllIndicator: "<div id=\"main\">"
                                                        , endReadAllIndicator: "<!-- #content -->"
                                                        , endCurrentEntryIndicator: "</article>"
                                                        , beginReadAuthorIndicator: "rel=\"me\">"
                                                        , beginReadAuthorOffset: 9
                                                        , endReadAuthorIndicator: "</a>"
                                                        , beginReadTitleWeblinkIndicator: "<h1 class=\"entry-title\">"
                                                        , beginReadTitleIndicator: "title=\"Permalink to "
                                                        , beginReadTitleOffset: 20
                                                        , endReadTitleIndicator: "\""
                                                        , beginReadWeblinkIndicator: "<a href=\""
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , beginReadDateIndicator: "DEFAULT DATE"
                                                        , dateFormat: "MMMM d, yyyy"
                                                        , beginReadPreviewIndicator: "<div class=\"entry-content\">"
                                                        , endReadPreviewIndicator: "<!-- .entry-content -->"
                                                        , defaultBlogName: "Treehouse Blog"
                                                        , defaultBlogCategory: "WEB PROGRAMMING"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://www.aisleone.net/"
                                                        , beginReadAllIndicator: "<!-- #content ends in footer.php -->"
                                                        , endReadAllIndicator: "<!-- end .posts-wrap -->"
                                                        , endCurrentEntryIndicator: "<!-- end .post -->"
                                                        , beginReadAuthorIndicator: "title=\"Posts by "
                                                        , beginReadAuthorOffset: 16
                                                        , endReadAuthorIndicator: "\""
                                                        , beginReadTitleWeblinkIndicator: "title=\"Permalink to "
                                                        , beginReadTitleIndicator: "title=\"Permalink to "
                                                        , beginReadTitleOffset: 20
                                                        , endReadTitleIndicator: "\""
                                                        , beginReadWeblinkIndicator: "<a href=\""
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , beginReadDateIndicator: "\t\t\t\t\t\t"
                                                        , endReadDateIndicator: ", 20"
                                                        , endReadDateOffset: 6
                                                        , dateFormat: "MMMM d, yyyy"
                                                        , beginReadPreviewIndicator: "<div class=\"entry-content\">"
                                                        , endReadPreviewIndicator: "<!-- end .entry-content -->"
                                                        , defaultBlogName: "AisleOne - Graphic Design, Typography and Grid Systems"
                                                        , defaultBlogCategory: "DESIGN"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://www.codinghorror.com"
                                                        , beginReadAllIndicator: "<!-- header -->"
                                                        , endReadAllIndicator: "<div class=\"footernav"
                                                        , endCurrentEntryIndicator: "</div> <!-- blog"
                                                        , beginReadAuthorIndicator: "AUTHOR DEFAULTED"
                                                        , beginReadTitleWeblinkIndicator: "<h2 class=\"title\">"
                                                        , beginReadTitleIndicator: "class=\"title-link\">"
                                                        , beginReadTitleOffset: 19
                                                        , endReadTitleIndicator: "<"
                                                        , beginReadWeblinkIndicator: "<a href=\""
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , beginReadDateIndicator: "<h3 class=\"date\">"
                                                        , beginReadDateOffset: 17
                                                        , endReadDateIndicator: "</h3>"
                                                        , dateFormat: "MMMM d, yyyy"
                                                        , beginReadPreviewIndicator: "<p>"
                                                        , endReadPreviewIndicator: "<td class=\"welovecodinghorror\">"
                                                        , defaultAuthor: "Jeff Atwood"
                                                        , defaultBlogName: "Coding Horror"
                                                        , defaultBlogCategory: "PROGRAMMING"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://mechanical-sympathy.blogspot.com/"
                                                        , beginReadAllIndicator: "<div class='blog-posts hfeed'>"
                                                        , endReadAllIndicator: "<span id='blog-pager-older-link'>"
                                                        , endCurrentEntryIndicator: "<span class='post-labels'>"
                                                        , beginReadAuthorIndicator: "AUTHOR DEFAULTED"
                                                        , beginReadTitleWeblinkIndicator: "<a href='http://mechanical-sympathy.blogspot.com/2"
                                                        , beginReadTitleIndicator: "'>"
                                                        , beginReadTitleOffset: 2
                                                        , endReadTitleIndicator: "</a>"
                                                        , beginReadWeblinkIndicator: "<a href='"
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "'"
                                                        , beginReadDateIndicator: "<h2 class='date-header'><span>"
                                                        , beginReadDateOffset: 30
                                                        , endReadDateIndicator: "</span>"
                                                        , dateFormat: "DD, d MMMM, yyyy"
                                                        , beginReadPreviewIndicator: "<div class='post-body entry-content'"
                                                        , endReadPreviewIndicator: "<div class='post-footer'>"
                                                        , defaultAuthor: "Martin Thompson"
                                                        , defaultBlogName: "Mechanical Sympathy"
                                                        , defaultBlogCategory: "PROGRAMMING"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://blog.regehr.org/"
                                                        , beginReadAllIndicator: "<div id=\"content\" class=\"hfeed\">"
                                                        , endReadAllIndicator: "<!-- #content .hfeed -->"
                                                        , endCurrentEntryIndicator: "<!-- .post -->"
                                                        , beginReadAuthorIndicator: "AUTHOR DEFAULTED"
                                                        , beginReadTitleWeblinkIndicator: "<h2 class=\"entry-title\">"
                                                        , beginReadTitleIndicator: "rel=\"bookmark\">"
                                                        , beginReadTitleOffset: 15
                                                        , endReadTitleIndicator: "</a>"
                                                        , beginReadWeblinkIndicator: "<a href="
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , beginReadDateIndicator: "<span class=\"entry-date\">"
                                                        , beginReadDateOffset: 82
                                                        , endReadDateIndicator: "</abbr>"
                                                        , dateFormat: "yyyy MM dd"
                                                        , beginReadPreviewIndicator: "<div class=\"entry-content\">"
                                                        , endReadPreviewIndicator: "<div class=\"entry-meta\">"
                                                        , defaultAuthor: "John Regehr"
                                                        , defaultBlogName: "Embedded in Academia"
                                                        , defaultBlogCategory: "PROGRAMMING"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://stevesouders.com/"
                                                        , beginReadAllIndicator: "<td id=maincol>"
                                                        , endReadAllIndicator: "<td id=miscinfo>"
                                                        , endCurrentEntryIndicator: "<!-- END POST -->"
                                                        , beginReadAuthorIndicator: "AUTHOR DEFAULTED"
                                                        , beginReadTitleWeblinkIndicator: "title=\"Permalink to "
                                                        , beginReadTitleIndicator: "rel=\"bookmark\">"
                                                        , beginReadTitleOffset: 15
                                                        , endReadTitleIndicator: "</a>"
                                                        , beginReadWeblinkIndicator: "<a href="
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , beginReadDateIndicator: "<p class=\"meta\">"
                                                        , beginReadDateOffset: 16
                                                        , endReadDateIndicator: " |"
                                                        , dateFormat: "MMMM d, yyyy h:mm tt"
                                                        , beginReadPreviewIndicator: "<p>"
                                                        , endReadPreviewIndicator: "<!-- END POST -->"
                                                        , defaultAuthor: "Steve Souders"
                                                        , defaultBlogName: "High Performance Web Sites"
                                                        , defaultBlogCategory: "WEB PROGRAMMING"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://techblog.netflix.com/"
                                                        , beginReadAllIndicator: "<div class='blog-posts hfeed'>"
                                                        , endReadAllIndicator: "<!-- google_ad_section_end -->"
                                                        , endCurrentEntryIndicator: "<div class='post-footer'>"
                                                        , beginReadAuthorIndicator: "NOT PULLING AUTHOR"
                                                        , beginReadTitleWeblinkIndicator: "<a href='http://techblog.netflix.com/2"
                                                        , beginReadTitleIndicator: ">"
                                                        , beginReadTitleOffset: 1
                                                        , endReadTitleIndicator: "</a>"
                                                        , beginReadWeblinkIndicator: "<a href="
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "'"
                                                        , beginReadDateIndicator: "<h2 class='date-header'><span>"
                                                        , beginReadDateOffset: 30
                                                        , endReadDateIndicator: "</span>"
                                                        , dateFormat: "dddd, MMMM d, yyyy"
                                                        , beginReadPreviewIndicator: "<div class='post-body entry-content'"
                                                        , endReadPreviewIndicator: "<div class='post-footer'>"
                                                        , defaultAuthor: "Various"
                                                        , defaultBlogName: "The Netflix Tech Blog"
                                                        , defaultBlogCategory: "PROGRAMMING"
                                                        );

                gatherEntriesGeneric(blogAddress: "http://www.toxel.com/"
                                                        , beginReadAllIndicator: "<!-- Start Posts -->"
                                                        , endReadAllIndicator: "<!-- End Posts -->"
                                                        , endCurrentEntryIndicator: "<!-- insert comments -->"
                                                        , beginReadAuthorIndicator: "NO AUTHOR AVAILABLE"
                                                        , beginReadTitleWeblinkIndicator: "<h2>"
                                                        , beginReadTitleIndicator: "title="
                                                        , beginReadTitleOffset: 7
                                                        , endReadTitleIndicator: "\""
                                                        , beginReadWeblinkIndicator: "<a href="
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , beginReadDateIndicator: "<div class=\"postinfo\">"
                                                        , beginReadDateOffset: 22
                                                        , endReadDateIndicator: " |"
                                                        , dateFormat: "MMMM dcc, yyyy"
                                                        , beginReadPreviewIndicator: "<div class=\"main\">"
                                                        , endReadPreviewIndicator: "Read Full Post"
                                                        , defaultAuthor: "Toxel"
                                                        , defaultBlogName: "Toxel"
                                                        , defaultBlogCategory: "DESIGN"
                                                        );

                #endregion

                #region Generic TypePad Gather Calls

                gatherTypePadBlogEntries("http://datamining.typepad.com/data_mining/", "Matthew Hurst", "Data Mining: Text Mining, Visualization and Social Media", "DATA", "MMMM dd, yyyy");
                gatherTypePadBlogEntries("http://hi-and-low.typepad.com/", "Abby Clawson Low", "HI + LOW", "DESIGN", "dd MMMM yyyy");

                #endregion

                #region Complex Coded Blog Gather Calls
                gatherTolzohEntries();
                gatherApartmentLivingEntries();
                gatherAdaptivePathEntries();
                gatherDanArielyEntries();
                #endregion
            }

        }

        #region Complex HTML Gatherers

        public static void gatherDanArielyEntries()
        {
            string blogAddress = "http://danariely.com/";
            WebClient client = new WebClient();

            using (var stream = client.OpenRead(blogAddress))
            using (StreamReader reader = new StreamReader(stream))
            {
                string lines, webString = "";
                int string_position;
                bool beginRead = false, previewContinue = false;
                Post nextPost = new Post();
                nextPost.author = "Dan Ariely";
                nextPost.blogCategory = "BEHAVIORAL ECONOMICS";
                nextPost.blogName = "Dan Ariely Blog";

                while ((lines = reader.ReadLine()) != null)
                {
                    //START OF ALL BLOG ENTRIES
                    if (lines.IndexOf("<!--END header-->") != -1 && !beginRead)
                        beginRead = true;
                    else if (beginRead)
                    {
                        //END OF ALL BLOG ENTRIES
                        if (lines.IndexOf("<div class=\"sidebar\">") != -1)
                            break;

                        //SET TITLE AND WEBLINK
                        if (lines.IndexOf("<h3>") != -1)
                        {
                            string_position = lines.IndexOf("<a href=\"") + 9;
                            nextPost.weblink = lines.Substring(string_position, lines.IndexOf("\"", string_position) - string_position);
                            string_position = lines.IndexOf("title=\"Permanent Link to ", string_position) + 25;
                            nextPost.title = lines.Substring(string_position, lines.IndexOf("\"", string_position) - string_position).Replace("\r", "").Replace("\n", "").Replace("\t", "").TrimStart();
                        }

                        //SET DATE
                        if (lines.IndexOf("<span class=\"month\">") != -1)
                        {
                            string_position = lines.IndexOf("<span class=\"month\">") + 20;
                            webString = lines.Substring(string_position, lines.IndexOf("</span>", string_position) - string_position).Replace("<b>Posted:</b>", "").Trim();
                        }

                        //SET DATE HELPER
                        if (lines.IndexOf("<span class=\"day\">") != -1)
                        {
                            string_position = lines.IndexOf("<span class=\"day\">") + 18;
                            webString += " " + lines.Substring(string_position, lines.IndexOf("</span>", string_position) - string_position).Replace("<b>Posted:</b>", "").Trim();
                            nextPost.publishDate = DateTime.ParseExact(webString + " " + DateTime.Today.Year, "MMM dd yyyy",
                                           System.Globalization.CultureInfo.InvariantCulture);
                        }

                        //SET PREVIEW
                        if ((lines.IndexOf("<div class=\"post-content\">") != -1) || previewContinue)
                        {
                            //Console.WriteLine(lines.IndexOf("<p><em>") + " " + lines.IndexOf("<p><em>—") + " " + lines.IndexOf("<p><em>Dear Dan,"));

                            if (!previewContinue)
                            {
                                previewContinue = true;
                                webString = "";
                            }
                            else if (lines.IndexOf("<p><em>") != -1
                                && lines.IndexOf("<p><em>—") == -1
                                && lines.IndexOf("<p><em>Dear Dan,") == -1)
                            {
                                webString += "\t" + lines + "\n";
                            }

                            //CHECK THAT EXCERPT ENDING OR OVER LIMIT
                            if (lines.IndexOf("<div id=\"jp-post-flair") != -1)
                            {
                                //REMOVE ENDLINES AND TABS
                                webString = webString.Replace("\r", "").Replace("\t", "");

                                bool webElement = false;
                                string webStringElementsRemoved = "";

                                for (int i = 0; i < webString.Length - 1; i++)
                                {
                                    if (webString[i] == '<' && !webElement)
                                        webElement = true;
                                    else if (webString[i] == '>' && webElement)
                                        webElement = false;
                                    else if (!webElement && webString.Substring(i, 2) != "  ")
                                        webStringElementsRemoved += webString[i].ToString();
                                }
                                /*
                                if (webStringElementsRemoved.Length > 344)
                                    webStringElementsRemoved = webStringElementsRemoved.Substring(0, 344).Trim() + " [...]";
                                else
                                    webStringElementsRemoved = webStringElementsRemoved.Replace("...", " [...]");
                                */
                                previewContinue = false;
                                nextPost.textPreview = webStringElementsRemoved.TrimEnd();
                                Console.WriteLine(webStringElementsRemoved);
                            }
                        }

                        //END OF CURRENT BLOG ENTRY
                        if (lines.IndexOf("<div id=\"jp-post-flair") != -1)
                        {
                            blogList.Add(nextPost);
                            nextPost = new Post();
                            nextPost.author = "Dan Ariely";
                            nextPost.blogCategory = "BEHAVIORAL ECONOMICS";
                            nextPost.blogName = "Dan Ariely Blog";
                        }
                    }
                }
            }
        }

        public static void gatherAdaptivePathEntries()
        {
            string blogAddress = "http://www.adaptivepath.com/ideas/";
            WebClient client = new WebClient();

            using (var stream = client.OpenRead(blogAddress))
            using (StreamReader reader = new StreamReader(stream))
            {
                string lines, webString = "", lineCombo = "";
                int string_position, string_position2;
                bool beginRead = false;
                Post nextPost = new Post();
                nextPost.author = "Adaptive Path";
                nextPost.blogCategory = "UX";
                nextPost.blogName = "Adaptive Path";

                while ((lines = reader.ReadLine()) != null)
                {
                    //START OF ALL BLOG ENTRIES
                    if (lines.IndexOf("<div id=\"content\" role=\"main\">") != -1 && !beginRead)
                        beginRead = true;
                    else if (beginRead)
                    {
                        lineCombo += lines;

                        //END OF ALL BLOG ENTRIES
                        if (lines.IndexOf("<!-- #content -->") != -1)
                            break;

                        //SET TITLE
                        if (lineCombo.IndexOf("<h3 class=\"entry-title") != -1 && lineCombo.IndexOf("</a>", lineCombo.IndexOf("<h3 class=\"entry-title")) != -1)
                        {
                            string_position = lineCombo.IndexOf("<h3 class=\"entry-title");
                            string_position2 = lineCombo.IndexOf("</a>", string_position);

                            webString = lineCombo.Substring(string_position, string_position2 - string_position);
                            //Console.WriteLine(string_position + " " + webString);
                            nextPost.title = webString.Substring(webString.LastIndexOf(">") + 1);
                            //titleNext = false;
                        }

                        //SET WEBLINK
                        if (lineCombo.IndexOf("<div class=\"topbar\">") != -1)
                        {
                            string_position = lineCombo.IndexOf("<a href=", lineCombo.IndexOf("<div class=\"topbar\">")) + 9;
                            nextPost.weblink = lineCombo.Substring(string_position, lineCombo.IndexOf("\">", string_position) - string_position);
                        }

                        //SET DATE
                        if (lineCombo.IndexOf("<div class=\"entry-date nudista-16\">") != -1)
                        {
                            string_position = lineCombo.IndexOf("<div class=\"entry-date nudista-16\">") + 35;
                            webString = lineCombo.Substring(string_position, lineCombo.IndexOf("</div>", string_position) - string_position).Trim();
                            nextPost.publishDate = DateTime.ParseExact(webString, "MMMM dd, yyyy",
                                           System.Globalization.CultureInfo.InvariantCulture);
                        }

                        //SET PREVIEW
                        if (lineCombo.IndexOf("...") != -1)
                        {
                            string_position = lineCombo.IndexOf("<div class=\"entry-summary");
                            //Console.WriteLine(string_position + " " + lineCombo);
                            webString = lineCombo.Substring(string_position, lineCombo.IndexOf("...", string_position) - string_position + 4).Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();

                            bool webElement = false;
                            string webStringElementsRemoved = "";

                            for (int i = 0; i < webString.Length - 1; i++)
                            {
                                if (webString[i] == '<' && !webElement)
                                    webElement = true;
                                else if (webString[i] == '>' && webElement)
                                    webElement = false;
                                else if (!webElement && webString.Substring(i, 2) != "  ")
                                    webStringElementsRemoved += webString[i].ToString();
                            }

                            if (webStringElementsRemoved.Length > 344)
                                webStringElementsRemoved = webStringElementsRemoved.Substring(0, 344).Trim() + " [...]";
                            else
                                webStringElementsRemoved = webStringElementsRemoved.Replace("...", " [...]");

                            nextPost.textPreview = webStringElementsRemoved.TrimEnd();
                        }

                        //END OF CURRENT BLOG ENTRY
                        if (lines.IndexOf("...") != -1)
                        {
                            blogList.Add(nextPost);
                            nextPost = new Post();
                            nextPost.author = "Adaptive Path";
                            nextPost.blogCategory = "UX";
                            nextPost.blogName = "Adaptive Path";
                            lineCombo = lineCombo.Substring(lineCombo.IndexOf("</article>") + 10);
                        }
                    }
                }
            }
        }

        public static void gatherTolzohEntries()
        {
            string blogAddress = "http://tolzoh.blogspot.nl/?m=1";
            WebClient client = new WebClient();

            using (var stream = client.OpenRead(blogAddress))
            using (StreamReader reader = new StreamReader(stream))
            {
                string lines, webString = "";
                int string_position;
                bool beginRead = false, previewContinue = false, dateNext = false, weblinkNext = false, titleNext = false;
                Post nextPost = new Post();
                nextPost.author = "Tolzoh";
                nextPost.blogCategory = "FUN";
                nextPost.blogName = "This And That...";

                while ((lines = reader.ReadLine()) != null)
                {
                    //START OF ALL BLOG ENTRIES
                    if (lines.IndexOf("<div class='main section' id='main'>") != -1 && !beginRead)
                        beginRead = true;
                    else if (beginRead)
                    {
                        //END OF ALL BLOG ENTRIES
                        if (lines.IndexOf("<!-- main -->") != -1)
                            break;

                        //SET TITLE
                        if (titleNext)
                        {
                            nextPost.title = lines.Replace("\r", "").Replace("\n", "").Replace("\t", "").TrimStart();
                            titleNext = false;
                        }

                        //SET TITLE HELPER
                        if (lines.IndexOf("<h3 class='mobile-index-title entry-title' itemprop='name'>") != -1)
                            titleNext = true;

                        //SET WEBLINK
                        if (weblinkNext)
                        {
                            string_position = lines.IndexOf("<a href='") + 9;
                            nextPost.weblink = lines.Substring(string_position, lines.IndexOf("\'>", string_position) - string_position);
                            weblinkNext = false;
                        }

                        //SET WEBLINK HELPER
                        if (lines.IndexOf("<div class='mobile-post-outer'>") != -1)
                            weblinkNext = true;


                        //SET DATE
                        if (dateNext)
                        {
                            string_position = lines.IndexOf("<span>") + 6;
                            webString = lines.Substring(string_position, lines.IndexOf("</span>", string_position) - string_position).Replace("<b>Posted:</b>", "").Trim();
                            nextPost.publishDate = DateTime.ParseExact(webString, "MMMM d, yyyy",
                                           System.Globalization.CultureInfo.InvariantCulture);
                            dateNext = false;
                        }

                        //SET DATE HELPER
                        if (lines.IndexOf("<div class='date-header'>") != -1)
                            dateNext = true;

                        //SET PREVIEW
                        if ((lines.IndexOf("<div class='post-body'>") != -1) || previewContinue)
                        {
                            if (!previewContinue)
                            {
                                previewContinue = true;
                                webString = lines;
                            }
                            else
                            {
                                webString += lines;
                            }

                            //CHECK THAT EXCERPT ENDING OR OVER LIMIT
                            if (webString.IndexOf("</div>") != -1)
                            {
                                //REMOVE ENDLINES AND TABS
                                webString = webString.Replace("\r", "").Replace("\n", "").Replace("\t", "");

                                bool webElement = false;
                                string webStringElementsRemoved = "";

                                for (int i = 0; i < webString.Length - 1; i++)
                                {
                                    if (webString[i] == '<' && !webElement)
                                        webElement = true;
                                    else if (webString[i] == '>' && webElement)
                                        webElement = false;
                                    else if (!webElement && webString.Substring(i, 2) != "  ")
                                        webStringElementsRemoved += webString[i].ToString();
                                }

                                if (webStringElementsRemoved.Length > 344)
                                    webStringElementsRemoved = webStringElementsRemoved.Substring(0, 344).Trim() + " [...]";
                                else
                                    webStringElementsRemoved = webStringElementsRemoved.Replace("...", " [...]");

                                previewContinue = false;
                                nextPost.textPreview = webStringElementsRemoved.TrimEnd();
                            }
                        }

                        //END OF CURRENT BLOG ENTRY
                        if (lines.IndexOf("<div class='mobile-index-comment'>") != -1)
                        {
                            blogList.Add(nextPost);
                            nextPost = new Post();
                            nextPost.author = "Tolzoh";
                            nextPost.blogCategory = "FUN";
                            nextPost.blogName = "This And That...";
                        }
                    }
                }
            }
        }

        public static void gatherApartmentLivingEntries()
        {
            string blogAddress = "http://www.apartmenttherapy.com/";
            WebClient client = new WebClient();

            using (var stream = client.OpenRead(blogAddress))
            using (StreamReader reader = new StreamReader(stream))
            {
                string lines, webString = "";
                int string_position;
                bool beginRead = false, previewContinue = false, titleNext = false;
                Post nextPost = new Post();
                nextPost.blogCategory = "HOME";
                nextPost.blogName = "Apartment Living";

                while ((lines = reader.ReadLine()) != null)
                {
                    //START OF ALL BLOG ENTRIES
                    if (lines.IndexOf("<div id=\"main\">") != -1 && !beginRead)
                        beginRead = true;
                    else if (beginRead)
                    {
                        //END OF ALL BLOG ENTRIES
                        if (lines.IndexOf("<span class=\"older\">") != -1)
                            break;

                        //SET AUTHOR
                        if (lines.IndexOf("rel=\"author\">") != -1)
                        {
                            string_position = lines.IndexOf("rel=\"author\">") + 13;
                            nextPost.author = lines.Substring(string_position, lines.IndexOf("<", string_position) - string_position);
                        }

                        //SET WEBLINK
                        if (titleNext && lines.IndexOf("<a href=") != -1)
                        {
                            string_position = lines.IndexOf("<a href=") + 9;
                            nextPost.weblink = lines.Substring(string_position, lines.IndexOf("'>", string_position) - string_position);
                        }

                        //SET TITLE
                        if (titleNext && lines.IndexOf("<h1>") != -1)
                        {
                            nextPost.title = lines.Replace("<h1>", "").Replace("</h1>", "").Replace("\r", "").Replace("\n", "").Replace("\t", "").TrimStart();
                            titleNext = false;
                        }

                        //SET TITLE/WEBLINK HELPER
                        if (lines.IndexOf("<div class='hgroup entry-title'>") != -1)
                            titleNext = true;

                        //SET PREVIEW
                        if ((lines.IndexOf("<div class='post-content'>") != -1) || previewContinue)
                        {
                            if (!previewContinue)
                            {
                                previewContinue = true;
                                webString = lines;
                            }
                            else
                            {
                                webString += lines;
                            }

                            //CHECK THAT EXCERPT ENDING OR OVER LIMIT
                            if (webString.IndexOf("<div class='post-footer'>") != -1)
                            {
                                //REMOVE ENDLINES AND TABS
                                webString = webString.Replace("\r", "").Replace("\n", "").Replace("\t", "");

                                bool webElement = false;
                                string webStringElementsRemoved = "";

                                for (int i = 0; i < webString.Length - 1; i++)
                                {
                                    if (webString[i] == '<' && !webElement)
                                        webElement = true;
                                    else if (webString[i] == '>' && webElement)
                                        webElement = false;
                                    else if (!webElement && webString.Substring(i, 2) != "  ")
                                        webStringElementsRemoved += webString[i].ToString();
                                }

                                if (webStringElementsRemoved.Length > 344)
                                    webStringElementsRemoved = webStringElementsRemoved.Substring(0, 344).Trim() + " [...]";
                                else
                                    webStringElementsRemoved = webStringElementsRemoved.Replace("...", " [...]");

                                previewContinue = false;
                                nextPost.textPreview = webStringElementsRemoved.TrimEnd();
                            }
                        }

                        //END OF CURRENT BLOG ENTRY
                        if (lines.IndexOf("class=\"more-link\">") != -1)
                        {
                            blogList.Add(nextPost);
                            nextPost = new Post();
                            nextPost.blogCategory = "HOME";
                            nextPost.blogName = "Apartment Living";
                        }
                    }
                }
            }
        }


        #endregion

        #region Generic Gather Entries Procedure

        public static void gatherTypePadBlogEntries(string blogAddress, string defaultAuthor, string defaultBlogName, string defaultBlogCategory, string dateTimeFormat)
        {
            gatherEntriesGeneric(blogAddress: blogAddress
                                                        , beginReadAllIndicator: "<!-- entry list sticky -->"
                                                        , endReadAllIndicator: "<!-- sidebar2 -->"
                                                        , endCurrentEntryIndicator: "<!-- post footer links -->"
                                                        , beginReadAuthorIndicator: "AUTHOR DEFAULTED"
                                                        , beginReadAuthorOffset: 0
                                                        , endReadAuthorIndicator: "AUTHOR DEFAULTED"
                                                        , endReadAuthorOffset: 0

                                                        , beginReadTitleWeblinkIndicator: "<h3 class=\"entry-header\">"
                                                        , beginReadTitleIndicator: ".html\">"
                                                        , beginReadTitleOffset: 7
                                                        , endReadTitleIndicator: "</a>"
                                                        , endReadTitleOffset: 0

                                                        , beginReadWeblinkIndicator: "<a href=\""
                                                        , beginReadWeblinkOffset: 9
                                                        , endReadWeblinkIndicator: "\""
                                                        , endReadWeblinkOffset: 0

                                                        , beginReadDateIndicator: "<h2 class=\"date-header\">"
                                                        , beginReadDateOffset: 24
                                                        , endReadDateIndicator: "</h2>"
                                                        , endReadDateOffset: 0
                                                        , dateFormat: dateTimeFormat

                                                        , beginReadPreviewIndicator: "<div class=\"entry-body\">"
                                                        , beginReadPreviewOffset: 0
                                                        , endReadPreviewIndicator: "<div class=\"entry-footer\">"
                                                        , endReadPreviewOffset: 0

                                                        , defaultTitle: ""
                                                        , defaultAuthor: defaultAuthor
                                                        , defaultTextPreview: ""
                                                        , defaultWeblink: ""
                                                        , defaultBlogName: defaultBlogName
                                                        , defaultBlogCategory: defaultBlogCategory
                                                        );
        }

        public static void gatherEntriesGeneric(string blogAddress = ""
                                                    , string beginReadAllIndicator = ""
                                                    , string endReadAllIndicator = ""
                                                    , string endCurrentEntryIndicator = ""
                                                    , string beginReadAuthorIndicator = ""
                                                    , int beginReadAuthorOffset = 0
                                                    , string endReadAuthorIndicator = ""
                                                    , int endReadAuthorOffset = 0

                                                    , string beginReadTitleWeblinkIndicator = ""
                                                    , string beginReadTitleIndicator = ""
                                                    , int beginReadTitleOffset = 0
                                                    , string endReadTitleIndicator = ""
                                                    , int endReadTitleOffset = 0

                                                    , string beginReadWeblinkIndicator = ""
                                                    , int beginReadWeblinkOffset = 0
                                                    , string endReadWeblinkIndicator = ""
                                                    , int endReadWeblinkOffset = 0

                                                    , string beginReadDateIndicator = ""
                                                    , int beginReadDateOffset = 0
                                                    , string endReadDateIndicator = ""
                                                    , int endReadDateOffset = 0
                                                    , string dateFormat = ""

                                                    , string beginReadPreviewIndicator = ""
                                                    , int beginReadPreviewOffset = 0
                                                    , string endReadPreviewIndicator = ""
                                                    , int endReadPreviewOffset = 0

                                                    , string defaultTitle = ""
                                                    , string defaultAuthor = ""
                                                    , string defaultTextPreview = ""
                                                    , string defaultWeblink = ""
                                                    , string defaultBlogName = ""
                                                    , string defaultBlogCategory = ""
                                                    , string weblinkPrefix = ""
                                                    )
        {
            WebClient client = new WebClient();

            using (var stream = client.OpenRead(blogAddress))
            using (StreamReader reader = new StreamReader(stream))
            {
                string lines, webString = "";
                int string_position;
                bool beginRead = false, previewContinue = false;
                Post nextPost = new Post(defaultTitle, defaultAuthor, DateTime.Today, defaultTextPreview, defaultWeblink, defaultBlogName, defaultBlogCategory);

                while ((lines = reader.ReadLine()) != null)
                {
                    //START OF ALL BLOG ENTRIES
                    if (lines.IndexOf(beginReadAllIndicator) != -1 && !beginRead)
                        beginRead = true;
                    else if (beginRead)
                    {
                        //END OF ALL BLOG ENTRIES
                        if (lines.IndexOf(endReadAllIndicator) != -1)
                            break;

                        //SET AUTHOR
                        if (lines.IndexOf(beginReadAuthorIndicator) != -1)
                        {
                            string_position = lines.IndexOf(beginReadAuthorIndicator) + beginReadAuthorOffset;
                            nextPost.author = lines.Substring(string_position, lines.IndexOf(endReadAuthorIndicator, string_position + endReadAuthorOffset) - string_position).Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                        }

                        //SET TITLE AND WEBLINK
                        if (lines.IndexOf(beginReadTitleWeblinkIndicator) != -1)
                        {
                            //Console.WriteLine("HERE");
                            if (lines.IndexOf(beginReadTitleIndicator) != -1)
                            {
                                string_position = lines.IndexOf(beginReadTitleIndicator) + beginReadTitleOffset;
                                nextPost.title = lines.Substring(string_position, lines.IndexOf(endReadTitleIndicator, string_position + endReadTitleOffset) - string_position);
                            }
                            if (lines.IndexOf(beginReadWeblinkIndicator) != -1)
                            {
                                string_position = lines.IndexOf(beginReadWeblinkIndicator) + beginReadWeblinkOffset;
                                nextPost.weblink = weblinkPrefix + lines.Substring(string_position, lines.IndexOf(endReadWeblinkIndicator, string_position + endReadWeblinkOffset) - string_position).Replace("\r", "").Replace("\n", "").Replace("\t", "").TrimStart();
                            }
                        }

                        //SET DATE
                        if (lines.IndexOf(beginReadDateIndicator) != -1)
                        {
                            string_position = lines.IndexOf(beginReadDateIndicator) + beginReadDateOffset;
                            //Console.WriteLine(string_position + " " + endReadDateIndicator + " " + lines.IndexOf(endReadDateIndicator, string_position + endReadDateOffset) + " " + lines);

                            try
                            {
                                webString = lines.Substring(string_position, lines.IndexOf(endReadDateIndicator, string_position + endReadDateOffset) - string_position).Trim();
                                if (dateFormat.IndexOf("cc,") != -1)
                                {
                                    nextPost.publishDate = DateTime.ParseExact(webString.Substring(0, webString.IndexOf(",") - 2) + webString.Substring(webString.IndexOf(","))
                                               , dateFormat.Replace("cc", "")
                                               , System.Globalization.CultureInfo.InvariantCulture);
                                }
                                else
                                {
                                    Console.WriteLine(lines + "\n" + dateFormat + " " + webString);
                                    nextPost.publishDate = DateTime.ParseExact(webString, dateFormat
                                               , System.Globalization.CultureInfo.InvariantCulture);
                                }
                            }
                            catch { }
                        }

                        //SET PREVIEW
                        if ((lines.IndexOf(beginReadPreviewIndicator) != -1) || previewContinue)
                        {
                            //Console.WriteLine("HERE");
                            if (!previewContinue)
                            {
                                previewContinue = true;
                                string_position = lines.IndexOf(beginReadPreviewIndicator) + beginReadPreviewOffset;
                                webString = lines;
                            }
                            else
                            {
                                webString += lines;
                            }

                            //CHECK THAT EXCERPT ENDING OR OVER LIMIT
                            if (webString.IndexOf(endReadPreviewIndicator) != -1)
                            {
                                //REMOVE ENDLINES AND TABS
                                webString = webString.Replace("\r", "").Replace("\n", "").Replace("\t", "");

                                bool webElement = false;
                                string webStringElementsRemoved = "";

                                for (int i = 0; i < webString.Length - 1; i++)
                                {
                                    if (webString[i] == '<' && !webElement)
                                        webElement = true;
                                    else if (webString[i] == '>' && webElement)
                                        webElement = false;
                                    else if (!webElement && webString.Substring(i, 2) != "  ")
                                        webStringElementsRemoved += webString[i].ToString();
                                }

                                if (webStringElementsRemoved.Length > 344)
                                    webStringElementsRemoved = webStringElementsRemoved.Substring(0, 344).Trim() + " [...]";

                                previewContinue = false;
                                nextPost.textPreview = webStringElementsRemoved.TrimEnd();
                            }
                        }

                        //END OF CURRENT BLOG ENTRY
                        if (lines.IndexOf(endCurrentEntryIndicator) != -1)
                        {
                            blogList.Add(nextPost);
                            nextPost = new Post(defaultTitle, defaultAuthor, DateTime.Today, defaultTextPreview, defaultWeblink, defaultBlogName, defaultBlogCategory);
                        }
                    }
                }
            }
        }
        #endregion

        #region Poorly Designed Gatherers
        public static void gatherFlowingDataEntries()
        {
            string blogAddress = "http://flowingdata.com/";
            WebClient client = new WebClient();

            using (var stream = client.OpenRead(blogAddress))
            using (StreamReader reader = new StreamReader(stream))
            {
                string lines, webString = "", tagInfo = "";
                int string_position;
                bool beginRead = false, previewContinue = false;
                Post nextPost = new Post();
                nextPost.author = "Nathan Yau";
                nextPost.blogName = "Flowing Data";
                nextPost.blogCategory = "DATA";

                while ((lines = reader.ReadLine()) != null)
                {
                    //START OF ALL BLOG ENTRIES
                    if (lines.IndexOf("<div id=\"main-wrapper\">") != -1 && !beginRead)
                        beginRead = true;
                    else if (beginRead)
                    {
                        //END OF ALL BLOG ENTRIES
                        if (lines.IndexOf("<div class=\"nav-link\">") != -1)
                            break;

                        //SET TITLE AND WEBLINK
                        if (lines.IndexOf("rel=\"bookmark\"") != -1 && lines.IndexOf("</a>") != -1)
                        {
                            string_position = lines.IndexOf("<a href=\"") + 9;
                            nextPost.weblink = lines.Substring(string_position, lines.IndexOf("\"", string_position) - string_position);
                            string_position = lines.IndexOf("\">", string_position) + 2;
                            //Console.WriteLine(string_position + " " + lines.IndexOf("</a>", string_position) + " " + lines);
                            nextPost.title = lines.Substring(string_position, lines.IndexOf("</a>", string_position) - string_position).Replace("\r", "").Replace("\n", "").Replace("\t", "").TrimStart();
                        }

                        //SET DATE
                        if (lines.IndexOf("rel=\"category tag\">") != -1)
                        {
                            webString = lines.Substring(0, lines.IndexOf("&nbsp;|&nbsp;")).Trim();
                            nextPost.publishDate = DateTime.ParseExact(webString, "MMMM d, yyyy",
                                           System.Globalization.CultureInfo.InvariantCulture);

                            string_position = lines.IndexOf("rel=\"category tag\">") + 19;
                            tagInfo = lines.Substring(string_position, lines.IndexOf("</a>", string_position) - string_position).Trim();
                        }

                        //SET PREVIEW
                        if ((lines.IndexOf("<div class=\"entry\">") != -1) || previewContinue)
                        {
                            if (!previewContinue)
                            {
                                previewContinue = true;
                                webString = tagInfo + " | ";
                            }
                            else
                            {
                                webString += lines;
                            }

                            //CHECK THAT EXCERPT ENDING OR OVER LIMIT
                            if (webString.IndexOf("<div class=\"clear-line\"></div>") != -1)
                            {
                                //REMOVE ENDLINES AND TABS
                                webString = webString.Replace("\r", "").Replace("\n", "").Replace("\t", "");

                                bool webElement = false;
                                string webStringElementsRemoved = "";

                                for (int i = 0; i < webString.Length - 1; i++)
                                {
                                    if (webString[i] == '<' && !webElement)
                                        webElement = true;
                                    else if (webString[i] == '>' && webElement)
                                        webElement = false;
                                    else if (!webElement && webString.Substring(i, 2) != "  ")
                                        webStringElementsRemoved += webString[i].ToString();
                                }

                                if (webStringElementsRemoved.Length > 344)
                                    webStringElementsRemoved = webStringElementsRemoved.Substring(0, 344).Trim() + " [...]";

                                previewContinue = false;
                                nextPost.textPreview = webStringElementsRemoved.TrimEnd();
                            }
                        }

                        //END OF CURRENT BLOG ENTRY
                        if (lines.IndexOf("<div class=\"clear-line\"></div>") != -1)
                        {
                            blogList.Add(nextPost);
                            nextPost = new Post();
                            nextPost.author = "Nathan Yau";
                            nextPost.blogName = "Flowing Data";
                            nextPost.blogCategory = "DATA";
                        }
                    }
                }
            }
        }

        public static void gatherThatJeffSmithEntries()
        {
            string blogAddress = "http://www.thatjeffsmith.com/";
            WebClient client = new WebClient();

            using (var stream = client.OpenRead(blogAddress))
            using (StreamReader reader = new StreamReader(stream))
            {
                string lines, webString = "";
                int string_position;
                bool beginRead = false, previewContinue = false;
                Post nextPost = new Post();
                nextPost.author = "Jeff Smith";
                nextPost.blogName = "That Jeff Smith";
                nextPost.blogCategory = "ORACLE";

                while ((lines = reader.ReadLine()) != null)
                {
                    //START OF ALL BLOG ENTRIES
                    if (lines.IndexOf("<!-- /#headerwrap -->") != -1 && !beginRead)
                        beginRead = true;
                    else if (beginRead)
                    {
                        //END OF ALL BLOG ENTRIES
                        if (lines.IndexOf("<!-- /#content -->") != -1)
                            break;

                        //SET TITLE AND WEBLINK
                        if (lines.IndexOf("<h1 class=\"post-title\">") != -1)
                        {
                            string_position = lines.IndexOf("<a href=\"") + 9;
                            nextPost.weblink = lines.Substring(string_position, lines.IndexOf("\"", string_position) - string_position);
                            string_position = lines.IndexOf("\">", string_position) + 2;
                            nextPost.title = lines.Substring(string_position, lines.IndexOf("</a>", string_position) - string_position).Replace("\r", "").Replace("\n", "").Replace("\t", "").TrimStart();
                        }

                        //SET DATE
                        if (lines.IndexOf("pubdate>") != -1)
                        {
                            string_position = lines.IndexOf("pubdate>") + 8;
                            webString = lines.Substring(string_position, lines.IndexOf("</time>", string_position) - string_position).Replace("<b>Posted:</b>", "").Trim();
                            nextPost.publishDate = DateTime.ParseExact(webString, "MMM d, yyyy",
                                           System.Globalization.CultureInfo.InvariantCulture);
                        }

                        //SET PREVIEW
                        if ((lines.IndexOf("<!-- post-content -->") != -1) || previewContinue)
                        {
                            if (!previewContinue)
                            {
                                previewContinue = true;
                                webString = lines;
                            }
                            else
                            {
                                webString += lines;
                            }

                            //CHECK THAT EXCERPT ENDING OR OVER LIMIT
                            if (webString.IndexOf("<!-- /post-content -->") != -1)
                            {
                                //REMOVE ENDLINES AND TABS
                                webString = webString.Replace("\r", "").Replace("\n", "").Replace("\t", "");

                                bool webElement = false;
                                string webStringElementsRemoved = "";

                                for (int i = 0; i < webString.Length - 1; i++)
                                {
                                    if (webString[i] == '<' && !webElement)
                                        webElement = true;
                                    else if (webString[i] == '>' && webElement)
                                        webElement = false;
                                    else if (!webElement && webString.Substring(i, 2) != "  ")
                                        webStringElementsRemoved += webString[i].ToString();
                                }

                                previewContinue = false;
                                nextPost.textPreview = webStringElementsRemoved.TrimEnd();
                            }
                        }

                        //END OF CURRENT BLOG ENTRY
                        if (lines.IndexOf("<!-- /post -->") != -1)
                        {
                            blogList.Add(nextPost);
                            nextPost = new Post();
                            nextPost.author = "Jeff Smith";
                            nextPost.blogName = "That Jeff Smith";
                            nextPost.blogCategory = "ORACLE";
                        }
                    }
                }
            }
        }

        public static void gatherTheRumpusEntries()
        {
            string blogAddress = "http://therumpus.net/";
            WebClient client = new WebClient();

            using (var stream = client.OpenRead(blogAddress))
            using (StreamReader reader = new StreamReader(stream))
            {
                string lines, webString = "";
                int string_position;
                bool beginRead = false, previewContinue = false;
                Post nextPost = new Post();
                nextPost.blogName = "The Rumpus";
                nextPost.blogCategory = "ECLECTIC";

                while ((lines = reader.ReadLine()) != null)
                {
                    //START OF ALL BLOG ENTRIES
                    if (lines.IndexOf("<!-- end header -->") != -1 && !beginRead)
                        beginRead = true;
                    else if (beginRead)
                    {
                        //END OF ALL BLOG ENTRIES
                        if (lines.IndexOf("<!-- #main -->") != -1)
                            break;

                        //SET AUTHOR
                        if (lines.IndexOf("rel=\"author\">") != -1)
                        {
                            string_position = lines.IndexOf("rel=\"author\">") + 13;
                            nextPost.author = lines.Substring(string_position, lines.IndexOf("</a>", string_position) - string_position).Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                        }

                        //SET TITLE AND WEBLINK
                        if (lines.IndexOf("<h3 class=\"title\">") != -1)
                        {
                            string_position = lines.IndexOf("<a href=\"") + 9;
                            nextPost.weblink = lines.Substring(string_position, lines.IndexOf("\"", string_position) - string_position);
                            string_position = lines.IndexOf("\">", string_position) + 2;
                            nextPost.title = lines.Substring(string_position, lines.IndexOf("</a>", string_position) - string_position).Replace("\r", "").Replace("\n", "").Replace("\t", "").TrimStart();
                        }

                        //SET DATE
                        if (lines.IndexOf("pubdate>") != -1)
                        {
                            string_position = lines.IndexOf("pubdate>") + 8;
                            webString = lines.Substring(string_position, lines.IndexOf("</time>", string_position) - string_position).Trim();
                            webString = webString.Substring(0, webString.IndexOf(",") - 2) + webString.Substring(webString.IndexOf(","));
                            nextPost.publishDate = DateTime.ParseExact(webString, "MMMM d, yyyy",
                                           System.Globalization.CultureInfo.InvariantCulture);
                        }

                        //SET PREVIEW
                        if ((lines.IndexOf("<div class=\"lead\">") != -1) || previewContinue)
                        {
                            if (!previewContinue)
                            {
                                previewContinue = true;
                                string_position = lines.IndexOf("<div class=\"lead\">") + 18;
                                webString = lines.Substring(string_position);
                            }
                            else
                            {
                                webString += lines;
                            }

                            //CHECK THAT EXCERPT ENDING OR OVER LIMIT
                            if (webString.IndexOf("</div>") != -1)
                            {
                                //REMOVE ENDLINES AND TABS
                                webString = webString.Replace("\r", "").Replace("\n", "").Replace("\t", "").Replace("...more", "...");

                                bool webElement = false;
                                string webStringElementsRemoved = "";

                                for (int i = 0; i < webString.Length - 1; i++)
                                {
                                    if (webString[i] == '<' && !webElement)
                                        webElement = true;
                                    else if (webString[i] == '>' && webElement)
                                        webElement = false;
                                    else if (!webElement && webString.Substring(i, 2) != "  ")
                                        webStringElementsRemoved += webString[i].ToString();
                                }

                                if (webStringElementsRemoved.Length > 344)
                                    webStringElementsRemoved = webStringElementsRemoved.Substring(0, 344).Trim() + " [...]";

                                previewContinue = false;
                                nextPost.textPreview = webStringElementsRemoved.TrimEnd();
                            }
                        }

                        //END OF CURRENT BLOG ENTRY
                        if (lines.IndexOf("<!-- end article -->") != -1)
                        {
                            blogList.Add(nextPost);
                            nextPost = new Post();
                            nextPost.blogName = "The Rumpus";
                            nextPost.blogCategory = "ECLECTIC";

                        }
                    }
                }
            }
        }

        public static void gatherApaxoEntries()
        {
            string blogAddress = "http://www.apaxo.de/blog/";
            WebClient client = new WebClient();

            using (var stream = client.OpenRead(blogAddress))
            using (StreamReader reader = new StreamReader(stream))
            {
                string lines, webString = "";
                int string_position;
                bool beginRead = false, previewContinue = false, titleNext = false;
                Post nextPost = new Post();
                nextPost.blogName = "Apaxo";
                nextPost.blogCategory = "BUSINESS";

                while ((lines = reader.ReadLine()) != null)
                {
                    //START OF ALL BLOG ENTRIES
                    if (lines.IndexOf("</header>") != -1 && !beginRead)
                        beginRead = true;
                    else if (beginRead)
                    {
                        //END OF ALL BLOG ENTRIES
                        if (lines.IndexOf("<!-- #nav-below .navigation -->") != -1)
                            break;

                        //SET AUTHOR
                        if (lines.IndexOf("rel=\"author\">") != -1)
                        {
                            string_position = lines.IndexOf("rel=\"author\">") + 13;
                            nextPost.author = lines.Substring(string_position, lines.IndexOf("</a>", string_position) - string_position).Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                        }

                        //SET TITLE AND WEBLINK
                        if (titleNext)
                        {
                            if (lines.IndexOf("<a href=\"") != -1)
                            {
                                string_position = lines.IndexOf("<a href=\"") + 9;
                                nextPost.weblink = lines.Substring(string_position, lines.IndexOf("\"", string_position) - string_position);
                            }
                            if (lines.IndexOf("\">") != -1)
                            {
                                string_position = lines.IndexOf("\">") + 2;
                                //Console.WriteLine(lines);
                                nextPost.title = lines.Substring(string_position, lines.IndexOf("</a>", string_position) - string_position).Replace("\r", "").Replace("\n", "").Replace("\t", "").TrimStart();
                            }
                            titleNext = false;
                        }

                        if (lines.IndexOf("<h1 class=\"entry-title\">") != -1)
                            titleNext = true;

                        //SET DATE
                        if (lines.IndexOf("<time") != -1)
                        {
                            string_position = lines.IndexOf(">", lines.IndexOf("<time")) + 1;
                            webString = lines.Substring(string_position, lines.IndexOf("</time>", string_position) - string_position).Replace("<b>Posted:</b>", "").Trim();
                            nextPost.publishDate = DateTime.ParseExact(webString, "yyyy-MM-dd",
                                           System.Globalization.CultureInfo.InvariantCulture);
                        }

                        //SET PREVIEW
                        if ((lines.IndexOf("<div class=\"entry-content\">") != -1) || previewContinue)
                        {
                            if (!previewContinue)
                            {
                                previewContinue = true;
                                webString = "";
                            }
                            else
                            {
                                webString += lines;
                            }

                            //CHECK THAT EXCERPT ENDING OR OVER LIMIT
                            if (webString.IndexOf("<!-- .entry-content -->") != -1)
                            {
                                //REMOVE ENDLINES AND TABS
                                webString = webString.Replace("\r", "").Replace("\n", "").Replace("\t", "");

                                bool webElement = false;
                                string webStringElementsRemoved = "";

                                for (int i = 0; i < webString.Length - 1; i++)
                                {
                                    if (webString[i] == '<' && !webElement)
                                        webElement = true;
                                    else if (webString[i] == '>' && webElement)
                                        webElement = false;
                                    else if (!webElement && webString.Substring(i, 2) != "  ")
                                        webStringElementsRemoved += webString[i].ToString();
                                }

                                if (webStringElementsRemoved.Length > 344)
                                    webStringElementsRemoved = webStringElementsRemoved.Substring(0, 344).Trim() + " [...]";

                                previewContinue = false;
                                nextPost.textPreview = webStringElementsRemoved.TrimEnd();
                            }
                        }

                        //END OF CURRENT BLOG ENTRY
                        if (lines.IndexOf("<!-- #Post -->") != -1)
                        {
                            blogList.Add(nextPost);
                            nextPost = new Post();
                            nextPost.blogName = "Apaxo";
                            nextPost.blogCategory = "BUSINESS";
                        }
                    }
                }
            }
        }

        public static void gatherBrainPickingsEntries()
        {
            string blogAddress = "http://www.brainpickings.org/";
            WebClient client = new WebClient();

            using (var stream = client.OpenRead(blogAddress))
            using (StreamReader reader = new StreamReader(stream))
            {
                string lines, webString = "";
                int string_position;
                bool beginRead = false, previewContinue = false;
                Post nextPost = new Post();
                nextPost.blogName = "Brain Pickings";
                nextPost.blogCategory = "ECLECTIC";

                while ((lines = reader.ReadLine()) != null)
                {
                    //START OF ALL BLOG ENTRIES
                    if (lines.IndexOf("<div class=\"home\">") != -1 && !beginRead)
                        beginRead = true;
                    else if (beginRead)
                    {
                        //END OF ALL BLOG ENTRIES
                        if (lines.IndexOf("<div id=\"sidebar\">") != -1)
                            break;

                        //SET AUTHOR
                        if (lines.IndexOf("rel=\"author\">") != -1)
                        {
                            string_position = lines.IndexOf("rel=\"author\">") + 13;
                            nextPost.author = lines.Substring(string_position, lines.IndexOf("</a>", string_position) - string_position).Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                        }

                        //SET TITLE AND WEBLINK
                        if (lines.IndexOf("<h2>") != -1)
                        {
                            string_position = lines.IndexOf("<a href=\"") + 9;
                            nextPost.weblink = lines.Substring(string_position, lines.IndexOf("\"", string_position) - string_position);

                            string_position = lines.IndexOf("\">") + 2;
                            nextPost.title = lines.Substring(string_position, lines.IndexOf("</a>", string_position) - string_position).Replace("\r", "").Replace("\n", "").Replace("\t", "").TrimStart();
                        }

                        //SET PREVIEW
                        if ((lines.IndexOf("<p class=\"intro\">") != -1) || previewContinue)
                        {
                            if (!previewContinue)
                            {
                                previewContinue = true;
                                webString = lines;
                            }
                            else
                            {
                                webString += lines;
                            }

                            //CHECK THAT EXCERPT ENDING OR OVER LIMIT
                            if (lines.IndexOf("Brain Pickings has a free weekly newsletter.") != -1 || webString.Length > 1000)
                            {
                                //REMOVE ENDLINES AND TABS
                                webString = webString.Replace("\r", "").Replace("\n", "").Replace("\t", "");

                                bool webElement = false;
                                string webStringElementsRemoved = "";

                                for (int i = 0; i < webString.Length - 1; i++)
                                {
                                    if (webString[i] == '<' && !webElement)
                                        webElement = true;
                                    else if (webString[i] == '>' && webElement)
                                        webElement = false;
                                    else if (!webElement && webString.Substring(i, 2) != "  ")
                                        webStringElementsRemoved += webString[i].ToString();
                                }

                                if (webStringElementsRemoved.Length > 344)
                                    webStringElementsRemoved = webStringElementsRemoved.Substring(0, 344).Trim() + " [...]";

                                previewContinue = false;
                                nextPost.textPreview = webStringElementsRemoved.TrimEnd();
                            }
                        }

                        //END OF CURRENT BLOG ENTRY
                        if (lines.IndexOf("Brain Pickings has a free weekly newsletter.") != -1)
                        {
                            blogList.Add(nextPost);
                            nextPost = new Post();
                            nextPost.blogName = "Brain Pickings";
                            nextPost.blogCategory = "ECLECTIC";
                        }
                    }
                }
            }
        }

        public static void gatherGapMinderEntries()
        {
            string blogAddress = "http://www.gapminder.org/news/";
            WebClient client = new WebClient();

            using (var stream = client.OpenRead(blogAddress))
            using (StreamReader reader = new StreamReader(stream))
            {
                string lines, webString = "";
                int string_position;
                bool beginRead = false, previewContinue = false;
                Post nextPost = new Post();
                nextPost.author = "Rosling";
                nextPost.blogName = "Gap Minder";
                nextPost.blogCategory = "DATA ";

                while ((lines = reader.ReadLine()) != null)
                {
                    //START OF ALL BLOG ENTRIES
                    if (lines.IndexOf("<h1>Blog</h1>") != -1 && !beginRead)
                        beginRead = true;
                    else if (beginRead)
                    {
                        //END OF ALL BLOG ENTRIES
                        if (lines.IndexOf("<div class=\"navigation-links\">") != -1)
                            break;

                        //SET AUTHOR
                        //if (lines.IndexOf("rel=\"author\">") != -1)
                        //{
                        //    string_position = lines.IndexOf("rel=\"author\">") + 13;
                        //    nextPost.author = lines.Substring(string_position, lines.IndexOf("</a>", string_position) - string_position).Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                        //}

                        //SET TITLE AND WEBLINK
                        if (lines.IndexOf("class=\"Post-title entry-title\">") != -1)
                        {
                            if (lines.IndexOf("<a href=\"") != -1)
                            {
                                string_position = lines.IndexOf("<a href=\"") + 9;
                                nextPost.weblink = lines.Substring(string_position, lines.IndexOf("\"", string_position) - string_position);
                            }
                            if (lines.IndexOf("\">") != -1)
                            {
                                string_position = lines.IndexOf("\">", lines.IndexOf("<a href=\"")) + 2;
                                nextPost.title = lines.Substring(string_position, lines.IndexOf("</a>", string_position) - string_position).Replace("\r", "").Replace("\n", "").Replace("\t", "").TrimStart();
                            }
                        }

                        //SET DATE
                        if (lines.IndexOf("<small>") != -1)
                        {
                            string_position = lines.IndexOf("<small>") + 7;
                            webString = lines.Substring(string_position, lines.IndexOf("</small>", string_position) - string_position).Replace("<b>Posted:</b>", "").Trim();
                            webString = webString.Substring(0, webString.IndexOf(",") - 2) + webString.Substring(webString.IndexOf(","));

                            nextPost.publishDate = DateTime.ParseExact(webString, "MMMM d, yyyy",
                                           System.Globalization.CultureInfo.InvariantCulture);
                        }

                        //SET PREVIEW
                        if ((lines.IndexOf("class=\"entry-summary entry\">") != -1) || previewContinue)
                        {
                            if (!previewContinue)
                            {
                                previewContinue = true;
                                webString = "";
                            }
                            else
                            {
                                webString += lines;
                            }

                            //CHECK THAT EXCERPT ENDING OR OVER LIMIT
                            if (webString.IndexOf("</p>") != -1)
                            {
                                //REMOVE ENDLINES AND TABS
                                webString = webString.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();

                                bool webElement = false;
                                string webStringElementsRemoved = "";

                                for (int i = 0; i < webString.Length - 1; i++)
                                {
                                    if (webString[i] == '<' && !webElement)
                                        webElement = true;
                                    else if (webString[i] == '>' && webElement)
                                        webElement = false;
                                    else if (!webElement && webString.Substring(i, 2) != "  ")
                                        webStringElementsRemoved += webString[i].ToString();
                                }

                                if (webStringElementsRemoved.Length > 344)
                                    webStringElementsRemoved = webStringElementsRemoved.Substring(0, 347).Trim();

                                previewContinue = false;
                                nextPost.textPreview = webStringElementsRemoved.TrimEnd();
                            }
                        }

                        //END OF CURRENT BLOG ENTRY
                        if (lines.IndexOf("<!-- .hentry .Post -->") != -1)
                        {
                            if (nextPost.title != "UNDEFINED")
                                blogList.Add(nextPost);
                            nextPost = new Post();
                            nextPost.author = "Rosling";
                            nextPost.blogName = "Gap Minder";
                            nextPost.blogCategory = "DATA ";
                        }
                    }
                }
            }
        }

        public static void gatherIdibonEntries()
        {
            string blogAddress = "http://idibon.com/blog/";
            WebClient client = new WebClient();

            using (var stream = client.OpenRead(blogAddress))
            using (StreamReader reader = new StreamReader(stream))
            {
                string lines, webString = "";
                int string_position;
                bool beginRead = false, previewNext = false;
                Post nextPost = new Post();
                nextPost.author = "Rob Munro";
                nextPost.blogName = "idibon";
                nextPost.blogCategory = "DATA ";

                while ((lines = reader.ReadLine()) != null)
                {
                    //START OF ALL BLOG ENTRIES
                    if (lines.IndexOf("</header>") != -1 && !beginRead)
                        beginRead = true;
                    else if (beginRead)
                    {
                        //END OF ALL BLOG ENTRIES
                        if (lines.IndexOf("<!-- main -->") != -1)
                            break;

                        //SET AUTHOR
                        //if (lines.IndexOf("rel=\"author\">") != -1)
                        //{
                        //    string_position = lines.IndexOf("rel=\"author\">") + 13;
                        //    nextPost.author = lines.Substring(string_position, lines.IndexOf("</a>", string_position) - string_position).Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                        //}

                        //SET TITLE AND WEBLINK
                        if (lines.IndexOf("<h2>") != -1)
                        {
                            if (lines.IndexOf("<a href=\"") != -1)
                            {
                                string_position = lines.IndexOf("<a href=\"") + 9;
                                nextPost.weblink = lines.Substring(string_position, lines.IndexOf("\"", string_position) - string_position);
                            }
                            if (lines.IndexOf("\">") != -1)
                            {
                                string_position = lines.IndexOf("\">") + 2;
                                nextPost.title = lines.Substring(string_position, lines.IndexOf("</a>", string_position) - string_position).Replace("\r", "").Replace("\n", "").Replace("\t", "").TrimStart();
                            }
                        }

                        //SET PREVIEW
                        if (previewNext)
                        {
                            //CHECK THAT EXCERPT ENDING OR OVER LIMIT
                            if (lines.IndexOf("<p>") != -1)
                            {
                                webString += lines;
                            }
                            else
                            {
                                //REMOVE ENDLINES AND TABS
                                webString = webString.Replace("\r", "").Replace("\n", "").Replace("\t", "");

                                bool webElement = false;
                                string webStringElementsRemoved = "";

                                for (int i = 0; i < webString.Length - 1; i++)
                                {
                                    if (webString[i] == '<' && !webElement)
                                        webElement = true;
                                    else if (webString[i] == '>' && webElement)
                                        webElement = false;
                                    else if (!webElement && webString.Substring(i, 2) != "  ")
                                        webStringElementsRemoved += webString[i].ToString();
                                }

                                if (webStringElementsRemoved.Length > 344)
                                    webStringElementsRemoved = webStringElementsRemoved.Substring(0, 344).Trim() + " [...]";

                                //previewContinue = false;
                                nextPost.textPreview = webStringElementsRemoved.TrimEnd();
                            }
                        }

                        //SET DATE
                        if (lines.IndexOf("<em class=\"meta\">") != -1)
                        {
                            string_position = lines.IndexOf("<em class=\"meta\">") + 24;
                            webString = lines.Substring(string_position, lines.IndexOf("</em>", string_position) - string_position).Replace("<b>Posted:</b>", "").Trim();
                            nextPost.publishDate = DateTime.ParseExact(webString, "MM.dd.yyyy",
                                           System.Globalization.CultureInfo.InvariantCulture);
                            previewNext = true;
                            webString = "";
                        }

                        //END OF CURRENT BLOG ENTRY
                        if (lines.IndexOf("<!-- Post -->") != -1)
                        {
                            blogList.Add(nextPost);
                            nextPost = new Post();
                            nextPost.author = "Rob Munro";
                            nextPost.blogName = "idibon";
                            nextPost.blogCategory = "DATA ";
                        }
                    }
                }
            }
        }

        public static void gatherHilaryMasonEntries()
        {
            string blogAddress = "http://www.hilarymason.com";
            WebClient client = new WebClient();

            using (var stream = client.OpenRead(blogAddress))
            using (StreamReader reader = new StreamReader(stream))
            {
                string lines, webString = "";
                int string_position;
                bool beginRead = false, previewContinue = false;
                Post nextPost = new Post();
                nextPost.blogName = "Hilary Mason";
                nextPost.blogCategory = "DATA ";

                while ((lines = reader.ReadLine()) != null)
                {
                    //START OF ALL BLOG ENTRIES
                    if (lines.IndexOf("<div class=\"content\">") != -1 && !beginRead)
                        beginRead = true;
                    else if (beginRead)
                    {
                        //END OF ALL BLOG ENTRIES
                        if (lines.IndexOf("<div class=\"navigation\">") != -1)
                            break;

                        //SET AUTHOR
                        if (lines.IndexOf("rel=\"author\">") != -1)
                        {
                            string_position = lines.IndexOf("rel=\"author\">") + 13;
                            nextPost.author = lines.Substring(string_position, lines.IndexOf("</a>", string_position) - string_position).Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                        }

                        //SET TITLE AND WEBLINK
                        if (lines.IndexOf("<h1><a href=") != -1)
                        {
                            if (lines.IndexOf("<a href=\"") != -1)
                            {
                                string_position = lines.IndexOf("<a href=\"") + 9;
                                nextPost.weblink = lines.Substring(string_position, lines.IndexOf("\"", string_position) - string_position);
                            }
                            if (lines.IndexOf("\">") != -1)
                            {
                                string_position = lines.IndexOf("\">") + 2;
                                nextPost.title = lines.Substring(string_position, lines.IndexOf("</a>", string_position) - string_position).Replace("\r", "").Replace("\n", "").Replace("\t", "").TrimStart();
                            }
                        }

                        //SET DATE
                        if (lines.IndexOf("<span class=\"Post-date\">") != -1)
                        {
                            string_position = lines.IndexOf("<span class=\"Post-date\">") + 24;
                            webString = lines.Substring(string_position, lines.IndexOf("|", string_position) - string_position).Replace("<b>Posted:</b>", "").Trim();
                            nextPost.publishDate = DateTime.ParseExact(webString, "MMMM d, yyyy",
                                           System.Globalization.CultureInfo.InvariantCulture);
                        }

                        //SET PREVIEW
                        if ((lines.IndexOf("<b>Tags:</b>") != -1) || previewContinue)
                        {
                            if (!previewContinue)
                            {
                                previewContinue = true;
                                string_position = lines.IndexOf("<b>Tags:</b>") + 3;
                                webString = lines.Substring(string_position, lines.IndexOf("|", string_position) - string_position);
                                webString += " -- ";
                            }
                            else
                            {
                                webString += lines;
                            }

                            //CHECK THAT EXCERPT ENDING OR OVER LIMIT
                            if (webString.IndexOf("<hr/>") != -1)
                            {
                                //REMOVE ENDLINES AND TABS
                                webString = webString.Replace("\r", "").Replace("\n", "").Replace("\t", "");

                                bool webElement = false;
                                string webStringElementsRemoved = "";

                                for (int i = 0; i < webString.Length - 1; i++)
                                {
                                    if (webString[i] == '<' && !webElement)
                                        webElement = true;
                                    else if (webString[i] == '>' && webElement)
                                        webElement = false;
                                    else if (!webElement && webString.Substring(i, 2) != "  ")
                                        webStringElementsRemoved += webString[i].ToString();
                                }

                                if (webStringElementsRemoved.Length > 344)
                                    webStringElementsRemoved = webStringElementsRemoved.Substring(0, 344).Trim() + " [...]";

                                previewContinue = false;
                                nextPost.textPreview = webStringElementsRemoved.TrimEnd();
                            }
                        }

                        //END OF CURRENT BLOG ENTRY
                        if (lines.IndexOf("<hr/>") != -1)
                        {
                            blogList.Add(nextPost);
                            nextPost = new Post();
                            nextPost.blogName = "Hilary Mason";
                            nextPost.blogCategory = "DATA ";
                        }
                    }
                }
            }
        }

        public static void gatherSimplyStatisticsEntries()
        {
            string blogAddress = "http://simplystatistics.org/";
            WebClient client = new WebClient();

            using (var stream = client.OpenRead(blogAddress))
            using (StreamReader reader = new StreamReader(stream))
            {
                string lines, webString = "";
                int string_position;
                bool beginRead = false, previewContinue = false;
                Post nextPost = new Post();
                nextPost.blogName = "Simply Statistics";
                nextPost.blogCategory = "DATA ";

                while ((lines = reader.ReadLine()) != null)
                {
                    //START OF ALL BLOG ENTRIES
                    if (lines.IndexOf("<!-- #header -->") != -1 && !beginRead)
                        beginRead = true;
                    else if (beginRead)
                    {
                        //END OF ALL BLOG ENTRIES
                        if (lines.IndexOf("<!-- #nav-below -->") != -1)
                            break;

                        //END OF CURRENT BLOG ENTRY
                        if (lines.IndexOf("<!-- #Post-## -->") != -1)
                        {
                            blogList.Add(nextPost);
                            nextPost = new Post();
                            nextPost.blogName = "Simply Statistics";
                            nextPost.blogCategory = "DATA ";
                        }

                        if (lines.IndexOf("<a class=\"url fn n") != -1)
                        {
                            string_position = lines.IndexOf(">", lines.IndexOf("<a class=\"url fn n")) + 1;
                            nextPost.author = lines.Substring(string_position, lines.IndexOf("</a>", string_position) - string_position).Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                        }

                        //SET TITLE AND WEBLINK
                        if (lines.IndexOf("<h2 class=\"entry-title\">") != -1)
                        {
                            if (lines.IndexOf("<a href=\"") != -1)
                            {
                                string_position = lines.IndexOf("<a href=\"") + 9;
                                nextPost.weblink = lines.Substring(string_position, lines.IndexOf("\"", string_position) - string_position);
                            }
                            if (lines.IndexOf("rel=\"bookmark\">") != -1)
                            {
                                string_position = lines.IndexOf("rel=\"bookmark\">") + 15;
                                nextPost.title = lines.Substring(string_position, lines.IndexOf("</a>", string_position) - string_position).Replace("\r", "").Replace("\n", "").Replace("\t", "").TrimStart();
                            }
                        }

                        //SET DATE
                        if (lines.IndexOf("<span class=\"entry-date\">") != -1)
                        {
                            string_position = lines.IndexOf("<span class=\"entry-date\">") + 25;
                            webString = lines.Substring(string_position, lines.IndexOf("</span>", string_position) - string_position).Replace(".", "").Trim();
                            nextPost.publishDate = DateTime.ParseExact(webString, "MMMM d, yyyy",
                                           System.Globalization.CultureInfo.InvariantCulture);
                        }

                        //SET PREVIEW
                        if ((lines.IndexOf("<div class=\"entry-content\">") != -1) || previewContinue)
                        {
                            if (!previewContinue)
                            {
                                previewContinue = true;
                                string_position = lines.IndexOf("<div class=\"entry-content\">") + 27;
                                webString = lines.Substring(string_position, lines.Length - string_position);
                            }
                            else
                            {
                                webString += lines;
                            }

                            //CHECK THAT EXCERPT ENDING OR OVER LIMIT
                            if (webString.IndexOf("<div id=\"fb-root\">") != -1)
                            {
                                //REMOVE ENDLINES AND TABS
                                webString = webString.Replace("\r", "").Replace("\n", "").Replace("\t", "");

                                bool webElement = false;
                                string webStringElementsRemoved = "";

                                for (int i = 0; i < webString.Length - 1; i++)
                                {
                                    if (webString[i] == '<' && !webElement)
                                        webElement = true;
                                    else if (webString[i] == '>' && webElement)
                                        webElement = false;
                                    else if (!webElement && webString.Substring(i, 2) != "  ")
                                        webStringElementsRemoved += webString[i].ToString();
                                }

                                if (webStringElementsRemoved.Length > 344)
                                    webStringElementsRemoved = webStringElementsRemoved.Substring(0, 344).Trim() + " [...]";

                                previewContinue = false;
                                nextPost.textPreview = webStringElementsRemoved.TrimEnd();
                            }
                        }
                    }
                }
            }
        }

        public static void gatherLanguageLogEntries()
        {
            string blogAddress = "http://languagelog.com/nll/";
            WebClient client = new WebClient();

            using (var stream = client.OpenRead(blogAddress))
            using (StreamReader reader = new StreamReader(stream))
            {
                string lines, webString = "";
                int string_position;
                bool beginRead = false, previewContinue = false, dateNext = false;
                Post nextPost = new Post();
                nextPost.blogName = "Language Log";
                nextPost.blogCategory = "RANDOM";

                while ((lines = reader.ReadLine()) != null)
                {
                    //START OF ALL BLOG ENTRIES
                    if (lines.IndexOf("<div id=\"content\">") != -1)
                        beginRead = true;
                    else if (beginRead)
                    {
                        //END OF ALL BLOG ENTRIES
                        if (lines.IndexOf("<div class=\"footnav\">") != -1)
                            break;

                        //END OF CURRENT BLOG ENTRY
                        if (lines.IndexOf("<p class=\"Postfeedback\">") != -1)
                        {
                            blogList.Add(nextPost);
                            nextPost = new Post();
                            nextPost.blogName = "Language Log";
                            nextPost.blogCategory = "RANDOM";
                        }

                        //SET DATE
                        if (dateNext)
                        {
                            string_position = 0;
                            webString = lines.Substring(string_position, lines.IndexOf("@", string_position) - string_position).Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                            //Console.WriteLine(webString);
                            nextPost.publishDate = DateTime.ParseExact(webString, "MMMM d, yyyy",
                                           System.Globalization.CultureInfo.InvariantCulture);
                            dateNext = false;
                        }

                        if (lines.IndexOf("<p class=\"Postmeta\">") != -1)
                            dateNext = true;

                        //SET WEBLINK AND TITLE
                        if (lines.IndexOf("<h2 class=\"Posttitle\"") != -1)
                        {
                            string_position = lines.IndexOf("<a href=") + 9;
                            nextPost.weblink = lines.Substring(string_position, lines.IndexOf("\"", string_position) - string_position);

                            bool htmlElement = false;

                            string_position = lines.IndexOf("title=\"", string_position) + 7;
                            string htmlString = lines.Substring(string_position, lines.IndexOf("\"", string_position) - string_position);
                            string htmlStringElementsRemoved = "";

                            for (int i = 0; i < htmlString.Length; i++)
                            {
                                if (htmlString[i] == '<' && !htmlElement)
                                    htmlElement = true;
                                else if (htmlString[i] == '>' && htmlElement)
                                    htmlElement = false;
                                else if (!htmlElement)
                                    htmlStringElementsRemoved += htmlString[i].ToString();
                            }

                            nextPost.title = htmlStringElementsRemoved;
                        }

                        //SET AUTHOR
                        if (lines.IndexOf("&#183; Filed by ") != -1)
                        {
                            string_position = lines.IndexOf(">") + 1;
                            nextPost.author = lines.Substring(string_position, lines.IndexOf("<", string_position) - string_position);
                        }

                        //SET PREVIEW
                        if ((lines.IndexOf("<div class=\"Postentry\">") != -1) || previewContinue)
                        {
                            if (!previewContinue)
                            {
                                previewContinue = true;
                                string_position = lines.IndexOf("<div class=\"Postentry\">") + 23;
                                webString = lines.Substring(string_position, lines.Length - string_position);
                            }
                            else
                            {
                                webString += lines;
                            }

                            //CHECK THAT EXCERPT ENDING OR OVER LIMIT
                            if (webString.IndexOf("Read the rest of this entry") != -1)
                            {
                                bool webElement = false;
                                string webStringElementsRemoved = "";

                                webString = webString.Replace("Read the rest of this entry &raquo;", "");

                                for (int i = 0; i < webString.Length; i++)
                                {
                                    if (webString[i] == '<' && !webElement)
                                        webElement = true;
                                    else if (webString[i] == '>' && webElement)
                                        webElement = false;
                                    else if (!webElement)
                                        webStringElementsRemoved += webString[i].ToString();
                                }

                                if (webStringElementsRemoved.Length > 344)
                                    webStringElementsRemoved = webStringElementsRemoved.Substring(0, 344).Trim() + " [...]";

                                previewContinue = false;
                                nextPost.textPreview = webStringElementsRemoved.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                            }
                        }
                    }
                }
            }
        }

        public static void gatherThoughtCatalogEntries()
        {
            string blogAddress = "http://thoughtcatalog.com/";
            WebClient client = new WebClient();

            using (var stream = client.OpenRead(blogAddress))
            using (StreamReader reader = new StreamReader(stream))
            {
                string lines, webString = "";
                int string_position;
                bool beginRead = false, previewContinue = false, dateNext = false, titleNext = false, authorNext = false;
                Post nextPost = new Post();
                nextPost.blogName = "Thought Catalog";
                nextPost.blogCategory = "FUN";

                while ((lines = reader.ReadLine()) != null)
                {
                    //START OF ALL BLOG ENTRIES
                    if (lines.IndexOf("<!-- end div#header -->") != -1 && !beginRead)
                        beginRead = true;
                    else if (beginRead)
                    {
                        //END OF ALL BLOG ENTRIES
                        if (lines.IndexOf("<!-- end div#page -->") != -1)
                            break;

                        //END OF CURRENT BLOG ENTRY
                        if (lines.IndexOf("<!-- div#Post-{id} -->") != -1)
                        {
                            blogList.Add(nextPost);
                            nextPost = new Post();
                            nextPost.blogName = "Thought Catalog";
                            nextPost.blogCategory = "FUN";
                        }

                        if (lines.IndexOf("<div class=\"author_container") != -1)
                            authorNext = true;

                        if (authorNext)
                        {
                            if (lines.IndexOf("</a>") != -1)
                            {
                                string_position = 0;
                                nextPost.author = lines.Substring(string_position, lines.IndexOf("</a>", string_position) - string_position).Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
                                authorNext = false;
                            }
                        }

                        if (lines.IndexOf("<h3 class=\"title_with_thumb") != -1)
                            titleNext = true;

                        //SET TITLE AND WEBLINK
                        if (titleNext)
                        {
                            if (lines.IndexOf("<a href=\"") != -1)
                            {
                                string_position = lines.IndexOf("<a href=\"") + 9;
                                nextPost.weblink = lines.Substring(string_position, lines.IndexOf("\">", string_position) - string_position);
                            }
                            if (lines.IndexOf("</span>") != -1)
                            {
                                string_position = 0;
                                nextPost.title = lines.Substring(string_position, lines.IndexOf("</span>", string_position) - string_position).Replace("\r", "").Replace("\n", "").Replace("\t", "").TrimStart();
                                titleNext = false;
                            }
                        }

                        //SET DATE
                        if (dateNext)
                        {
                            lines = lines.Replace("<span class=\"bold\">New:</span>", "");
                            //Console.WriteLine(lines);
                            string_position = lines.IndexOf("<span>") + 6;
                            webString = lines.Substring(string_position, lines.IndexOf("</span>", string_position) - string_position).Replace(".", "").Trim();
                            //Console.WriteLine(webString);
                            nextPost.publishDate = DateTime.ParseExact(webString, "MMM d, yyyy",
                                           System.Globalization.CultureInfo.InvariantCulture);
                            dateNext = false;
                        }

                        //SET DATE HELPER
                        if (lines.IndexOf("<a class=\"timestamp caps") != -1)
                            dateNext = true;

                        //SET PREVIEW
                        if ((lines.IndexOf("<div class=\"excerpt\">") != -1) || previewContinue)
                        {
                            if (!previewContinue)
                            {
                                previewContinue = true;
                                string_position = lines.IndexOf("<div class=\"excerpt\">") + 21;
                                webString = lines.Substring(string_position, lines.Length - string_position);
                            }
                            else
                            {
                                webString += lines;
                            }

                            //CHECK THAT EXCERPT ENDING OR OVER LIMIT
                            if (webString.IndexOf("</p>") != -1)
                            {
                                //REMOVE ENDLINES AND TABS
                                webString = webString.Replace("\r", "").Replace("\n", "").Replace("\t", "");

                                bool webElement = false;
                                string webStringElementsRemoved = "";

                                for (int i = 0; i < webString.Length - 1; i++)
                                {
                                    if (webString[i] == '<' && !webElement)
                                        webElement = true;
                                    else if (webString[i] == '>' && webElement)
                                        webElement = false;
                                    else if (!webElement && webString.Substring(i, 2) != "  ")
                                        webStringElementsRemoved += webString[i].ToString();
                                }

                                if (webStringElementsRemoved.Length > 344)
                                    webStringElementsRemoved = webStringElementsRemoved.Substring(0, 347) + "...";

                                previewContinue = false;
                                nextPost.textPreview = webStringElementsRemoved.TrimEnd();
                            }
                        }
                    }
                }
            }
        }

        public static void gatherWalkingRandomlyEntries()
        {
            string blogAddress = "http://www.walkingrandomly.com/";
            WebClient client = new WebClient();

            using (var stream = client.OpenRead(blogAddress))
            using (StreamReader reader = new StreamReader(stream))
            {
                string lines, webString = "";
                int string_position;
                bool beginRead = false, previewContinue = false, dateNext = false;
                Post nextPost = new Post();
                nextPost.author = "Mike Croucher";
                nextPost.blogName = "Walking Randomly";
                nextPost.blogCategory = "PROGRAMMING";

                while ((lines = reader.ReadLine()) != null)
                {
                    //START OF ALL BLOG ENTRIES
                    if (lines.IndexOf("<div class=\"Post\">") != -1 && !beginRead)
                        beginRead = true;
                    else if (beginRead)
                    {
                        //END OF ALL BLOG ENTRIES
                        if (lines.IndexOf("<!-- sidebar START -->") != -1)
                            break;

                        //END OF CURRENT BLOG ENTRY
                        if (lines.IndexOf("<div class=\"Post\">") != -1)
                        {
                            blogList.Add(nextPost);
                            nextPost = new Post();
                            nextPost.author = "Mike Croucher";
                            nextPost.blogName = "Walking Randomly";
                            nextPost.blogCategory = "PROGRAMMING";
                        }

                        //SET TITLE AND WEBLINK
                        if (lines.IndexOf("rel=\"bookmark\">") != -1)
                        {
                            string_position = lines.IndexOf("rel=\"bookmark\">") + 15;
                            nextPost.title = lines.Substring(string_position, lines.IndexOf("</a>", string_position) - string_position);
                            string_position = lines.IndexOf("<a href=\"") + 9;
                            nextPost.weblink = lines.Substring(string_position, lines.IndexOf("\"", string_position) - string_position);
                        }

                        //SET DATE HELPER
                        if (lines.IndexOf("<div class=\"info\">") != -1)
                            dateNext = true;

                        //SET DATE
                        if (lines.IndexOf("<span>") != -1 && dateNext)
                        {
                            string_position = lines.IndexOf("<span>") + 6;
                            webString = lines.Substring(string_position, lines.IndexOf("</span>", string_position) - string_position);
                            webString = webString.Substring(0, webString.IndexOf(",") - 2) + webString.Substring(webString.IndexOf(","));

                            nextPost.publishDate = DateTime.ParseExact(webString, "MMMM d, yyyy",
                                           System.Globalization.CultureInfo.InvariantCulture);
                            dateNext = false;
                        }

                        //SET PREVIEW
                        if ((lines.IndexOf("| Categories:") != -1) || previewContinue)
                        {
                            if (!previewContinue)
                            {
                                previewContinue = true;
                                string_position = lines.IndexOf("| Categories:") + 2;
                                webString = lines.Substring(string_position, lines.Length - string_position);
                            }
                            else
                            {
                                webString += lines;
                            }

                            //CHECK THAT EXCERPT ENDING OR OVER LIMIT
                            if (webString.IndexOf("<div class=\"comments\">") != -1)
                            {
                                webString = webString.Replace("<div class=\"content\">", "; Preview: ").Replace("| Tags:", "");
                                webString = webString.Replace("\r", "").Replace("\n", "").Replace("\t", "");

                                bool webElement = false;
                                string webStringElementsRemoved = "";

                                for (int i = 0; i < webString.Length - 1; i++)
                                {
                                    if (webString[i] == '<' && !webElement)
                                        webElement = true;
                                    else if (webString[i] == '>' && webElement)
                                        webElement = false;
                                    else if (!webElement && webString.Substring(i, 2) != "  ")
                                        webStringElementsRemoved += webString[i].ToString();
                                }

                                if (webStringElementsRemoved.Length > 344)
                                    webStringElementsRemoved = webStringElementsRemoved.Substring(0, 347);

                                previewContinue = false;
                                nextPost.textPreview = webStringElementsRemoved.TrimEnd() + "...";
                            }
                        }
                    }
                }
            }
        }

        public static void gatherRevolutionsEntries()
        {
            string blogAddress = "http://blog.revolutionanalytics.com/";
            WebClient client = new WebClient();

            using (var stream = client.OpenRead(blogAddress))
            using (StreamReader reader = new StreamReader(stream))
            {
                string lines, webString = "";
                int string_position;
                bool beginRead = false, previewContinue = false;
                Post nextPost = new Post();
                nextPost.blogName = "Revolutions";
                nextPost.blogCategory = "R";

                while ((lines = reader.ReadLine()) != null)
                {
                    //START OF ALL BLOG ENTRIES
                    if (lines.IndexOf("<!-- entry list sticky -->") != -1)
                        beginRead = true;
                    else if (beginRead)
                    {
                        //END OF ALL BLOG ENTRIES
                        if (lines.IndexOf("<!-- sidebar -->") != -1)
                            break;

                        //END OF CURRENT BLOG ENTRY
                        if (lines.IndexOf("<!-- technorati tags -->") != -1)
                        {
                            blogList.Add(nextPost);
                            nextPost = new Post();
                            nextPost.blogName = "Revolutions";
                            nextPost.blogCategory = "R";
                        }

                        //SET DATE
                        if (lines.IndexOf("class=\"date-header\">") != -1)
                        {
                            string_position = lines.IndexOf("class=\"date-header\">") + 20;
                            webString = lines.Substring(string_position, lines.IndexOf("</h2>", string_position) - string_position);

                            nextPost.publishDate = DateTime.ParseExact(webString, "MMMM dd, yyyy",
                                           System.Globalization.CultureInfo.InvariantCulture);
                        }

                        //SET WEBLINK AND TITLE
                        if (lines.IndexOf("<h3 class=\"entry-header\">") != -1)
                        {
                            string_position = lines.IndexOf("<a href=") + 9;
                            nextPost.weblink = lines.Substring(string_position, lines.IndexOf("\"", string_position) - string_position);

                            string_position = lines.IndexOf("\">", string_position) + 2;
                            nextPost.title = lines.Substring(string_position, lines.IndexOf("</a>", string_position) - string_position);
                        }

                        //SET AUTHOR
                        if (lines.IndexOf("<a rel=\"author\"") != -1)
                        {
                            string_position = lines.IndexOf(">", lines.IndexOf("<a rel=\"author\"") + 15) + 1;
                            nextPost.author = lines.Substring(string_position, lines.IndexOf("</a>", string_position) - string_position);
                        }

                        //SET PREVIEW
                        if ((lines.IndexOf("<div class=\"entry-body\">") != -1) || previewContinue)
                        {
                            if (!previewContinue)
                            {
                                previewContinue = true;
                                string_position = lines.IndexOf("<div class=\"entry-body\">") + 24;
                                webString += lines.Substring(string_position, lines.Length - string_position);
                            }
                            else
                            {
                                webString += lines;
                            }

                            //CHECK THAT EXCERPT ENDING OR OVER LIMIT
                            if (webString.IndexOf("</p>") != -1)
                            {
                                string_position = webString.IndexOf("<p>");
                                webString = webString.Substring(string_position + 3, webString.IndexOf("</p>") - (string_position + 3));

                                bool webElement = false;
                                string webStringElementsRemoved = "";

                                for (int i = 0; i < webString.Length; i++)
                                {
                                    if (webString[i] == '<' && !webElement)
                                        webElement = true;
                                    else if (webString[i] == '>' && webElement)
                                        webElement = false;
                                    else if (!webElement)
                                        webStringElementsRemoved += webString[i].ToString();
                                }

                                if (webStringElementsRemoved.Length > 344)
                                    webStringElementsRemoved = webStringElementsRemoved.Substring(0, 347);

                                previewContinue = false;
                                nextPost.textPreview = webStringElementsRemoved.TrimEnd() + "...";
                            }
                        }
                    }
                }
            }
        }

        public static void gatherRBloggersEntries()
        {
            string blogAddress = "http://www.r-bloggers.com";
            WebClient client = new WebClient();
            //Console.WriteLine(blogAddress);

            using (var stream = client.OpenRead(blogAddress))
            using (StreamReader reader = new StreamReader(stream))
            {
                string lines, webString;
                int string_position;
                bool beginRead = false;
                Post nextPost = new Post();
                nextPost.blogName = "R Bloggers";
                nextPost.blogCategory = "R";

                while ((lines = reader.ReadLine()) != null)
                {
                    //START OF ALL BLOG ENTRIES
                    if (lines.IndexOf("<!-- end sidebar -->") != -1)
                        beginRead = true;
                    else if (beginRead)
                    {
                        //END OF ALL BLOG ENTRIES
                        if (lines.IndexOf("<!-- begin second sidebar -->") != -1)
                            break;

                        //END OF CURRENT BLOG ENTRY
                        if (lines.IndexOf("<!-- #Post-## -->") != -1)
                        {
                            blogList.Add(nextPost);
                            nextPost = new Post();
                            nextPost.blogName = "R Bloggers";
                            nextPost.blogCategory = "R";
                        }

                        //SET DATE
                        if (lines.IndexOf("<div class=\"date\">") != -1)
                        {
                            string_position = lines.IndexOf("<div class=\"date\">") + 18;
                            webString = lines.Substring(string_position, lines.IndexOf("</div>", string_position) - string_position);

                            nextPost.publishDate = DateTime.ParseExact(webString, "MMMM d, yyyy",
                                           System.Globalization.CultureInfo.InvariantCulture);
                        }

                        //SET WEBLINK
                        if (lines.IndexOf("<a href=") != -1)
                        {
                            string_position = lines.IndexOf("<a href=") + 9;
                            nextPost.weblink = lines.Substring(string_position, lines.IndexOf("\"", string_position) - string_position);
                        }

                        //SET TITLE
                        if (lines.IndexOf("rel=\"bookmark\">") != -1)
                        {
                            string_position = lines.IndexOf("rel=\"bookmark\">") + 15;
                            nextPost.title = lines.Substring(string_position, lines.IndexOf("</a>", string_position) - string_position);
                        }

                        //SET AUTHOR
                        if (lines.IndexOf("rel=\"author\">") != -1)
                        {
                            string_position = lines.IndexOf("rel=\"author\">") + 13;
                            nextPost.author = lines.Substring(string_position, lines.IndexOf("</a>", string_position) - string_position);
                        }

                        //SET PREVIEW
                        if (lines.IndexOf("<p class=\"excerpt\">") != -1)
                        {
                            string_position = lines.IndexOf("<p class=\"excerpt\">") + 19;

                            //CHECK THAT EXCEPT ENDING
                            if (lines.IndexOf("</p>", string_position) == -1)
                                break;

                            webString = lines.Substring(string_position, lines.IndexOf("</p>", string_position) - string_position);

                            bool webElement = false;
                            string webStringElementsRemoved = "";
                            for (int i = 0; i < webString.Length; i++)
                            {
                                if (webString[i] == '<' && !webElement)
                                    webElement = true;
                                else if (webString[i] == '>' && webElement)
                                    webElement = false;
                                else
                                    webStringElementsRemoved += webString[i].ToString();
                            }
                            nextPost.textPreview = webStringElementsRemoved;
                        }
                    }
                }
            }
        }
        #endregion

        #region Sender and Logging Procedures
        public static void sendEmail(string sTo, string sSubject, string sBody)
        {
            var fromAddress = new MailAddress("brawler.sender@gmail.com", "Brawler");
            var toAddress = new MailAddress(sTo);
            const string fromPassword = "vcreijn($MS##O@XC(09ced3!!!";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 20000
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = sSubject,
                //<head><style>p.tab{margin-left:40px;}p.title{font-size:16px;font-weight:bold;}</style></head>
                Body = "<html><body>" + sBody + "</body></html>"
            })
            {
                message.IsBodyHtml = true;
                smtp.Send(message);
            }
        }

        public static void writeSentEntries(List<Post> sentPosts, String filepath = "C:\\BLOG_ENTRIES\\blogs.osl")
        {
            Stream stream = File.Open(filepath, FileMode.Create);
            BinaryFormatter bformatter = new BinaryFormatter();

            Console.WriteLine("Writing Post Information");

            bformatter.Serialize(stream, sentPosts);

            stream.Flush();
            stream.Close();
            stream.Dispose();
        }

        public static void readSentEntries(String filepath = "C:\\BLOG_ENTRIES\\blogs.osl")
        {
            try
            {
                Stream stream = File.Open(filepath, FileMode.Open);
                BinaryFormatter bformatter = new BinaryFormatter();

                var PostList = (List<Post>)bformatter.Deserialize(stream);

                foreach (Post p in PostList)
                    sentBlogList.Add(p);

                stream.Flush();
                stream.Close();
                stream.Dispose();
            }
            catch (SerializationException ex)
            {
                Console.WriteLine("No previously sent data found.  Proceeding anyway.\n\n{0}", ex);
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("Sent blog list file not found.  Proceeding anyway.\n\n{0}", ex);
            }
        }

        public static void writeBlogHTMLToFile(String[] webList)
        {
            foreach (String s in webList)
            {
                using (var writer = new StreamWriter("C:\\BLOG_ENTRIES\\" + s.Replace(":", "").Replace("-", "").Replace("/", "").Replace(".", "") + ".txt"))
                {
                    WebClient client = new WebClient();

                    writer.WriteLine(s);
                    writer.WriteLine(" ");

                    using (var stream = client.OpenRead(s))
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        string lines;

                        while ((lines = reader.ReadLine()) != null)
                        {
                            writer.WriteLine(lines);
                        }

                    }

                    if (TESTING)
                        Console.WriteLine(s + "written to file.");
                }
            }
        }
        #endregion
    }
}
