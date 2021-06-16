namespace NumericExpressionEngine
{
    internal class MinusOperatorToken : OperatorToken
    {
        internal MinusOperatorToken() : base("-") { Priority = 1; }

        internal override NumericToken Apply(NumericToken leftToken, NumericToken rightToken)
        {
            return new NumericToken(leftToken.Value - rightToken.Value);
        }
    }


}
