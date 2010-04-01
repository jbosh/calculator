// $ANTLR 3.2 Sep 23, 2009 12:02:23 Calculator.g 2010-04-01 13:19:03

// The variable 'variable' is assigned but its value is never used.
#pragma warning disable 168, 219
// Unreachable code detected.
#pragma warning disable 162


using System;
using Antlr.Runtime;
using IList 		= System.Collections.IList;
using ArrayList 	= System.Collections.ArrayList;
using Stack 		= Antlr.Runtime.Collections.StackList;



using Antlr.Runtime.Tree;

namespace  Calculator.Grammar 
{
public partial class CalculatorParser : Parser
{
    public static readonly string[] tokenNames = new string[] 
	{
        "<invalid>", 
		"<EOR>", 
		"<DOWN>", 
		"<UP>", 
		"VECTOR", 
		"UNARY", 
		"NEGATION", 
		"FACTORIAL", 
		"ID", 
		"NEGATE", 
		"EXCLAIMATION", 
		"SIN", 
		"COS", 
		"TAN", 
		"ATAN", 
		"ACOS", 
		"ASIN", 
		"ABS", 
		"DEG", 
		"RAD", 
		"LN", 
		"LOG", 
		"SQRT", 
		"DOUBLE", 
		"MOD", 
		"POW", 
		"EQUALS", 
		"PLUS", 
		"MINUS", 
		"MULT", 
		"DIVIDE", 
		"NEWLINE", 
		"WS", 
		"'{'", 
		"'}'", 
		"'('", 
		"')'", 
		"';'"
    };

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
    public const int POW = 25;
    public const int T__34 = 34;
    public const int NEWLINE = 31;
    public const int T__35 = 35;
    public const int NEGATION = 6;
    public const int T__36 = 36;
    public const int VECTOR = 4;
    public const int SIN = 11;
    public const int T__37 = 37;
    public const int COS = 12;
    public const int TAN = 13;
    public const int RAD = 19;
    public const int DOUBLE = 23;
    public const int PLUS = 27;
    public const int ASIN = 16;
    public const int ATAN = 14;
    public const int UNARY = 5;
    public const int FACTORIAL = 7;
    public const int DIVIDE = 30;

    // delegates
    // delegators



        public CalculatorParser(ITokenStream input)
    		: this(input, new RecognizerSharedState()) {
        }

        public CalculatorParser(ITokenStream input, RecognizerSharedState state)
    		: base(input, state) {
            InitializeCyclicDFAs();

             
        }
        
    protected ITreeAdaptor adaptor = new CommonTreeAdaptor();

    public ITreeAdaptor TreeAdaptor
    {
        get { return this.adaptor; }
        set {
    	this.adaptor = value;
    	}
    }

    override public string[] TokenNames {
		get { return CalculatorParser.tokenNames; }
    }

    override public string GrammarFileName {
		get { return "Calculator.g"; }
    }


