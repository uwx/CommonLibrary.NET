using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

using ComLib;
using ComLib.Reflection;


namespace ComLib.Macros
{
    
    /// <summary>
    /// Information Service
    /// </summary>
    public class MacroService : ExtensionService<MacroAttribute, IMacro>, IMacroService
    {
        private char _prefix;
        private char _openBracket;
        private char _closeBracket;


        /// <summary>
        /// Initialize.
        /// </summary>
        public MacroService() : this('$', '[', ']')
        {
        }


        /// <summary>
        /// Initialize.
        /// </summary>
        /// <param name="prefix">Macro prefix.</param>
        /// <param name="open">Macro start tag.</param>
        /// <param name="close">Macro end tag.</param>
        public MacroService(char prefix, char open, char close)
        {
            _prefix = prefix;
            _openBracket = open;
            _closeBracket = close;
            _onLoadCompleteCallback = () => LoadAdditionalAttributes<MacroParameterAttribute>();
        }


        /// <summary>
        /// Register the macro programmatically with lamda func.
        /// </summary>
        /// <param name="name">The name of the macro.</param>
        /// <param name="displayName">The display name of the macro</param>
        /// <param name="description">The description of the macro</param>
        /// <param name="callback">The callback to call when running the macro.</param>
        public virtual void Register(string name, string displayName, string description, Func<Tag, string> callback)
        {
            Register(new MacroAttribute() { Name = name, Description = description }, null, callback);
        }


        /// <summary>
        /// Register the macro programmatically with lamda func.
        /// </summary>
        /// <param name="attrib">The macro attribute to register.</param>
        /// <param name="args">The list of macro parameters.</param>
        /// <param name="callback">The callback to use for the macro.</param>
        public virtual void Register(MacroAttribute attrib, MacroParameterAttribute[] args, Func<Tag, string> callback)
        {
            List<object> macroArgs = null;
            if (args != null && args.Length > 0)
                macroArgs = new List<object>(args);

            var metadata = new ExtensionMetaData
            {
                Id = attrib.Name,
                Attribute = attrib,
                Instance = null,
                DataType = attrib.DeclaringDataType,
                Lamda = callback,
                AdditionalAttributes = macroArgs
            };
            Register(attrib.Name, metadata);
        }


        /// <summary>
        /// Builds the content for the specified tag.
        /// </summary>
        /// <param name="tag">Instance of tag to process.</param>
        /// <returns>Content of tag.</returns>
        public string BuildContent(Tag tag)
        {
            var macro = Create(tag.Name);
            string content = macro.Process(tag);
            return content;
        }


        /// <summary>
        /// Builds the content by replacing any custom tags with their actual content by running the macros.
        /// </summary>
        /// <param name="content">String with content to process.</param>
        /// <returns>Processed content.</returns>
        public string BuildContent(string content)
        {
            var doc = new MacroDocParser(_prefix, _openBracket, _closeBracket);
            var result = doc.Parse(content);
            if (result == null || result.Tags == null || result.Tags.Count == 0)
            {
                return content;
            }

            int start = 0;
            int tagIndex = 0;
            var buffer = new StringBuilder();
            while (start < content.Length && tagIndex < result.Tags.Count)
            {
                var tag = result.Tags[tagIndex];

                // Add existing content without macros.
                string nonMacroContent = content.Substring(start, tag.Position - start);
                buffer.Append(nonMacroContent);

                // Add macro content.
                var meta = Lookup[tag.Name];
                string macroContent = string.Empty;
                if (meta.Lamda == null)
                {
                    var macro = Create(tag.Name);
                    macroContent = macro.Process(tag);
                }
                else
                {
                    var callback = meta.Lamda as Func<Tag, string>;
                    macroContent = callback(tag);
                }
                buffer.Append(macroContent);

                // Increment 
                // 1. next starting postion of content after macro tag.
                // 2. next index of tag
                start = tag.Position + tag.Length;
                tagIndex++;
            }

            // Remaining content
            if (start < content.Length)
                buffer.Append(content.Substring(start));

            string finalText = buffer.ToString();
            return finalText;
        }
    }
}
