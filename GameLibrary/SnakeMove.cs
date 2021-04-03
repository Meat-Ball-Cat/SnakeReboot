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
            public bool Alife { get; private set; }
            public SnakeMove(int speed)
            {
                Alife = true;
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
                while (Alife)
                {
                    actions?.Invoke();
                    Thread.Sleep(Speed);
                }
            }
            public void Acceleration(double acc)
            {
                Speed = (int)(Speed * acc);
            }
            public void Stop()
            {
                Alife = false;
            }
        }
    }
}
