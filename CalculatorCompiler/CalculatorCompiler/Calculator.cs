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
        Console.WriteLine("Four Function Calculator (Ctrl-D to quit, Enter after each line, ';' to end expression)");
        Lexer lexer = new Lexer(Console.ReadLine());
        Parser parser = new Parser(lexer);

       
        try
        {
            parser.Parse();
        }
        catch
        {
            Console.WriteLine("Doh!!");
        }

        Console.WriteLine("Finished.");
        lexer = new Lexer(Console.ReadLine());
    }
}

////////////////////////////////////////////////////////////////////////////////////////////////////////

