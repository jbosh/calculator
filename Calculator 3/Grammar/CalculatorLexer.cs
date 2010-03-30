// $ANTLR 3.2 Sep 23, 2009 12:02:23 Calculator.g 2010-03-30 16:58:38

// The variable 'variable' is assigned but its value is never used.
#pragma warning disable 168, 219
// Unreachable code detected.
#pragma warning disable 162


using System;
using Antlr.Runtime;
using IList 		= System.Collections.IList;
using ArrayList 	= System.Collections.ArrayList;
using Stack 		= Antlr.Runtime.Collections.StackList;


namespace  Calculator.Grammar 
{
public partial class CalculatorLexer : Lexer {
    public const int EXCLAIMATION = 10;
    public const int DEG = 18;
    public const int ACOS = 15;
    public const int LN = 20;
    public const int MOD = 24;
    public const int LOG = 21;
    public const int NEGATE = 9;
    public const int EQUALS = 26;
    public const int MULT = 29;
    public const int MINUS = 28;
    public const int SQRT = 22;
    public const int ID = 8;
    public const int EOF = -1;
    public const int ABS = 17;
    public const int WS = 32;
    public const int T__33 = 33;
    public const int T__34 = 34;
    public const int POW = 25;
    public const int NEGATION = 6;
    public const int T__35 = 35;
    public const int NEWLINE = 31;
    public const int SIN = 11;
    public const int VECTOR = 4;
    public const int T__36 = 36;
    public const int T__37 = 37;
    public const int COS = 12;
    public const int TAN = 13;
    public const int RAD = 19;
    public const int DOUBLE = 23;
    public const int ASIN = 16;
    public const int PLUS = 27;
    public const int ATAN = 14;
    public const int UNARY = 5;
    public const int FACTORIAL = 7;
    public const int DIVIDE = 30;

    // delegates
    // delegators

    public CalculatorLexer() 
    {
		InitializeCyclicDFAs();
    }
    public CalculatorLexer(ICharStream input)
		: this(input, null) {
    }
    public CalculatorLexer(ICharStream input, RecognizerSharedState state)
		: base(input, state) {
		InitializeCyclicDFAs(); 

    }
    
    override public string GrammarFileName
    {
    	get { return "Calculator.g";} 
    }

