using System;
using System.Runtime.Serialization;

namespace NumericExpressionEngine
{
    [Serializable]
    public class NumericExpressionException : Exception
    {
 
        public NumericExpressionException(string message) : base(message)
        {
        }
 
    }
}