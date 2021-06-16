namespace NumericExpressionEngine
{
    internal class OpenOperatorToken: OperatorToken
    {
        internal OpenOperatorToken():base("(")
        {
            Priority = ushort.MaxValue;
        }

        internal override NumericToken Apply(NumericToken leftToken, NumericToken rightToken)
        {
            return null;
        }
    }


}
