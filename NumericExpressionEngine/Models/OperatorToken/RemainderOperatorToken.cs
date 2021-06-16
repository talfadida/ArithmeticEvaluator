namespace NumericExpressionEngine
{
    internal class RemainderOperatorToken : OperatorToken
    {
        internal RemainderOperatorToken() : base("%") { Priority = 3; }

        internal override NumericToken Apply(NumericToken leftToken, NumericToken rightToken)
        {
            return new NumericToken(leftToken.Value % rightToken.Value);
        }
    }


}
