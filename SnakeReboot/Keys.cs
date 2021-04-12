using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TolsLibrary;

namespace SnakeReboot
{
    public enum needOption
    {
        menuUp,
        menuDown,
        menuPress,
        snakePause,
        snakeUp,
        snakeDown,
        snakeLeft,
        snakeRigh
    } 
    public class Keys
    {
        public Keys()
        {
            foreach(var key in (needOption[])Enum.GetValues(typeof(needOption)))
            {
                this[key] = new Event(null);
            }
        }
        Dictionary<needOption, Event> Options { get; } = new Dictionary<needOption, Event>();
        public Event this[needOption option]
        {
            get 
            {
                if (Options.ContainsKey(option))
                {
                    return Options[option];
                }
                else 
                { 
                    return null; 
                }               
            }
            set
            {
                if (Options.ContainsKey(option))
                {
                    Options.Remove(option);
                }
                Options.Add(option, value);
            }
        }
    }
}
