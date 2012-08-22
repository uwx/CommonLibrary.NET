using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;


namespace ComLib.VersionCheck
{    

    /// <summary>
    /// Version definition.
    /// </summary>
    [XmlRoot("VersionDefinition")]
    public class VersionDefinition : IXmlSerializable
    {
        private static Type[] _supportedTypes;

        /// <summary>
        /// Get all the derived types of versiondefitem.
        /// </summary>
        static VersionDefinition()
        {
            _supportedTypes = VersionImport.GetVersionTypes().ToArray();
        }


        /// <summary>
        /// The name of the definition.
        /// </summary>
        public string Name { get; set; }
                       
                   
        public List<VersionDefItem> Components { get; set; }


        #region IXmlSerializable Members
        /// <summary>
        /// Get the schema.
        /// </summary>
        /// <returns></returns>
        public System.Xml.Schema.XmlSchema GetSchema()
        {
            return null;
        }


        /// <summary>
        /// Read the xml
        /// </summary>
        /// <param name="reader"></param>
        public void ReadXml(System.Xml.XmlReader reader)
        {
            bool empty = reader.IsEmptyElement;
            reader.Read();
            if (empty) return;

            reader.MoveToContent();            
            reader.ReadStartElement("Components");
            Components = VersionImport.Load<List<VersionDefItem>>(reader, _supportedTypes);
            reader.ReadEndElement();

            //Read Closing Element
            reader.ReadEndElement();
        }


        /// <summary>
        /// Write the components.
        /// </summary>
        /// <param name="writer"></param>
        public void WriteXml(System.Xml.XmlWriter writer)
        {
            writer.WriteStartElement("Components");
            VersionImport.Save<List<VersionDefItem>>(Components, writer, _supportedTypes);
            writer.WriteEndElement();                        
        }

        #endregion
    }



    /// <summary>
    /// Definition for a specific component ( file, environment variable, dir, drive etc.
    /// </summary>
    [XmlInclude(typeof(VersionDefItem))]
    public class VersionDefItem
    {
        /// <summary>
        /// Initialize.
        /// </summary>
        public VersionDefItem()
        {
            Requirement = VersionRequirement.Required;
            Match = VersionMatch.Equal;
            FailOnError = false;
        }


        /// <summary>
        /// Used to associate checks with a group.
        /// </summary>
        [XmlAttribute("Group")]        
        public string Group { get; set; }


        /// <summary>
        /// The requirment mode ( required | optional ).
        /// </summary>
        [XmlAttribute("Requirement")]
        public VersionRequirement Requirement { get; set; }


        /// <summary>
        /// Match type ( =, !=, >=, less= ).
        /// </summary>
        [XmlAttribute("Match")]      
        public VersionMatch Match { get; set; }


        /// <summary>
        /// Whether or version checking should continue if this component is missing.
        /// </summary>
        [XmlAttribute("FailOnError")]
        public bool FailOnError { get; set; }


        /// <summary>
        /// Whether or not this is required.
        /// </summary>
        [XmlAttribute("IsRequired")]        
        public bool IsRequired
        {
            get { return Requirement == VersionRequirement.Required; }
            set { Requirement = value == true ? VersionRequirement.Required : VersionRequirement.Optional; }
        }
    }



    /// <summary>
    /// Definition for a specific component ( file, environment variable, dir, drive etc.
    /// </summary>
    [XmlRoot("EnvironmentVariable", Namespace = "")]
    public class VersionDefEnv : VersionDefItem
    {
        /// <summary>
        /// The name of the environment variable.
        /// </summary>
        [XmlAttribute("Name")]                
        public string Name { get; set; }


        /// <summary>
        /// The value of the environment variable.
        /// </summary>
        [XmlAttribute("EnvValue")]
        public string EnvValue { get; set; }


        /// <summary>
        /// Return env variable name and expected value.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Env Var: {0} - with expected value : {1}", Name, EnvValue);
        }
    }



    /// <summary>
    /// Definition for a specific directory.
    /// </summary>
    [XmlRoot("Directory", Namespace = "")]
    public class VersionDefDir : VersionDefItem
    {
        /// <summary>
        /// The path to the directory.
        /// </summary>
        [XmlAttribute("Path")]                
        public string Path { get; set; }


        /// <summary>
        /// Just return path for now.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Path;
        }
    }


    
    /// <summary>
    /// VersionFileDefinition.
    /// </summary>
    [XmlRoot("File", Namespace = "")]
    public class VersionDefFile : VersionDefItem
    {
        /// <summary>
        /// The path to this component.
        /// MyApp\MyApplication.exe
        /// </summary>
        [XmlAttribute("Path")]
        public string Path { get; set; }


        /// <summary>
        /// The version to expect for this file.
        /// e.g. 0.9.2.0
        /// </summary>
        [XmlAttribute("VersionReq")]
        public string VersionReq { get; set; }


        /// <summary>
        /// The date to expect for this file.
        /// e.g. 8/29/2009
        /// </summary>
        [XmlAttribute("DateReq")]
        public string DateReq { get; set; }


        /// <summary>
        /// The size to expect for this file.
        /// </summary>
        [XmlAttribute("SizeReq")]
        public string SizeReq { get; set; }


        /// <summary>
        /// Whether or not the file must be writable.
        /// </summary>
        [XmlAttribute("IsWritable")]
        public bool IsWritable { get; set; }


        /// <summary>
        /// Return env variable name and expected value.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("File : {0} - with version:{1}, date:{2}", Path, VersionReq, DateReq);
        }
    }



    /// <summary>
    /// Definition for a specific registry key.
    /// </summary>
    [XmlRoot("Registry", Namespace = "")]
    public class VersionDefReg : VersionDefItem
    {
        /// <summary>
        /// The root directory.
        /// e.g. HKEY_LOCAL_MACHINE
        /// </summary>
        [XmlAttribute("RootDir")]
        public string RootDir { get; set; }


        /// <summary>
        /// The path to the key.
        /// </summary>
        [XmlAttribute("Path")]
        public string Path { get; set; }


        /// <summary>
        /// The key name to check under the path
        /// </summary>
        [XmlAttribute("Key")]
        public string Key { get; set; }


        /// <summary>
        /// The value to expect.
        /// </summary>
        [XmlAttribute("ExpectedValue")]
        public string ExpectedValue { get; set; }


        /// <summary>
        /// Return env variable name and expected value.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Registry : {0}/{1}/{2}", RootDir, Path, Key);
        }
    }



    /// <summary>
    /// Definition for a specific network drive.
    /// </summary>
    [XmlRoot("Drive")]
    public class VersionDefDrive : VersionDefItem
    {
        /// <summary>
        /// The path to the drive.
        /// e.g. \\cnyc12p20011c\energy$
        /// </summary>
        [XmlAttribute("Path")]
        public string Path { get; set; }


        /// <summary>
        /// The drive letter name.
        /// e.g. "k:"
        /// </summary>
        [XmlAttribute("Name")]
        public string Name { get; set; }


        /// <summary>
        /// Return env variable name and expected value.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("Drive: {0}:{1}", Name, Path);
        } 
    }

}


