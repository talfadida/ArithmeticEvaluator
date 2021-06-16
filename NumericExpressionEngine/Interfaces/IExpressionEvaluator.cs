namespace NumericExpressionEngine
{
    public interface IExpressionEvaluator
    {
        int Eval(IToken[] tokens);
    }
}