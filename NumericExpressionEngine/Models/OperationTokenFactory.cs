namespace NumericExpressionEngine
{
    internal class OperationTokenFactory
    {
        internal OperatorToken Create(string name)
        {
            switch (name)
            {
                case "*": return new MultipleOperatorToken();
                case "+": return new PlusOperatorToken();
                case "-": return new MinusOperatorToken();
                case "/": return new DivideOperatorToken();
                case "%": return new RemainderOperatorToken();
                case "^": return new PowerOperatorToken();

                case "(": return new OpenOperatorToken();
                case ")": return new CloseOperatorToken();
            }

            return null;
        }

        internal IToken CreateOpen()
        {
            return new OpenOperatorToken();
        }

        internal IToken CreateClose()
        {
            return new CloseOperatorToken();
        }
    }


}
