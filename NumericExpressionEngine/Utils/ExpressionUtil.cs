using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace NumericExpressionEngine
{
    public class ExpressionUtil
    {
                   

        public const string EMPTY_NOT_ALLOWED = "ValidationFailed: Empty expression is not valid";
        public const string STRUCTURE_ERROR =   "ValidationFailed: Expression is not structured correctly";
        public const string VAR_NAME_MISMATCH = "ValidationFailed: Variable name on the left-hand is not valid";
        public const string TOKEN_MISMATCH =    "ValidationFailed: Token name on the right-hand is not valid";
        public const string VAR_NOT_EXIST =     "RuntimeFailed: Couldn't resolve variable. Are you sure you have it added in the right order?";
        public const string TOKEN_ORDER_MISMTACH = "RuntimeFailed: You right-hand expression is not valid.";
        
        private OperationTokenFactory _tokenFactory;

        internal ExpressionUtil(OperationTokenFactory tf)
        {
            this._tokenFactory = tf;
        }

        /// <summary>
        /// Main validator method. Confirms that the expression is valid and generates the relevant tokens/components out it
        /// </summary>
        /// <param name="exp"></param>
        /// <param name="varName"></param>
        /// <param name="varBindEqOp"></param>
        /// <param name="valid_tokens"></param>
        internal void ValidateExpression(string exp, out string varName, out string varBindEqOp, out IToken[] valid_tokens)
        {
            
            if (string.IsNullOrEmpty(exp.Trim()))
                throw new NumericExpressionException(EMPTY_NOT_ALLOWED);

            //check general structure
            varBindEqOp = "";             
            string[] exp_parts = exp.Split(OperatorToken.COMBINED_EQUALS, StringSplitOptions.RemoveEmptyEntries).Select(p => p.Trim()).ToArray() ;
            if (exp_parts.Length != 2)
                throw new NumericExpressionException($"{STRUCTURE_ERROR} : {exp}");
            
            varBindEqOp = exp.Substring(exp_parts[0].Length).Trim().Split(' ')[0].Replace("=",""); //extract only the operator


            //check left hand - variable
            if (!IsVariable(exp_parts[0], out var varToken))
                throw new NumericExpressionException($"{VAR_NAME_MISMATCH} : {exp}");
            varName = varToken.Name;



            //check right hand - exp
            var valid_tokens_list = new List<IToken>();            
            IEnumerable<string> tokens = exp_parts[1].Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries).Select(p=>p.Trim());
            foreach (var token in tokens)
            {
                IToken validToken;
                if(IsValidUnaryEmbedVar(token, out validToken) || 
                   IsVariable(token, out validToken) ||
                   IsNumber(token, out validToken) ||
                   IsValidOperator(token, out validToken))
                {
                    valid_tokens_list.Add(validToken);                    
                }
                else
                {
                    throw new NumericExpressionException($"{TOKEN_MISMATCH} : '{exp}' => token: '{token}'");
                }                                
            }

            valid_tokens = valid_tokens_list.ToArray();

        }


        private bool IsVariable(string token, out IToken tuo)
        {
            tuo = null;
            if (Regex.IsMatch(token, @"^[_a-z]\w*$"))              
            {
                tuo = new VariableToken(token);
                return true;
            }
            return false;  
        }
        private bool IsValidUnaryEmbedVar(string token, out IToken tuo)
        {
            string op, var;
            UnaryOrderEnum order;
            tuo = null;
            if (token.StartsWith("++") || token.StartsWith("--"))
            {
                op = token[0].ToString();
                order = UnaryOrderEnum.Before;
                var = token.Replace("++", "").Replace("--", "");
                if (!IsVariable(var, out var temp))
                    return false; 
                tuo = new UnaryToken(var, op, order);
                return true;
            }
            else if (token.EndsWith("++") || token.EndsWith("--"))
            {
                op = token[token.Length - 1].ToString();
                order = UnaryOrderEnum.After;
                var = token.Replace("++", "").Replace("--", "");
                if (!IsVariable(var, out var temp))
                    return false;
                tuo = new UnaryToken(var, op, order);
                return true;
            }
            return false;
        }
        private bool IsValidOperator(string token, out IToken tuo)
        {
            tuo = null;
            if (OperatorToken.OPERATORS.Contains(token)){
                tuo = _tokenFactory.Create(token);
                return true;
            }
            return false;
        }
        private bool IsNumber(string token, out IToken tuo)
        {
            tuo = null;
            if (int.TryParse(token, out var temp))
            {
                tuo = new NumericToken(temp);
                return true;
            }
            return false;
        }

        internal IToken[] WrapOpen(IToken[] expTokens)
        {
            //this is needed for the ExpressionEvaluator
            return new List<IToken>() { _tokenFactory.CreateOpen() }.Concat(expTokens).ToArray();
        }
    }
}
