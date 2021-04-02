﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToolsLibrary;

namespace GameLibrary
{
    namespace SnakeGame
    {
        public static class Direction
        {
            public enum direction
            {
                up,
                left,
                down,
                right,
            }
            public static bool IsOpposit(direction first, direction last)
            {
                if (((first - last) * (first - last)) % 2 == 0)
                {
                    return true;
                }
                return false;
            }
        }
        public class Snake
        {
            public SnakeField Location { get; }
            List<Coord> Body { get; }
            bool Controbility { get; set; }
            Direction.direction NowDirection { get; set; }
            Direction.direction NextDirection { get; set; }
            public event Action<Snake> IsEat;
            public event Action<Snake> Die = (Snake) => Snake.IsDie();
            public bool Alife { get; private set; } = true;
            public Coord Head { get { return Body[0]; } }

            public Snake(SnakeField location, Coord startCoord, Direction.direction startDirection)
            {
                Location = location;
                NowDirection = startDirection;
                NextDirection = startDirection;
                Body = new List<Coord>();
                Add(startCoord);
            }
            public Snake(SnakeField location) : this(location, new Coord(location.Width / 2, location.Height / 2), Direction.direction.up) { }
            public void Move()
            {
                if (Alife)
                {
                    var next = Head;
                    switch (NowDirection)
                    {
                        case Direction.direction.up: { next = next.Up(); } break;
                        case Direction.direction.down: { next = next.Down(); } break;
                        case Direction.direction.left: { next = next.Left(); } break;
                        case Direction.direction.right: { next = next.Right(); } break;
                    }
                    switch (Location.ReturnCell(next))
                    {
                        case GamesSquareValues.nothing:
                            {
                                Add(next);
                                Remove(Body[Body.Count - 1]);
                            }
                            break;
                        case GamesSquareValues.snakeBerry:
                            {
                                Add(next);
                                IsEat(this);
                            }
                            break;
                        case GamesSquareValues.snakeWall: { Die(this); } break;
                        case GamesSquareValues.snake: { Die(this); } break;
                        default: throw new Exception("Неверное значение поля змейки");
                    }
                    Controbility = true;
                    ChangeDirection(NextDirection);

                }
            }

            public void ChangeDirection(Direction.direction direction)
            {
                NextDirection = direction;
                if (Controbility && !Direction.IsOpposit(NextDirection, NowDirection))
                {
                    NowDirection = NextDirection;
                    Controbility = false;
                }
            }
            void IsDie()
            {
                Alife = false;
            }
            void Add(Coord xy)
            {
                Body.Insert(0, xy);
                Location.AddSnake(xy);
            }
            void Remove(Coord xy)
            {
                Body.RemoveAt(Body.Count - 1);
                Location.RemoveSnake(xy);
            }
        }
    }
}