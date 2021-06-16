namespace NumericExpressionEngine
{
    internal class MultipleOperatorToken : OperatorToken
    {
        internal MultipleOperatorToken() : base("*") { Priority = 2; }

        internal override NumericToken Apply(NumericToken leftToken, NumericToken rightToken)
        {
            return new NumericToken(leftToken.Value * rightToken.Value);
        }
    }


}
