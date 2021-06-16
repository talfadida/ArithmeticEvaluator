using System;

namespace NumericExpressionEngine
{


    public enum UnaryOrderEnum
    {
        Before,
        After
    }


    public class UnaryToken : BaseToken
    {

        public string Op { get; set; } //+ for '++' or - for '--'

        public UnaryOrderEnum Order { get; set; }

        public UnaryToken(string name, string op, UnaryOrderEnum order):base(name)
        {          
            Op = op;
            Order = order;
        }

        public IToken ToVariableToken()
        {
            return new VariableToken(this.Name);
        }
    }
}
