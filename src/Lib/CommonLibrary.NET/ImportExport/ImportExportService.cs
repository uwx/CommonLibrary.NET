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
using System.Collections;
using System.IO;


using ComLib;
using ComLib.IO;
using ComLib.CsvParse;
using ComLib.MapperSupport;


namespace ComLib.ImportExport
{
    /// <summary>
    /// Interface for an import/export service on objects.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ImportExportService<T> : IImportExportService<T> where T : class, new()
    {
        /// <summary>
        /// Validator to be used in import.
        /// </summary>
        protected IValidator _validator;


        /// <summary>
        /// Mappers for various formats.
        /// </summary>
        protected IDictionary<string, IMapper<T>> _mappers = new Dictionary<string, IMapper<T>>();

        // Handlers for the import / export calls.

        /// <summary>
        /// Import handler.
        /// </summary>
        protected Action<IList<T>> _importHandler;


        /// <summary>
        /// Export page handler.
        /// </summary>
        protected Func<int, int, IList<T>> _exportPageHandler;


        /// <summary>
        /// Export handler.
        /// </summary>
        protected Func<IList<T>> _exportAllHandler;


        /// <summary>
        /// Total handler.
        /// </summary>
        protected Func<int> _totalHandler;


        /// <summary>
        /// Default initialization.
        /// </summary>
        public ImportExportService()
        {
            _mappers = new Dictionary<string, IMapper<T>>();
            _mappers["csv"] = new MapperCsv<T>();
            _mappers["ini"] = new MapperIni<T>();
            _mappers["xml"] = new MapperXml<T>();
        }


        /// <summary>
        /// Initialize import/export service.
        /// </summary>
        /// <param name="validator">Validator to validate the objects before importing.</param>
        /// <param name="supportedFormatsDelimited">Comma delimited formats. e.g. "xml,csv,ini".</param>
        public void Init(IValidator validator, string supportedFormatsDelimited)
        {
            string[] formats = new string[]{ supportedFormatsDelimited };
            if (supportedFormatsDelimited.Contains(","))
                formats = supportedFormatsDelimited.Split(',');

            Init(validator, formats);
        }


        /// <summary>
        /// Initialize import/export service.
        /// </summary>
        /// <param name="validator">Validator to validate the objects before importing.</param>
        /// <param name="supportedFormats">Comma delimited formats. e.g. "xml,csv,ini".</param>
        public void Init(IValidator validator, string[] supportedFormats)
        {
            _validator = validator;
            foreach (string format in supportedFormats)
            {
                string formatlcase = format.ToLower().Trim();

                if (formatlcase == "csv")
                    _mappers["csv"] = new MapperCsv<T>();
                else if (formatlcase == "ini")
                    _mappers["ini"] = new MapperIni<T>();
                else if (formatlcase == "xml")
                    _mappers["xml"] = new MapperXml<T>();
            }
        }


        /// <summary>
        /// Initialize import/export service.
        /// </summary>
        /// <param name="validator">Validator to validate the objects before importing.</param>
        /// <param name="mappers"></param>
        public void Init(IValidator validator, params IMapper<T>[] mappers)
        {
            _validator = validator;
            foreach (var mapper in mappers)
                _mappers[mapper.SupportedFormat] = mapper;
        }


        /// <summary>
        /// The validator to use when importing.
        /// </summary>
        public IValidator Validator
        {
            get { return _validator; }
            set { _validator = value; }
        }


        /// <summary>
        /// Is there a validator.
        /// </summary>
        public bool HasValidator
        {
            get { return _validator != null; }
        }


        #region Core Behaviour - import - count - export
        /// <summary>
        /// Import items.
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public virtual BoolErrorsItem<IList<T>> Import(IList<T> items)
        {
            if (_importHandler == null) return new BoolErrorsItem<IList<T>>(null, false, "Import lamda not initialized.", null);

            _importHandler(items);
            return new BoolErrorsItem<IList<T>>(items, true, string.Empty, null);
        }


        /// <summary>
        /// Gets the total count of the items that can be exported.
        /// </summary>
        /// <returns></returns>
        public virtual int TotalExportable()
        {
            if (_totalHandler == null)
                throw new NotImplementedException("Not yet implemented.");

            return _totalHandler();
        }


        /// <summary>
        /// Exports items in a batch/page.
        /// </summary>
        /// <returns></returns>
        public virtual BoolMessageItem<IList<T>> Export(int page, int pageSize)
        {
            if (_exportPageHandler == null)
                throw new NotImplementedException("Not yet implemented.");

            return new BoolMessageItem<IList<T>>(_exportPageHandler(page, pageSize), true, string.Empty);
        }
        #endregion


        #region Overloaded & Convenience import/export methods
        /// <summary>
        /// Determines whether this instance can import the specified items.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public BoolErrorsItem<IList<T>> Load(IDictionary data, string format)
        {
            var errors = new ValidationResults();
            var result = new BoolErrorsItem<IList<T>>(null, false, string.Empty, errors);

            // For each section.
            Try.CatchLog(() =>
            {
                IMapper<T> mapper = _mappers[format];
                IList<T> results = mapper.Map(data, errors);
                result = new BoolErrorsItem<IList<T>>(results, results != null && results.Count > 0, errors.Message(), errors);

            });
            return result;
        }


        /// <summary>
        /// Determines whether this instance can import the specified items.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public virtual BoolErrorsItem<IList<T>> LoadFile(string filePath)
        {
            // If the items are null, and import text is provided, parse it.
            if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
                return new BoolErrorsItem<IList<T>>(null, false, "Import file path is valid.", null);

            FileInfo file = new FileInfo(filePath);
            string text = System.IO.File.ReadAllText(filePath);
            return LoadText(text, file.Extension);
        }


        /// <summary>
        /// Determines whether this instance can import the specified items.
        /// </summary>
        /// <param name="text">The text to import( as csv, xml, ini)</param>
        /// <param name="format">csv, xml, ini, json</param>
        /// <returns></returns>
        public virtual BoolErrorsItem<IList<T>> LoadText(string text, string format)
        {
            if (!_mappers.ContainsKey(format.ToLower()))
                return new BoolErrorsItem<IList<T>>(null, false, "Format : " + format + " not supported.", null);

            BoolErrorsItem<IList<T>> canImport = new BoolErrorsItem<IList<T>>(null, false, "", null);

            // For each section.
            Try.CatchLog(() =>
            {
                var mapper = _mappers[format.ToLower()];
                var errors = new ValidationResults();
                IList<T> items = mapper.MapFromText(text, errors);
                bool success = items != null && items.Count > 0;
                canImport = new BoolErrorsItem<IList<T>>(items, success, string.Empty, errors);
            });
            return canImport;
        }


        /// <summary>
        /// Imports the specified items.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public virtual BoolErrorsItem<IList<T>> Import(IDictionary data, string format)
        {
            var result = Load(data, format);
            if (!result.Success) return result;

            return Import(result.Item);
        }


        /// <summary>
        /// Imports the specified items from the file.
        /// </summary>
        /// <param name="filePath">The path to the file to import.</param>
        /// <returns></returns>
        public BoolMessageItem ImportFileAsObjects(string filePath)
        {
            return ImportFile(filePath) as BoolMessageItem;
        }


        /// <summary>
        /// Imports the specified items from the text
        /// </summary>
        /// <param name="text">The text to import( as csv, xml, ini)</param>
        /// <param name="format">csv, xml, ini, json</param>
        /// <returns></returns>
        public BoolErrorsItem ImportTextAsObjects(string text, string format)
        {
            return ImportText(text, format) as BoolErrorsItem;
        }


        /// <summary>
        /// Imports the specified items.
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public virtual BoolErrorsItem<IList<T>> ImportFile(string filePath)
        {
            // If the items are null, and import text is provided, parse it.
            if (string.IsNullOrEmpty(filePath) || !System.IO.File.Exists(filePath))
                return new BoolErrorsItem<IList<T>>(null, false, "Import file path is valid.", null);

            FileInfo file = new FileInfo(filePath);
            string text = System.IO.File.ReadAllText(filePath);
            return ImportText(text, file.Extension);
        }


        /// <summary>
        /// Imports the specified items.
        /// </summary>/// <param name="text">The text to import( as csv, xml, ini)</param>
        /// <param name="format">csv, xml, ini, json</param>
        /// <returns></returns>
        public virtual BoolErrorsItem<IList<T>> ImportText(string text, string format)
        {
            // If the items are null, and import text is provided, parse it.
            if (string.IsNullOrEmpty(text))
                return new BoolErrorsItem<IList<T>>(null, false, "Import content is empty.", null);

            var result = LoadText(text, format);

            // Unable to import from text ?
            if (!result.Success) return result;

            // Set the item list on the context as internal method only handles parsed nodes.       
            return Import(result.Item);
        }


        /// <summary>
        /// Exports all.
        /// </summary>
        /// <returns></returns>
        public virtual BoolMessage ExportToFile(string filename, string format)
        {
            return ExportToFile(filename, format, 1, TotalExportable() + 1);
        }


        /// <summary>
        /// Exports the range of items to a file.
        /// </summary>
        /// <returns></returns>
        public virtual BoolMessage ExportToFile(string filename, string format, int page, int pageSize)
        {
            var result = ExportToText(format, page, pageSize);
            if (!result.Success) return result;

            try
            {
                File.WriteAllText(filename, result.Message);
            }
            catch (Exception ex)
            {
                return new BoolMessage(false, ex.Message);
            }
            return BoolMessage.True;
        }


        /// <summary>
        /// Exports the batch as text.
        /// </summary>
        /// <returns></returns>
        public virtual BoolMessageItem<string> ExportToText(string format, int page, int pageSize)
        {
            ToDo.Implement(ToDo.Priority.High, "kishore", "Need to provide default export functionality for various formats(xml,csv,ini).", () =>
            {
                var items = Export(page, pageSize);
                var mapper = _mappers[format];
            });
            throw new NotImplementedException("Not yet implemented.");
        }
        #endregion


        #region Setters for Handlers
        /// <summary>
        /// Sets all the import/export handlers in on go.
        /// </summary>
        /// <param name="importHandler"></param>
        /// <param name="exportByPageHandler"></param>
        /// <param name="exportAllHandler"></param>
        /// <param name="totalExportableHandler"></param>
        public void SetHandlers(Action<IList<T>> importHandler, Func<int, int, IList<T>> exportByPageHandler, Func<IList<T>> exportAllHandler, Func<int> totalExportableHandler)
        {
            SetImport(importHandler);
            SetExportPage(exportByPageHandler);
            SetExportAll(exportAllHandler);
            SetTotal(totalExportableHandler);
        }


        /// <summary>
        /// Set the on import handler.
        /// </summary>
        /// <param name="handler"></param>
        public void SetImport(Action<IList<T>> handler)
        {
            _importHandler = handler;
        }


        /// <summary>
        /// Set the on export page handler.
        /// </summary>
        /// <param name="handler"></param>
        public void SetExportPage(Func<int, int, IList<T>> handler)
        {
            _exportPageHandler = handler;
        }


        /// <summary>
        /// Set the on import handler.
        /// </summary>
        /// <param name="handler"></param>
        public void SetExportAll(Func<IList<T>> handler)
        {
            _exportAllHandler = handler;
        }


        /// <summary>
        /// Set the on import handler.
        /// </summary>
        /// <param name="handler"></param>
        public void SetTotal(Func<int> handler)
        {
            _totalHandler = handler;
        }
        #endregion
    }
}
