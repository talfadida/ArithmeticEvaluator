using System;
using System.Collections.Generic;
using System.Text;

namespace NumericExpressionEngine
{
    public class ExpressionEvaluator : IExpressionEvaluator
    {
        //we can define it as singelton, since we dont hold any state   
     
        #region Singleton
        private static ExpressionEvaluator _instance;
        private static readonly object padlock = new object();
        public static ExpressionEvaluator Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                            _instance = new ExpressionEvaluator();
                    }
                }
                return _instance;
            }
        }
        #endregion

        private ExpressionEvaluator() { }


        /// <summary>
        /// thread-safe method for evaluating arithematic expression 
        /// Code inspired from the following post: https://codinghelmet.com/exercises/expression-evaluator 
        /// </summary>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public int Eval(IToken[] tokens)
        {
            Stack<NumericToken> numbersStack = new Stack<NumericToken>();
            Stack<OperatorToken> opStack = new Stack<OperatorToken>();
            //opStack.Push(new OperatorToken("(")); // Implicit opening parenthesis

            for (int pos = 0; pos <= tokens.Length; pos++)
            {
                if (pos == tokens.Length || tokens[pos] is CloseOperatorToken)
                {
                    ProcessClosingParenthesis(numbersStack, opStack);
                }
                else if (tokens[pos] is NumericToken)
                {
                    numbersStack.Push(tokens[pos] as NumericToken);
                }
                else
                {
                    ProcessInputOperator(tokens[pos] as OperatorToken, numbersStack, opStack);
                }
            }

            return numbersStack.Pop().Value; // Result remains on values stacks
        }

        private void ProcessInputOperator(OperatorToken op, Stack<NumericToken> numbersStack, Stack<OperatorToken> opStack)
        {
            while (opStack.Count > 0 && OperatorCausesEvaluation(op, opStack.Peek()))
                ExecuteOperation(numbersStack, opStack);

            opStack.Push(op);
        }

        private bool OperatorCausesEvaluation(OperatorToken nextOp, OperatorToken currentOp)
        {
             
            if (currentOp is OpenOperatorToken) return false;

            return nextOp.Priority <= currentOp.Priority;

        }

        private void ProcessClosingParenthesis(Stack<NumericToken> numbersStack, Stack<OperatorToken> opStack)
        {
            
            while (!(opStack.Peek() is OpenOperatorToken))
                ExecuteOperation(numbersStack, opStack);

            opStack.Pop();
        }

        private void ExecuteOperation(Stack<NumericToken> numbersStack, Stack<OperatorToken> opStack)
        {
            var rightOperand = numbersStack.Pop();
            var leftOperand = numbersStack.Pop();
            var op = opStack.Pop();

            numbersStack.Push(op.Apply(leftOperand, rightOperand));
        }
 

    }
}
