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
using System.Web;
using System.Xml;
using System.Net;
using System.Xml.Linq;


namespace ComLib.Web.Services.Search
{
    /// <summary>
    /// Type of data to search.
    /// </summary>
    public enum SearchSource { 
        /// <summary>
        /// Search for web pages.
        /// </summary>
        web, 
        
        
        /// <summary>
        /// Search for images.
        /// </summary>
        image, 
        
        
        /// <summary>
        /// Search for news.
        /// </summary>
        news };



    /// <summary>
    /// SearchEngine
    /// </summary>
    public class SearchEngine
    {
        private static ISearchEngineService _provider;

        /// <summary>
        /// Inits the specified service.
        /// </summary>
        /// <param name="service">The service.</param>
        public static void Init(ISearchEngineService service)
        {
            _provider = service;
        }


        /// <summary>
        /// Gets the name of the provider.
        /// </summary>
        /// <value>The name of the provider.</value>
        public string ProviderName
        {
            get { return _provider.GetType().Name; }
        }


        /// <summary>
        /// Searches the specified keywords.
        /// </summary>
        /// <param name="keywords">The keywords.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="recordsPerPage">The records per page.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public static PagedList<SearchResult> Search(string keywords, int pageIndex, int recordsPerPage, string culture)
        {
            SearchRequest req = new SearchRequest()
            {
                QueryText = keywords, PageIndex = pageIndex,
                ResultsPerPage = recordsPerPage, Culture = culture, Highlight = false,
                SiteToSearch = ""
            };
            return _provider.Search(req);
        }


        /// <summary>
        /// Searches the site.
        /// </summary>
        /// <param name="keywords">The keywords.</param>
        /// <param name="siteToSearch">The site to search.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="recordsPerPage">The records per page.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public static PagedList<SearchResult> SearchSite(string keywords, string siteToSearch, int pageIndex, int recordsPerPage, string culture)
        {
            SearchRequest req = new SearchRequest()
            {
                QueryText = keywords, PageIndex = pageIndex,
                ResultsPerPage = recordsPerPage, Culture = culture, Highlight = false,
                SiteToSearch = siteToSearch
            };
            return _provider.Search(req);
        }
    }



    /// <summary>
    /// Search Engine.
    /// commoncms
    /// 7F2DBB51D1FE49D765EE5BE05984D589872A74E6
    /// </summary>
    public class SearchEngineBing : ISearchEngineService
    {
        /// <summary>
        /// Initialize.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public SearchEngineBing(SearchSettings settings)
        {
            if (settings == null)
                //AppId = "7F2DBB51D1FE49D765EE5BE05984D589872A74E6";
                throw new ArgumentNullException("The appid for bing search must be supplied.");

            Settings = settings;
        }


        /// <summary>
        /// Settings for searching.
        /// </summary>
        public SearchSettings Settings { get; set; }


        /// <summary>
        /// Executes the query.
        /// </summary>
        /// <returns></returns>            
        public PagedList<SearchResult> Search(SearchRequest request)
        {
            if (request.PageIndex == 0)
                request.PageIndex = 1;
            if (request.ResultsPerPage == 0)
                request.ResultsPerPage = 20;

            string url = Settings.Url + "Appid={0}&sources={1}&query={2}&web.offset={3}&web.count={4}";
            string query = HttpUtility.UrlEncode(request.QueryText);
            if (!string.IsNullOrEmpty(request.SiteToSearch))
            {
                query += "+site:" + HttpUtility.UrlEncode(request.SiteToSearch);
            }


            // Need to convert the page index to the record offset.
            // e.g. if pageindex = 1, recordsPerPage = 10, offset = 0;
            //         pageindex = 2, recordsPerPage = 10, offset = 11;
            int offset = request.PageIndex <= 1 ? 0 : ((request.PageIndex - 1) * request.ResultsPerPage) + 1;
            string completeUri = String.Format(url, Settings.AppId, request.Source.ToString(), query, offset, request.ResultsPerPage);

            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(completeUri);
            HttpWebResponse res = (HttpWebResponse)req.GetResponse();
            XmlDocument document = new XmlDocument();
            document.Load(res.GetResponseStream());

            var nodelist = document.DocumentElement.GetElementsByTagName("web:WebResult");
            var totals = document.DocumentElement.GetElementsByTagName("web:Total");
            int total = Convert.ToInt32(totals[0].InnerText);
            IList<SearchResult> results = new List<SearchResult>();
            foreach (XmlNode node in nodelist)
            {
                var result = new SearchResult();
                XmlElement elem = (XmlElement)node;
                result.Title = elem.GetElementsByTagName("web:Title")[0].InnerText;
                var items = elem.GetElementsByTagName("web:Description");
                if(items != null && items.Count > 0)
                {
                    result.Description = items[0].InnerText;
                } 
                
                result.Uri = elem.GetElementsByTagName("web:Url")[0].InnerText;
                items = elem.GetElementsByTagName("web:DateTime");
                if (items != null && items.Count > 0)
                {
                    string strDate = items[0].InnerText;
                    result.Date = Convert.ToDateTime(strDate);
                }
                results.Add(result);
            }
            return new PagedList<SearchResult>(request.PageIndex, request.ResultsPerPage, total, results);
        }
    }



    /// <summary>
    /// 
    /// </summary>
    public interface ISearchEngineService
    {
        /// <summary>
        /// Settings for search.
        /// </summary>
        SearchSettings Settings { get; set; }


        /// <summary>
        /// Searches the specified request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        PagedList<SearchResult> Search(SearchRequest request);
    }



    /// <summary>
    /// Settings for the search engine.
    /// </summary>
    public class SearchSettings
    {
        /// <summary>
        /// Gets or sets the app id.
        /// </summary>
        /// <value>The app id.</value>
        public string AppId { get; set; }


        /// <summary>
        /// Gets or sets the URL.
        /// </summary>
        /// <value>The URL.</value>
        public string Url { get; set; }
    }



    /// <summary>
    /// Search Engine results.
    /// </summary>
    public class SearchResult
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }


        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description { get; set; }


        /// <summary>
        /// Gets or sets the URI.
        /// </summary>
        /// <value>The URI.</value>
        public string Uri { get; set; }


        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>The date.</value>
        public DateTime Date { get; set; }
    }



    /// <summary>
    /// SearchEngine request
    /// </summary>
    public class SearchRequest
    {
        /// <summary>
        /// Used to search only a specific site.
        /// </summary>
        public string SiteToSearch { get; set; }


        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        public SearchSource Source { get; set; }


        /// <summary>
        /// Gets or sets the results per page.
        /// </summary>
        /// <value>The results per page.</value>
        public int ResultsPerPage { get; set; }


        /// <summary>
        /// Gets or sets the index of the page.
        /// </summary>
        /// <value>The index of the page.</value>
        public int PageIndex { get; set; }


        /// <summary>
        /// Gets or sets the culture.
        /// </summary>
        /// <value>The culture.</value>
        public string Culture { get; set; }


        /// <summary>
        /// Gets or sets the query text.
        /// </summary>
        /// <value>The query text.</value>
        public string QueryText { get; set; }


        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="SearchEngine"/> is highlight.
        /// </summary>
        /// <value><c>true</c> if highlight; otherwise, <c>false</c>.</value>
        public bool Highlight { get; set; }
    }
}
