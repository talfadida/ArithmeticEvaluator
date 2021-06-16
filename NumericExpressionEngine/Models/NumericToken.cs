namespace NumericExpressionEngine
{
            
    public class NumericToken : BaseToken
    {

        public int Value => int.Parse(Name);

        public NumericToken(int val) : base(val.ToString())
        {
           
        }
 
    }
}
