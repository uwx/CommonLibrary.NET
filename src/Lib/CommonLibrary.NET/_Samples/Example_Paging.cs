using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Security.Cryptography;
using ComLib;
using ComLib.Application;
using ComLib.Paging;


namespace ComLib.Samples
{
    /// <summary>
    /// Example for the Paging namespace.
    /// </summary>
    public class Example_Paging : App
    {
        /// <summary>
        /// Run the application.
        /// </summary>
        public override BoolMessageItem Execute()
        {
            // ************************************************************************************
            // NON - AJAX PAGER - URL BASED
            //
            // 1. The non-ajax pager examples are 1 to 3 below and use URL's for each page.
            // 2. The non-ajax pager generate url's for each page.
            // 3. The non-ajax pager requires a lamda that you supply
            //    to build the url for the pager number.
            // 4. The non-ajax pager must be reloaded with the page from the url
            //    that it is linking to. This means that the current page must be 
            //    loaded from the url
            //
            //
            // NON - AJAX PAGER - FORMS BASED
            //
            // 1. There is a AJAX - based pager available.            
            //    This is part of the CommonLibrary.NET CMS in the /scripts/app/PagerAjax.js file
            // *************************************************************************************

            // Example 1:
            // Non-Ajax based pager using "ToHtml" method
            // This example supplies, the current page, total pages, and the settings for css for current, and non-current page links.
            Pager pager = new Pager(1,  15, new PagerSettings(5, "pageCurrent", "page"));
            string html = pager.ToHtml( pageNum => "/blogpost/index/" + pageNum );

            // Example 2:
            // You can build the same html using the underlying pager builder.
            Pager pager2 = new Pager(1, 15, new PagerSettings(5, "pageCurrent", "page"));
            string html2 = PagerBuilderWeb.Instance.Build(pager2, pager2.Settings, pageNum => "/blogpost/index/" + pageNum);

            // Example 3:
            // You can also build the html using underlying builder and only specifying the pagenumber and total pages.
            string html3 = PagerBuilderWeb.Instance.Build(1, 15, PagerSettings.Default, pageNum => "/blogpost/index/" + pageNum);

            // Example 4:
            // There is a AJAX - BASED PAGER.
            // This is actually part of the CommonLibrary.NET CMS project.
            // Here is an example of the javascript useage of it.
            // Constructor: function PagerAjax(pageNumber, pagesInMiddle, totalPages, pagerDivId, onPageSelectedCallback, cssPage, cssCurrentPage)
            // Example:
            // 
            // pager = new PagerAjax(1, 7, 1, "pagerview", "Comments_OnPageSelected", "", "current");
            //
            // NOTE: You can see this being used in the Comments.ascx control in the project, located at "/views/shared/controls/comments.ascx"

            return BoolMessageItem.True;
        }
    }
}
