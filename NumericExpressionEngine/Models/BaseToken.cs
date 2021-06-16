namespace NumericExpressionEngine
{

    public abstract class BaseToken : IToken
    {
        public string Name { get; set; }

        public BaseToken(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}
