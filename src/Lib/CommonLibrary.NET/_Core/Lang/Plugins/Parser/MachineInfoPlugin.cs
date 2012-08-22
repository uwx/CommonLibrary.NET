using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ComLib.Lang;


namespace ComLib.Lang.Extensions
{

    /* *************************************************************************
    <doc:example>	
    // Machine info plugin provides variable names to get information about the machine
        
    print ( @machine        )
    print ( @domain         )
    print ( @user           )
    print ( @cmdline        )
    
    print ( mac.numprocs    )
    print ( mac.osname      )
    print ( mac.osversion   )
    print ( mac.osspack     )
    
    print ( mac sysdir      )
    print ( mac memory      )
    print ( mac version     )
    print ( mac currentdir  )  
	
	// NOTES:
    // Any of the properties above can be prefixed with either
    // 1. mac.<property>
    // 2. mac <property>
    // 3. @<property>
    
    </doc:example>
    ***************************************************************************/

    /// <summary>
    /// Combinator for handling days of the week.
    /// </summary>
    public class MachineInfoPlugin : ExprPlugin
    {
        private static IDictionary<string, Func<string>> _map;


        static MachineInfoPlugin()
        {
            _map = new Dictionary<string, Func<string>>();
            _map["machine"] = () => Environment.MachineName;
            _map["domain"] = () => Environment.UserDomainName;
            _map["user"] = () => Environment.UserName;
            _map["cmdline"] = () => Environment.CommandLine;
            _map["numprocs"] = () => Environment.ProcessorCount.ToString();
            _map["osname"] = () => Environment.OSVersion.Platform.ToString();
            _map["osversion"] = () => Environment.OSVersion.Version.ToString();
            _map["osspack"] = () => Environment.OSVersion.ServicePack;
            _map["sysdir"] = () => Environment.SystemDirectory;
            _map["memory"] = () => Environment.WorkingSet.ToString();
            _map["version"] = () => Environment.Version.ToString();
            _map["currentdir"] = () => Environment.CurrentDirectory;
        }


        /// <summary>
        /// Initialize
        /// </summary>
        public MachineInfoPlugin()
        {
            _startTokens = new string[]{ "mac", "@" };
            _canHandleExpression = true;
        }


        /// <summary>
        /// The grammer for the function declaration
        /// </summary>
        public override string Grammer
        {
            get
            {
                return "( (mac '.' ) | '@' ) <ident>";
            }
        }


        /// <summary>
        /// Examples
        /// </summary>
        public override string[] Examples
        {
            get
            {
                return new string[]
                {
                     "mac machine",
                     "mac domain",
                     "mac user",
                     "mac cmdline",
                     "mac numprocs",
                     "mac.osname",
                     "mac.osversion",
                     "mac.osspack",
                     "mac.sysdir",
                     "mac.memory",
                     "mac.version",
                     "mac.currentdir"
                };
            }
        }


        /// <summary>
        /// Whether or not this plugin can handle the current token.
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        public override bool CanHandle(Token current)
        {
            var next = _tokenIt.Peek();
            if (next.Token == Tokens.Dot)
            {
                next = _tokenIt.Peek(2);
            }
            if (_map.ContainsKey(next.Token.Text))
                return true;
            return false;
        }


        /// <summary>
        /// Parses the day expression.
        /// Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday
        /// </summary>
        /// <returns></returns>
        public override Expr Parse()
        {
            _tokenIt.Advance();

            // mac.user : move past "."
            if (_tokenIt.NextToken.Token == Tokens.Dot)
                _tokenIt.Advance();
            
            var propToken = _tokenIt.NextToken;
            var propname = _tokenIt.ExpectId();

            // Invalid property name?
            if (!_map.ContainsKey(propname))
                throw _tokenIt.BuildSyntaxException("Unknown machine property : " + propname, propToken);

            var val = _map[propname]();
            return new ConstantExpr(val);
        }
    }
}
