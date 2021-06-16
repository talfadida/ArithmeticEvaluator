using NumericExpressionEngine;
using System;
using System.Linq;
using System.IO;

namespace NumericExpressionConsole
{
    class Program
    {
        static void Main(string[] args)
        {
                      
            Console.WriteLine("Welcome to Numeric Expression Evaluator!\r\n==============================================");
            Console.WriteLine(@"
    # EACH LINE SHOULD CONTAIN VALID ARITHEMATIC EXPRESSION
    # MAKE SURE TO HAVE AT LEAST ONCE SPACE CHAR BETWEEN EACH TOKEN
    # LINE BEGINNING WITH # WILL REFER AS COMMENTS AND NOT EVALUATED
    # FOR ADDING NEW OPERATOR, INHERIT FROM OPERATOR CLASS AND UPDATE THE FACTORY
    # THE NUMERIC VALUES SHOULD BE OF INT32 RANGE
    # THE EXPECTED CALCULATION RESULT OF THE EXPRESSION SHOULD NOT EXCEED INT32 AS WELL
    # VARIABLE NAMES ARE CASE SENSITIVE");

            string filePath = @"..\..\..\input.txt";
            var inputPath = Path.GetFullPath(filePath);
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"\nMissing input file: please create {inputPath}  with valid expressions in the same folder of this executable");
                return;
            }
            
            var expressions = File.ReadAllLines(filePath)
                                  .Where(p => !string.IsNullOrEmpty(p.Trim()) && !p.StartsWith('#'));

            Console.WriteLine($"\nEvaluating {inputPath}.. \r\nResult:");
            ExperssionCalculator ec = new ExperssionCalculator(ExpressionEvaluator.Instance);
            Console.WriteLine(ec.AddBulkAndExecute(expressions));
            //In order to run with trace mode, add "true" parameter to the method. e.g.:
            //Console.WriteLine(ec.AddBulkAndExecute(expressions, true));

        }
    }
}
