using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculatorCompiler
{
    public class NumberLexer
    {
        ILexer lex;
        public NumberLexer(ILexer lex) {
            this.lex = lex;
        }

        public void ReadNumber() 
        {
        
        }
    }
}