    public class root_return : ParserRuleReturnScope
    {
        private CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (CommonTree) value; }
        }
    };

    // $ANTLR start "root"
    // Calculator.g:21:1: root : stat ;
    public CalculatorParser.root_return root() // throws RecognitionException [1]
    {   
        CalculatorParser.root_return retval = new CalculatorParser.root_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        CalculatorParser.stat_return stat1 = default(CalculatorParser.stat_return);



        try 
    	{
            // Calculator.g:22:2: ( stat )
            // Calculator.g:22:4: stat
            {
            	root_0 = (CommonTree)adaptor.GetNilNode();

            	PushFollow(FOLLOW_stat_in_root77);
            	stat1 = stat();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, stat1.Tree);

            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (CommonTree)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "root"

    public class stat_return : ParserRuleReturnScope
    {
        private CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (CommonTree) value; }
        }
    };

    // $ANTLR start "stat"
    // Calculator.g:25:1: stat : ( expr -> expr | ID '=' expr -> ^( '=' ID expr ) );
    public CalculatorParser.stat_return stat() // throws RecognitionException [1]
    {   
        CalculatorParser.stat_return retval = new CalculatorParser.stat_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        IToken ID3 = null;
        IToken char_literal4 = null;
        CalculatorParser.expr_return expr2 = default(CalculatorParser.expr_return);

        CalculatorParser.expr_return expr5 = default(CalculatorParser.expr_return);


        CommonTree ID3_tree=null;
        CommonTree char_literal4_tree=null;
        RewriteRuleTokenStream stream_EQUALS = new RewriteRuleTokenStream(adaptor,"token EQUALS");
        RewriteRuleTokenStream stream_ID = new RewriteRuleTokenStream(adaptor,"token ID");
        RewriteRuleSubtreeStream stream_expr = new RewriteRuleSubtreeStream(adaptor,"rule expr");
        try 
    	{
            // Calculator.g:26:2: ( expr -> expr | ID '=' expr -> ^( '=' ID expr ) )
            int alt1 = 2;
            int LA1_0 = input.LA(1);

            if ( (LA1_0 == NEGATE || (LA1_0 >= SIN && LA1_0 <= DOUBLE) || LA1_0 == 33 || LA1_0 == 35) )
            {
                alt1 = 1;
            }
            else if ( (LA1_0 == ID) )
            {
                int LA1_2 = input.LA(2);

                if ( (LA1_2 == EQUALS) )
                {
                    alt1 = 2;
                }
                else if ( (LA1_2 == EOF || LA1_2 == EXCLAIMATION || (LA1_2 >= MOD && LA1_2 <= POW) || (LA1_2 >= PLUS && LA1_2 <= DIVIDE)) )
                {
                    alt1 = 1;
                }
                else 
                {
                    NoViableAltException nvae_d1s2 =
                        new NoViableAltException("", 1, 2, input);

                    throw nvae_d1s2;
                }
            }
            else 
            {
                NoViableAltException nvae_d1s0 =
                    new NoViableAltException("", 1, 0, input);

                throw nvae_d1s0;
            }
            switch (alt1) 
            {
                case 1 :
                    // Calculator.g:26:4: expr
                    {
                    	PushFollow(FOLLOW_expr_in_stat88);
                    	expr2 = expr();
                    	state.followingStackPointer--;

                    	stream_expr.Add(expr2.Tree);


                    	// AST REWRITE
                    	// elements:          expr
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 26:9: -> expr
                    	{
                    	    adaptor.AddChild(root_0, stream_expr.NextTree());

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;
                case 2 :
                    // Calculator.g:27:4: ID '=' expr
                    {
                    	ID3=(IToken)Match(input,ID,FOLLOW_ID_in_stat97);  
                    	stream_ID.Add(ID3);

                    	char_literal4=(IToken)Match(input,EQUALS,FOLLOW_EQUALS_in_stat99);  
                    	stream_EQUALS.Add(char_literal4);

                    	PushFollow(FOLLOW_expr_in_stat101);
                    	expr5 = expr();
                    	state.followingStackPointer--;

                    	stream_expr.Add(expr5.Tree);


                    	// AST REWRITE
                    	// elements:          EQUALS, expr, ID
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 27:16: -> ^( '=' ID expr )
                    	{
                    	    // Calculator.g:27:19: ^( '=' ID expr )
                    	    {
                    	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
                    	    root_1 = (CommonTree)adaptor.BecomeRoot(stream_EQUALS.NextNode(), root_1);

                    	    adaptor.AddChild(root_1, stream_ID.NextNode());
                    	    adaptor.AddChild(root_1, stream_expr.NextTree());

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (CommonTree)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "stat"

    public class expr_return : ParserRuleReturnScope
    {
        private CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (CommonTree) value; }
        }
    };

    // $ANTLR start "expr"
    // Calculator.g:30:1: expr : multExpr ( ( '+' | '-' ) multExpr )* ;
    public CalculatorParser.expr_return expr() // throws RecognitionException [1]
    {   
        CalculatorParser.expr_return retval = new CalculatorParser.expr_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        IToken char_literal7 = null;
        IToken char_literal8 = null;
        CalculatorParser.multExpr_return multExpr6 = default(CalculatorParser.multExpr_return);

        CalculatorParser.multExpr_return multExpr9 = default(CalculatorParser.multExpr_return);


        CommonTree char_literal7_tree=null;
        CommonTree char_literal8_tree=null;

        try 
    	{
            // Calculator.g:31:2: ( multExpr ( ( '+' | '-' ) multExpr )* )
            // Calculator.g:31:4: multExpr ( ( '+' | '-' ) multExpr )*
            {
            	root_0 = (CommonTree)adaptor.GetNilNode();

            	PushFollow(FOLLOW_multExpr_in_expr122);
            	multExpr6 = multExpr();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, multExpr6.Tree);
            	// Calculator.g:31:13: ( ( '+' | '-' ) multExpr )*
            	do 
            	{
            	    int alt3 = 2;
            	    int LA3_0 = input.LA(1);

            	    if ( ((LA3_0 >= PLUS && LA3_0 <= MINUS)) )
            	    {
            	        alt3 = 1;
            	    }


            	    switch (alt3) 
            		{
            			case 1 :
            			    // Calculator.g:31:14: ( '+' | '-' ) multExpr
            			    {
            			    	// Calculator.g:31:14: ( '+' | '-' )
            			    	int alt2 = 2;
            			    	int LA2_0 = input.LA(1);

            			    	if ( (LA2_0 == PLUS) )
            			    	{
            			    	    alt2 = 1;
            			    	}
            			    	else if ( (LA2_0 == MINUS) )
            			    	{
            			    	    alt2 = 2;
            			    	}
            			    	else 
            			    	{
            			    	    NoViableAltException nvae_d2s0 =
            			    	        new NoViableAltException("", 2, 0, input);

            			    	    throw nvae_d2s0;
            			    	}
            			    	switch (alt2) 
            			    	{
            			    	    case 1 :
            			    	        // Calculator.g:31:15: '+'
            			    	        {
            			    	        	char_literal7=(IToken)Match(input,PLUS,FOLLOW_PLUS_in_expr126); 
            			    	        		char_literal7_tree = (CommonTree)adaptor.Create(char_literal7);
            			    	        		root_0 = (CommonTree)adaptor.BecomeRoot(char_literal7_tree, root_0);


            			    	        }
            			    	        break;
            			    	    case 2 :
            			    	        // Calculator.g:31:20: '-'
            			    	        {
            			    	        	char_literal8=(IToken)Match(input,MINUS,FOLLOW_MINUS_in_expr129); 
            			    	        		char_literal8_tree = (CommonTree)adaptor.Create(char_literal8);
            			    	        		root_0 = (CommonTree)adaptor.BecomeRoot(char_literal8_tree, root_0);


            			    	        }
            			    	        break;

            			    	}

            			    	PushFollow(FOLLOW_multExpr_in_expr133);
            			    	multExpr9 = multExpr();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, multExpr9.Tree);

            			    }
            			    break;

            			default:
            			    goto loop3;
            	    }
            	} while (true);

            	loop3:
            		;	// Stops C# compiler whining that label 'loop3' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (CommonTree)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "expr"

    public class multExpr_return : ParserRuleReturnScope
    {
        private CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (CommonTree) value; }
        }
    };

    // $ANTLR start "multExpr"
    // Calculator.g:34:1: multExpr : powExpr ( ( '*' | '/' | '%' ) powExpr )* ;
    public CalculatorParser.multExpr_return multExpr() // throws RecognitionException [1]
    {   
        CalculatorParser.multExpr_return retval = new CalculatorParser.multExpr_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        IToken char_literal11 = null;
        IToken char_literal12 = null;
        IToken char_literal13 = null;
        CalculatorParser.powExpr_return powExpr10 = default(CalculatorParser.powExpr_return);

        CalculatorParser.powExpr_return powExpr14 = default(CalculatorParser.powExpr_return);


        CommonTree char_literal11_tree=null;
        CommonTree char_literal12_tree=null;
        CommonTree char_literal13_tree=null;

        try 
    	{
            // Calculator.g:35:2: ( powExpr ( ( '*' | '/' | '%' ) powExpr )* )
            // Calculator.g:35:4: powExpr ( ( '*' | '/' | '%' ) powExpr )*
            {
            	root_0 = (CommonTree)adaptor.GetNilNode();

            	PushFollow(FOLLOW_powExpr_in_multExpr146);
            	powExpr10 = powExpr();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, powExpr10.Tree);
            	// Calculator.g:35:12: ( ( '*' | '/' | '%' ) powExpr )*
            	do 
            	{
            	    int alt5 = 2;
            	    int LA5_0 = input.LA(1);

            	    if ( (LA5_0 == MOD || (LA5_0 >= MULT && LA5_0 <= DIVIDE)) )
            	    {
            	        alt5 = 1;
            	    }


            	    switch (alt5) 
            		{
            			case 1 :
            			    // Calculator.g:35:13: ( '*' | '/' | '%' ) powExpr
            			    {
            			    	// Calculator.g:35:13: ( '*' | '/' | '%' )
            			    	int alt4 = 3;
            			    	switch ( input.LA(1) ) 
            			    	{
            			    	case MULT:
            			    		{
            			    	    alt4 = 1;
            			    	    }
            			    	    break;
            			    	case DIVIDE:
            			    		{
            			    	    alt4 = 2;
            			    	    }
            			    	    break;
            			    	case MOD:
            			    		{
            			    	    alt4 = 3;
            			    	    }
            			    	    break;
            			    		default:
            			    		    NoViableAltException nvae_d4s0 =
            			    		        new NoViableAltException("", 4, 0, input);

            			    		    throw nvae_d4s0;
            			    	}

            			    	switch (alt4) 
            			    	{
            			    	    case 1 :
            			    	        // Calculator.g:35:14: '*'
            			    	        {
            			    	        	char_literal11=(IToken)Match(input,MULT,FOLLOW_MULT_in_multExpr150); 
            			    	        		char_literal11_tree = (CommonTree)adaptor.Create(char_literal11);
            			    	        		root_0 = (CommonTree)adaptor.BecomeRoot(char_literal11_tree, root_0);


            			    	        }
            			    	        break;
            			    	    case 2 :
            			    	        // Calculator.g:35:19: '/'
            			    	        {
            			    	        	char_literal12=(IToken)Match(input,DIVIDE,FOLLOW_DIVIDE_in_multExpr153); 
            			    	        		char_literal12_tree = (CommonTree)adaptor.Create(char_literal12);
            			    	        		root_0 = (CommonTree)adaptor.BecomeRoot(char_literal12_tree, root_0);


            			    	        }
            			    	        break;
            			    	    case 3 :
            			    	        // Calculator.g:35:24: '%'
            			    	        {
            			    	        	char_literal13=(IToken)Match(input,MOD,FOLLOW_MOD_in_multExpr156); 
            			    	        		char_literal13_tree = (CommonTree)adaptor.Create(char_literal13);
            			    	        		root_0 = (CommonTree)adaptor.BecomeRoot(char_literal13_tree, root_0);


            			    	        }
            			    	        break;

            			    	}

            			    	PushFollow(FOLLOW_powExpr_in_multExpr160);
            			    	powExpr14 = powExpr();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, powExpr14.Tree);

            			    }
            			    break;

            			default:
            			    goto loop5;
            	    }
            	} while (true);

            	loop5:
            		;	// Stops C# compiler whining that label 'loop5' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (CommonTree)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "multExpr"

    public class powExpr_return : ParserRuleReturnScope
    {
        private CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (CommonTree) value; }
        }
    };

    // $ANTLR start "powExpr"
    // Calculator.g:47:1: powExpr : unary ( ( '^' ) unary )* ;
    public CalculatorParser.powExpr_return powExpr() // throws RecognitionException [1]
    {   
        CalculatorParser.powExpr_return retval = new CalculatorParser.powExpr_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        IToken char_literal16 = null;
        CalculatorParser.unary_return unary15 = default(CalculatorParser.unary_return);

        CalculatorParser.unary_return unary17 = default(CalculatorParser.unary_return);


        CommonTree char_literal16_tree=null;

        try 
    	{
            // Calculator.g:48:2: ( unary ( ( '^' ) unary )* )
            // Calculator.g:48:4: unary ( ( '^' ) unary )*
            {
            	root_0 = (CommonTree)adaptor.GetNilNode();

            	PushFollow(FOLLOW_unary_in_powExpr184);
            	unary15 = unary();
            	state.followingStackPointer--;

            	adaptor.AddChild(root_0, unary15.Tree);
            	// Calculator.g:48:10: ( ( '^' ) unary )*
            	do 
            	{
            	    int alt6 = 2;
            	    int LA6_0 = input.LA(1);

            	    if ( (LA6_0 == POW) )
            	    {
            	        alt6 = 1;
            	    }


            	    switch (alt6) 
            		{
            			case 1 :
            			    // Calculator.g:48:11: ( '^' ) unary
            			    {
            			    	// Calculator.g:48:11: ( '^' )
            			    	// Calculator.g:48:12: '^'
            			    	{
            			    		char_literal16=(IToken)Match(input,POW,FOLLOW_POW_in_powExpr188); 
            			    			char_literal16_tree = (CommonTree)adaptor.Create(char_literal16);
            			    			root_0 = (CommonTree)adaptor.BecomeRoot(char_literal16_tree, root_0);


            			    	}

            			    	PushFollow(FOLLOW_unary_in_powExpr192);
            			    	unary17 = unary();
            			    	state.followingStackPointer--;

            			    	adaptor.AddChild(root_0, unary17.Tree);

            			    }
            			    break;

            			default:
            			    goto loop6;
            	    }
            	} while (true);

            	loop6:
            		;	// Stops C# compiler whining that label 'loop6' has no statements


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (CommonTree)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "powExpr"

    public class unary_return : ParserRuleReturnScope
    {
        private CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (CommonTree) value; }
        }
    };

    // $ANTLR start "unary"
    // Calculator.g:51:1: unary : ( ( NEGATE )+ factorial -> ^( NEGATION factorial ( NEGATE )+ ) | factorial );
    public CalculatorParser.unary_return unary() // throws RecognitionException [1]
    {   
        CalculatorParser.unary_return retval = new CalculatorParser.unary_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        IToken NEGATE18 = null;
        CalculatorParser.factorial_return factorial19 = default(CalculatorParser.factorial_return);

        CalculatorParser.factorial_return factorial20 = default(CalculatorParser.factorial_return);


        CommonTree NEGATE18_tree=null;
        RewriteRuleTokenStream stream_NEGATE = new RewriteRuleTokenStream(adaptor,"token NEGATE");
        RewriteRuleSubtreeStream stream_factorial = new RewriteRuleSubtreeStream(adaptor,"rule factorial");
        try 
    	{
            // Calculator.g:52:2: ( ( NEGATE )+ factorial -> ^( NEGATION factorial ( NEGATE )+ ) | factorial )
            int alt8 = 2;
            int LA8_0 = input.LA(1);

            if ( (LA8_0 == NEGATE) )
            {
                alt8 = 1;
            }
            else if ( (LA8_0 == ID || (LA8_0 >= SIN && LA8_0 <= DOUBLE) || LA8_0 == 33 || LA8_0 == 35) )
            {
                alt8 = 2;
            }
            else 
            {
                NoViableAltException nvae_d8s0 =
                    new NoViableAltException("", 8, 0, input);

                throw nvae_d8s0;
            }
            switch (alt8) 
            {
                case 1 :
                    // Calculator.g:52:4: ( NEGATE )+ factorial
                    {
                    	// Calculator.g:52:4: ( NEGATE )+
                    	int cnt7 = 0;
                    	do 
                    	{
                    	    int alt7 = 2;
                    	    int LA7_0 = input.LA(1);

                    	    if ( (LA7_0 == NEGATE) )
                    	    {
                    	        alt7 = 1;
                    	    }


                    	    switch (alt7) 
                    		{
                    			case 1 :
                    			    // Calculator.g:52:4: NEGATE
                    			    {
                    			    	NEGATE18=(IToken)Match(input,NEGATE,FOLLOW_NEGATE_in_unary206);  
                    			    	stream_NEGATE.Add(NEGATE18);


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

                    	PushFollow(FOLLOW_factorial_in_unary209);
                    	factorial19 = factorial();
                    	state.followingStackPointer--;

                    	stream_factorial.Add(factorial19.Tree);


                    	// AST REWRITE
                    	// elements:          NEGATE, factorial
                    	// token labels:      
                    	// rule labels:       retval
                    	// token list labels: 
                    	// rule list labels:  
                    	// wildcard labels: 
                    	retval.Tree = root_0;
                    	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

                    	root_0 = (CommonTree)adaptor.GetNilNode();
                    	// 52:22: -> ^( NEGATION factorial ( NEGATE )+ )
                    	{
                    	    // Calculator.g:52:25: ^( NEGATION factorial ( NEGATE )+ )
                    	    {
                    	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
                    	    root_1 = (CommonTree)adaptor.BecomeRoot((CommonTree)adaptor.Create(NEGATION, "NEGATION"), root_1);

                    	    adaptor.AddChild(root_1, stream_factorial.NextTree());
                    	    if ( !(stream_NEGATE.HasNext()) ) {
                    	        throw new RewriteEarlyExitException();
                    	    }
                    	    while ( stream_NEGATE.HasNext() )
                    	    {
                    	        adaptor.AddChild(root_1, stream_NEGATE.NextNode());

                    	    }
                    	    stream_NEGATE.Reset();

                    	    adaptor.AddChild(root_0, root_1);
                    	    }

                    	}

                    	retval.Tree = root_0;retval.Tree = root_0;
                    }
                    break;
                case 2 :
                    // Calculator.g:53:4: factorial
                    {
                    	root_0 = (CommonTree)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_factorial_in_unary225);
                    	factorial20 = factorial();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, factorial20.Tree);

                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (CommonTree)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "unary"

    public class factorial_return : ParserRuleReturnScope
    {
        private CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (CommonTree) value; }
        }
    };

    // $ANTLR start "factorial"
    // Calculator.g:56:1: factorial : func ( ( EXCLAIMATION )+ -> ^( FACTORIAL func ( EXCLAIMATION )+ ) | -> func ) ;
    public CalculatorParser.factorial_return factorial() // throws RecognitionException [1]
    {   
        CalculatorParser.factorial_return retval = new CalculatorParser.factorial_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        IToken EXCLAIMATION22 = null;
        CalculatorParser.func_return func21 = default(CalculatorParser.func_return);


        CommonTree EXCLAIMATION22_tree=null;
        RewriteRuleTokenStream stream_EXCLAIMATION = new RewriteRuleTokenStream(adaptor,"token EXCLAIMATION");
        RewriteRuleSubtreeStream stream_func = new RewriteRuleSubtreeStream(adaptor,"rule func");
        try 
    	{
            // Calculator.g:57:2: ( func ( ( EXCLAIMATION )+ -> ^( FACTORIAL func ( EXCLAIMATION )+ ) | -> func ) )
            // Calculator.g:57:4: func ( ( EXCLAIMATION )+ -> ^( FACTORIAL func ( EXCLAIMATION )+ ) | -> func )
            {
            	PushFollow(FOLLOW_func_in_factorial237);
            	func21 = func();
            	state.followingStackPointer--;

            	stream_func.Add(func21.Tree);
            	// Calculator.g:58:3: ( ( EXCLAIMATION )+ -> ^( FACTORIAL func ( EXCLAIMATION )+ ) | -> func )
            	int alt10 = 2;
            	int LA10_0 = input.LA(1);

            	if ( (LA10_0 == EXCLAIMATION) )
            	{
            	    alt10 = 1;
            	}
            	else if ( (LA10_0 == EOF || (LA10_0 >= MOD && LA10_0 <= POW) || (LA10_0 >= PLUS && LA10_0 <= DIVIDE) || LA10_0 == 34 || (LA10_0 >= 36 && LA10_0 <= 37)) )
            	{
            	    alt10 = 2;
            	}
            	else 
            	{
            	    NoViableAltException nvae_d10s0 =
            	        new NoViableAltException("", 10, 0, input);

            	    throw nvae_d10s0;
            	}
            	switch (alt10) 
            	{
            	    case 1 :
            	        // Calculator.g:58:4: ( EXCLAIMATION )+
            	        {
            	        	// Calculator.g:58:4: ( EXCLAIMATION )+
            	        	int cnt9 = 0;
            	        	do 
            	        	{
            	        	    int alt9 = 2;
            	        	    int LA9_0 = input.LA(1);

            	        	    if ( (LA9_0 == EXCLAIMATION) )
            	        	    {
            	        	        alt9 = 1;
            	        	    }


            	        	    switch (alt9) 
            	        		{
            	        			case 1 :
            	        			    // Calculator.g:58:4: EXCLAIMATION
            	        			    {
            	        			    	EXCLAIMATION22=(IToken)Match(input,EXCLAIMATION,FOLLOW_EXCLAIMATION_in_factorial242);  
            	        			    	stream_EXCLAIMATION.Add(EXCLAIMATION22);


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



            	        	// AST REWRITE
            	        	// elements:          EXCLAIMATION, func
            	        	// token labels:      
            	        	// rule labels:       retval
            	        	// token list labels: 
            	        	// rule list labels:  
            	        	// wildcard labels: 
            	        	retval.Tree = root_0;
            	        	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	        	root_0 = (CommonTree)adaptor.GetNilNode();
            	        	// 58:18: -> ^( FACTORIAL func ( EXCLAIMATION )+ )
            	        	{
            	        	    // Calculator.g:58:21: ^( FACTORIAL func ( EXCLAIMATION )+ )
            	        	    {
            	        	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
            	        	    root_1 = (CommonTree)adaptor.BecomeRoot((CommonTree)adaptor.Create(FACTORIAL, "FACTORIAL"), root_1);

            	        	    adaptor.AddChild(root_1, stream_func.NextTree());
            	        	    if ( !(stream_EXCLAIMATION.HasNext()) ) {
            	        	        throw new RewriteEarlyExitException();
            	        	    }
            	        	    while ( stream_EXCLAIMATION.HasNext() )
            	        	    {
            	        	        adaptor.AddChild(root_1, stream_EXCLAIMATION.NextNode());

            	        	    }
            	        	    stream_EXCLAIMATION.Reset();

            	        	    adaptor.AddChild(root_0, root_1);
            	        	    }

            	        	}

            	        	retval.Tree = root_0;retval.Tree = root_0;
            	        }
            	        break;
            	    case 2 :
            	        // Calculator.g:59:5: 
            	        {

            	        	// AST REWRITE
            	        	// elements:          func
            	        	// token labels:      
            	        	// rule labels:       retval
            	        	// token list labels: 
            	        	// rule list labels:  
            	        	// wildcard labels: 
            	        	retval.Tree = root_0;
            	        	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	        	root_0 = (CommonTree)adaptor.GetNilNode();
            	        	// 59:5: -> func
            	        	{
            	        	    adaptor.AddChild(root_0, stream_func.NextTree());

            	        	}

            	        	retval.Tree = root_0;retval.Tree = root_0;
            	        }
            	        break;

            	}


            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (CommonTree)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "factorial"

    public class func_return : ParserRuleReturnScope
    {
        private CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (CommonTree) value; }
        }
    };

    // $ANTLR start "func"
    // Calculator.g:63:1: func : ( ( SIN | COS | TAN | ATAN | ACOS | ASIN | ABS | DEG | RAD | LN | LOG | SQRT ) func | atom );
    public CalculatorParser.func_return func() // throws RecognitionException [1]
    {   
        CalculatorParser.func_return retval = new CalculatorParser.func_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        IToken SIN23 = null;
        IToken COS24 = null;
        IToken TAN25 = null;
        IToken ATAN26 = null;
        IToken ACOS27 = null;
        IToken ASIN28 = null;
        IToken ABS29 = null;
        IToken DEG30 = null;
        IToken RAD31 = null;
        IToken LN32 = null;
        IToken LOG33 = null;
        IToken SQRT34 = null;
        CalculatorParser.func_return func35 = default(CalculatorParser.func_return);

        CalculatorParser.atom_return atom36 = default(CalculatorParser.atom_return);


        CommonTree SIN23_tree=null;
        CommonTree COS24_tree=null;
        CommonTree TAN25_tree=null;
        CommonTree ATAN26_tree=null;
        CommonTree ACOS27_tree=null;
        CommonTree ASIN28_tree=null;
        CommonTree ABS29_tree=null;
        CommonTree DEG30_tree=null;
        CommonTree RAD31_tree=null;
        CommonTree LN32_tree=null;
        CommonTree LOG33_tree=null;
        CommonTree SQRT34_tree=null;

        try 
    	{
            // Calculator.g:64:2: ( ( SIN | COS | TAN | ATAN | ACOS | ASIN | ABS | DEG | RAD | LN | LOG | SQRT ) func | atom )
            int alt12 = 2;
            int LA12_0 = input.LA(1);

            if ( ((LA12_0 >= SIN && LA12_0 <= SQRT)) )
            {
                alt12 = 1;
            }
            else if ( (LA12_0 == ID || LA12_0 == DOUBLE || LA12_0 == 33 || LA12_0 == 35) )
            {
                alt12 = 2;
            }
            else 
            {
                NoViableAltException nvae_d12s0 =
                    new NoViableAltException("", 12, 0, input);

                throw nvae_d12s0;
            }
            switch (alt12) 
            {
                case 1 :
                    // Calculator.g:64:4: ( SIN | COS | TAN | ATAN | ACOS | ASIN | ABS | DEG | RAD | LN | LOG | SQRT ) func
                    {
                    	root_0 = (CommonTree)adaptor.GetNilNode();

                    	// Calculator.g:64:4: ( SIN | COS | TAN | ATAN | ACOS | ASIN | ABS | DEG | RAD | LN | LOG | SQRT )
                    	int alt11 = 12;
                    	switch ( input.LA(1) ) 
                    	{
                    	case SIN:
                    		{
                    	    alt11 = 1;
                    	    }
                    	    break;
                    	case COS:
                    		{
                    	    alt11 = 2;
                    	    }
                    	    break;
                    	case TAN:
                    		{
                    	    alt11 = 3;
                    	    }
                    	    break;
                    	case ATAN:
                    		{
                    	    alt11 = 4;
                    	    }
                    	    break;
                    	case ACOS:
                    		{
                    	    alt11 = 5;
                    	    }
                    	    break;
                    	case ASIN:
                    		{
                    	    alt11 = 6;
                    	    }
                    	    break;
                    	case ABS:
                    		{
                    	    alt11 = 7;
                    	    }
                    	    break;
                    	case DEG:
                    		{
                    	    alt11 = 8;
                    	    }
                    	    break;
                    	case RAD:
                    		{
                    	    alt11 = 9;
                    	    }
                    	    break;
                    	case LN:
                    		{
                    	    alt11 = 10;
                    	    }
                    	    break;
                    	case LOG:
                    		{
                    	    alt11 = 11;
                    	    }
                    	    break;
                    	case SQRT:
                    		{
                    	    alt11 = 12;
                    	    }
                    	    break;
                    		default:
                    		    NoViableAltException nvae_d11s0 =
                    		        new NoViableAltException("", 11, 0, input);

                    		    throw nvae_d11s0;
                    	}

                    	switch (alt11) 
                    	{
                    	    case 1 :
                    	        // Calculator.g:64:5: SIN
                    	        {
                    	        	SIN23=(IToken)Match(input,SIN,FOLLOW_SIN_in_func279); 
                    	        		SIN23_tree = (CommonTree)adaptor.Create(SIN23);
                    	        		root_0 = (CommonTree)adaptor.BecomeRoot(SIN23_tree, root_0);


                    	        }
                    	        break;
                    	    case 2 :
                    	        // Calculator.g:64:12: COS
                    	        {
                    	        	COS24=(IToken)Match(input,COS,FOLLOW_COS_in_func284); 
                    	        		COS24_tree = (CommonTree)adaptor.Create(COS24);
                    	        		root_0 = (CommonTree)adaptor.BecomeRoot(COS24_tree, root_0);


                    	        }
                    	        break;
                    	    case 3 :
                    	        // Calculator.g:64:19: TAN
                    	        {
                    	        	TAN25=(IToken)Match(input,TAN,FOLLOW_TAN_in_func289); 
                    	        		TAN25_tree = (CommonTree)adaptor.Create(TAN25);
                    	        		root_0 = (CommonTree)adaptor.BecomeRoot(TAN25_tree, root_0);


                    	        }
                    	        break;
                    	    case 4 :
                    	        // Calculator.g:64:26: ATAN
                    	        {
                    	        	ATAN26=(IToken)Match(input,ATAN,FOLLOW_ATAN_in_func294); 
                    	        		ATAN26_tree = (CommonTree)adaptor.Create(ATAN26);
                    	        		root_0 = (CommonTree)adaptor.BecomeRoot(ATAN26_tree, root_0);


                    	        }
                    	        break;
                    	    case 5 :
                    	        // Calculator.g:64:34: ACOS
                    	        {
                    	        	ACOS27=(IToken)Match(input,ACOS,FOLLOW_ACOS_in_func299); 
                    	        		ACOS27_tree = (CommonTree)adaptor.Create(ACOS27);
                    	        		root_0 = (CommonTree)adaptor.BecomeRoot(ACOS27_tree, root_0);


                    	        }
                    	        break;
                    	    case 6 :
                    	        // Calculator.g:64:42: ASIN
                    	        {
                    	        	ASIN28=(IToken)Match(input,ASIN,FOLLOW_ASIN_in_func304); 
                    	        		ASIN28_tree = (CommonTree)adaptor.Create(ASIN28);
                    	        		root_0 = (CommonTree)adaptor.BecomeRoot(ASIN28_tree, root_0);


                    	        }
                    	        break;
                    	    case 7 :
                    	        // Calculator.g:64:50: ABS
                    	        {
                    	        	ABS29=(IToken)Match(input,ABS,FOLLOW_ABS_in_func309); 
                    	        		ABS29_tree = (CommonTree)adaptor.Create(ABS29);
                    	        		root_0 = (CommonTree)adaptor.BecomeRoot(ABS29_tree, root_0);


                    	        }
                    	        break;
                    	    case 8 :
                    	        // Calculator.g:64:57: DEG
                    	        {
                    	        	DEG30=(IToken)Match(input,DEG,FOLLOW_DEG_in_func314); 
                    	        		DEG30_tree = (CommonTree)adaptor.Create(DEG30);
                    	        		root_0 = (CommonTree)adaptor.BecomeRoot(DEG30_tree, root_0);


                    	        }
                    	        break;
                    	    case 9 :
                    	        // Calculator.g:64:64: RAD
                    	        {
                    	        	RAD31=(IToken)Match(input,RAD,FOLLOW_RAD_in_func319); 
                    	        		RAD31_tree = (CommonTree)adaptor.Create(RAD31);
                    	        		root_0 = (CommonTree)adaptor.BecomeRoot(RAD31_tree, root_0);


                    	        }
                    	        break;
                    	    case 10 :
                    	        // Calculator.g:64:71: LN
                    	        {
                    	        	LN32=(IToken)Match(input,LN,FOLLOW_LN_in_func324); 
                    	        		LN32_tree = (CommonTree)adaptor.Create(LN32);
                    	        		root_0 = (CommonTree)adaptor.BecomeRoot(LN32_tree, root_0);


                    	        }
                    	        break;
                    	    case 11 :
                    	        // Calculator.g:64:77: LOG
                    	        {
                    	        	LOG33=(IToken)Match(input,LOG,FOLLOW_LOG_in_func329); 
                    	        		LOG33_tree = (CommonTree)adaptor.Create(LOG33);
                    	        		root_0 = (CommonTree)adaptor.BecomeRoot(LOG33_tree, root_0);


                    	        }
                    	        break;
                    	    case 12 :
                    	        // Calculator.g:64:84: SQRT
                    	        {
                    	        	SQRT34=(IToken)Match(input,SQRT,FOLLOW_SQRT_in_func334); 
                    	        		SQRT34_tree = (CommonTree)adaptor.Create(SQRT34);
                    	        		root_0 = (CommonTree)adaptor.BecomeRoot(SQRT34_tree, root_0);


                    	        }
                    	        break;

                    	}

                    	PushFollow(FOLLOW_func_in_func338);
                    	func35 = func();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, func35.Tree);

                    }
                    break;
                case 2 :
                    // Calculator.g:65:4: atom
                    {
                    	root_0 = (CommonTree)adaptor.GetNilNode();

                    	PushFollow(FOLLOW_atom_in_func343);
                    	atom36 = atom();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, atom36.Tree);

                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (CommonTree)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "func"

    public class atom_return : ParserRuleReturnScope
    {
        private CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (CommonTree) value; }
        }
    };

    // $ANTLR start "atom"
    // Calculator.g:70:1: atom : ( DOUBLE | ID | '{' vector '}' | '(' expr ')' );
    public CalculatorParser.atom_return atom() // throws RecognitionException [1]
    {   
        CalculatorParser.atom_return retval = new CalculatorParser.atom_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        IToken DOUBLE37 = null;
        IToken ID38 = null;
        IToken char_literal39 = null;
        IToken char_literal41 = null;
        IToken char_literal42 = null;
        IToken char_literal44 = null;
        CalculatorParser.vector_return vector40 = default(CalculatorParser.vector_return);

        CalculatorParser.expr_return expr43 = default(CalculatorParser.expr_return);


        CommonTree DOUBLE37_tree=null;
        CommonTree ID38_tree=null;
        CommonTree char_literal39_tree=null;
        CommonTree char_literal41_tree=null;
        CommonTree char_literal42_tree=null;
        CommonTree char_literal44_tree=null;

        try 
    	{
            // Calculator.g:71:2: ( DOUBLE | ID | '{' vector '}' | '(' expr ')' )
            int alt13 = 4;
            switch ( input.LA(1) ) 
            {
            case DOUBLE:
            	{
                alt13 = 1;
                }
                break;
            case ID:
            	{
                alt13 = 2;
                }
                break;
            case 33:
            	{
                alt13 = 3;
                }
                break;
            case 35:
            	{
                alt13 = 4;
                }
                break;
            	default:
            	    NoViableAltException nvae_d13s0 =
            	        new NoViableAltException("", 13, 0, input);

            	    throw nvae_d13s0;
            }

            switch (alt13) 
            {
                case 1 :
                    // Calculator.g:71:4: DOUBLE
                    {
                    	root_0 = (CommonTree)adaptor.GetNilNode();

                    	DOUBLE37=(IToken)Match(input,DOUBLE,FOLLOW_DOUBLE_in_atom358); 
                    		DOUBLE37_tree = (CommonTree)adaptor.Create(DOUBLE37);
                    		adaptor.AddChild(root_0, DOUBLE37_tree);


                    }
                    break;
                case 2 :
                    // Calculator.g:72:4: ID
                    {
                    	root_0 = (CommonTree)adaptor.GetNilNode();

                    	ID38=(IToken)Match(input,ID,FOLLOW_ID_in_atom363); 
                    		ID38_tree = (CommonTree)adaptor.Create(ID38);
                    		adaptor.AddChild(root_0, ID38_tree);


                    }
                    break;
                case 3 :
                    // Calculator.g:73:4: '{' vector '}'
                    {
                    	root_0 = (CommonTree)adaptor.GetNilNode();

                    	char_literal39=(IToken)Match(input,33,FOLLOW_33_in_atom368); 
                    	PushFollow(FOLLOW_vector_in_atom371);
                    	vector40 = vector();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, vector40.Tree);
                    	char_literal41=(IToken)Match(input,34,FOLLOW_34_in_atom373); 

                    }
                    break;
                case 4 :
                    // Calculator.g:74:4: '(' expr ')'
                    {
                    	root_0 = (CommonTree)adaptor.GetNilNode();

                    	char_literal42=(IToken)Match(input,35,FOLLOW_35_in_atom379); 
                    	PushFollow(FOLLOW_expr_in_atom382);
                    	expr43 = expr();
                    	state.followingStackPointer--;

                    	adaptor.AddChild(root_0, expr43.Tree);
                    	char_literal44=(IToken)Match(input,36,FOLLOW_36_in_atom384); 

                    }
                    break;

            }
            retval.Stop = input.LT(-1);

            	retval.Tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (CommonTree)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "atom"

    public class vector_return : ParserRuleReturnScope
    {
        private CommonTree tree;
        override public object Tree
        {
        	get { return tree; }
        	set { tree = (CommonTree) value; }
        }
    };

    // $ANTLR start "vector"
    // Calculator.g:77:1: vector : expr ( ';' expr )* -> ^( VECTOR ( expr )+ ) ;
    public CalculatorParser.vector_return vector() // throws RecognitionException [1]
    {   
        CalculatorParser.vector_return retval = new CalculatorParser.vector_return();
        retval.Start = input.LT(1);

        CommonTree root_0 = null;

        IToken char_literal46 = null;
        CalculatorParser.expr_return expr45 = default(CalculatorParser.expr_return);

        CalculatorParser.expr_return expr47 = default(CalculatorParser.expr_return);


        CommonTree char_literal46_tree=null;
        RewriteRuleTokenStream stream_37 = new RewriteRuleTokenStream(adaptor,"token 37");
        RewriteRuleSubtreeStream stream_expr = new RewriteRuleSubtreeStream(adaptor,"rule expr");
        try 
    	{
            // Calculator.g:78:2: ( expr ( ';' expr )* -> ^( VECTOR ( expr )+ ) )
            // Calculator.g:78:4: expr ( ';' expr )*
            {
            	PushFollow(FOLLOW_expr_in_vector397);
            	expr45 = expr();
            	state.followingStackPointer--;

            	stream_expr.Add(expr45.Tree);
            	// Calculator.g:78:9: ( ';' expr )*
            	do 
            	{
            	    int alt14 = 2;
            	    int LA14_0 = input.LA(1);

            	    if ( (LA14_0 == 37) )
            	    {
            	        alt14 = 1;
            	    }


            	    switch (alt14) 
            		{
            			case 1 :
            			    // Calculator.g:78:10: ';' expr
            			    {
            			    	char_literal46=(IToken)Match(input,37,FOLLOW_37_in_vector400);  
            			    	stream_37.Add(char_literal46);

            			    	PushFollow(FOLLOW_expr_in_vector402);
            			    	expr47 = expr();
            			    	state.followingStackPointer--;

            			    	stream_expr.Add(expr47.Tree);

            			    }
            			    break;

            			default:
            			    goto loop14;
            	    }
            	} while (true);

            	loop14:
            		;	// Stops C# compiler whining that label 'loop14' has no statements



            	// AST REWRITE
            	// elements:          expr
            	// token labels:      
            	// rule labels:       retval
            	// token list labels: 
            	// rule list labels:  
            	// wildcard labels: 
            	retval.Tree = root_0;
            	RewriteRuleSubtreeStream stream_retval = new RewriteRuleSubtreeStream(adaptor, "rule retval", retval!=null ? retval.Tree : null);

            	root_0 = (CommonTree)adaptor.GetNilNode();
            	// 78:21: -> ^( VECTOR ( expr )+ )
            	{
            	    // Calculator.g:78:24: ^( VECTOR ( expr )+ )
            	    {
            	    CommonTree root_1 = (CommonTree)adaptor.GetNilNode();
            	    root_1 = (CommonTree)adaptor.BecomeRoot((CommonTree)adaptor.Create(VECTOR, "VECTOR"), root_1);

            	    if ( !(stream_expr.HasNext()) ) {
            	        throw new RewriteEarlyExitException();
            	    }
            	    while ( stream_expr.HasNext() )
            	    {
            	        adaptor.AddChild(root_1, stream_expr.NextTree());

            	    }
            	    stream_expr.Reset();

            	    adaptor.AddChild(root_0, root_1);
            	    }

            	}

            	retval.Tree = root_0;retval.Tree = root_0;
            }

            retval.Stop = input.LT(-1);

            	retval.Tree = (CommonTree)adaptor.RulePostProcessing(root_0);
            	adaptor.SetTokenBoundaries(retval.Tree, (IToken) retval.Start, (IToken) retval.Stop);
        }
        catch (RecognitionException re) 
    	{
            ReportError(re);
            Recover(input,re);
    	// Conversion of the second argument necessary, but harmless
    	retval.Tree = (CommonTree)adaptor.ErrorNode(input, (IToken) retval.Start, input.LT(-1), re);

        }
        finally 
    	{
        }
        return retval;
    }
    // $ANTLR end "vector"

    // Delegated rules


	private void InitializeCyclicDFAs()
	{
	}

 

    public static readonly BitSet FOLLOW_stat_in_root77 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_expr_in_stat88 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ID_in_stat97 = new BitSet(new ulong[]{0x0000000004000000UL});
    public static readonly BitSet FOLLOW_EQUALS_in_stat99 = new BitSet(new ulong[]{0x0000000A00FFFB00UL});
    public static readonly BitSet FOLLOW_expr_in_stat101 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_multExpr_in_expr122 = new BitSet(new ulong[]{0x0000000018000002UL});
    public static readonly BitSet FOLLOW_PLUS_in_expr126 = new BitSet(new ulong[]{0x0000000A00FFFB00UL});
    public static readonly BitSet FOLLOW_MINUS_in_expr129 = new BitSet(new ulong[]{0x0000000A00FFFB00UL});
    public static readonly BitSet FOLLOW_multExpr_in_expr133 = new BitSet(new ulong[]{0x0000000018000002UL});
    public static readonly BitSet FOLLOW_powExpr_in_multExpr146 = new BitSet(new ulong[]{0x0000000061000002UL});
    public static readonly BitSet FOLLOW_MULT_in_multExpr150 = new BitSet(new ulong[]{0x0000000A00FFFB00UL});
    public static readonly BitSet FOLLOW_DIVIDE_in_multExpr153 = new BitSet(new ulong[]{0x0000000A00FFFB00UL});
    public static readonly BitSet FOLLOW_MOD_in_multExpr156 = new BitSet(new ulong[]{0x0000000A00FFFB00UL});
    public static readonly BitSet FOLLOW_powExpr_in_multExpr160 = new BitSet(new ulong[]{0x0000000061000002UL});
    public static readonly BitSet FOLLOW_unary_in_powExpr184 = new BitSet(new ulong[]{0x0000000002000002UL});
    public static readonly BitSet FOLLOW_POW_in_powExpr188 = new BitSet(new ulong[]{0x0000000A00FFFB00UL});
    public static readonly BitSet FOLLOW_unary_in_powExpr192 = new BitSet(new ulong[]{0x0000000002000002UL});
    public static readonly BitSet FOLLOW_NEGATE_in_unary206 = new BitSet(new ulong[]{0x0000000A00FFFB00UL});
    public static readonly BitSet FOLLOW_factorial_in_unary209 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_factorial_in_unary225 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_func_in_factorial237 = new BitSet(new ulong[]{0x0000000000000402UL});
    public static readonly BitSet FOLLOW_EXCLAIMATION_in_factorial242 = new BitSet(new ulong[]{0x0000000000000402UL});
    public static readonly BitSet FOLLOW_SIN_in_func279 = new BitSet(new ulong[]{0x0000000A00FFFB00UL});
    public static readonly BitSet FOLLOW_COS_in_func284 = new BitSet(new ulong[]{0x0000000A00FFFB00UL});
    public static readonly BitSet FOLLOW_TAN_in_func289 = new BitSet(new ulong[]{0x0000000A00FFFB00UL});
    public static readonly BitSet FOLLOW_ATAN_in_func294 = new BitSet(new ulong[]{0x0000000A00FFFB00UL});
    public static readonly BitSet FOLLOW_ACOS_in_func299 = new BitSet(new ulong[]{0x0000000A00FFFB00UL});
    public static readonly BitSet FOLLOW_ASIN_in_func304 = new BitSet(new ulong[]{0x0000000A00FFFB00UL});
    public static readonly BitSet FOLLOW_ABS_in_func309 = new BitSet(new ulong[]{0x0000000A00FFFB00UL});
    public static readonly BitSet FOLLOW_DEG_in_func314 = new BitSet(new ulong[]{0x0000000A00FFFB00UL});
    public static readonly BitSet FOLLOW_RAD_in_func319 = new BitSet(new ulong[]{0x0000000A00FFFB00UL});
    public static readonly BitSet FOLLOW_LN_in_func324 = new BitSet(new ulong[]{0x0000000A00FFFB00UL});
    public static readonly BitSet FOLLOW_LOG_in_func329 = new BitSet(new ulong[]{0x0000000A00FFFB00UL});
    public static readonly BitSet FOLLOW_SQRT_in_func334 = new BitSet(new ulong[]{0x0000000A00FFFB00UL});
    public static readonly BitSet FOLLOW_func_in_func338 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_atom_in_func343 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_DOUBLE_in_atom358 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_ID_in_atom363 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_33_in_atom368 = new BitSet(new ulong[]{0x0000000A00FFFB00UL});
    public static readonly BitSet FOLLOW_vector_in_atom371 = new BitSet(new ulong[]{0x0000000400000000UL});
    public static readonly BitSet FOLLOW_34_in_atom373 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_35_in_atom379 = new BitSet(new ulong[]{0x0000000A00FFFB00UL});
    public static readonly BitSet FOLLOW_expr_in_atom382 = new BitSet(new ulong[]{0x0000001000000000UL});
    public static readonly BitSet FOLLOW_36_in_atom384 = new BitSet(new ulong[]{0x0000000000000002UL});
    public static readonly BitSet FOLLOW_expr_in_vector397 = new BitSet(new ulong[]{0x0000002000000002UL});
    public static readonly BitSet FOLLOW_37_in_vector400 = new BitSet(new ulong[]{0x0000000A00FFFB00UL});
    public static readonly BitSet FOLLOW_expr_in_vector402 = new BitSet(new ulong[]{0x0000002000000002UL});

}
}