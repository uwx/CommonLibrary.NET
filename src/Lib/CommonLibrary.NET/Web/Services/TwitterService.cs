using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.Xml.Linq;
using System.Web;

namespace ComLib.Web.Services.TwitterSupport
{
    /// <summary>
    /// Tweet
    /// </summary>
    public class Tweet
    {
        public string Id;
        public string User;
        public string Text;
        public string Content;
        public DateTime Published;
        public string Link;
        public string Author;


        /// <summary>
        /// Default construction.
        /// </summary>
        public Tweet() { }


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="id"></param>
        /// <param name="text"></param>
        /// <param name="content"></param>
        /// <param name="published"></param>
        /// <param name="url"></param>
        /// <param name="author"></param>
        public Tweet(string id, string text, string content, DateTime published, string url, string author)
        {
            Id = id;
            Text = text;
            Content = content;
            Published = published;
            Link = url;
            Author = author;
        }
    }



    /// <summary>
    /// Twitter class for getting tweets
    /// </summary>
    public class Twitter
    {

        /// <summary>
        /// Get the latest tweets from twitter for the specified username.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static IList<Tweet> GetFeed(string username, int maxEntries)
        {
            // http://search.twitter.com/search.atom?q=ADefWebserver
            string url = string.Format("http://search.twitter.com/search.atom?q={0}", HttpUtility.UrlEncode(username));
            IList<Tweet> tweets = null;
            try
            {
                XDocument atomDoc = XDocument.Load(url);
                XNamespace atomNS = "http://www.w3.org/2005/Atom";

                var entries = from tweet in atomDoc.Descendants(atomNS + "entry")
                              select new Tweet()
                                {
                                    Id = (string)tweet.Element(atomNS + "id"),
                                    User = username,
                                    Text = (string)tweet.Element(atomNS + "title"),
                                    Content = (string)tweet.Element(atomNS + "content"),
                                    Published = DateTime.Parse((string)tweet.Element(atomNS + "published")),
                                    Link = (string)tweet.Elements(atomNS + "link")
                                                .Where(link => (string)link.Attribute("rel") == "alternate")
                                                .Select(link => (string)link.Attribute("href"))
                                                .First(),
                                    Author = (from author in tweet.Descendants(atomNS + "author")
                                              select (string)author.Element(atomNS + "name")).First()
                                };

                tweets = new List<Tweet>();
                foreach (var tweet in entries)
                {
                    tweets.Add(tweet);
                    if (tweets.Count == maxEntries)
                        break;
                }
            }
            catch (Exception ex)
            {
                Logging.Logger.Error("Unable to get tweets for user : " + username, ex);
                tweets = new List<Tweet>();
            }
            return tweets;
        }
    }
}
