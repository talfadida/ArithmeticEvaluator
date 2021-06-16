using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace NumericExpressionEngine
{

    public class ExperssionCalculator
    {
        private List<Expression> _expLinkedList = new List<Expression>();
        private bool _isDebug;
        private readonly IExpressionEvaluator _evaluator;
        private readonly OperationTokenFactory _tokenFact;
        private readonly ExpressionUtil _expressionUtil;

        public ExperssionCalculator(IExpressionEvaluator evaluator)
        {
            this._evaluator = evaluator;
            this._tokenFact = new OperationTokenFactory();
            this._expressionUtil = new ExpressionUtil(this._tokenFact);
        }

        public string AddBulkAndExecute(IEnumerable<string> expressions, bool isDebug = false)
        {
            lock (_expLinkedList) //make it thread-safe if will be called concurrently..
            {
                _isDebug = isDebug;
                foreach (var exp in expressions)
                {
                    //validate the expression and the components within it
                    _expressionUtil.ValidateExpression(exp, out var varName, out var mixedOpInEquality, out IToken[] validTokens);
                    //initial the expression
                    Expression initialExp = new Expression(varName, exp, validTokens);
                    //simplify_1 e.g.: i+=1 => i=i+1 
                    SimplifyMixOfOpAndEqualityExpression(initialExp, mixedOpInEquality);
                    //simplify_2 e.g. i=j++ + --k => {k=k-1, i=j+k, k=k-1}
                    Expression[] UnrayExpSplitted = SplitEmbededUnaryArithematicOp(initialExp);
                    _expLinkedList.AddRange(UnrayExpSplitted);                   
                }
                //execute
                return Execute();
            }
        }

        /// <summary>
        /// change 'j *= i + 1' into 'j = j * ( i + 1 )'
        /// </summary>       
        private void SimplifyMixOfOpAndEqualityExpression(Expression initialExp, string mixedOpInEquality)
        {
            if(!string.IsNullOrEmpty(mixedOpInEquality))  
            {
                var updateTokenList = new List<IToken> {
                    new VariableToken(initialExp.Variable),
                    _tokenFact.Create(mixedOpInEquality), //new OperatorToken(mixedOpInEquality) ,
                     _tokenFact.CreateOpen(), //new OperatorToken("(") ,
                    // here - will inject the original expression
                    _tokenFact.CreateClose() //new OperatorToken(")")
                };
                updateTokenList.InsertRange(3, initialExp.ExpTokens);
                initialExp.ExpTokens = updateTokenList.ToArray();
            }
            
        }


        /// <summary>
        /// Given that expression tokens contain i++, extract it to another operation (e.g. i=i+1) and return in correct order in the chain of expressions
        /// </summary>         
        private Expression[] SplitEmbededUnaryArithematicOp(Expression expression)
        {
            

            LinkedList<Expression> FlatExpListFromEmbedUnary = new LinkedList<Expression>();
            var currExpPtr = FlatExpListFromEmbedUnary.AddFirst(expression); //e.g: j = i++ + 5 + --p

            List<IToken> revisedTokens = new List<IToken>();
            StringBuilder revisedTokensStr = new StringBuilder();
            foreach (var token in expression.ExpTokens)  
            {
                
                if (token is UnaryToken unToken) //using pattern matching; there is alike in java? :)  
                {
                    IToken simplifiedToken = SplitAndReturnSimpleVar(FlatExpListFromEmbedUnary, unToken);
                    revisedTokens.Add(simplifiedToken);
                    revisedTokensStr.Append(simplifiedToken.Name);
                }
                else //
                {                     
                    revisedTokens.Add(token);
                    revisedTokensStr.Append(token.Name);
                }

            }

            //change the original complex exp ( i++ + 5 + ++p) into simple exp (i + 5 + p)  
            currExpPtr.Value = new Expression(expression.Variable, revisedTokensStr.ToString(), revisedTokens.ToArray());           
            return FlatExpListFromEmbedUnary.ToArray();
        }


        private IToken SplitAndReturnSimpleVar(LinkedList<Expression> FlatExpListFromEmbedUnary, UnaryToken unToken)
        {
            //build the expression, e.g. i++ will become i=i+1
            var exp = new Expression(unToken.Name, $"{unToken.Name} {unToken.Op} 1",
                                    new List<IToken>() { new VariableToken(unToken.Name), _tokenFact.Create(unToken.Op), new NumericToken(1) }.ToArray());
            
            //place it in the right order (e.g. ++i or i++)
            if (unToken.Order == UnaryOrderEnum.After)
                FlatExpListFromEmbedUnary.AddLast(exp);
            else
                FlatExpListFromEmbedUnary.AddFirst(exp);

            //simplify the original token from i++ to i  
            var simplifiedToken = unToken.ToVariableToken();
            return simplifiedToken;
        }

        /// <summary>
        /// Executing the ExpressionEvaluator over the expressions
        /// </summary>       
        private string Execute()
        {            
             
            Dictionary<string, int> calculatedHash = new Dictionary<string, int>();
            
            foreach (var exp in _expLinkedList)            
            {
                 
                //replace vars with values
                for (int i = 0; i < exp.ExpTokens.Length; i++)
                {
                    if (exp.ExpTokens[i] is VariableToken )
                    {
                        if (!calculatedHash.ContainsKey(exp.ExpTokens[i].Name))
                            throw new NumericExpressionException($"{ExpressionUtil.VAR_NOT_EXIST}: {exp.ExpTokens[i].Name} ");
                        exp.ExpTokens[i] = new NumericToken(calculatedHash[exp.ExpTokens[i].Name]); //replace the Var with Num                       
                    }
                }
                int result = 0;
                try
                {
                    result = _evaluator.Eval(_expressionUtil.WrapOpen(exp.ExpTokens));                    
                }
                catch (Exception ex)
                {
                    throw new NumericExpressionException($"{ExpressionUtil.TOKEN_ORDER_MISMTACH}: {exp}");
                }
                calculatedHash.AddOrUpdate(exp.Variable, result);
                Trace($"Eval: {exp.ToString()} result => {result}");
                Trace($"Vars: {string.Join(",", calculatedHash.Select(p => $"{p.Key}={p.Value}"))}");
                
            }          
            return string.Join(", ",calculatedHash.Select(p => $"{p.Key}={p.Value}"));
 
        }

        private void Trace(string str)
        {
            if (_isDebug)
            {
                Console.WriteLine(str);
            }
        }
    }
}
