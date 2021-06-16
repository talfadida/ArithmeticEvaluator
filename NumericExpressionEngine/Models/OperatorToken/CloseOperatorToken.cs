namespace NumericExpressionEngine
{
    internal class CloseOperatorToken : OperatorToken
    {
        internal CloseOperatorToken() : base(")")
        {
            Priority = ushort.MaxValue;
        }

        internal override NumericToken Apply(NumericToken leftToken, NumericToken rightToken)
        {
            return null;
        }
    }


}
