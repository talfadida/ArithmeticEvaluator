namespace NumericExpressionEngine
{
    internal class DivideOperatorToken : OperatorToken
    {
        internal DivideOperatorToken() : base("/") { Priority = 2; }

        internal override NumericToken Apply(NumericToken leftToken, NumericToken rightToken)
        {
            return new NumericToken(leftToken.Value / rightToken.Value);
        }
    }


}
