using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.Reflection;


namespace ComLib.VersionCheck
{
    public class VersionHelper
    {
        /// <summary>
        /// Check version definition for the directory
        /// </summary>
        /// <param name="defItem">The directory definition</param>
        /// <returns>True if successful, false otherwise.</returns>
        public static BoolMessageItem<VersionDefItem> CheckDir(VersionDefItem defItem)
        {
            VersionDefDir dirDef = defItem as VersionDefDir;
            string dir = VersionHelper.Translate(dirDef.Path);
            bool success = Directory.Exists(dir);
            string message = string.Empty;
                        
            if (!success) message = string.Format("Directory '{0}' does not exist.", dir);

            return new BoolMessageItem<VersionDefItem>(defItem, success, message);
        }


        /// <summary>
        /// Check version definition for the directory
        /// </summary>
        /// <param name="defItem">The directory definition</param>
        /// <returns>True if successful, false otherwise.</returns>
        public static BoolMessageItem<VersionDefItem> CheckDrive(VersionDefItem defItem)
        {
            return new BoolMessageItem<VersionDefItem>(defItem, false, string.Empty);
        }


        /// <summary>
        /// Check version definition for the directory
        /// </summary>
        /// <param name="defItem">The directory definition</param>
        /// <returns>True if successful, false otherwise.</returns>
        public static BoolMessageItem<VersionDefItem> CheckEnv(VersionDefItem defItem)
        {
            VersionDefEnv def = defItem as VersionDefEnv;
            string envVal = EnvironmentHelper.GetEnv(def.Name);
            envVal = VersionHelper.Translate(envVal);
            string message = string.Empty;
            bool success = true;
            bool isEmpty = string.IsNullOrEmpty(envVal);

            // Empty value.
            if (isEmpty) 
            {
                message = string.Format("EnvironmentVariable '{0}' is not set.", def.Name);
                if (def.IsRequired ) success = false;
            }
            
            // Expected values do not match and the value is required.
            if (isEmpty && def.IsRequired && string.Compare(envVal, def.EnvValue, true) != 0)
            {
                message = string.Format("EnvironmentVariable '{0}' does not match '{1}'.", def.EnvValue);
                success = false;
            }
            
            return new BoolMessageItem<VersionDefItem>(def, success, message);
        }


        /// <summary>
        /// Check version definition for the directory
        /// </summary>
        /// <param name="defItem">The directory definition</param>
        /// <returns>True if successful, false otherwise.</returns>
        public static BoolMessageItem<VersionDefItem> CheckFile(VersionDefItem defItem)
        {
            VersionDefFile def = defItem as VersionDefFile;
            if (def.Path.Contains("sojara-config-cj-base-clean-arc8-eod-prod.xml"))
            {
                Console.WriteLine("testing");
            }
            bool success = true;
            string filePath = VersionHelper.Translate(def.Path);
            bool exists = File.Exists(filePath);
            string message = string.Empty;
            
            // Exists.
            if (!exists && def.Requirement == VersionRequirement.Required)
            {
                message = string.Format("File '{0}' does not exist.", filePath);
                return new BoolMessageItem<VersionDefItem>(defItem, false, message);
            }

            // Same version?
            if (!string.IsNullOrEmpty(def.VersionReq))
            {
                string fileVersion = FileUtils.GetVersion(filePath);
                if (string.Compare(def.VersionReq, fileVersion, true) != 0)
                {
                    success = false;
                    message += Environment.NewLine + string.Format("File version '{0}' does not match expected version '{1}'.", fileVersion, def.VersionReq);
                }
            }
            // Same date ?
            if (!string.IsNullOrEmpty(def.DateReq))
            {
                DateTime fileDateTime = File.GetLastWriteTime(filePath);
                DateTime expectedDateTime = DateTime.Parse(def.DateReq);
                if (fileDateTime.Date.CompareTo(expectedDateTime.Date) != 0)
                {
                    string fileDate = fileDateTime.ToString();
                    success = false;
                    message += Environment.NewLine + string.Format("File Date '{0}' does not match expected date '{1}'.", fileDate, def.DateReq);
                }
            }

            // Same size ?
            if (!string.IsNullOrEmpty(def.SizeReq))
            {
                FileInfo fileInfo = new FileInfo(filePath);
                long expectedFileSize = Convert.ToInt64(def.SizeReq);
                if(fileInfo.Length != expectedFileSize)
                {
                    success = false;
                    message += Environment.NewLine + string.Format("File Size '{0}' does not match expected size '{1}'.", fileInfo.Length, expectedFileSize);
                }
            }

            return new BoolMessageItem<VersionDefItem>(defItem, success, message);
        }


