using System;
using System.Collections.Generic;

// <lang:using>
using ComLib.Lang.Core;
using ComLib.Lang.AST;
using ComLib.Lang.Types;
using ComLib.Lang.Parsing;
using ComLib.Lang.Helpers;
// </lang:using>

namespace ComLib.Lang.Plugins
{
    /// <summary>
    /// Plugin for throwing errors from the script.
    /// </summary>
    public class VarPlugin : ExprPlugin, IParserCallbacks
    {
        /// <summary>
        /// Intialize.
        /// </summary>
        public VarPlugin()
        {
            this.ConfigureAsSystemStatement(false, true, "var,$IdToken");
            this.IsAutoMatched = false;
            this.Precedence = 1000;
        }


        /// <summary>
        /// The grammer for the function declaration
        /// </summary>
        public override string Grammer
        {
            get { return "var <id> ( '=' <expression> )? ( ',' <id> ( '=' <expression> )? )* <statementterminator>"; }
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
                    "var name;",
                    "var name, age;",
                    "var name = 'kishore', age = 33;",
                    "var name = 'kishore', age = getage('kishore');",
                    "var name = 'kishore', age;"
                };
            }
        }


        /// <summary>
        /// Whether or not this can handle the current token.
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        public override bool CanHandle(Token current)
        {
            if (current == Tokens.Var) return true;
            var next = _tokenIt.Peek().Token;
            if (next == Tokens.Assignment) return true;

            return false;
        }


        /// <summary>
        /// Parses a assignment statement with declaration.
        /// </summary>
        /// <returns></returns>
        public override Expr Parse()
        {
            bool expectVar = _tokenIt.NextToken.Token == Tokens.Var;
            return ParseAssignment(expectVar, true);
        }


        /// <summary>
        /// Parses an assignment statement. 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public override Expr Parse(object context)
        {
            return ParseAssignment(false, false, context as Expr);
        }


        /// <summary>
        /// 1. var name;
        /// 2. var age = 21;
        /// 3. canDrink = age >= 21;
        /// 4. canVote = CanVote(age);
        /// </summary>
        /// <returns></returns>
        public Expr ParseAssignment(bool expectVar, bool expectId = true, Expr varExp = null)
        {
            string name = null;
            if (expectVar) _tokenIt.Expect(Tokens.Var);
            if (expectId)
            {
                name = _tokenIt.ExpectId();
                varExp = new VariableExpr(name);
            }

            // Case 1: var name;
            if (_tokenIt.IsEndOfStmtOrBlock()) return new AssignExpr(expectVar, varExp, null);

            // Case 2: var name = <expression>
            Expr valueExp = null;
            if (_tokenIt.NextToken.Token == Tokens.Assignment)
            {
                _tokenIt.Advance();
                valueExp = _parser.ParseExpression(Terminators.ExpVarDeclarationEnd, passNewLine: false);
                //if (valueExp is MemberAccessExpr)
                //    ((MemberAccessExpr)valueExp).IsAssignment = true;
            }
            // ; ? only 1 declaration / initialization.
            if (_tokenIt.IsEndOfStmtOrBlock())
                return new AssignExpr(expectVar, varExp, valueExp) { Ctx = Ctx };

            // Multiple 
            // Example 1: var a,b,c;
            // Example 2: var a = 1, b = 2, c = 3;
            _tokenIt.Expect(Tokens.Comma);
            var declarations = new List<Tuple<Expr, Expr>>();
            declarations.Add(new Tuple<Expr, Expr>(varExp, valueExp));

            while (true)
            {
                // Reset to null.
                varExp = null; valueExp = null;
                name = _tokenIt.ExpectId();
                varExp = new VariableExpr(name);

                // , or expression?
                if (_tokenIt.NextToken.Token == Tokens.Assignment)
                {
                    _tokenIt.Advance();
                    valueExp = _parser.ParseExpression(Terminators.ExpVarDeclarationEnd, passNewLine: false);
                }
                // Add to list
                declarations.Add(new Tuple<Expr, Expr>(varExp, valueExp));

                if (_tokenIt.IsEndOfStmtOrBlock())
                    break;

                _tokenIt.Expect(Tokens.Comma);
            }
            return new AssignExpr(expectVar, declarations);
        }


        /// <summary>
        /// Called by the framework after the parse method is called
        /// </summary>
        /// <param name="node">The node returned by this implementations Parse method</param>
        public void OnParseComplete(AstNode node)
        {
            var stmt = node as AssignExpr;
            if (stmt._declarations.IsNullOrEmpty())
                return;
            foreach (var decl in stmt._declarations)
            {
                var exp = decl.Item1;
                if (exp is VariableExpr)
                {
                    var varExp = exp as VariableExpr;
                    var valExp = decl.Item2;
                    var name = varExp.Name;
                    bool registeredTypeVar = false;
                    if(valExp is NewExpr )
                    {
                        var newExp = valExp as NewExpr;
                        if (this.Ctx.Types.Contains(newExp.TypeName))
                        {
                            var type = this.Ctx.Types.Get(newExp.TypeName);
                            var ltype = LangTypeHelper.ConvertToLangTypeClass(type);
                            this.Ctx.Symbols.Current.DefineVariable(name, ltype);
                            registeredTypeVar = true;
                        }
                    }
                    if(!registeredTypeVar)
                        this.Ctx.Symbols.Current.DefineVariable(name);
                }
            }
        }
    }


    /// <summary>
    /// Variable expression data
    /// </summary>
    public class AssignExpr : Expr
    {
        private bool _isDeclaration;
        private Expr VarExp;
        private Expr ValueExp;

        /// <summary>
        /// The declarations
        /// </summary>
        internal List<Tuple<Expr, Expr>> _declarations;


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="isDeclaration">Whether or not the variable is being declared in addition to assignment.</param>
        /// <param name="name">Name of the variable</param>
        /// <param name="valueExp">Expression representing the value to set variable to.</param>
        public AssignExpr(bool isDeclaration, string name, Expr valueExp)
            : this(isDeclaration, new VariableExpr(name), valueExp)
        {
        }


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="isDeclaration">Whether or not the variable is being declared in addition to assignment.</param>
        /// <param name="varExp">Expression representing the variable name to set</param>
        /// <param name="valueExp">Expression representing the value to set variable to.</param>
        public AssignExpr(bool isDeclaration, Expr varExp, Expr valueExp)
        {
            this._isDeclaration = isDeclaration;
            this._declarations = new List<Tuple<Expr, Expr>>();
            this._declarations.Add(new Tuple<Expr, Expr>(varExp, valueExp));
        }


        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="isDeclaration">Whether or not the variable is being declared in addition to assignment.</param>
        /// <param name="declarations"></param>        
        public AssignExpr(bool isDeclaration, List<Tuple<Expr, Expr>> declarations)
        {
            this._isDeclaration = isDeclaration;
            this._declarations = declarations;
        }
        

        /// <summary>
        /// Evaluate
        /// </summary>
        /// <returns></returns>
        public override object DoEvaluate()
        {
            object result = null;
            foreach (var assigment in _declarations)
            {
                this.VarExp = assigment.Item1;
                this.ValueExp = assigment.Item2;
                                
                // CASE 1: Assign variable.  a = 1
                if (this.VarExp is VariableExpr)
                {
                    AssignHelper.SetVariableValue(this.Ctx, this, _isDeclaration, this.VarExp, this.ValueExp);
                }
                // CASE 2: Assign member.    
                //      e.g. dictionary       :  user.name = 'kishore'
                //      e.g. property on class:  user.age  = 20
                else if (this.VarExp is MemberAccessExpr)
                {
                    AssignHelper.SetMemberValue(this.Ctx, this, this.VarExp, this.ValueExp);
                }
                // Case 3: Assign value to index: "users[0]" = <expression>;
                else if (this.VarExp is IndexExpr)
                {
                    AssignHelper.SetIndexValue(this.Ctx, this, this.VarExp, this.ValueExp);
                }
            }
            return LObjects.Null;
        }
    }
}
