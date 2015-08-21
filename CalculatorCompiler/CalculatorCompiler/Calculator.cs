////////////////////////////////////////////////////////////////////////////////////////
//
// Application
//      Four function calculator
//      Example showing recursive-descent parser in C# for Compiler Construction course
//
// Author
//      Neil Johnson, Cambridge University Computer Laboratory, 2004
//
// Build
//      With Rotor/SSCLI command line tools:
//
//          csc 4calc.cs
//          clix 4calc.exe 
//
////////////////////////////////////////////////////////////////////////////////////////

using System;

//////////////////////////////////////////////////////////////////////////////////////////
//
//   Tokens representing lexical items
//
//////////////////////////////////////////////////////////////////////////////////////////

public enum Token
{
    NO_TOK,
    EOF,
    EOL,
    PLUS,
    MINUS,
    TIMES,
    DIVIDE,
    LPAREN,
    RPAREN,
    NUMBER
}

/////////////////////////////////////////////////////////////////////////////////////////
//
// Class
//      Lexer
//
// Implements
//      Lexical analyser for 4-function calculator
//
// Notes
//      Simple state machine consuming chars from Console, returning tokens when the
//      largest one matches (esp. in the case of numbers).
//
////////////////////////////////////////////////////////////////////////////////////////

public class Lexer
{
    int lastchar;
    UInt32 number;
    Token token;

    ////////////////////////////////////////////////////////////////////////////////////
    //
    // Method
    //      Lexer (constructor)
    //
    // Implements
    //      Initialises a lexer object, clearing number and setting the first character
    //      to a space (will be consumed later).
    //
    public Lexer()
    {
        lastchar = ' ';
        number = 0;
        token = Token.NO_TOK;
    }

    ////////////////////////////////////////////////////////////////////////////////////
    //
    // Method
    //      lex
    //
    // Implements
    //      The lexer itself.  Each call consumes characters from Console until a match
    //      the largest lexical item is found.  For most items this will be a single 
    //      character, but for numbers we keep consuming chars until either the next
    //      char is not a number (in which case we stop munching and return a NUMBER
    //      token), or the number that would be produced is just far too large to fit
    //      into the integer type we're using for numbers.
    //      Note: a future implementation might want to handle this second case a bit
    //      more elegantly than just maxing out.
    //
    public Token lex()
    {
        int nextchar;

        token = Token.NO_TOK;
        number = 0;

        while (token == Token.NO_TOK)
        {
            nextchar = Console.Read();

            switch (lastchar)
            {
                // Catch end of input
                case -1: token = Token.EOF; break;

                // Operators and other tokens
                case '+': token = Token.PLUS; break;
                case '-': token = Token.MINUS; break;
                case '*': token = Token.TIMES; break;
                case '/': token = Token.DIVIDE; break;
                case '(': token = Token.LPAREN; break;
                case ')': token = Token.RPAREN; break;
                case ';': token = Token.EOL; break;

                // Numbers.  Check for maxmimum
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                    if (number <= (UInt32.MaxValue / 10))
                        number = number * 10 + (UInt32)(((UInt32)lastchar) - '0');
                    else
                        number = UInt32.MaxValue;

                    if (nextchar < '0' || nextchar > '9')
                        token = Token.NUMBER;
                    break;

                // Consume everything else
                default: break;
            }

            lastchar = nextchar;
        }

        return token;
    }

    ////////////////////////////////////////////////////////////////////////////////////
    //
    // Method
    //      getNumber
    //
    // Implements
    //      Accessor method for number.
    //
    public UInt32 getNumber() { return number; }

    ////////////////////////////////////////////////////////////////////////////////////
    //
    // Method
    //      getToken
    //
    // Implements
    //      Accessor method for token.
    //
    public Token getToken() { return token; }

}  // class Lexer ////////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////////////////////////
//
// Class
//      Parser
//
// Implements
//      Recursive-descent parser for four function calculator.
//
//////////////////////////////////////////////////////////////////////////////////////////

public class Parser
{
    Lexer lexer;

    //////////////////////////////////////////////////////////////////////////////////////
    //
    // Method
    //      Parser (constructor)
    //
    // Implements
    //      Saves the lexer object in this parser instance.
    //
    public Parser(Lexer l)
    {
        lexer = l;
    }

    //////////////////////////////////////////////////////////////////////////////////////
    //
    // Method
    //      Parse
    //
    // Implements
    //      Four-function calculator with parentheses.  Additionally, ';' terminates an
    //      expression, allowing an expression to span multiple lines.
    //      The grammar is straight from the lecture notes, section 4.1:
    //
    //      T -> T + F | T - F | F
    //      F -> F * P | F / P | P
    //      P -> ( T ) | n
    //
    //      To end a session enter Ctrl-D followed by Enter.
    //
    //      The top-level function Parse() make suse of helper functions RdT(), RdF() and
    //      which implement each of the rules above.
    //
    public void Parse()
    {
        Int32 result = 0;

        while (true)
        {
            lexer.lex();

            switch (lexer.getToken())
            {
                case Token.EOF: return;

                case Token.EOL:
                    Console.WriteLine("= " + result);
                    break;

                default:
                    result = RdT();
                    Console.WriteLine("= " + result);
                    break;
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////
    //
    // Method
    //      RdT
    //
    // Implements
    //      The production rule:   T -> T + F | T - F | F
    //
    Int32 RdT()
    {
        Int32 result = RdF();

        while (true)
        {
            switch (lexer.getToken())
            {
                case Token.PLUS:
                    lexer.lex();
                    result += RdF();
                    continue;

                case Token.MINUS:
                    lexer.lex();
                    result -= RdF();
                    continue;

                default:
                    return result;
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////
    //
    // Method
    //      RdF
    //
    // Implements
    //      The production rule:  F -> F * P | F / P | P
    //
    Int32 RdF()
    {
        Int32 result = RdP();

        while (true)
        {
            switch (lexer.getToken())
            {
                case Token.TIMES:
                    lexer.lex();
                    result *= RdP();
                    continue;

                case Token.DIVIDE:
                    lexer.lex();
                    result /= RdP();
                    continue;

                default:
                    return result;
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////////////////
    //
    // Method
    //      RdP
    //
    // Implements
    //      The production rule:  P -> ( T ) | n
    //
    Int32 RdP()
    {
        Int32 result;

        switch (lexer.getToken())
        {
            case Token.NUMBER:
                result = (Int32)lexer.getNumber();
                break;

            case Token.LPAREN:
                lexer.lex();
                result = RdT();
                if (lexer.getToken() != Token.RPAREN)
                {
                    Console.WriteLine("Error: missing closing ')'");
                    throw new System.Exception();
                }
                break;

            default:
                Console.WriteLine("Error in expression.");
                throw new System.Exception();
        }

        lexer.lex();
        return result;
    }

} // class Parser ////////////////////////////////////////////////////////////////////////

//////////////////////////////////////////////////////////////////////////////////////////
//
// Class
//      FourFuncCalcApp
//
// Implements
//      A simple four function calculator to show recursive-descent parsing.
//
class FourFuncCalcApp
{
    public static void Main()
    {
        Lexer lexer = new Lexer();
        Parser parser = new Parser(lexer);

        Console.WriteLine("Four Function Calculator (Ctrl-D to quit, Enter after each line, ';' to end expression)");
        try
        {
            parser.Parse();
        }
        catch
        {
            Console.WriteLine("Doh!!");
        }

        Console.WriteLine("Finished.");
    }
}

////////////////////////////////////////////////////////////////////////////////////////////////////////

