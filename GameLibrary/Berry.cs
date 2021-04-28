using System;
using System.Collections.Generic;
using ToolsLibrary;

namespace GameLibrary
{
    namespace SnakeGame
    {
        public class EventBerry
        {
            static Dictionary<SnakeField, Dictionary<Coordinates, EventBerry>> Beries { get; } = new Dictionary<SnakeField, Dictionary<Coordinates, EventBerry>>();
            SnakeField Location { get; }
            Coordinates Position { get; }
            int Live { get; set; }
            SnakeMove Mover;
            public EventBerry(SnakeField location, Coordinates coord, SnakeMove mover, int live)
            {
                Location = location;
                Position = coord;
                Mover = mover;
                if (live > 0)
                {
                    Live = live;
                }
                Mover.Add(Next);
                location.AddEventBerry(coord);
                if (!Beries.ContainsKey(location))
                {
                    Beries.Add(location, new Dictionary<Coordinates, EventBerry>());
                }
                Beries[location].Add(coord, this);
            }
            public void Next()
            {               
                if (Live == 0)
                {
                    Mover.Remove(Next);
                    Beries[Location].Remove(Position);
                    if (Location.ReturnCell(Position) == GamesSquareValues.snakeEventBerry)
                    {
                        Location.RemoveEventBerry(Position);                       
                    }
                    return;
                }
                Live--;
            }
            public static void Clear(SnakeField field)
            {
                if (Beries.ContainsKey(field))
                {
                    Coordinates[] array = new Coordinates[Beries[field].Count];
                    Beries[field].Keys.CopyTo(array, 0);
                    foreach (var x in array)
                    {
                        DeleteBerry(field, x);
                    }
                    Beries.Remove(field);
                }
            }
            public void Delete() 
            { 
                Live = 0; 
                Next(); 
            }
            public static void DeleteBerry(SnakeField field, Coordinates coord) 
            {
                if (Beries.ContainsKey(field))
                {
                    if (Beries[field].ContainsKey(coord))
                    {
                        Beries[field][coord].Delete();
                    }
                }
            }
            public static void RandomEventBerry(bool multi, SnakeField location, double chanse, SnakeMove Mover, int time)
            {
                if (multi || (Beries.ContainsKey(location) ? Beries[location].Count == 0 : true))
                {
                    chanse = Math.Min(1, Math.Max(0, chanse));
                    var rnd = new Random();
                    var number = rnd.NextDouble();
                    if (number < chanse)
                    {
                        Coordinates newCoord;
                        while (true)
                        {
                            newCoord = new Coordinates(rnd.Next(location.Width), rnd.Next(location.Height));
                            if (location.ReturnCell(newCoord) == GamesSquareValues.nothing)
                            {
                                new EventBerry(location, newCoord, Mover, time);
                                break;
                            }
                        }
                    }
                }
            }
        }
        public class Berry
        {
            public static void RandomBerry(SnakeField location)
            {
                var rnd = new Random();
                Coordinates newCoord;
                while (true)
                {
                    newCoord = new Coordinates(rnd.Next(location.Width), rnd.Next(location.Height));
                    if (location.ReturnCell(newCoord) == GamesSquareValues.nothing)
                    {
                        location.AddBerry(newCoord);
                        break;
                    }
                }
            }
        }
    }
}
