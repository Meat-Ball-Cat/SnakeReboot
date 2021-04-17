using System;
using System.Threading;

namespace GameLibrary
{
    namespace SnakeGame
    {
        public class SnakeMove
        {
            Action actions = default;
            public int Speed { get; private set; }
            public bool? Live { get; private set; }
            public SnakeMove(int speed)
            {
                Live = true;
                Speed = speed;
            }
            public void ResetSpead(int newSpeed)
            {
                Speed = newSpeed;
            }
            public void Add(Action newAction)
            {
                actions += newAction;
            }
            public void Remove(Action removeAction)
            {
                actions -= removeAction;
            }
            public void Start()
            {
                while (Live != false)
                {
                    if (Live == true)
                    {
                        actions?.Invoke();
                        Thread.Sleep(Speed);
                    }
                }
            }
            public void Pause() => Live = null;
            public void Continue() => Live = true;
            public void Stop() => Live = false;
            public void Acceleration(double acc) => Speed = (int)(Speed * acc);
            
        }
    }
}
