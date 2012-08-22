using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComLib.Feeds
{
    /// <summary>
    /// Interface for items that can generated into syndication feeds(rss, atom).
    /// </summary>
    public interface IPublishable
    {
        /// <summary>
        /// Gets the title of the object
        /// </summary>
        string Title { get; }


        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        string Description { get; }


        /// <summary>
        /// Gets the content.
        /// </summary>
        /// <value>The content.</value>
        string Content { get; }


        /// <summary>
        /// Gets the date created.
        /// </summary>
        /// <value>The date created.</value>
        DateTime CreateDate { get; }


        /// <summary>
        /// Gets the date modified.
        /// </summary>
        /// <value>The date modified.</value>
        DateTime UpdateDate { get; }


        /// <summary>
        /// Gets the id.
        /// </summary>
        /// <value>The id.</value>
        string GuidId { get; }


        /// <summary>
        /// Gets the relative link.
        /// </summary>
        /// <value>The relative link.</value>
        string UrlRelative { get; }


        /// <summary>
        /// Gets the absolute link.
        /// </summary>
        /// <value>The absolute link.</value>
        string UrlAbsolute { get; }
        

        /// <summary>
        /// Gets the author.
        /// </summary>
        /// <value>The author.</value>
        string Author { get; }


        /// <summary>
        /// Whether or not this is published.
        /// </summary>
        bool IsPublished { get; }


        /// <summary>
        /// Whether or not this is publically available.
        /// </summary>
        bool IsPublic { get; }
    }
}
