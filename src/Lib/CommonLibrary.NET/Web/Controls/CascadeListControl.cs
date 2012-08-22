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
using System.Text;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Collections.Specialized;


namespace ComLib.Web.UI.Controls
{
    /// <summary>
    /// This class implements a cascaded drop down list control.
    /// </summary>
    [DefaultProperty("Text"), ToolboxData("<{0}:CascadeListControl runat=\"server\"> </{0}:CascadeListControl>")]
    public class CascadeDropDownListControl : WebControl, INamingContainer, IPostBackDataHandler
    {
        private HtmlSelect _lstCascade;
        private string _selectedValue;


        /// <summary>
        /// Flag to indicate if this client side object's data
        /// will be initiated / loaded from url ajax page.
        /// </summary>
        [Category("Behavior"), DefaultValue(""),
        Description("Specifies whether the control needs to initialize to restore state.")]
        public bool NeedsInitialization
        {
            get { return GetViewStateBoolItem("NeedsInitialization"); }
            set { ViewState["NeedsInitialization"] = value; }
        }


        /// <summary>
        /// Flag to indicate if this client side object's data
        /// will be initiated / loaded from url ajax page.
        /// </summary>
        [Category("Behavior"), DefaultValue(""),
        Description("Specifies whether the control needs to initialize to restore state.")]
        public bool AddCustomInitialItem
        {
            get { return GetViewStateBoolItem("AddInitialItem"); }            
            set { ViewState["AddInitialItem"] = value; }
        }


        /// <summary>
        /// Flag to indicate if this client side object's data
        /// will be initiated / loaded from url ajax page.
        /// </summary>
        [Category("Behavior"), DefaultValue(""),
        Description("Specifies whether the control needs to initialize to restore state.")]
        public string InitialItemName
        {
            get { return GetViewStateItem("InitialItemName"); }
            set { ViewState["InitialItemName"] = value; }
        }


        /// <summary>
        /// Flag to indicate if this client side object's data
        /// will be initiated / loaded from url ajax page.
        /// </summary>
        [Category("Behavior"), DefaultValue(""),
        Description("Specifies whether the control needs to initialize to restore state.")]
        public string InitialItemValue
        {
            get { return GetViewStateItem("InitialItemValue"); }
            set { ViewState["InitialItemValue"] = value; }
        }


        /// <summary>
        /// Javascript event handler for the selection changed event
        /// on the drop-down listbox.
        /// </summary>
        /// <example>"kdStates.LoadDataUsingFilter" or "countryChangedHandler"
        /// </example>
        [Category("Behavior"), DefaultValue(""),
        Description("Specifiy a client side javascript function to call when selection is changed.")]
        public string OnSelectionChangedClientSideHandler
        {
            get { return GetViewStateItem("OnSelectionChangedClientHandler"); }
            set { ViewState["OnSelectionChangedClientHandler"] = value; }
        }


        /// <summary>
        /// Flag to indicate if this client side object's data
        /// will be initiated / loaded from url ajax page.
        /// </summary>
        [Category("Behavior"), DefaultValue(""),
        Description("Specify whether to use the Id for making a call to get items")]        
        public bool UseIdForFilterValue
        {
            get { return GetViewStateBoolItem("UseIdForFilterValue"); }
            set { ViewState["UseIdForFilterValue"] = value; }
        }


        /// <summary>
        /// Url of the page to get all the values.
        /// </summary>
        /// <example>AjaxLocation.aspx?action=GetAllCountries</example>
        [Category("Behavior"), DefaultValue(""),
        Description("Url to use to retrieve items for this control.")]
        public string UrlRetrieveAllService
        {
            get { return GetViewStateItem("UrlRetrieveAllService"); }
            set { ViewState["UrlRetrieveAllService"] = value; }
        }


        /// <summary>
        /// The value to use when restoring state.
        /// </summary>
        /// <example>"1" This is the value of the drop-down list option.</example>
        [Category("Behavior"), DefaultValue(""),
        Description("Value to use to represent the selected item")]
        public string SelectedValue
        {
            get { return _selectedValue; }
            set { _selectedValue = value; }
        }


        /// <summary>
        /// Name of the javascript client side object.
        /// </summary>
        /// <example>"kdCountry"</example>
        [Category("Behavior"), DefaultValue(""),
        Description("Name of the client side js object")]
        public string JsObjectName
        {
            get { return GetViewStateItem("JsObjectName"); }
            set { ViewState["JsObjectName"] = value; }
        }
        
        
        /// <summary>
        /// Name of the result tag name from the url to retrieve
        /// values.
        /// </summary>
        /// <example>"state" or "country"</example>
        [Category("Behavior"), DefaultValue(""),
        Description("Xml tag name containing the value to use for the controls.")]
        public string ResultTagName
        {
            get { return GetViewStateItem("ResultTagName"); }
            set { ViewState["ResultTagName"] = value; }
        }


