
using System;

//////////////////////////////////////////////////////////////////////////////////////////
//
// Class
//      Parser
//
// Implements
//      Recursive-descent parser for four function calculator.
//
//////////////////////////////////////////////////////////////////////////////////////////



public class NodeParser
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
    public NodeParser(Lexer l)
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
        Node result = new Node();

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
                    result = DoAddSubtraction();
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
    /*
     this is the first thing that gets run when a token is found because it is for add/subtract you have to keep going further to do the multiple/dividse first
     * you can see it is calling Rdf which represnts rules that need to be run before Rdt is run
     */
    Node DoAddSubtraction()
    {
        Node result = DoMultiplyDivide();

        while (true)
        {
            switch (lexer.getToken())
            {
                case Token.PLUS:
                    lexer.lex();
                    result += DoMultiplyDivide();
                    continue;

                case Token.MINUS:
                    lexer.lex();
                    result -= DoMultiplyDivide();
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
    /*
     
     */
    Node DoMultiplyDivide()
    {
        //call the function with the rules in it that need to be checked before this
        Node result = DoNumberParenthesis();

        while (true)
        {
            switch (lexer.getToken())
            {
                case Token.TIMES:
                    lexer.lex();
                    result *= DoNumberParenthesis();
                    continue;

                case Token.DIVIDE:
                    lexer.lex();
                    result /= DoNumberParenthesis();
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
    /*
     I don't understand why getting a number token and a parenthesis token are put together
     */
    Node DoNumberParenthesis()
    {
        Node result = new Node();

        switch (lexer.getToken())
        {
            case Token.NUMBER:
                result.Value = lexer.getNumber().ToString();
                break;

            case Token.LPAREN:
                lexer.lex();
                result = DoAddSubtraction();
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
        //so this is the bottom of the logic tree, you have a number that you are returning as the result, so why are you calling lex again?
        lexer.lex();
        return result;
    }

} // class Parser ////////////////////////////////////////////////////////////////////////

public class Node
{
    public Node Left { get; set; }
    public Node Right { get; set; }
    public string Value { get; set; }
    public void Run()
    {

    }
    public static Node operator +(Node c1, Node c2)
    {
        if (c1.Left == null)
            c1.Left = c2;
        else if (c1.Right == null)
            c1.Right = c2;
        return c1;

    }
    public static Node operator -(Node c1, Node c2)
    {
        if (c1.Left == null)
            c1.Left = c2;
        else if (c1.Right == null)
            c1.Right = c2;
        return c1;

    }
    public static Node operator *(Node c1, Node c2)
    {
        if (c1.Left == null)
            c1.Left = c2;
        else if (c1.Right == null)
            c1.Right = c2;
        return c1;

    }
    public static Node operator /(Node c1, Node c2)
    {
        if (c1.Left == null)
            c1.Left = c2;
        else if (c1.Right == null)
            c1.Right = c2;
        return c1;

    }
}