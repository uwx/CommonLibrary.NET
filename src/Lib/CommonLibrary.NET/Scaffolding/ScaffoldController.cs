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
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Reflection;
using System.Text;
using System.Data;

using ComLib;
using ComLib.Entities;
using ComLib.Entities.Management;


namespace ComLib.Scaffolding
{
    /// <summary>
    /// View for scaffolding.
    /// </summary>
    public interface IScaffoldingView
    {
        IList<string> BusinessEntities { get; set; }
        bool IsPostBack { get; }
        void AddControl(Control control);
        DataTable AllResultsTable { get; set; }        
    }



    /// <summary>
    /// Presenter class for reference data management.
    /// </summary>
    public class ScaffoldController
    {
        private ScaffoldService _service;
        private IScaffoldingView _view;
        private ScaffoldUtils _scaffoldUtils;
        private NameValueCollection _params;
        private StringDictionary _propertiesToExclude;


        public ScaffoldController(IScaffoldingView view, NameValueCollection parameters)
        {
            _view = view;
            _service = new ScaffoldService();
            _service.Settings = new ScaffoldSettings();
            _params = parameters;
            if (!_view.IsPostBack)
            {
                _view.BusinessEntities = EntityRegistration.GetManagableEntities();
            }
            _propertiesToExclude = new StringDictionary();
            _propertiesToExclude.Add("Service", "Service");
            _propertiesToExclude.Add("Validator", "Validator");

            _scaffoldUtils = new ScaffoldUtils(_params);
        }


        /// <summary>
        /// Display UI for adding new entity.
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="clientId"></param>
        public void DisplayAddUI(ScaffoldContext context)
        {
            DisplayEntity(context.EntityName, context.ParentControlId, false, null, _propertiesToExclude);
        }


        /// <summary>
        /// Display the UI for editing an entity.
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="entityId"></param>
        /// <param name="clientId"></param>
        public void DisplayEditUI(ScaffoldContext context)
        {
            BoolMessageItem result = _service.Get(context);
            if(result.Success)
                DisplayEntity(context.EntityName, context.ParentControlId, true, result.Item, _propertiesToExclude);
        }


        /// <summary>
        /// Get all the records for a specific type of business entity.
        /// </summary>
        /// <param name="context"></param>
        public void GetAll(ScaffoldContext context)
        {
            _view.AllResultsTable = _service.GetAll(context);
        }


        /// <summary>
        /// Delete the entity.
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="entityId"></param>
        /// <param name="clientId"></param>
        public void Delete(ScaffoldContext context)
        {
            _service.Delete(context);
            _view.AllResultsTable = _service.GetAll(context);
        }


        /// <summary>
        /// Submit the data for adding a new entity
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="entityId"></param>
        /// <param name="clientId"></param>
        public void Add(ScaffoldContext context)
        {
            context.PropValues = _scaffoldUtils.GetValuesFromUI(context.EntityName, context.ParentControlId, _propertiesToExclude); 
            _service.Create(context);
            _view.AllResultsTable = _service.GetAll(context);
        }


        /// <summary>
        /// Submit the data for editing the entity.
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="entityId"></param>
        /// <param name="clientId"></param>
        public void Update(ScaffoldContext context)
        {
            context.PropValues = _scaffoldUtils.GetValuesFromUI(context.EntityName, context.ParentControlId, _propertiesToExclude);
            _service.Update(context);
            _view.AllResultsTable = _service.GetAll(context);
        }


        #region Private methods
        private void DisplayEntity(string entityName, string clientId, bool loadEntityValues, object entityObject, StringDictionary propertiesToExclude)
        {
            Table table = _scaffoldUtils.BuildEntityUITable(entityName, clientId, loadEntityValues, entityObject, propertiesToExclude);
            _view.AddControl(table);
        }
        
        #endregion
    }
}
