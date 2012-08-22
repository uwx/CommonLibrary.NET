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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using ComLib;
using ComLib.Entities;
using ComLib.MapperSupport;


namespace ComLib.ImportExport
{
    /// <summary>
    /// This interface is implemented by an import export service.
    /// </summary>
    public interface IImportExportService
    {
        /// <summary>
        /// Imports the specified items from the file.
        /// </summary>
        /// <param name="filePath">The path to the file to import.</param>
        /// <returns></returns>
        BoolMessageItem ImportFileAsObjects(string filePath);


        /// <summary>
        /// Imports the specified items from the text
        /// </summary>
        /// <param name="text">The text to import( as csv, xml, ini)</param>
        /// <param name="format">csv, xml, ini, json</param>
        /// <returns></returns>
        BoolErrorsItem ImportTextAsObjects(string text, string format);


        /// <summary>
        /// Gets the total count of the items that can be exported.
        /// </summary>
        /// <returns></returns>
        int TotalExportable();


        /// <summary>
        /// Exports all to the file.
        /// </summary>
        /// <param name="filename">The path to the file to export to.</param>
        /// <param name="format">The data format to export as.</param>
        /// <returns></returns>
        BoolMessage ExportToFile(string filename, string format);


        /// <summary>
        /// Exports the specified batch to the file.
        /// </summary>
        /// <param name="filename">The path to the file to export to.</param>
        /// <param name="format">The data format to export as.</param>
        /// <param name="page">The page of the data to export.</param>
        /// <param name="pageSize">The number of items per page to export.</param>
        /// <returns></returns>
        BoolMessage ExportToFile(string filename, string format, int page, int pageSize);


        /// <summary>
        /// Exports the specified batch as a text string.
        /// </summary>
        /// <param name="format">The data format to export as.</param>
        /// <param name="page">The page of the data to export.</param>
        /// <param name="pageSize">The number of items per page to export.</param>
        /// <returns></returns>
        BoolMessageItem<string> ExportToText(string format, int page, int pageSize);
    }



    /// <summary>
    /// Interface for an import/export service on objects.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IImportExportService<T> : IImportExportService
    {
        /// <summary>
        /// The validator for validating items during import.
        /// </summary>
        IValidator Validator { get; set; }


        /// <summary>
        /// Initialize import/export service.
        /// </summary>
        /// <param name="validator">Validator to validate the objects before importing.</param>
        /// <param name="supportedFormats">Comma delimited formats. e.g. "xml,csv,ini".</param>
        void Init(IValidator validator, string[] supportedFormats);        


        /// <summary>
        /// Initialize import/export service.
        /// </summary>
        /// <param name="validator">Validator to validate the objects before importing.</param>
        /// <param name="mappers"></param>
        void Init(IValidator validator, params IMapper<T>[] mappers);


        /// <summary>
        /// Determines whether this instance can import the specified items.
        /// </summary>
        /// <param name="data"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        BoolErrorsItem<IList<T>> Load(IDictionary data, string format);


        /// <summary>
        /// Determines whether this instance can import the specified items from the file
        /// </summary>
        /// <param name="filePath">The path to the file to import.</param>
        /// <returns></returns>
        BoolErrorsItem<IList<T>> LoadFile(string filePath);


        /// <summary>
        /// Determines whether this instance can import the specified items from text.
        /// </summary>
        /// <param name="text">The text to import( as csv, xml, ini)</param>
        /// <param name="format">csv, xml, ini, json</param>
        /// <returns></returns>
        BoolErrorsItem<IList<T>> LoadText(string text, string format);


        /// <summary>
        /// Imports the specified items
        /// </summary>
        /// <param name="items">The items to import.</param>
        /// <returns></returns>
        BoolErrorsItem<IList<T>> Import(IList<T> items);


        /// <summary>
        /// Imports the specified items from the file.
        /// </summary>
        /// <param name="filePath">The path to the file to import.</param>
        /// <returns></returns>
        BoolErrorsItem<IList<T>> ImportFile(string filePath);


        /// <summary>
        /// Imports the specified items from the text
        /// </summary>
        /// <param name="text">The text to import( as csv, xml, ini)</param>
        /// <param name="format">csv, xml, ini, json</param>
        /// <returns></returns>
        BoolErrorsItem<IList<T>> ImportText(string text, string format);


        /// <summary>
        /// Exports a batch of items.
        /// </summary>
        /// <returns></returns>
        /// <param name="page">The page of the data to export.</param>
        /// <param name="pageSize">The number of items per page to export.</param>
        BoolMessageItem<IList<T>> Export(int page, int pageSize);


        /// <summary>
        /// Sets all the import/export handlers in on go.
        /// </summary>
        /// <param name="importHandler"></param>
        /// <param name="exportByPageHandler"></param>
        /// <param name="exportAllHandler"></param>
        /// <param name="totalExportableHandler"></param>
        void SetHandlers(Action<IList<T>> importHandler, Func<int, int, IList<T>> exportByPageHandler, Func<IList<T>> exportAllHandler, Func<int> totalExportableHandler);


        /// <summary>
        /// Set the on import handler.
        /// </summary>
        /// <param name="handler"></param>
        void SetImport(Action<IList<T>> handler);


        /// <summary>
        /// Set the on export page handler.
        /// </summary>
        /// <param name="handler"></param>
        void SetExportPage(Func<int, int, IList<T>> handler);


        /// <summary>
        /// Set the on import handler.
        /// </summary>
        /// <param name="handler"></param>
        void SetExportAll(Func<IList<T>> handler);


        /// <summary>
        /// Set the on import handler.
        /// </summary>
        /// <param name="handler"></param>
        void SetTotal(Func<int> handler);
    }
}
