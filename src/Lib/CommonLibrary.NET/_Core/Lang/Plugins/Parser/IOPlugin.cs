using System;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Linq;
using System.Text;
using ComLib.Lang;


namespace ComLib.Lang.Extensions
{

    /* *************************************************************************
    <doc:example>	
    // File/Directory Input/Output operations.
    
    var day = Monday;
    var date = tomorrow at 3:30 pm;
    
    if tommorrow is Saturday then
	    print Thank god it's Friday
    </doc:example>
    ***************************************************************************/
    /// <summary>
    /// Combinator for handling days of the week.
    /// </summary>
    public class IOPlugin : SetupPlugin
    {        
        /// <summary>
        /// Executes further Registration actions
        /// </summary>
        /// <param name="i">The instance of the interpreter</param>
        /// <param name="ctx">The context of the interperter</param>
        public override void Execute(Context ctx)
        {
            ctx.Types.Register(typeof(File), null);
            ctx.Types.Register(typeof(Dir),  null);
        }
    }



    /// <summary>
    /// API for File based IO.
    /// </summary>
    public class File
    {
        /// <summary>
        /// Creates to a file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="contents"></param>
        public static void Create(string path, string contents)
        {
            System.IO.File.WriteAllText(path, contents);
        }


        /// <summary>
        /// Writes to a file
        /// </summary>
        /// <param name="path"></param>
        /// <param name="contents"></param>
        public static void Write(string path, string contents)
        {
            System.IO.File.WriteAllText(path, contents);
        }


        /// <summary>
        /// Appends text to a file
        /// </summary>
        /// <param name="path">The path to the file</param>
        /// <param name="contents"></param>
        public static void Append(string path, string contents)
        {
            System.IO.File.AppendAllText(path, contents);
        }


        /// <summary>
        /// Copy a file
        /// </summary>
        /// <param name="path">The path to the file</param>
        /// <param name="to">The path to copy to</param>
        /// <param name="overwrite">Whether or not to overwrite any existing files</param>
        public static void Copy(string path, string to, bool overwrite)
        {
            System.IO.File.Copy(path, to, overwrite);
        }


        /// <summary>
        /// Appends text to a file
        /// </summary>
        /// <param name="path">The path to the file to append to</param>
        /// <param name="to">The new name of the file</param>
        /// <param name="overwrite">Whether or not to overwrite any existing files</param>
        public static void Rename(string path, string to, bool overwrite)
        {
            var dirpath = Path.GetDirectoryName(path);
            var newpath = dirpath + Path.DirectorySeparatorChar + to;
            System.IO.File.Move(path, newpath);
        }


        /// <summary>
        /// Moves a file
        /// </summary>
        /// <param name="path">The path to the file</param>
        /// <param name="to">The new location of the file</param>
        public static void Move(string path, string to)
        {
            System.IO.File.Move(path, to);
        }


        /// <summary>
        /// Appends text to a file
        /// </summary>
        /// <param name="path">The path to the file to append to</param>
        public static void Delete(string path)
        {
            System.IO.File.Delete(path);
        }


        /// <summary>
        /// whether or not the file exists
        /// </summary>
        /// <param name="path">The path to the file</param>
        /// <returns></returns>
        public static bool Exists(string path)
        {
            return System.IO.File.Exists(path);
        }


        /// <summary>
        /// Gets the version of the file specified.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static LVersion GetVersion(string path)
        {
            // ? This is a problem. Do not have the debug/source code information here.
            if (!System.IO.File.Exists(path))
                throw new LangException("File not found", "File : " + path + " not found", string.Empty, 0, 0);

            // Throw exception?
            var ext = Path.GetExtension(path);
            if (ext != "dll" && ext != "exe")
                return new LVersion(Version.Parse("0.0.0.0"));

            var asm = Assembly.LoadFrom(path);
            var version = asm.GetName().Version;
            var lversion = new LVersion(version);
            return lversion;
        }
    }



    /// <summary>
    /// API for File based IO.
    /// </summary>
    public class Dir
    {
        /// <summary>
        /// Creates the directory
        /// </summary>
        /// <param name="path"></param>
        public static void Create(string path)
        {
            System.IO.Directory.CreateDirectory(path);
        }


        /// <summary>
        /// Creates the directory
        /// </summary>
        /// <param name="path"></param>
        public static void Make(string path)
        {
            System.IO.Directory.CreateDirectory(path);
        }


        /// <summary>
        /// Copy a file
        /// </summary>
        /// <param name="path">The path to the file</param>
        /// <param name="to">The path to copy to</param>
        /// <param name="overwrite">Whether or not to overwrite any existing files</param>
        public static void Copy(string path, string to, bool overwrite)
        {
            if (!Directory.Exists(to))
                Directory.CreateDirectory(to);

            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                string name = Path.GetFileName( file );
                string dest = Path.Combine(to, name);
                System.IO.File.Copy( file, dest );
            }
            string[] folders = Directory.GetDirectories(path);
            foreach (string folder in folders)
            {
                string name = Path.GetFileName( folder );
                string dest = Path.Combine(to, name);
                Copy( folder, dest, overwrite );
            }
        }


        /// <summary>
        /// Renames a directory
        /// </summary>
        /// <param name="path">The path to the directory to rename</param>
        /// <param name="to">The new name of the directory</param>
        public static void Rename(string path, string to)
        {
            var dirpath = Path.GetDirectoryName(path);
            var newpath = dirpath + Path.DirectorySeparatorChar + to;
            System.IO.Directory.Move(path, newpath);
        }


        /// <summary>
        /// Moves a directory
        /// </summary>
        /// <param name="path">The path to the directory</param>
        /// <param name="to">The new location of the directory</param>
        public static void Move(string path, string to)
        {
            System.IO.Directory.Move(path, to);
        }


        /// <summary>
        /// Appends text to a directory
        /// </summary>
        /// <param name="path">The path to the directory to delete</param>
        public static void Delete(string path)
        {
            System.IO.Directory.Delete(path);
        }


        /// <summary>
        /// whether or not the directory exists
        /// </summary>
        /// <param name="path">The path to the directory</param>
        /// <returns></returns>
        public static bool Exists(string path)
        {
            return System.IO.Directory.Exists(path);
        }
    }
}

