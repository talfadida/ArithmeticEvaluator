namespace NumericExpressionEngine
{
    internal class PlusOperatorToken : OperatorToken
    {
        internal PlusOperatorToken() : base("+") { Priority = 1;  }

        internal override NumericToken Apply(NumericToken leftToken, NumericToken rightToken)
        {
            return new NumericToken(leftToken.Value + rightToken.Value);
        }
    }


}
