/*
 * Author: Kishore Reddy
 * Url: http://commonlibrarynet.codeplex.com/
 * Title: CommonLibrary.NET
 * Copyright: � 2009 Kishore Reddy
 * License: LGPL License
 * LicenseUrl: http://commonlibrarynet.codeplex.com/license
 * Description: A C# based .NET 3.5 Open-Source collection of reusable components.
 * Usage: Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Xml;
using System.Xml.Linq;
using System.Web;
using ComLib.Feeds;

namespace ComLib.Web.Services.TwitterSupport
{
    /// <summary>
    /// Tweet
    /// </summary>
    public class Tweet
    {
        /// <summary>
        /// Twitter user id
        /// </summary>
        public string Id;


        /// <summary>
        /// User name.
        /// </summary>
        public string User;


        /// <summary>
        /// Text of the tweet
        /// </summary>
        public string Text;


        /// <summary>
        /// Content of the tweet
        /// </summary>
        public string Content;


        /// <summary>
        /// When the tweet was published
        /// </summary>
        public DateTime Published;


        /// <summary>
        /// A link to the tweet
        /// </summary>
        public string Link;


        /// <summary>
        /// An author of the tweet
        /// </summary>
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
        /// <param name="maxEntries"></param>
        /// <returns></returns>
        public static IList<Tweet> GetFeed(string username, int maxEntries)
        {
            string url = string.Format("http://twitter.com/statuses/user_timeline/{0}.rss", HttpUtility.UrlEncode(username));
            IList<Tweet> tweets = null;
            try
            {
                var feed = FeedHelper.LoadUrl(url);
                tweets = new List<Tweet>();
                
                foreach (var item in feed.Items)
                {
                    var tweet = new Tweet()
                    {
                        Id = item.Id,
                        User = item.Contributors.IsNullOrEmpty() ? string.Empty : item.Contributors[0].Name,
                        Text = item.Title.Text,
                        Content = item.Title.Text,
                        Published = feed.LastUpdatedTime.DateTime,
                        Link = item.Links.IsNullOrEmpty() ? string.Empty : item.Links[0].Uri.OriginalString,
                        Author = item.Authors.IsNullOrEmpty() ? string.Empty : item.Authors[0].Name
                    };
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
