using SquareRectangle;

namespace GameLibrary
{
    public class Score
    {
        int _val;
        int Value {
            get 
            { return _val; }
            set 
            { 
                _val = value;
                var str = _val.ToString();
                if (str.Length > Writer.Length)
                {
                    Value = Value % 100;
                }
                else
                {
                    for (int i = str.Length; i < Writer.Length; i++)
                    {
                        str = " " + str;
                    }
                    Writer.WriteLine(str);
                }
            }
        }
        IWriter Writer { get; }
        public Score(IWriter writer)
        {
            Writer = writer;
            Value = 0;
        }
        public void Add()
        {
            Value++;
        }
        public void Add(int value)
        {
            Value += value;
        }
    }
}
