grammar Calc;

compileUnit : expression EOF;
expression :
LPAREN expression RPAREN #ParenthesizedExpr
| expression DESP expression #DespExpr
| '-' expression #UnaryMinus
| '+' expression #UnaryPlus
| expression '%' expression #ModExpr
| expression EXPONENT expression #ExponentialExpr
| expression operatorToken=(MULTIPLY | DIVIDE) expression #MultiplicativeExpr
| expression operatorToken=(ADD | SUBTRACT) expression #AdditiveExpr
| NUMBER #NumberExpr
| IDENTIFIER #IdentifierExpr
| operatorToken=(MAX | MIN) LPAREN expression SEM expression RPAREN #MaxMinExpr
| expression INC #IncExpr
| expression DEC #DecExpr
| 'mmin' LPAREN paramlist=arglist #Mmin
| 'mmax' LPAREN paramlist=arglist #Mmax
;
arglist: expression (SEM expression)+;

/*
Lexer Rules
*/
NUMBER : INT ('.' INT)?;
IDENTIFIER : [A-Z]+[1-9]+;
INT : ('0'..'9')+;
EXPONENT : '^';
MULTIPLY : '*';
DIVIDE : '/';
SUBTRACT : '-';
ADD : '+';
LPAREN : '(';
RPAREN : ')';
MMIN : 'mmin';
MMAX : 'mmax';
MIN : 'min';
MAX : 'max';
DESP : ',';
INC : '++';
DEC : '--';
SEM : ';';
WS : [ \t\r\n] -> channel(HIDDEN);
