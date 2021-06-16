using System;

namespace NumericExpressionEngine
{
    internal class PowerOperatorToken : OperatorToken
    {
        internal PowerOperatorToken() : base("^") { Priority = 4; }

        internal override NumericToken Apply(NumericToken leftToken, NumericToken rightToken)
        {
            return new NumericToken(Convert.ToInt32(Math.Pow(leftToken.Value, rightToken.Value)));
        }
    }


}
