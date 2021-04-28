using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLibrary
{
    namespace SnakeGame
    {
        public enum SnakeStartSpeed
        {
            slow = 250,
            medium = 200,
            fast = 150
        }
        public enum SnakeAcceleration
        {
            stop = 100,
            slow = 95,
            medium = 90,
            fast = 75
        }
        public class SnakeSettings
        {
            public bool Multy { get; set; }
            public bool Event { get; set; }
            double _eventChance;
            public double EventChance
            {
                get
                {
                    double result;
                    if (Event)
                    {
                        result = _eventChance;
                    }
                    else
                    {
                        throw new Exception("Значение не определено");
                    }
                    return result;
                }
                set
                {
                    _eventChance = Math.Max(0, Math.Min(1, value));
                }
            }
            bool _eventMulty;
            public bool EventMulty
            {
                get
                {
                    bool result;
                    if (Event)
                    {
                        result = _eventMulty;
                    }
                    else
                    {
                        result =  false;
                    }
                    return result;
                }
                set
                {
                    if (Event)
                    {
                        _eventMulty = value;
                    }
                }
            }

            SnakeStartSpeed? _startSpeed;
            public SnakeStartSpeed StartSpeed 
            {
                get { return _startSpeed ?? SnakeStartSpeed.medium; }
                set
                {
                    if (Enum.IsDefined(typeof(SnakeStartSpeed), value))
                    {
                        _startSpeed = value;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                }
            }
            SnakeAcceleration? _acceleration;
            public SnakeAcceleration Acceleration
            {
                get { return _acceleration ?? SnakeAcceleration.medium; }
                set
                {
                    if (Enum.IsDefined(typeof(SnakeAcceleration), value))
                    {
                        _acceleration = value;
                    }
                    else
                    {
                        throw new ArgumentOutOfRangeException();
                    }
                }
            }

        }
    }
}
