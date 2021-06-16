using System;
using System.Linq;

namespace NumericExpressionEngine
{
 
    internal class Expression
    {
        internal string Variable { get; set; } //represent left-side var name

        internal IToken[] ExpTokens { get; set; }

        private string _rawExp;
 

        internal Expression(string variable, string rawExp,  IToken[] validTokens)
        {
            
            this.Variable = variable;
            this.ExpTokens = validTokens;
            this._rawExp = rawExp;
        }

        public override string ToString()
        {
            return _rawExp;
        }
    }
}
