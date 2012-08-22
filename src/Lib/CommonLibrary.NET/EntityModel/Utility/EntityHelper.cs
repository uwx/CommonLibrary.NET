using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Syndication;

using ComLib.Authentication;
using ComLib.Feeds;


namespace ComLib.Entities
{
    /// <summary>
    /// Helper class for entities.
    /// </summary>
    public static class EntityHelper
    {
        /// <summary>
        /// Get entity from the type of (T) and from the id in the title. The id must by at the end of the url prefixed by "-" as in "my-post-123"
        /// </summary>
        /// <typeparam name="T">The type of entity to get.</typeparam>
        /// <param name="title">The title of the entity with the id in the end.</param>
        /// <returns></returns>
        public static T FromUrl<T>(string title)
        {
            int id = IdFromTitle(title);
            IEntityService<T> service = EntityRegistration.GetService<T>();
            T entity = service.Get(id);
            return entity;
        }


        /// <summary>
        /// Get the id from the title.
        /// </summary>
        /// <param name="title"></param>
        /// <returns></returns>
        public static int IdFromTitle(string title)
        {
            string strId = title.Substring(title.LastIndexOf("-") + 1);
            int id = Convert.ToInt32(strId);
            return id;
        }



        /// <summary>
        /// Convert this to a feed.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="author"></param>
        /// <param name="description"></param>
        /// <param name="title"></param>
        /// <param name="uri">The uri of the request</param>
        public static SyndicationFeed AsFeed<T>(Uri uri, string author, string title, string description)
        {
            IEntityService<T> service = EntityRegistration.GetService<T>();
            IList<IPublishable> entries = service.GetRecentAs<IPublishable>(1, 20);
            //http://localhost:49739/Post/rss/

            string host = uri.Host;
            if (host.Contains("localhost"))
                host = "http://localhost.com";
            else
                host = uri.AbsoluteUri;
            var feed = FeedBuilder.Build(author, title, description, host, entries);
            return feed;
        }
    }
}