    // $ANTLR start "T__33"
    public void mT__33() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__33;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Calculator.g:9:7: ( '{' )
            // Calculator.g:9:9: '{'
            {
            	Match('{'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__33"

    // $ANTLR start "T__34"
    public void mT__34() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__34;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Calculator.g:10:7: ( '}' )
            // Calculator.g:10:9: '}'
            {
            	Match('}'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__34"

    // $ANTLR start "T__35"
    public void mT__35() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__35;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Calculator.g:11:7: ( '(' )
            // Calculator.g:11:9: '('
            {
            	Match('('); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__35"

    // $ANTLR start "T__36"
    public void mT__36() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__36;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Calculator.g:12:7: ( ')' )
            // Calculator.g:12:9: ')'
            {
            	Match(')'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__36"

    // $ANTLR start "T__37"
    public void mT__37() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = T__37;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Calculator.g:13:7: ( ';' )
            // Calculator.g:13:9: ';'
            {
            	Match(';'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "T__37"

    // $ANTLR start "EXCLAIMATION"
    public void mEXCLAIMATION() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = EXCLAIMATION;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Calculator.g:81:13: ( '!' )
            // Calculator.g:81:15: '!'
            {
            	Match('!'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "EXCLAIMATION"

    // $ANTLR start "SQRT"
    public void mSQRT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = SQRT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Calculator.g:82:5: ( 'sqrt' )
            // Calculator.g:82:7: 'sqrt'
            {
            	Match("sqrt"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "SQRT"

    // $ANTLR start "LN"
    public void mLN() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = LN;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Calculator.g:83:3: ( 'ln' )
            // Calculator.g:83:5: 'ln'
            {
            	Match("ln"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "LN"

    // $ANTLR start "LOG"
    public void mLOG() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = LOG;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Calculator.g:84:4: ( 'log' )
            // Calculator.g:84:6: 'log'
            {
            	Match("log"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "LOG"

    // $ANTLR start "RAD"
    public void mRAD() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = RAD;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Calculator.g:85:4: ( 'rad' )
            // Calculator.g:85:6: 'rad'
            {
            	Match("rad"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "RAD"

    // $ANTLR start "DEG"
    public void mDEG() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DEG;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Calculator.g:86:4: ( 'deg' )
            // Calculator.g:86:6: 'deg'
            {
            	Match("deg"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DEG"

    // $ANTLR start "SIN"
    public void mSIN() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = SIN;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Calculator.g:87:4: ( 'sin' )
            // Calculator.g:87:6: 'sin'
            {
            	Match("sin"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "SIN"

    // $ANTLR start "COS"
    public void mCOS() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = COS;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Calculator.g:88:4: ( 'cos' )
            // Calculator.g:88:6: 'cos'
            {
            	Match("cos"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "COS"

    // $ANTLR start "TAN"
    public void mTAN() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = TAN;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Calculator.g:89:4: ( 'tan' )
            // Calculator.g:89:6: 'tan'
            {
            	Match("tan"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "TAN"

    // $ANTLR start "ATAN"
    public void mATAN() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = ATAN;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Calculator.g:90:5: ( 'atan' )
            // Calculator.g:90:7: 'atan'
            {
            	Match("atan"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "ATAN"

    // $ANTLR start "ACOS"
    public void mACOS() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = ACOS;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Calculator.g:91:5: ( 'acos' )
            // Calculator.g:91:7: 'acos'
            {
            	Match("acos"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "ACOS"

    // $ANTLR start "ASIN"
    public void mASIN() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = ASIN;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Calculator.g:92:5: ( 'asin' )
            // Calculator.g:92:7: 'asin'
            {
            	Match("asin"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "ASIN"

    // $ANTLR start "ABS"
    public void mABS() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = ABS;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Calculator.g:93:4: ( 'abs' )
            // Calculator.g:93:6: 'abs'
            {
            	Match("abs"); 


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "ABS"

    // $ANTLR start "NEGATE"
    public void mNEGATE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = NEGATE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Calculator.g:95:7: ( '~' )
            // Calculator.g:95:9: '~'
            {
            	Match('~'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "NEGATE"

    // $ANTLR start "MOD"
    public void mMOD() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = MOD;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Calculator.g:96:4: ( '%' )
            // Calculator.g:96:6: '%'
            {
            	Match('%'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "MOD"

    // $ANTLR start "POW"
    public void mPOW() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = POW;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Calculator.g:97:4: ( '^' )
            // Calculator.g:97:6: '^'
            {
            	Match('^'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "POW"

    // $ANTLR start "EQUALS"
    public void mEQUALS() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = EQUALS;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Calculator.g:98:7: ( '=' )
            // Calculator.g:98:9: '='
            {
            	Match('='); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "EQUALS"

    // $ANTLR start "PLUS"
    public void mPLUS() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = PLUS;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Calculator.g:99:5: ( '+' )
            // Calculator.g:99:7: '+'
            {
            	Match('+'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "PLUS"

    // $ANTLR start "MINUS"
    public void mMINUS() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = MINUS;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Calculator.g:100:6: ( '-' )
            // Calculator.g:100:8: '-'
            {
            	Match('-'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "MINUS"

    // $ANTLR start "MULT"
    public void mMULT() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = MULT;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Calculator.g:101:5: ( '*' )
            // Calculator.g:101:7: '*'
            {
            	Match('*'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "MULT"

    // $ANTLR start "DIVIDE"
    public void mDIVIDE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DIVIDE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Calculator.g:102:7: ( '/' )
            // Calculator.g:102:9: '/'
            {
            	Match('/'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DIVIDE"

    // $ANTLR start "ID"
    public void mID() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = ID;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Calculator.g:104:3: ( ( '_' | 'a' .. 'z' | 'A' .. 'Z' | '\\\\' | '\\u00C0' .. '\\uFFFF' ) ( '_' | 'a' .. 'z' | 'A' .. 'Z' | '0' .. '9' | '\\\\' | '\\u00C0' .. '\\uFFFF' )* )
            // Calculator.g:104:5: ( '_' | 'a' .. 'z' | 'A' .. 'Z' | '\\\\' | '\\u00C0' .. '\\uFFFF' ) ( '_' | 'a' .. 'z' | 'A' .. 'Z' | '0' .. '9' | '\\\\' | '\\u00C0' .. '\\uFFFF' )*
            {
            	if ( (input.LA(1) >= 'A' && input.LA(1) <= 'Z') || input.LA(1) == '\\' || input.LA(1) == '_' || (input.LA(1) >= 'a' && input.LA(1) <= 'z') || (input.LA(1) >= '\u00C0' && input.LA(1) <= '\uFFFF') ) 
            	{
            	    input.Consume();

            	}
            	else 
            	{
            	    MismatchedSetException mse = new MismatchedSetException(null,input);
            	    Recover(mse);
            	    throw mse;}

            	// Calculator.g:104:52: ( '_' | 'a' .. 'z' | 'A' .. 'Z' | '0' .. '9' | '\\\\' | '\\u00C0' .. '\\uFFFF' )*
            	do 
            	{
            	    int alt1 = 2;
            	    int LA1_0 = input.LA(1);

            	    if ( ((LA1_0 >= '0' && LA1_0 <= '9') || (LA1_0 >= 'A' && LA1_0 <= 'Z') || LA1_0 == '\\' || LA1_0 == '_' || (LA1_0 >= 'a' && LA1_0 <= 'z') || (LA1_0 >= '\u00C0' && LA1_0 <= '\uFFFF')) )
            	    {
            	        alt1 = 1;
            	    }


            	    switch (alt1) 
            		{
            			case 1 :
            			    // Calculator.g:
            			    {
            			    	if ( (input.LA(1) >= '0' && input.LA(1) <= '9') || (input.LA(1) >= 'A' && input.LA(1) <= 'Z') || input.LA(1) == '\\' || input.LA(1) == '_' || (input.LA(1) >= 'a' && input.LA(1) <= 'z') || (input.LA(1) >= '\u00C0' && input.LA(1) <= '\uFFFF') ) 
            			    	{
            			    	    input.Consume();

            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    Recover(mse);
            			    	    throw mse;}


            			    }
            			    break;

            			default:
            			    goto loop1;
            	    }
            	} while (true);

            	loop1:
            		;	// Stops C# compiler whining that label 'loop1' has no statements


            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "ID"

    // $ANTLR start "DOUBLE"
    public void mDOUBLE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = DOUBLE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Calculator.g:106:2: ( ( '0' .. '9' )+ '.' ( '0' .. '9' )* | '.' ( '0' .. '9' )+ | ( '0' .. '9' )+ | ( '0' .. '9' )+ 'E' ( '0' .. '9' )+ | ( '0' .. '9' )+ 'E' '.' ( '0' .. '9' )+ | ( '0' .. '9' )+ 'E' ( '0' .. '9' )+ '.' ( '0' .. '9' )* | '.' ( '0' .. '9' )+ 'E' ( '0' .. '9' )+ | '.' ( '0' .. '9' )+ 'E' '.' ( '0' .. '9' )+ | '.' ( '0' .. '9' )+ 'E' ( '0' .. '9' )+ '.' ( '0' .. '9' )* | ( '0' .. '9' )+ '.' ( '0' .. '9' )* 'E' ( '0' .. '9' )+ | ( '0' .. '9' )+ '.' ( '0' .. '9' )* 'E' '.' ( '0' .. '9' )+ | ( '0' .. '9' )+ '.' ( '0' .. '9' )* 'E' ( '0' .. '9' )+ '.' ( '0' .. '9' )* )
            int alt30 = 12;
            alt30 = dfa30.Predict(input);
            switch (alt30) 
            {
                case 1 :
                    // Calculator.g:106:4: ( '0' .. '9' )+ '.' ( '0' .. '9' )*
                    {
                    	// Calculator.g:106:4: ( '0' .. '9' )+
                    	int cnt2 = 0;
                    	do 
                    	{
                    	    int alt2 = 2;
                    	    int LA2_0 = input.LA(1);

                    	    if ( ((LA2_0 >= '0' && LA2_0 <= '9')) )
                    	    {
                    	        alt2 = 1;
                    	    }


                    	    switch (alt2) 
                    		{
                    			case 1 :
                    			    // Calculator.g:106:5: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    if ( cnt2 >= 1 ) goto loop2;
                    		            EarlyExitException eee2 =
                    		                new EarlyExitException(2, input);
                    		            throw eee2;
                    	    }
                    	    cnt2++;
                    	} while (true);

                    	loop2:
                    		;	// Stops C# compiler whining that label 'loop2' has no statements

                    	Match('.'); 
                    	// Calculator.g:106:20: ( '0' .. '9' )*
                    	do 
                    	{
                    	    int alt3 = 2;
                    	    int LA3_0 = input.LA(1);

                    	    if ( ((LA3_0 >= '0' && LA3_0 <= '9')) )
                    	    {
                    	        alt3 = 1;
                    	    }


                    	    switch (alt3) 
                    		{
                    			case 1 :
                    			    // Calculator.g:106:21: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    goto loop3;
                    	    }
                    	} while (true);

                    	loop3:
                    		;	// Stops C# compiler whining that label 'loop3' has no statements


                    }
                    break;
                case 2 :
                    // Calculator.g:107:4: '.' ( '0' .. '9' )+
                    {
                    	Match('.'); 
                    	// Calculator.g:107:8: ( '0' .. '9' )+
                    	int cnt4 = 0;
                    	do 
                    	{
                    	    int alt4 = 2;
                    	    int LA4_0 = input.LA(1);

                    	    if ( ((LA4_0 >= '0' && LA4_0 <= '9')) )
                    	    {
                    	        alt4 = 1;
                    	    }


                    	    switch (alt4) 
                    		{
                    			case 1 :
                    			    // Calculator.g:107:9: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    if ( cnt4 >= 1 ) goto loop4;
                    		            EarlyExitException eee4 =
                    		                new EarlyExitException(4, input);
                    		            throw eee4;
                    	    }
                    	    cnt4++;
                    	} while (true);

                    	loop4:
                    		;	// Stops C# compiler whining that label 'loop4' has no statements


                    }
                    break;
                case 3 :
                    // Calculator.g:108:4: ( '0' .. '9' )+
                    {
                    	// Calculator.g:108:4: ( '0' .. '9' )+
                    	int cnt5 = 0;
                    	do 
                    	{
                    	    int alt5 = 2;
                    	    int LA5_0 = input.LA(1);

                    	    if ( ((LA5_0 >= '0' && LA5_0 <= '9')) )
                    	    {
                    	        alt5 = 1;
                    	    }


                    	    switch (alt5) 
                    		{
                    			case 1 :
                    			    // Calculator.g:108:5: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    if ( cnt5 >= 1 ) goto loop5;
                    		            EarlyExitException eee5 =
                    		                new EarlyExitException(5, input);
                    		            throw eee5;
                    	    }
                    	    cnt5++;
                    	} while (true);

                    	loop5:
                    		;	// Stops C# compiler whining that label 'loop5' has no statements


                    }
                    break;
                case 4 :
                    // Calculator.g:110:4: ( '0' .. '9' )+ 'E' ( '0' .. '9' )+
                    {
                    	// Calculator.g:110:4: ( '0' .. '9' )+
                    	int cnt6 = 0;
                    	do 
                    	{
                    	    int alt6 = 2;
                    	    int LA6_0 = input.LA(1);

                    	    if ( ((LA6_0 >= '0' && LA6_0 <= '9')) )
                    	    {
                    	        alt6 = 1;
                    	    }


                    	    switch (alt6) 
                    		{
                    			case 1 :
                    			    // Calculator.g:110:5: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    if ( cnt6 >= 1 ) goto loop6;
                    		            EarlyExitException eee6 =
                    		                new EarlyExitException(6, input);
                    		            throw eee6;
                    	    }
                    	    cnt6++;
                    	} while (true);

                    	loop6:
                    		;	// Stops C# compiler whining that label 'loop6' has no statements

                    	Match('E'); 
                    	// Calculator.g:110:20: ( '0' .. '9' )+
                    	int cnt7 = 0;
                    	do 
                    	{
                    	    int alt7 = 2;
                    	    int LA7_0 = input.LA(1);

                    	    if ( ((LA7_0 >= '0' && LA7_0 <= '9')) )
                    	    {
                    	        alt7 = 1;
                    	    }


                    	    switch (alt7) 
                    		{
                    			case 1 :
                    			    // Calculator.g:110:21: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    if ( cnt7 >= 1 ) goto loop7;
                    		            EarlyExitException eee7 =
                    		                new EarlyExitException(7, input);
                    		            throw eee7;
                    	    }
                    	    cnt7++;
                    	} while (true);

                    	loop7:
                    		;	// Stops C# compiler whining that label 'loop7' has no statements


                    }
                    break;
                case 5 :
                    // Calculator.g:111:4: ( '0' .. '9' )+ 'E' '.' ( '0' .. '9' )+
                    {
                    	// Calculator.g:111:4: ( '0' .. '9' )+
                    	int cnt8 = 0;
                    	do 
                    	{
                    	    int alt8 = 2;
                    	    int LA8_0 = input.LA(1);

                    	    if ( ((LA8_0 >= '0' && LA8_0 <= '9')) )
                    	    {
                    	        alt8 = 1;
                    	    }


                    	    switch (alt8) 
                    		{
                    			case 1 :
                    			    // Calculator.g:111:5: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    if ( cnt8 >= 1 ) goto loop8;
                    		            EarlyExitException eee8 =
                    		                new EarlyExitException(8, input);
                    		            throw eee8;
                    	    }
                    	    cnt8++;
                    	} while (true);

                    	loop8:
                    		;	// Stops C# compiler whining that label 'loop8' has no statements

                    	Match('E'); 
                    	Match('.'); 
                    	// Calculator.g:111:24: ( '0' .. '9' )+
                    	int cnt9 = 0;
                    	do 
                    	{
                    	    int alt9 = 2;
                    	    int LA9_0 = input.LA(1);

                    	    if ( ((LA9_0 >= '0' && LA9_0 <= '9')) )
                    	    {
                    	        alt9 = 1;
                    	    }


                    	    switch (alt9) 
                    		{
                    			case 1 :
                    			    // Calculator.g:111:25: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    if ( cnt9 >= 1 ) goto loop9;
                    		            EarlyExitException eee9 =
                    		                new EarlyExitException(9, input);
                    		            throw eee9;
                    	    }
                    	    cnt9++;
                    	} while (true);

                    	loop9:
                    		;	// Stops C# compiler whining that label 'loop9' has no statements


                    }
                    break;
                case 6 :
                    // Calculator.g:112:4: ( '0' .. '9' )+ 'E' ( '0' .. '9' )+ '.' ( '0' .. '9' )*
                    {
                    	// Calculator.g:112:4: ( '0' .. '9' )+
                    	int cnt10 = 0;
                    	do 
                    	{
                    	    int alt10 = 2;
                    	    int LA10_0 = input.LA(1);

                    	    if ( ((LA10_0 >= '0' && LA10_0 <= '9')) )
                    	    {
                    	        alt10 = 1;
                    	    }


                    	    switch (alt10) 
                    		{
                    			case 1 :
                    			    // Calculator.g:112:5: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    if ( cnt10 >= 1 ) goto loop10;
                    		            EarlyExitException eee10 =
                    		                new EarlyExitException(10, input);
                    		            throw eee10;
                    	    }
                    	    cnt10++;
                    	} while (true);

                    	loop10:
                    		;	// Stops C# compiler whining that label 'loop10' has no statements

                    	Match('E'); 
                    	// Calculator.g:112:20: ( '0' .. '9' )+
                    	int cnt11 = 0;
                    	do 
                    	{
                    	    int alt11 = 2;
                    	    int LA11_0 = input.LA(1);

                    	    if ( ((LA11_0 >= '0' && LA11_0 <= '9')) )
                    	    {
                    	        alt11 = 1;
                    	    }


                    	    switch (alt11) 
                    		{
                    			case 1 :
                    			    // Calculator.g:112:21: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    if ( cnt11 >= 1 ) goto loop11;
                    		            EarlyExitException eee11 =
                    		                new EarlyExitException(11, input);
                    		            throw eee11;
                    	    }
                    	    cnt11++;
                    	} while (true);

                    	loop11:
                    		;	// Stops C# compiler whining that label 'loop11' has no statements

                    	Match('.'); 
                    	// Calculator.g:112:36: ( '0' .. '9' )*
                    	do 
                    	{
                    	    int alt12 = 2;
                    	    int LA12_0 = input.LA(1);

                    	    if ( ((LA12_0 >= '0' && LA12_0 <= '9')) )
                    	    {
                    	        alt12 = 1;
                    	    }


                    	    switch (alt12) 
                    		{
                    			case 1 :
                    			    // Calculator.g:112:37: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    goto loop12;
                    	    }
                    	} while (true);

                    	loop12:
                    		;	// Stops C# compiler whining that label 'loop12' has no statements


                    }
                    break;
                case 7 :
                    // Calculator.g:114:4: '.' ( '0' .. '9' )+ 'E' ( '0' .. '9' )+
                    {
                    	Match('.'); 
                    	// Calculator.g:114:8: ( '0' .. '9' )+
                    	int cnt13 = 0;
                    	do 
                    	{
                    	    int alt13 = 2;
                    	    int LA13_0 = input.LA(1);

                    	    if ( ((LA13_0 >= '0' && LA13_0 <= '9')) )
                    	    {
                    	        alt13 = 1;
                    	    }


                    	    switch (alt13) 
                    		{
                    			case 1 :
                    			    // Calculator.g:114:9: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    if ( cnt13 >= 1 ) goto loop13;
                    		            EarlyExitException eee13 =
                    		                new EarlyExitException(13, input);
                    		            throw eee13;
                    	    }
                    	    cnt13++;
                    	} while (true);

                    	loop13:
                    		;	// Stops C# compiler whining that label 'loop13' has no statements

                    	Match('E'); 
                    	// Calculator.g:114:24: ( '0' .. '9' )+
                    	int cnt14 = 0;
                    	do 
                    	{
                    	    int alt14 = 2;
                    	    int LA14_0 = input.LA(1);

                    	    if ( ((LA14_0 >= '0' && LA14_0 <= '9')) )
                    	    {
                    	        alt14 = 1;
                    	    }


                    	    switch (alt14) 
                    		{
                    			case 1 :
                    			    // Calculator.g:114:25: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    if ( cnt14 >= 1 ) goto loop14;
                    		            EarlyExitException eee14 =
                    		                new EarlyExitException(14, input);
                    		            throw eee14;
                    	    }
                    	    cnt14++;
                    	} while (true);

                    	loop14:
                    		;	// Stops C# compiler whining that label 'loop14' has no statements


                    }
                    break;
                case 8 :
                    // Calculator.g:115:4: '.' ( '0' .. '9' )+ 'E' '.' ( '0' .. '9' )+
                    {
                    	Match('.'); 
                    	// Calculator.g:115:8: ( '0' .. '9' )+
                    	int cnt15 = 0;
                    	do 
                    	{
                    	    int alt15 = 2;
                    	    int LA15_0 = input.LA(1);

                    	    if ( ((LA15_0 >= '0' && LA15_0 <= '9')) )
                    	    {
                    	        alt15 = 1;
                    	    }


                    	    switch (alt15) 
                    		{
                    			case 1 :
                    			    // Calculator.g:115:9: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    if ( cnt15 >= 1 ) goto loop15;
                    		            EarlyExitException eee15 =
                    		                new EarlyExitException(15, input);
                    		            throw eee15;
                    	    }
                    	    cnt15++;
                    	} while (true);

                    	loop15:
                    		;	// Stops C# compiler whining that label 'loop15' has no statements

                    	Match('E'); 
                    	Match('.'); 
                    	// Calculator.g:115:28: ( '0' .. '9' )+
                    	int cnt16 = 0;
                    	do 
                    	{
                    	    int alt16 = 2;
                    	    int LA16_0 = input.LA(1);

                    	    if ( ((LA16_0 >= '0' && LA16_0 <= '9')) )
                    	    {
                    	        alt16 = 1;
                    	    }


                    	    switch (alt16) 
                    		{
                    			case 1 :
                    			    // Calculator.g:115:29: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    if ( cnt16 >= 1 ) goto loop16;
                    		            EarlyExitException eee16 =
                    		                new EarlyExitException(16, input);
                    		            throw eee16;
                    	    }
                    	    cnt16++;
                    	} while (true);

                    	loop16:
                    		;	// Stops C# compiler whining that label 'loop16' has no statements


                    }
                    break;
                case 9 :
                    // Calculator.g:116:4: '.' ( '0' .. '9' )+ 'E' ( '0' .. '9' )+ '.' ( '0' .. '9' )*
                    {
                    	Match('.'); 
                    	// Calculator.g:116:8: ( '0' .. '9' )+
                    	int cnt17 = 0;
                    	do 
                    	{
                    	    int alt17 = 2;
                    	    int LA17_0 = input.LA(1);

                    	    if ( ((LA17_0 >= '0' && LA17_0 <= '9')) )
                    	    {
                    	        alt17 = 1;
                    	    }


                    	    switch (alt17) 
                    		{
                    			case 1 :
                    			    // Calculator.g:116:9: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    if ( cnt17 >= 1 ) goto loop17;
                    		            EarlyExitException eee17 =
                    		                new EarlyExitException(17, input);
                    		            throw eee17;
                    	    }
                    	    cnt17++;
                    	} while (true);

                    	loop17:
                    		;	// Stops C# compiler whining that label 'loop17' has no statements

                    	Match('E'); 
                    	// Calculator.g:116:24: ( '0' .. '9' )+
                    	int cnt18 = 0;
                    	do 
                    	{
                    	    int alt18 = 2;
                    	    int LA18_0 = input.LA(1);

                    	    if ( ((LA18_0 >= '0' && LA18_0 <= '9')) )
                    	    {
                    	        alt18 = 1;
                    	    }


                    	    switch (alt18) 
                    		{
                    			case 1 :
                    			    // Calculator.g:116:25: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    if ( cnt18 >= 1 ) goto loop18;
                    		            EarlyExitException eee18 =
                    		                new EarlyExitException(18, input);
                    		            throw eee18;
                    	    }
                    	    cnt18++;
                    	} while (true);

                    	loop18:
                    		;	// Stops C# compiler whining that label 'loop18' has no statements

                    	Match('.'); 
                    	// Calculator.g:116:40: ( '0' .. '9' )*
                    	do 
                    	{
                    	    int alt19 = 2;
                    	    int LA19_0 = input.LA(1);

                    	    if ( ((LA19_0 >= '0' && LA19_0 <= '9')) )
                    	    {
                    	        alt19 = 1;
                    	    }


                    	    switch (alt19) 
                    		{
                    			case 1 :
                    			    // Calculator.g:116:41: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    goto loop19;
                    	    }
                    	} while (true);

                    	loop19:
                    		;	// Stops C# compiler whining that label 'loop19' has no statements


                    }
                    break;
                case 10 :
                    // Calculator.g:118:4: ( '0' .. '9' )+ '.' ( '0' .. '9' )* 'E' ( '0' .. '9' )+
                    {
                    	// Calculator.g:118:4: ( '0' .. '9' )+
                    	int cnt20 = 0;
                    	do 
                    	{
                    	    int alt20 = 2;
                    	    int LA20_0 = input.LA(1);

                    	    if ( ((LA20_0 >= '0' && LA20_0 <= '9')) )
                    	    {
                    	        alt20 = 1;
                    	    }


                    	    switch (alt20) 
                    		{
                    			case 1 :
                    			    // Calculator.g:118:5: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    if ( cnt20 >= 1 ) goto loop20;
                    		            EarlyExitException eee20 =
                    		                new EarlyExitException(20, input);
                    		            throw eee20;
                    	    }
                    	    cnt20++;
                    	} while (true);

                    	loop20:
                    		;	// Stops C# compiler whining that label 'loop20' has no statements

                    	Match('.'); 
                    	// Calculator.g:118:20: ( '0' .. '9' )*
                    	do 
                    	{
                    	    int alt21 = 2;
                    	    int LA21_0 = input.LA(1);

                    	    if ( ((LA21_0 >= '0' && LA21_0 <= '9')) )
                    	    {
                    	        alt21 = 1;
                    	    }


                    	    switch (alt21) 
                    		{
                    			case 1 :
                    			    // Calculator.g:118:21: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    goto loop21;
                    	    }
                    	} while (true);

                    	loop21:
                    		;	// Stops C# compiler whining that label 'loop21' has no statements

                    	Match('E'); 
                    	// Calculator.g:118:36: ( '0' .. '9' )+
                    	int cnt22 = 0;
                    	do 
                    	{
                    	    int alt22 = 2;
                    	    int LA22_0 = input.LA(1);

                    	    if ( ((LA22_0 >= '0' && LA22_0 <= '9')) )
                    	    {
                    	        alt22 = 1;
                    	    }


                    	    switch (alt22) 
                    		{
                    			case 1 :
                    			    // Calculator.g:118:37: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    if ( cnt22 >= 1 ) goto loop22;
                    		            EarlyExitException eee22 =
                    		                new EarlyExitException(22, input);
                    		            throw eee22;
                    	    }
                    	    cnt22++;
                    	} while (true);

                    	loop22:
                    		;	// Stops C# compiler whining that label 'loop22' has no statements


                    }
                    break;
                case 11 :
                    // Calculator.g:119:4: ( '0' .. '9' )+ '.' ( '0' .. '9' )* 'E' '.' ( '0' .. '9' )+
                    {
                    	// Calculator.g:119:4: ( '0' .. '9' )+
                    	int cnt23 = 0;
                    	do 
                    	{
                    	    int alt23 = 2;
                    	    int LA23_0 = input.LA(1);

                    	    if ( ((LA23_0 >= '0' && LA23_0 <= '9')) )
                    	    {
                    	        alt23 = 1;
                    	    }


                    	    switch (alt23) 
                    		{
                    			case 1 :
                    			    // Calculator.g:119:5: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    if ( cnt23 >= 1 ) goto loop23;
                    		            EarlyExitException eee23 =
                    		                new EarlyExitException(23, input);
                    		            throw eee23;
                    	    }
                    	    cnt23++;
                    	} while (true);

                    	loop23:
                    		;	// Stops C# compiler whining that label 'loop23' has no statements

                    	Match('.'); 
                    	// Calculator.g:119:20: ( '0' .. '9' )*
                    	do 
                    	{
                    	    int alt24 = 2;
                    	    int LA24_0 = input.LA(1);

                    	    if ( ((LA24_0 >= '0' && LA24_0 <= '9')) )
                    	    {
                    	        alt24 = 1;
                    	    }


                    	    switch (alt24) 
                    		{
                    			case 1 :
                    			    // Calculator.g:119:21: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    goto loop24;
                    	    }
                    	} while (true);

                    	loop24:
                    		;	// Stops C# compiler whining that label 'loop24' has no statements

                    	Match('E'); 
                    	Match('.'); 
                    	// Calculator.g:119:40: ( '0' .. '9' )+
                    	int cnt25 = 0;
                    	do 
                    	{
                    	    int alt25 = 2;
                    	    int LA25_0 = input.LA(1);

                    	    if ( ((LA25_0 >= '0' && LA25_0 <= '9')) )
                    	    {
                    	        alt25 = 1;
                    	    }


                    	    switch (alt25) 
                    		{
                    			case 1 :
                    			    // Calculator.g:119:41: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    if ( cnt25 >= 1 ) goto loop25;
                    		            EarlyExitException eee25 =
                    		                new EarlyExitException(25, input);
                    		            throw eee25;
                    	    }
                    	    cnt25++;
                    	} while (true);

                    	loop25:
                    		;	// Stops C# compiler whining that label 'loop25' has no statements


                    }
                    break;
                case 12 :
                    // Calculator.g:120:4: ( '0' .. '9' )+ '.' ( '0' .. '9' )* 'E' ( '0' .. '9' )+ '.' ( '0' .. '9' )*
                    {
                    	// Calculator.g:120:4: ( '0' .. '9' )+
                    	int cnt26 = 0;
                    	do 
                    	{
                    	    int alt26 = 2;
                    	    int LA26_0 = input.LA(1);

                    	    if ( ((LA26_0 >= '0' && LA26_0 <= '9')) )
                    	    {
                    	        alt26 = 1;
                    	    }


                    	    switch (alt26) 
                    		{
                    			case 1 :
                    			    // Calculator.g:120:5: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    if ( cnt26 >= 1 ) goto loop26;
                    		            EarlyExitException eee26 =
                    		                new EarlyExitException(26, input);
                    		            throw eee26;
                    	    }
                    	    cnt26++;
                    	} while (true);

                    	loop26:
                    		;	// Stops C# compiler whining that label 'loop26' has no statements

                    	Match('.'); 
                    	// Calculator.g:120:20: ( '0' .. '9' )*
                    	do 
                    	{
                    	    int alt27 = 2;
                    	    int LA27_0 = input.LA(1);

                    	    if ( ((LA27_0 >= '0' && LA27_0 <= '9')) )
                    	    {
                    	        alt27 = 1;
                    	    }


                    	    switch (alt27) 
                    		{
                    			case 1 :
                    			    // Calculator.g:120:21: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    goto loop27;
                    	    }
                    	} while (true);

                    	loop27:
                    		;	// Stops C# compiler whining that label 'loop27' has no statements

                    	Match('E'); 
                    	// Calculator.g:120:36: ( '0' .. '9' )+
                    	int cnt28 = 0;
                    	do 
                    	{
                    	    int alt28 = 2;
                    	    int LA28_0 = input.LA(1);

                    	    if ( ((LA28_0 >= '0' && LA28_0 <= '9')) )
                    	    {
                    	        alt28 = 1;
                    	    }


                    	    switch (alt28) 
                    		{
                    			case 1 :
                    			    // Calculator.g:120:37: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    if ( cnt28 >= 1 ) goto loop28;
                    		            EarlyExitException eee28 =
                    		                new EarlyExitException(28, input);
                    		            throw eee28;
                    	    }
                    	    cnt28++;
                    	} while (true);

                    	loop28:
                    		;	// Stops C# compiler whining that label 'loop28' has no statements

                    	Match('.'); 
                    	// Calculator.g:120:52: ( '0' .. '9' )*
                    	do 
                    	{
                    	    int alt29 = 2;
                    	    int LA29_0 = input.LA(1);

                    	    if ( ((LA29_0 >= '0' && LA29_0 <= '9')) )
                    	    {
                    	        alt29 = 1;
                    	    }


                    	    switch (alt29) 
                    		{
                    			case 1 :
                    			    // Calculator.g:120:53: '0' .. '9'
                    			    {
                    			    	MatchRange('0','9'); 

                    			    }
                    			    break;

                    			default:
                    			    goto loop29;
                    	    }
                    	} while (true);

                    	loop29:
                    		;	// Stops C# compiler whining that label 'loop29' has no statements


                    }
                    break;

            }
            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "DOUBLE"

    // $ANTLR start "NEWLINE"
    public void mNEWLINE() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = NEWLINE;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Calculator.g:123:8: ( ( '\\r' )? '\\n' )
            // Calculator.g:123:10: ( '\\r' )? '\\n'
            {
            	// Calculator.g:123:10: ( '\\r' )?
            	int alt31 = 2;
            	int LA31_0 = input.LA(1);

            	if ( (LA31_0 == '\r') )
            	{
            	    alt31 = 1;
            	}
            	switch (alt31) 
            	{
            	    case 1 :
            	        // Calculator.g:123:10: '\\r'
            	        {
            	        	Match('\r'); 

            	        }
            	        break;

            	}

            	Match('\n'); 

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "NEWLINE"

    // $ANTLR start "WS"
    public void mWS() // throws RecognitionException [2]
    {
    		try
    		{
            int _type = WS;
    	int _channel = DEFAULT_TOKEN_CHANNEL;
            // Calculator.g:124:3: ( ( ' ' | '\\t' | '\\n' | '\\r' )+ )
            // Calculator.g:124:5: ( ' ' | '\\t' | '\\n' | '\\r' )+
            {
            	// Calculator.g:124:5: ( ' ' | '\\t' | '\\n' | '\\r' )+
            	int cnt32 = 0;
            	do 
            	{
            	    int alt32 = 2;
            	    int LA32_0 = input.LA(1);

            	    if ( ((LA32_0 >= '\t' && LA32_0 <= '\n') || LA32_0 == '\r' || LA32_0 == ' ') )
            	    {
            	        alt32 = 1;
            	    }


            	    switch (alt32) 
            		{
            			case 1 :
            			    // Calculator.g:
            			    {
            			    	if ( (input.LA(1) >= '\t' && input.LA(1) <= '\n') || input.LA(1) == '\r' || input.LA(1) == ' ' ) 
            			    	{
            			    	    input.Consume();

            			    	}
            			    	else 
            			    	{
            			    	    MismatchedSetException mse = new MismatchedSetException(null,input);
            			    	    Recover(mse);
            			    	    throw mse;}


            			    }
            			    break;

            			default:
            			    if ( cnt32 >= 1 ) goto loop32;
            		            EarlyExitException eee32 =
            		                new EarlyExitException(32, input);
            		            throw eee32;
            	    }
            	    cnt32++;
            	} while (true);

            	loop32:
            		;	// Stops C# compiler whining that label 'loop32' has no statements

            	Skip();

            }

            state.type = _type;
            state.channel = _channel;
        }
        finally 
    	{
        }
    }
    // $ANTLR end "WS"

    override public void mTokens() // throws RecognitionException 
    {
        // Calculator.g:1:8: ( T__33 | T__34 | T__35 | T__36 | T__37 | EXCLAIMATION | SQRT | LN | LOG | RAD | DEG | SIN | COS | TAN | ATAN | ACOS | ASIN | ABS | NEGATE | MOD | POW | EQUALS | PLUS | MINUS | MULT | DIVIDE | ID | DOUBLE | NEWLINE | WS )
        int alt33 = 30;
        alt33 = dfa33.Predict(input);
        switch (alt33) 
        {
            case 1 :
                // Calculator.g:1:10: T__33
                {
                	mT__33(); 

                }
                break;
            case 2 :
                // Calculator.g:1:16: T__34
                {
                	mT__34(); 

                }
                break;
            case 3 :
                // Calculator.g:1:22: T__35
                {
                	mT__35(); 

                }
                break;
            case 4 :
                // Calculator.g:1:28: T__36
                {
                	mT__36(); 

                }
                break;
            case 5 :
                // Calculator.g:1:34: T__37
                {
                	mT__37(); 

                }
                break;
            case 6 :
                // Calculator.g:1:40: EXCLAIMATION
                {
                	mEXCLAIMATION(); 

                }
                break;
            case 7 :
                // Calculator.g:1:53: SQRT
                {
                	mSQRT(); 

                }
                break;
            case 8 :
                // Calculator.g:1:58: LN
                {
                	mLN(); 

                }
                break;
            case 9 :
                // Calculator.g:1:61: LOG
                {
                	mLOG(); 

                }
                break;
            case 10 :
                // Calculator.g:1:65: RAD
                {
                	mRAD(); 

                }
                break;
            case 11 :
                // Calculator.g:1:69: DEG
                {
                	mDEG(); 

                }
                break;
            case 12 :
                // Calculator.g:1:73: SIN
                {
                	mSIN(); 

                }
                break;
            case 13 :
                // Calculator.g:1:77: COS
                {
                	mCOS(); 

                }
                break;
            case 14 :
                // Calculator.g:1:81: TAN
                {
                	mTAN(); 

                }
                break;
            case 15 :
                // Calculator.g:1:85: ATAN
                {
                	mATAN(); 

                }
                break;
            case 16 :
                // Calculator.g:1:90: ACOS
                {
                	mACOS(); 

                }
                break;
            case 17 :
                // Calculator.g:1:95: ASIN
                {
                	mASIN(); 

                }
                break;
            case 18 :
                // Calculator.g:1:100: ABS
                {
                	mABS(); 

                }
                break;
            case 19 :
                // Calculator.g:1:104: NEGATE
                {
                	mNEGATE(); 

                }
                break;
            case 20 :
                // Calculator.g:1:111: MOD
                {
                	mMOD(); 

                }
                break;
            case 21 :
                // Calculator.g:1:115: POW
                {
                	mPOW(); 

                }
                break;
            case 22 :
                // Calculator.g:1:119: EQUALS
                {
                	mEQUALS(); 

                }
                break;
            case 23 :
                // Calculator.g:1:126: PLUS
                {
                	mPLUS(); 

                }
                break;
            case 24 :
                // Calculator.g:1:131: MINUS
                {
                	mMINUS(); 

                }
                break;
            case 25 :
                // Calculator.g:1:137: MULT
                {
                	mMULT(); 

                }
                break;
            case 26 :
                // Calculator.g:1:142: DIVIDE
                {
                	mDIVIDE(); 

                }
                break;
            case 27 :
                // Calculator.g:1:149: ID
                {
                	mID(); 

                }
                break;
            case 28 :
                // Calculator.g:1:152: DOUBLE
                {
                	mDOUBLE(); 

                }
                break;
            case 29 :
                // Calculator.g:1:159: NEWLINE
                {
                	mNEWLINE(); 

                }
                break;
            case 30 :
                // Calculator.g:1:167: WS
                {
                	mWS(); 

                }
                break;

        }

    }


    protected DFA30 dfa30;
    protected DFA33 dfa33;
	private void InitializeCyclicDFAs()
	{
	    this.dfa30 = new DFA30(this);
	    this.dfa33 = new DFA33(this);
	}

    const string DFA30_eotS =
        "\x01\uffff\x01\x04\x03\uffff\x01\x0b\x01\x0d\x01\uffff\x01\x0e"+
        "\x01\x0b\x07\uffff\x01\x14\x01\uffff\x01\x17\x04\uffff";
    const string DFA30_eofS =
        "\x18\uffff";
    const string DFA30_minS =
        "\x02\x2e\x01\x30\x01\x2e\x01\uffff\x02\x30\x01\uffff\x01\x2e\x01"+
        "\x30\x01\x2e\x01\uffff\x01\x2e\x04\uffff\x01\x2e\x01\uffff\x01\x2e"+
        "\x04\uffff";
    const string DFA30_maxS =
        "\x01\x39\x01\x45\x02\x39\x01\uffff\x02\x45\x01\uffff\x01\x39\x01"+
        "\x45\x01\x39\x01\uffff\x01\x39\x04\uffff\x01\x39\x01\uffff\x01\x39"+
        "\x04\uffff";
    const string DFA30_acceptS =
        "\x04\uffff\x01\x03\x02\uffff\x01\x05\x03\uffff\x01\x01\x01\uffff"+
        "\x01\x02\x01\x04\x01\x06\x01\x0b\x01\uffff\x01\x08\x01\uffff\x01"+
        "\x0a\x01\x0c\x01\x09\x01\x07";
    const string DFA30_specialS =
        "\x18\uffff}>";
    static readonly string[] DFA30_transitionS = {
            "\x01\x02\x01\uffff\x0a\x01",
            "\x01\x05\x01\uffff\x0a\x01\x0b\uffff\x01\x03",
            "\x0a\x06",
            "\x01\x07\x01\uffff\x0a\x08",
            "",
            "\x0a\x09\x0b\uffff\x01\x0a",
            "\x0a\x06\x0b\uffff\x01\x0c",
            "",
            "\x01\x0f\x01\uffff\x0a\x08",
            "\x0a\x09\x0b\uffff\x01\x0a",
            "\x01\x10\x01\uffff\x0a\x11",
            "",
            "\x01\x12\x01\uffff\x0a\x13",
            "",
            "",
            "",
            "",
            "\x01\x15\x01\uffff\x0a\x11",
            "",
            "\x01\x16\x01\uffff\x0a\x13",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA30_eot = DFA.UnpackEncodedString(DFA30_eotS);
    static readonly short[] DFA30_eof = DFA.UnpackEncodedString(DFA30_eofS);
    static readonly char[] DFA30_min = DFA.UnpackEncodedStringToUnsignedChars(DFA30_minS);
    static readonly char[] DFA30_max = DFA.UnpackEncodedStringToUnsignedChars(DFA30_maxS);
    static readonly short[] DFA30_accept = DFA.UnpackEncodedString(DFA30_acceptS);
    static readonly short[] DFA30_special = DFA.UnpackEncodedString(DFA30_specialS);
    static readonly short[][] DFA30_transition = DFA.UnpackEncodedStringArray(DFA30_transitionS);

    protected class DFA30 : DFA
    {
        public DFA30(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 30;
            this.eot = DFA30_eot;
            this.eof = DFA30_eof;
            this.min = DFA30_min;
            this.max = DFA30_max;
            this.accept = DFA30_accept;
            this.special = DFA30_special;
            this.transition = DFA30_transition;

        }

        override public string Description
        {
            get { return "105:1: DOUBLE : ( ( '0' .. '9' )+ '.' ( '0' .. '9' )* | '.' ( '0' .. '9' )+ | ( '0' .. '9' )+ | ( '0' .. '9' )+ 'E' ( '0' .. '9' )+ | ( '0' .. '9' )+ 'E' '.' ( '0' .. '9' )+ | ( '0' .. '9' )+ 'E' ( '0' .. '9' )+ '.' ( '0' .. '9' )* | '.' ( '0' .. '9' )+ 'E' ( '0' .. '9' )+ | '.' ( '0' .. '9' )+ 'E' '.' ( '0' .. '9' )+ | '.' ( '0' .. '9' )+ 'E' ( '0' .. '9' )+ '.' ( '0' .. '9' )* | ( '0' .. '9' )+ '.' ( '0' .. '9' )* 'E' ( '0' .. '9' )+ | ( '0' .. '9' )+ '.' ( '0' .. '9' )* 'E' '.' ( '0' .. '9' )+ | ( '0' .. '9' )+ '.' ( '0' .. '9' )* 'E' ( '0' .. '9' )+ '.' ( '0' .. '9' )* );"; }
        }

    }

    const string DFA33_eotS =
        "\x07\uffff\x07\x16\x0a\uffff\x01\x1a\x01\x27\x01\uffff\x02\x16"+
        "\x01\x2a\x09\x16\x01\uffff\x01\x16\x01\x35\x01\uffff\x01\x36\x01"+
        "\x37\x01\x38\x01\x39\x01\x3a\x03\x16\x01\x3e\x01\x3f\x06\uffff\x01"+
        "\x40\x01\x41\x01\x42\x05\uffff";
    const string DFA33_eofS =
        "\x43\uffff";
    const string DFA33_minS =
        "\x01\x09\x06\uffff\x01\x69\x01\x6e\x01\x61\x01\x65\x01\x6f\x01"+
        "\x61\x01\x62\x0a\uffff\x01\x0a\x01\x09\x01\uffff\x01\x72\x01\x6e"+
        "\x01\x30\x01\x67\x01\x64\x01\x67\x01\x73\x01\x6e\x01\x61\x01\x6f"+
        "\x01\x69\x01\x73\x01\uffff\x01\x74\x01\x30\x01\uffff\x05\x30\x01"+
        "\x6e\x01\x73\x01\x6e\x02\x30\x06\uffff\x03\x30\x05\uffff";
    const string DFA33_maxS =
        "\x01\uffff\x06\uffff\x01\x71\x01\x6f\x01\x61\x01\x65\x01\x6f\x01"+
        "\x61\x01\x74\x0a\uffff\x01\x0a\x01\x20\x01\uffff\x01\x72\x01\x6e"+
        "\x01\uffff\x01\x67\x01\x64\x01\x67\x01\x73\x01\x6e\x01\x61\x01\x6f"+
        "\x01\x69\x01\x73\x01\uffff\x01\x74\x01\uffff\x01\uffff\x05\uffff"+
        "\x01\x6e\x01\x73\x01\x6e\x02\uffff\x06\uffff\x03\uffff\x05\uffff";
    const string DFA33_acceptS =
        "\x01\uffff\x01\x01\x01\x02\x01\x03\x01\x04\x01\x05\x01\x06\x07"+
        "\uffff\x01\x13\x01\x14\x01\x15\x01\x16\x01\x17\x01\x18\x01\x19\x01"+
        "\x1a\x01\x1b\x01\x1c\x02\uffff\x01\x1e\x0c\uffff\x01\x1d\x02\uffff"+
        "\x01\x08\x0a\uffff\x01\x0c\x01\x09\x01\x0a\x01\x0b\x01\x0d\x01\x0e"+
        "\x03\uffff\x01\x12\x01\x07\x01\x0f\x01\x10\x01\x11";
    const string DFA33_specialS =
        "\x43\uffff}>";
    static readonly string[] DFA33_transitionS = {
            "\x01\x1a\x01\x19\x02\uffff\x01\x18\x12\uffff\x01\x1a\x01\x06"+
            "\x03\uffff\x01\x0f\x02\uffff\x01\x03\x01\x04\x01\x14\x01\x12"+
            "\x01\uffff\x01\x13\x01\x17\x01\x15\x0a\x17\x01\uffff\x01\x05"+
            "\x01\uffff\x01\x11\x03\uffff\x1a\x16\x01\uffff\x01\x16\x01\uffff"+
            "\x01\x10\x01\x16\x01\uffff\x01\x0d\x01\x16\x01\x0b\x01\x0a\x07"+
            "\x16\x01\x08\x05\x16\x01\x09\x01\x07\x01\x0c\x06\x16\x01\x01"+
            "\x01\uffff\x01\x02\x01\x0e\x41\uffff\uff40\x16",
            "",
            "",
            "",
            "",
            "",
            "",
            "\x01\x1c\x07\uffff\x01\x1b",
            "\x01\x1d\x01\x1e",
            "\x01\x1f",
            "\x01\x20",
            "\x01\x21",
            "\x01\x22",
            "\x01\x26\x01\x24\x0f\uffff\x01\x25\x01\x23",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "",
            "\x01\x19",
            "\x02\x1a\x02\uffff\x01\x1a\x12\uffff\x01\x1a",
            "",
            "\x01\x28",
            "\x01\x29",
            "\x0a\x16\x07\uffff\x1a\x16\x01\uffff\x01\x16\x02\uffff\x01"+
            "\x16\x01\uffff\x1a\x16\x45\uffff\uff40\x16",
            "\x01\x2b",
            "\x01\x2c",
            "\x01\x2d",
            "\x01\x2e",
            "\x01\x2f",
            "\x01\x30",
            "\x01\x31",
            "\x01\x32",
            "\x01\x33",
            "",
            "\x01\x34",
            "\x0a\x16\x07\uffff\x1a\x16\x01\uffff\x01\x16\x02\uffff\x01"+
            "\x16\x01\uffff\x1a\x16\x45\uffff\uff40\x16",
            "",
            "\x0a\x16\x07\uffff\x1a\x16\x01\uffff\x01\x16\x02\uffff\x01"+
            "\x16\x01\uffff\x1a\x16\x45\uffff\uff40\x16",
            "\x0a\x16\x07\uffff\x1a\x16\x01\uffff\x01\x16\x02\uffff\x01"+
            "\x16\x01\uffff\x1a\x16\x45\uffff\uff40\x16",
            "\x0a\x16\x07\uffff\x1a\x16\x01\uffff\x01\x16\x02\uffff\x01"+
            "\x16\x01\uffff\x1a\x16\x45\uffff\uff40\x16",
            "\x0a\x16\x07\uffff\x1a\x16\x01\uffff\x01\x16\x02\uffff\x01"+
            "\x16\x01\uffff\x1a\x16\x45\uffff\uff40\x16",
            "\x0a\x16\x07\uffff\x1a\x16\x01\uffff\x01\x16\x02\uffff\x01"+
            "\x16\x01\uffff\x1a\x16\x45\uffff\uff40\x16",
            "\x01\x3b",
            "\x01\x3c",
            "\x01\x3d",
            "\x0a\x16\x07\uffff\x1a\x16\x01\uffff\x01\x16\x02\uffff\x01"+
            "\x16\x01\uffff\x1a\x16\x45\uffff\uff40\x16",
            "\x0a\x16\x07\uffff\x1a\x16\x01\uffff\x01\x16\x02\uffff\x01"+
            "\x16\x01\uffff\x1a\x16\x45\uffff\uff40\x16",
            "",
            "",
            "",
            "",
            "",
            "",
            "\x0a\x16\x07\uffff\x1a\x16\x01\uffff\x01\x16\x02\uffff\x01"+
            "\x16\x01\uffff\x1a\x16\x45\uffff\uff40\x16",
            "\x0a\x16\x07\uffff\x1a\x16\x01\uffff\x01\x16\x02\uffff\x01"+
            "\x16\x01\uffff\x1a\x16\x45\uffff\uff40\x16",
            "\x0a\x16\x07\uffff\x1a\x16\x01\uffff\x01\x16\x02\uffff\x01"+
            "\x16\x01\uffff\x1a\x16\x45\uffff\uff40\x16",
            "",
            "",
            "",
            "",
            ""
    };

    static readonly short[] DFA33_eot = DFA.UnpackEncodedString(DFA33_eotS);
    static readonly short[] DFA33_eof = DFA.UnpackEncodedString(DFA33_eofS);
    static readonly char[] DFA33_min = DFA.UnpackEncodedStringToUnsignedChars(DFA33_minS);
    static readonly char[] DFA33_max = DFA.UnpackEncodedStringToUnsignedChars(DFA33_maxS);
    static readonly short[] DFA33_accept = DFA.UnpackEncodedString(DFA33_acceptS);
    static readonly short[] DFA33_special = DFA.UnpackEncodedString(DFA33_specialS);
    static readonly short[][] DFA33_transition = DFA.UnpackEncodedStringArray(DFA33_transitionS);

    protected class DFA33 : DFA
    {
        public DFA33(BaseRecognizer recognizer)
        {
            this.recognizer = recognizer;
            this.decisionNumber = 33;
            this.eot = DFA33_eot;
            this.eof = DFA33_eof;
            this.min = DFA33_min;
            this.max = DFA33_max;
            this.accept = DFA33_accept;
            this.special = DFA33_special;
            this.transition = DFA33_transition;

        }

        override public string Description
        {
            get { return "1:1: Tokens : ( T__33 | T__34 | T__35 | T__36 | T__37 | EXCLAIMATION | SQRT | LN | LOG | RAD | DEG | SIN | COS | TAN | ATAN | ACOS | ASIN | ABS | NEGATE | MOD | POW | EQUALS | PLUS | MINUS | MULT | DIVIDE | ID | DOUBLE | NEWLINE | WS );"; }
        }

    }

 
    
}
}