        private string ConvertToJSBoolean(bool b)
        {
            if (b) return "true";
            return "false";
        }


        private string GetViewStateItem(string name)
        {
            string text = (string)ViewState[name];
            if (text != null)
                return text;

            return string.Empty;
        }


        private bool GetViewStateBoolItem(string name)
        {
            if (ViewState[name] == null) { return false; }

            return (bool)ViewState[name];
        }


        /// <summary>
        /// Add server controls to the controls collection so that the framework
        /// can render them.
        /// </summary>
        protected override void  CreateChildControls()
        {
            _lstCascade = new HtmlSelect();
            _lstCascade.ID = "lstCascade";
            _lstCascade.Attributes.Add("onchange", "javascript:" + JsObjectName + ".OnSelectionChanged();");
            //_hdnSelectedId = new HtmlInputHidden();
            //_hdnSelectedId.ID = "hdnSelectedId";
            //_hdnSelectedId.Value = _selectedValue;

            this.Controls.Add(_lstCascade);
            //this.Controls.Add(_hdnSelectedId);
            this.Page.ClientScript.RegisterHiddenField(HiddenSelectedId, _selectedValue);
            base.CreateChildControls();
        }


        /// <summary>
        /// OnInit event handler.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            Page.RegisterRequiresPostBack(this);
        }


        private string GetSelectedId()
        {
            return _selectedValue;
        }


        /// <summary>
        /// Render the contents of the control.
        /// </summary>
        /// <param name="writer">Rendering target.</param>
        protected override void RenderContents(HtmlTextWriter writer)
        {            
            base.RenderContents(writer);
            string clientJs = BuildClientSideJavascript();
            writer.Write(clientJs);
        }


        /// <summary>
        /// kdcat1.ListControlId = "&lt;%=lstC.ClientID %&gt;";
        /// kdcat1.RetrieveAllService = "AjaxServer.aspx";
        /// kdcat1.ResultTagName = "country";
        /// kdcat1.DependentCat = "kdcat2";
        /// kdcat1.FilterValueFromState = "1";  
        /// kdcat1.UseIdForFilterValue = true;           
        /// kdcat1.Init(); 
        /// </summary>
        /// <returns>String with client side java script.</returns>
        protected string BuildClientSideJavascript()
        {
            StringBuilder buffer = new StringBuilder();
            string _jsObjectName = JsObjectName;
            // Declare the js client side object.

            buffer.Append(Environment.NewLine + "<script type=\"text/javascript\">" + Environment.NewLine);
            buffer.Append("var " + _jsObjectName + " = new KdCascadeList();" + Environment.NewLine);
            buffer.Append(_jsObjectName + ".ListControlId = \"" + _lstCascade.ClientID + "\";" + Environment.NewLine);
            buffer.Append(_jsObjectName + ".RetrieveAllService = \"" + UrlRetrieveAllService + "\";" + Environment.NewLine);
            buffer.Append(_jsObjectName + ".ResultTagName = \"" + ResultTagName + "\";" + Environment.NewLine);
            buffer.Append(_jsObjectName + ".JSOnSelectionChangedHandlerName = \"" + OnSelectionChangedClientSideHandler + "\";" + Environment.NewLine);
            buffer.Append(_jsObjectName + ".FilterValueFromState = \"" + SelectedValue + "\";" + Environment.NewLine);
            buffer.Append(_jsObjectName + ".UseIdForFilterValue = " + ConvertToJSBoolean(UseIdForFilterValue) + ";" + Environment.NewLine);
            buffer.Append(_jsObjectName + ".InitialItemName = \"" + InitialItemName + "\";" + Environment.NewLine);
            buffer.Append(_jsObjectName + ".InitialItemValue = \"" + InitialItemValue + "\";" + Environment.NewLine);
            buffer.Append(_jsObjectName + ".HiddenSelectedItemId = \"" + HiddenSelectedId + "\";" + Environment.NewLine);

            if (NeedsInitialization)
            {
                buffer.Append(_jsObjectName + ".Init();" + Environment.NewLine);
            }
            buffer.Append(Environment.NewLine + "</script>" + Environment.NewLine);
            
            string js = buffer.ToString();
            return js;
        }


        private string HiddenSelectedId
        {
            get { return this.ClientID + "_hdnSelectedId"; }
            
        }


        #region IPostBackDataHandler Members

        bool IPostBackDataHandler.LoadPostData(string postDataKey, NameValueCollection postCollection)
        {
            _selectedValue = postCollection[HiddenSelectedId];
            return true;
        }


        /// <summary>
        /// Raises the data changed event.
        /// </summary>
        public void RaisePostDataChangedEvent()
        {            
        }

        #endregion
    }
}
