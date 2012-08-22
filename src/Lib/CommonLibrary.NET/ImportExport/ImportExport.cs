using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ComLib.MapperSupport;


namespace ComLib.ImportExport
{
    /// <summary>
    /// Import Export convenience class to store collection of various importexport services.
    /// </summary>
    public class ImportExports
    {
        private static ImportExports _instance = new ImportExports();
        private IDictionary<string, IImportExportService> _ioServices = new Dictionary<string, IImportExportService>();
        private IDictionary<string, IImportExportService> _ioServicesShortName = new Dictionary<string, IImportExportService>();

        /// <summary>
        /// Gets the default instance of importexports
        /// </summary>
        public static ImportExports Instance
        {
            get { return _instance; }
        }


        /// <summary>
        /// Register the ImportExport service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public void Register<T>() where T : class, new()
        {
            Register<T>(null, new string[] { "csv", "ini", "xml" }, true);
        }


        /// <summary>
        /// Register the import export service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="validator">Validator for the importexport service.</param>
        /// <param name="supportedFormats">Formats supported. e.g. "csv,xml,ini".</param>
        /// <param name="isReusable">Whether or not the service is reusable.</param>
        public void Register<T>(IValidator validator, string[] supportedFormats, bool isReusable) where T: class, new()
        {
            IImportExportService<T> ioService = new ImportExportService<T>();
            ioService.Init(validator, supportedFormats);
            Register<T>(ioService);
        }


        /// <summary>
        /// Register the import export service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="validator">Validator for the importexport service.</param>
        /// <param name="isReusable">Whether or not the service is reusable.</param>
        /// <param name="mappers">The mappers that can be used for importing/exporting the type.</param>
        public void Register<T>(IValidator validator, bool isReusable, params IMapper<T>[] mappers) where T : class, new()
        {
            IImportExportService<T> ioService = new ImportExportService<T>();
            ioService.Init(validator, mappers);
            Register<T>(ioService);
        }


        /// <summary>
        /// Register the import export service.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ioService"></param>
        public void Register<T>(IImportExportService ioService) where T : class, new()
        {
            _ioServices[typeof(T).FullName] = ioService;
            _ioServicesShortName[typeof(T).Name] = ioService;
        }


        /// <summary>
        /// Get the ImportExportService as type T.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IImportExportService<T> For<T>()
        {
            var ioService = _ioServices[typeof(T).FullName];
            return ioService as IImportExportService<T>;
        }


        /// <summary>
        /// Get the ImportExportService as type T.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IImportExportService For(string model)
        {
            var ioService = _ioServicesShortName[model];
            return ioService as IImportExportService;
        }
    }
}
