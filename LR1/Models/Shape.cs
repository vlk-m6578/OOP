namespace PAINT.Models
{
    public abstract class Shape
    {
        protected string name;
        protected int x;
        protected int y;
        protected char symbol;
        protected char symbolBackground;
        public Shape(string name, char s, char sB)
        {
            this.name = name;
            symbol = s;
            symbolBackground = sB;
        }
        
        public Shape() { }
        public string Name{ 
            get { return name; }
            set { name = value; }
        }
        public int X
        {
            get { return x; }
            set { x = value; }
        }
        public int Y
        {
            get { return y; }
            set { y = value; }
        }
        public char Symbol
        {
            get { return symbol; }
            set { symbol = value; }
        }
        public char SymbolBackground
        {
            get { return symbolBackground; }
            set { symbolBackground = value; }
        }
        public abstract void Draw(int width, int height);
        public abstract void Move(int deltaX, int deltaY);
        public abstract Shape Clone();
    }
}