        /// <summary>
        /// Check version definition for the directory
        /// </summary>
        /// <param name="defItem">The directory definition</param>
        /// <returns>True if successful, false otherwise.</returns>
        public static BoolMessageItem<VersionDefItem> CheckReg(VersionDefItem defItem)
        {
            return new BoolMessageItem<VersionDefItem>(defItem, false, string.Empty);
        }


        /// <summary>
        /// Translate environment variables to their full values.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Translate(string input)
        {
            // Handle environment variables as working dir.
            string pattern = @"\$\{env\('(?<VAR>.+)'\)\}";
            Match match = Regex.Match(input, pattern);
            if (match.Success)
            {                
                string envVar = match.Groups["VAR"].Value;
                string envValue = EnvironmentHelper.GetEnv(envVar);                
                string capture = match.Captures[0].Value;
                input = input.Replace(capture, envValue);
            }
            return input;
        }
    }



    public class VersionImport
    {
        /// <summary>
        /// Get all the types that inherit from VersionDefItem
        /// </summary>
        /// <returns></returns>
        public static List<Type> GetVersionTypes()
        {

            List<Type> types = new List<Type>();
            Assembly assembly = typeof(VersionDefinition).Assembly;
            Type t = typeof(VersionDefItem);

            foreach (Type currType in assembly.GetTypes())
            {
                if (!currType.IsAbstract
                    && !currType.IsInterface
                    && t.IsAssignableFrom(currType))
                    types.Add(currType);
            }

            return types;
        }


        /// <summary>
        /// Save the version definition to the filepath specified.
        /// </summary>
        /// <param name="xml"></param>
        public static void Save<T>(string filepath, T def)
        {
            // Serialization
            //Create our own namespaces for the output
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

            //Add an empty namespace and empty value
            ns.Add("", "");

            XmlSerializer serializer = new XmlSerializer(typeof(T));
            TextWriter writer = new StreamWriter(filepath);
            serializer.Serialize(writer, def, ns);
            writer.Close();
        }
        

        /// <summary>
        /// Serializes the given object.
        /// </summary>
        /// <typeparam name="T">The type of the object to be serialized.</typeparam>
        /// <param name="obj">The object to be serialized.</param>
        /// <param name="writer">The writer to be used for output in the serialization.</param>
        /// <param name="extraTypes"><c>Type</c> array
        ///       of additional object types to serialize.</param>
        public static void Save<T>(T obj, XmlWriter writer, Type[] extraTypes)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T), extraTypes);
            xs.Serialize(writer, obj);
        }


        /// <summary>
        /// Load version definition from the xml string.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T LoadXml<T>(string xml) where T : class
        {
            // Deserialization
            T def = default(T);
            XmlSerializer serializer = new XmlSerializer(typeof(VersionDefinition));
            TextReader reader = new StringReader(xml);
            def = (T)serializer.Deserialize(reader);
            reader.Close();
            return def;
        }


        /// <summary>
        /// Load version definition from the xml string.
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T Load<T>(string filepath) where T : class
        {
            // Deserialization
            T def = default(T);
            XmlSerializer serializer = new XmlSerializer(typeof(VersionDefinition));
            TextReader reader = new StreamReader(filepath);
            def = (T)serializer.Deserialize(reader);
            reader.Close();
            return def;
        }


        /// <summary>
        /// Deserializes the given object.
        /// </summary>
        /// <typeparam name="T">The type of the object to be deserialized.</typeparam>
        /// <param name="reader">The reader used to retrieve the serialized object.</param>
        /// <param name="extraTypes"><c>Type</c> array
        ///           of additional object types to deserialize.</param>
        /// <returns>The deserialized object of type T.</returns>
        public static T Load<T>(XmlReader reader, Type[] extraTypes)
        {
            XmlSerializer xs = new XmlSerializer(typeof(T), extraTypes);
            return (T)xs.Deserialize(reader);
        }
    }
}
