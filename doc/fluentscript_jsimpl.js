
function Context()
{
	this.Symbols = new Symbols();
	this.Memory = new Memory();
	this.Plugins = new Plugins();
}


function Symbols()
{
	this.DefineVar = function(name, type)
	{
	};
}


function Memory()
{
}


function Plugins()
{
}


function LangSettings()
{
}


var TokenKind =
{
	Keyword: 1,
	Symbol:  2,
	Ident:   3,
	Literal: 4
}


// summary: Token class
// param text {string}: The text for the token
// param kind {TokenKind}: The type of the token
// param val  {object}: A value for the token
function Token(text, kind, val)
{
	this.Text = text;
	this.Kind = kind;
	this.Value = val;
}

Token.prototype.Kind = null;
Token.prototype.Text = "";
Token.prototype.Value = "";


var Tokens = 
{
	 Var: 	new Token("var",      TokenKind.Keyword ),
	 For: 	new Token("for",      TokenKind.Keyword ),
	 While: new Token("while",    TokenKind.Keyword ),
	 If: 	new Token("if",       TokenKind.Keyword ),
	 Else: 	new Token("else",     TokenKind.Keyword ),
	 Swt:	new Token("switch",   TokenKind.Keyword ),
	 Fun: 	new Token("function", TokenKind.Keyword ),
	                           
	 Plus:  new Token("+",        TokenKind.Symbol  ),
	 Minus: new Token("-",        TokenKind.Symbol  ),
	 Div:   new Token("/",        TokenKind.Symbol  ),
	 Mult:  new Token("*",        TokenKind.Symbol  ),
	 Mod:   new Token("%",        TokenKind.Symbol  ),
	                              
	 Dot:   new Token(".",        TokenKind.Symbol  ),
	 LBrk:  new Token("[",        TokenKind.Symbol  ),
	 RBrk:  new Token("]",        TokenKind.Symbol  ),
	 LPar:  new Token("(",        TokenKind.Symbol  ),
	 RPar:  new Token(")",        TokenKind.Symbol  ),
	 LBrc:  new Token("{",        TokenKind.Symbol  ),
	 RBrc:  new Token("}",        TokenKind.Symbol  ),
	 
	 Dot:   new Token("true",     TokenKind.Literal ),
	 LBrk:  new Token("false",    TokenKind.Literal ),
	 RBrk:  new Token("null",     TokenKind.Literal )
};


function Lexer()
{
	// summary: Converts the text into a series of tokens.
	// param text {string}: The text representing source code.
	this.Tokenize = function(text)
	{
		return null;
	};
}


function VariableExp()
{
}


function CompareExpr()
{
}


function BinaryExpr()
{
	this.Left = null;
	this.Right = null;
	this.Op = null;
	
	// summary: Evaluates
	this.DoEvaluate = function()
	{
		
	}
}


Klass( "VarPlugin").Extends("StmtPlugin").As(function()
{
});


function Parser()
{
	// summary: Peek at the next token
	// param count {number}: The number of tokens to peek forward
	// param passNewLine {bool}: Whether or not to pass new line when peeking.
	this.Advance = function(count, passNewLine)
	{
	};
	
	
	// summary: Peek at the next token
	// param count {number}: The number of tokens to peek forward
	// param passNewLine {bool}: Whether or not to pass new line when peeking.
	this.Peek = function(count, passNewLine)
	{
	};
	
	
	// summary: Peek at the next token
	// param token {Token}: The token to expect
	// param advance {bool}: Whether or not to advance after expecting.
	this.Expect = function(token, advance)
	{		
		if(_tokenIt.NextToken != token )
			throw BuildError();
	};
	
	
	// summary: Parses a statement.
	this.ParseStmt = function()
	{			
		var t = this._tokens.Next;
		var stmt = null;
		
		if( this._context.Plugins.IsSysStmt( t.Token ) )
		{
			stmt = this.ParseSystemStatement();				
		}			
		if ( stmt != null )
		{
			stmt.Context = this._context;
			this.SetScriptSource( stmt, t );
		}
		return stmt;
	}
	
	
	// summary: Parse a system statement.
	this.ParseSystemStatement = function()
	{
		
	};
	
	
	// summary: Parses an expression.
	this.ParseExpr = function()
	{
		while( true )
		{
			if( this.IsEnd( t ) )
				break;
				
			// variable : user, total
			if( t.Token.Kind == TokenKind.Ident )
			{
			}
			// Literal const: true, false, 1, 2
			else if( t.Token.Kind == TokenKind.Literal )
			{
			}
			// [ : array
			else if( t.Token == Tokens.LBrk )
			{
			}
			// { : map
			else if( t.Token == Tokens.LBrc )
			{
			}
			
			if( this.IsEnd( t ))
				break;
		}
	}
	
	
	// summary: Parses an array
	this.ParseArray = function()
	{
		var expr = null;
		this.Expect( Tokens.LBrk, true );		
		
		// parse until ] or end token.
		while( true )
		{
			var t = _tokens.Next;
		
			if( this.IsEnd( Tokens.RBrk ) )
				break;
				
			t = this.Advance(1, true );
		}
		
		this.Expect( Tokens.RBrk, true );
		return expr;
	};
	
	
	// summary: Parses a map
	this.ParseMap = function()
	{
		var expr = null;
		this.Expect( Tokens.LBrc, true );			
		
		// parse until } or end token.
		while( true )
		{
			var t = _tokens.Next;
		
			if( this.IsEnd( Tokens.RBrc ) )
				break;
				
			t = this.Advance(1, true );
		}
		
		this.Expect( Tokens.RBrc, true );
		return expr;
	};
	
	
	// summary: Parses parameters
	this.ParseParams = function()
	{
		var expr = null;
		this.Expect( Tokens.LPar, true );
		
		// parse until } or end token.
		while( true )
		{
			var t = _tokens.Next;			
			if( this.IsEnd( Tokens.RPar ) )
				break;
				
			t = this.Advance(1, true );
		}
		
		this.Expect( Tokens.RPar, true );
		return expr;
	};
	
	
	this.IsEnd = function(token)
	{
		var curr = this._tokens.Next.Token;
		if( curr == token ) return true;
		if( curr == Tokens.End ) return true;
		if( curr == Tokens.Semicolon ) return true;
		return false;			
	};
}


function Interpreter()
{
	this._parser = new Parser();
	this._context = new Context();
	
	
	// summary: Executes the text supplied.
	this.Execute = function(text)
	{
		this._context = new Context();
		this._parser = new Parser();
	};
}


var i = new Interpreter();
i.Execute("var num = 2;");
var result = i.Context.Memory.Get("num");

