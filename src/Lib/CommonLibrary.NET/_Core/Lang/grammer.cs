// Grammer file for most ( not all ) of fluentscript syntax.
// This file is used to auto-generate some of the code, which includes
// 1. statment classes such as TryPlugin

// ******************************************************************************
[TOKENS]
Keywords			:   "var", "if", "while", "for", "switch", "try", 
					    "return", "break", "continue", "catch",
					    "function", "null", "as", "then"

Symbols				:   "{", "}", "(", ")", "[", "]", ".", ",", ";", ":" "#", 
						"-", "+",  "*", "/",  "%",
						"<", "<=",  ">",  ">=", "==", "!=",
						"++", "--", "+=", "-=", "*=", "/=",
						"&&", "||",
						"=",  "!"
    
Newline				:   "\n", "\r\n"

EOS					:   ";" , newline


[TOKEN_CATEGORIES]
Cat_Compareops		:   "<", "<=", ">", ">=", "==", "!="
Cat_Mathops			:   "-", "+",  "*", "/",  "%"
Cat_Conditionops	:   "&&", "||"
Cat_Unaryops		:   "++", "--", "+=", "-=", "*=", "/="


// ******************************************************************************
[LITERALS]
bool 		"true" | "false"
null		"null"
string		"'" [.]* "'" | "\"" [.]* "\""
number		[0-9][0-9]+ "."? [0-9]+


// ******************************************************************************
[TOKEN_TYPES]
ident			 [a-zA-Z]( [a-zA-Z0-9] | _ [a-zA-Z0-9] )*
literal		     number | string | bool 
literalwithnull  number | string | bool | null
paramlist	     newline | ( "(" ident ( "," ident )* ")" )


// ******************************************************************************
[STATEMENTS]
stmt		:   ( var | if | while | for | switch | try | return | break | continue )
stmtblock	:   "{" stmt* "}" | stmt
condblock	:   ( "(" expr ")" stmtblock | expr "then" stmt | expr newline stmtblock )
var   		:   "var"		ident = literal ( ( "," ident = literal )* | EOS )
unary       :   ident       
break		:   "break"   	EOS
continue	:   "continue"	EOS
return		:   "return"  	expr? EOS
if    		:   "if" 	   	condblock
while 		:   "while"    	condblock
for			:   "for" 		"(" var assign unary ")" stmtblock
try			:   "try" 		stmtblock "catch" "(" <multiident> <ident> ")" stmtblock
def			:   "def"		( ident | string ) ( "," ( ident | string ) )* paramlist stmtblock