using System;


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
    string textToParse;
    int index = 0;

    ////////////////////////////////////////////////////////////////////////////////////
    //
    // Method
    //      Lexer (constructor)
    //
    // Implements
    //      Initialises a lexer object, clearing number and setting the first character
    //      to a space (will be consumed later).
    //
    public Lexer(string textToParse)
    {
        if (string.IsNullOrEmpty(textToParse)) {
            throw new ArgumentException("I cant parse nothing", "textToParse");
        }
        lastchar = ' ';
        number = 0;
        token = Token.NO_TOK;
        this.textToParse = textToParse;
        
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
    /*
     This thing is keeping a reference to the last character (starting it off with " ") and the next character so that you don't have to do a Peek type thing
     * you always have a reference to the current character you are parsing and the next character
     lastchar = the thing you are currently dealing with
     * nextchar = is the character after the current character so that you can use it to determine what to do, like if its a multi digit number
     * 
     * 
     * this structure is avoiding doing things like having a readNumber function to read in characters that form one number
     * the whole point of the lexer is to tokenize things from one character at a time, when the token may be built from multiple characters like "3222" vs "+"
     */
    public Token lex()
    {
        int nextchar;

        token = Token.NO_TOK;
        number = 0;
        //Once a valid meaning full token is found in full like a number "111" or a operator "+" then the loop will exit and the token will exist for the parser
        while (token == Token.NO_TOK)
        {
            if (this.index == this.textToParse.Length)
            {
                token = Token.EOF;
                return token;
            }
            nextchar = this.textToParse[this.index];

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
                    //no idea what this is doing?? Its just a complicated way to check that an number is within range so no stack overflow happens
                    if (number <= (UInt32.MaxValue / 10))
                        //what the hell is this going. just a complicated way to concat number values together i guess?
                        number = number * 10 + (UInt32)(((UInt32)lastchar) - '0');
                    else
                        number = UInt32.MaxValue;
                    //this is checking the next character to determine if its a number, if its not, then the number part is done and you can move to the next step
                    //if it is a number then you have to keep going in order to get the entire number with all the digits
                    if (nextchar < '0' || nextchar > '9')
                        token = Token.NUMBER;
                    break;

                // Consume everything else
                default: break;
            }

            lastchar = nextchar;
            this.index++;
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