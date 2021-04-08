using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace GameLibrary
{
    [Serializable]

    public class Letters
    {
        static List<string> Names { get; }
        static Letters()
        {
            using (var input = new FileStream($"data_Name", FileMode.OpenOrCreate))
            {
                if (input.Length != 0)
                {
                    var form = new BinaryFormatter();
                    Names = (List<string>)form.Deserialize(input);
                }
                else
                {
                    Names = new List<string>();
                }
            }
        }
        String Name { get; }
        public bool[] this[char index]
        {
            get {return ValuePairs[index]; }            
        }
        Dictionary<char, bool[]> ValuePairs { get; } = new Dictionary<char, bool[]>();
        public Letters(string name)
        {
            Name = name;
            if (!Names.Contains(name))
            {
                Names.Add(name);
            }
            using (var input = new FileStream($"data{name}", FileMode.OpenOrCreate))
            {
                
                if (input.Length != 0)
                {
                    PixelLetter value;
                    var form = new BinaryFormatter();

                    while ((value = (PixelLetter)form.Deserialize(input)) != null)
                    {
                        ValuePairs.Add(value.Letter, value.Values);
                    }
                }
            }
        }
        public void Add(PixelLetter pixelLetter)
        {
            ValuePairs.Add(pixelLetter.Letter, pixelLetter.Values);
        }
        public void Remove(PixelLetter letter)
        {
            ValuePairs.Remove(letter.Letter);
        }
        public void Save()
        {
            using (var input = new FileStream($"data{Name}", FileMode.Create))
            {
                var form = new BinaryFormatter();
                foreach(var pix in ValuePairs)
                {
                    form.Serialize(input, pix);
                }
            }
        }
        public static void AllSave()
        {
            using (var input = new FileStream($"data", FileMode.Create))
            {
                var form = new BinaryFormatter();
                form.Serialize(input, Names);
            }
        }
    }
    public class PixelLetter
    {
        public char Letter { get; }
        public bool[] Values { get; }
        public PixelLetter(char letter, bool[] value)
        {
            if(value.Length == 25)
            {
                Letter = letter;
                Values = value;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }
    }
}
