using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace GameLibrary
{
    [Serializable]
    public class Letters
    {
        static List<string> Names { get; }
        static Letters()
        {
            using (var input = new FileStream($"data_Name.xml", FileMode.Open))
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
            using (var input = new FileStream($"data{name}.xml", FileMode.OpenOrCreate))
            {              
                while (input.Position != input.Length)
                {
                    var form = new BinaryFormatter();
                    var value = form.Deserialize(input);
                    if (value.GetType() == typeof(KeyValuePair<char, bool[]>))
                    {
                        var key = (KeyValuePair<char, bool[]>)value;
                        ValuePairs.Add(key.Key, key.Value);
                    }
                    
                }
            }
        }
        public void Add(PixelLetter pixelLetter)
        {
            if (!ValuePairs.ContainsKey(pixelLetter.Letter))
            {
                ValuePairs.Add(pixelLetter.Letter, pixelLetter.Values);
            }          
        }
        public void Remove(char value)
        {
            if (ValuePairs.ContainsKey(value))
            {
                ValuePairs.Remove(value);
            }
        }
        public void Save()
        {
            using (var input = new FileStream("data{Name}.xml", FileMode.Create))
            {
                var form = new BinaryFormatter();
                foreach(var value in ValuePairs)
                form.Serialize(input, value);
            }
        }
        public static void AllSave()
        {
            using (var input = new FileStream("data_Name.xml", FileMode.Create))
            {
                var form = new BinaryFormatter();
                form.Serialize(input, Names);
            }
        }
    }
    [Serializable]
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
