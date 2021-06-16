using System.Collections.Generic;
using System.Linq;

namespace NumericExpressionEngine
{

    internal abstract class OperatorToken : BaseToken
    {

        internal static readonly string[] OPERATORS = new[] { "(", ")", "+", "-", "*", "/", "%", "^" };

        internal static readonly string[] COMBINED_EQUALS = new[] { "=", "-=", "+=", "*=", "/=", "%=", "^=" };

        internal ushort Priority { get; set; }
        internal OperatorToken(string name) : base(name) { }

        internal abstract NumericToken Apply(NumericToken leftToken, NumericToken rightToken);
    }


}
