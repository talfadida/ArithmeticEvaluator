using NumericExpressionEngine;
using System;
using System.Collections.Generic;
using Xunit;

namespace NumericExpressionUnitTest
{
    public class NumericExpressionUnitTest
    {

        readonly ExperssionCalculator _expCalc;

        public NumericExpressionUnitTest() => _expCalc = new ExperssionCalculator(ExpressionEvaluator.Instance);

        [Theory]
        [InlineData("( 1 + 2 )", 3)]
        [InlineData("2 + 2 ^ 2 * 2", 10 )]
        [InlineData("( 2 + 2 ) ^ 2 * 2", 32)]
        [InlineData("2 + 2 ^ ( 2 * 2 )", 18)]
        [InlineData("( 12 / 2 ) % 5 ", 1)]
        [InlineData("( 13 - 1 ) / ( 2  % 5 )", 6)]        
        public void Test_Operations_And_Evaluator(string expression, int expected)
        {
            var calc = _expCalc.AddBulkAndExecute(new string[] { "i = " + expression });
            Assert.Equal(calc, "i=" + expected);
        }

        [Theory]
        [InlineData(new[] { "i = 2", "i += 1", "j = i++ + 1"},  "i=4, j=4")]
        [InlineData(new[] { "i = 2", "i += 1", "j = ++i + 1" }, "i=4, j=5")]
        [InlineData(new[] { "i = 2", "i -= 1", "j = i-- + 1" }, "i=0, j=2")]
        [InlineData(new[] { "i = 2", "i -= 1", "j = --i + 1" }, "i=0, j=1")]
        public void Test_Complex_Unary_Operation(string[] expressions, string expected)
        {
            var calc = _expCalc.AddBulkAndExecute(expressions);
            Assert.Equal(calc, expected);
        }

        [Theory]
        [InlineData("", ExpressionUtil.EMPTY_NOT_ALLOWED)]
        [InlineData("a + c + d", ExpressionUtil.STRUCTURE_ERROR)]
        [InlineData("a b = c + d ", ExpressionUtil.VAR_NAME_MISMATCH)]
        [InlineData("a = 1 & 2 ", ExpressionUtil.TOKEN_MISMATCH )]        
        [InlineData("a = 1++ + --2 ", ExpressionUtil.TOKEN_MISMATCH)]
        [InlineData("a = 1asd + 3", ExpressionUtil.TOKEN_MISMATCH)]
        [InlineData("12av = 1 + 3", ExpressionUtil.VAR_NAME_MISMATCH )]
        [InlineData("a = b + 3", ExpressionUtil.VAR_NOT_EXIST )]
        [InlineData("a = 1 + + 2 ", ExpressionUtil.TOKEN_ORDER_MISMTACH)]
        public void Test_Validation_And_Exceptions(string expression, string errorExpected)
        {
            var ex = Assert.Throws<NumericExpressionException>(() => _expCalc.AddBulkAndExecute(new string[] { expression }));
            Assert.Contains(errorExpected ,ex.Message);                           
        }




    }
}
