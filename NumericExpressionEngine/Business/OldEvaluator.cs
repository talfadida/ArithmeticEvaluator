using System;
using System.Collections.Generic;
using System.Text;

namespace NumericExpressionEngine
{
    public class OldEvaluator
    {
        #region Singleton
        private static OldEvaluator _instance;
        private static readonly object padlock = new object();
        public static OldEvaluator Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (padlock)
                    {
                        if (_instance == null)
                            _instance = new OldEvaluator();
                    }
                }
                return _instance;
            }
        }
        #endregion
      
        private OldEvaluator() { }

       
        public int Eval(string[] tokens) 
        {           
            Stack<int> numbersStack = new Stack<int>();
            Stack<string> opStack = new Stack<string>();

            opStack.Push("("); // Implicit opening parenthesis
                        

            int pos = 0;
            while (pos <= tokens.Length)
            {
                if (pos == tokens.Length || tokens[pos] == ")")
                {
                    ProcessClosingParenthesis(numbersStack, opStack);
                    pos++;
                }
                else if (int.TryParse(tokens[pos], out var num))  
                {
                    numbersStack.Push(num);
                    pos++;
                     
                }
                else
                {
                    ProcessInputOperator(tokens[pos], numbersStack, opStack);
                    pos++;
                }

            }

            return numbersStack.Pop(); // Result remains on values stacks

        }

        private void ProcessClosingParenthesis(Stack<int> vStack, Stack<string> opStack)
        {

            while (opStack.Peek() != "(")
                ExecuteOperation(vStack, opStack);

            opStack.Pop(); // Remove the opening parenthesis

        }
 
        private void ProcessInputOperator(string op, Stack<int> vStack, Stack<string> opStack)
        {

            while (opStack.Count > 0 && OperatorCausesEvaluation(op, opStack.Peek()))
                ExecuteOperation(vStack, opStack);

            opStack.Push(op);

        }

        private bool OperatorCausesEvaluation(string nextOp/**/, string currentOp)
        {
            if (currentOp == ")") return true;
            if (currentOp == "(") return false;

            bool evaluate = false;

            switch (nextOp)
            {
                case "+":
                case "-":
                    evaluate = true; // (currentOp != "(");
                    break;
                case "*":
                case "/":
                    evaluate = (currentOp == "*" || currentOp == "/"); //eval only if next.priority <= current.priority
                    break;


                //case ")":
                //    evaluate = true;
                //    break;
            }

            return evaluate;

        }

        private void ExecuteOperation(Stack<int> vStack, Stack<string> opStack)
        {

            int rightOperand = vStack.Pop();
            int leftOperand = vStack.Pop();
            string op = opStack.Pop();

            int result = 0;
            switch (op)
            {
                case "+":
                    result = leftOperand + rightOperand;
                    break;
                case "-":
                    result = leftOperand - rightOperand;
                    break;
                case "*":
                    result = leftOperand * rightOperand;
                    break;
                case "/":
                    result = leftOperand / rightOperand;
                    break;
                case "%":
                    result = leftOperand % rightOperand;
                    break;
            }

            vStack.Push(result);

        }


    }
}
