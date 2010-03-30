grammar Calculator;

options
{
	backtrack=false;
	language=CSharp2;
	output=AST;
	ASTLabelType=CommonTree;
}
tokens
{
	VECTOR;
	UNARY;
	NEGATION;
	FACTORIAL;
}

@parser::namespace { Calculator.Grammar }
@lexer::namespace { Calculator.Grammar }

root
	: stat
	;

stat
	: expr -> expr
	| ID '=' expr -> ^('=' ID expr)
	;

expr
	: multExpr (('+'^|'-'^) multExpr)*
	;

multExpr
	: powExpr (('*'^|'/'^|'%'^) powExpr)*
	;
	
//multExpr
//	: (a=powExpr->$a)
//		( '*' a2=powExpr -> ^('*' $a $a2)
//		| '/' a2=powExpr -> ^('/' $a $a2)
//		| '%' a2=powExpr -> ^('%' $a $a2)
//		| a3=(func+)-> ^(MULT $a $a3)
//		)*
//	;
	
powExpr
	: unary (('^'^) unary)*
	;
	
unary
	: NEGATE+ factorial -> ^(NEGATION factorial NEGATE+)
	| factorial
	;
	
factorial
	: func
		(EXCLAIMATION+ -> ^(FACTORIAL func EXCLAIMATION+)
		| -> func
		)
	;
	
func
	: (SIN^ | COS^ | TAN^ | ATAN^ | ACOS^ | ASIN^ | ABS^ | DEG^ | RAD^ | LN^ | LOG^ | SQRT^) func
	| atom
	;


		
atom
	: DOUBLE
	| ID
	| '{'! vector '}'!
	| '('! expr ')'!
	;
	
vector
	: expr (';' expr)* -> ^(VECTOR expr+)
	;

EXCLAIMATION: '!';
SQRT: 'sqrt';
LN: 'ln';
LOG: 'log';
RAD: 'rad';
DEG: 'deg';
SIN: 'sin';
COS: 'cos';
TAN: 'tan';
ATAN: 'atan';
ACOS: 'acos';
ASIN: 'asin';
ABS: 'abs';

NEGATE: '~';
MOD: '%';
POW: '^';
EQUALS: '=';
PLUS: '+';
MINUS: '-';
MULT: '*';
DIVIDE: '/';
//UNICODE	: ('\u03B8'|'\u0398'|'\u03C0'|'\u221E'|'\u2603'
ID: ('_'|'a'..'z'|'A'..'Z'|'\\'|'\u00C0'..'\uFFFF')('_'|'a'..'z'|'A'..'Z'|'0'..'9'|'\\'|'\u00C0'..'\uFFFF')* ;
DOUBLE
	: ('0'..'9')+ '.' ('0'..'9')*
	| '.' ('0'..'9')+
	| ('0'..'9')+
	
	| ('0'..'9')+ 'E' ('0'..'9')+
	| ('0'..'9')+ 'E' '.' ('0'..'9')+
	| ('0'..'9')+ 'E' ('0'..'9')+ '.' ('0'..'9')*
	
	| '.' ('0'..'9')+ 'E' ('0'..'9')+
	| '.' ('0'..'9')+ 'E' '.' ('0'..'9')+
	| '.' ('0'..'9')+ 'E' ('0'..'9')+ '.' ('0'..'9')*
	
	| ('0'..'9')+ '.' ('0'..'9')* 'E' ('0'..'9')+
	| ('0'..'9')+ '.' ('0'..'9')* 'E' '.' ('0'..'9')+
	| ('0'..'9')+ '.' ('0'..'9')* 'E' ('0'..'9')+ '.' ('0'..'9')*
	;

NEWLINE: '\r'? '\n' ;
WS: (' '|'\t'|'\n'|'\r')+ {Skip();} ;