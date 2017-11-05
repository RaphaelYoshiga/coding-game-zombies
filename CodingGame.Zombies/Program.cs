using System;
using System.Collections.Generic;
using System.Linq;

namespace CodingGame.Zombies
{
    class Player
    {
        static void Main(string[] args)
        {
            string[] inputs;

            // game loop
            while (true)
            {
                var map = ParseInputs();

                var first = map.DecideNextPosition();

                Console.WriteLine(first.X + " " + first.Y); // Your destination coordinates        
            }
        }

        private static Map ParseInputs()
        {
            string[] inputs;
            inputs = Console.ReadLine().Split(' ');
            var player = ParsePlayer(inputs);
            var humans = ParseHumans();
            var zombies = ParseZombies();
            return new Map(player, humans, zombies);
        }

        private static ControllableHuman ParsePlayer(string[] inputs)
        {
            int x = int.Parse(inputs[0]);
            int y = int.Parse(inputs[1]);
            return new ControllableHuman(new Point(x, y));
        }

        private static Humans ParseHumans()
        {
            var humanCount = int.Parse(Console.ReadLine());
            var humans = new Humans();
            for (int i = 0; i < humanCount; i++)
            {
                var inputs = Console.ReadLine().Split(' ');
                humans.Add(ParseHuman(inputs));
            }
            return humans;
        }

        private static Zombies ParseZombies()
        {
            var zombies = new Zombies();
            var zombieCount = int.Parse(Console.ReadLine());
            for (int i = 0; i < zombieCount; i++)
            {
                var inputs = Console.ReadLine().Split(' ');
                zombies.Add(ParseZombie(inputs));
            }
            return zombies;
        }

        private static Zombie ParseZombie(string[] inputs)
        {
            int zombieId = int.Parse(inputs[0]);
            int zombieX = int.Parse(inputs[1]);
            int zombieY = int.Parse(inputs[2]);
            int zombieXNext = int.Parse(inputs[3]);
            int zombieYNext = int.Parse(inputs[4]);
            return new Zombie(zombieId, new Point(zombieX, zombieY), new Point(zombieXNext, zombieYNext));
        }

        private static Human ParseHuman(string[] inputs)
        {
            var id = int.Parse(inputs[0]);
            var x = int.Parse(inputs[1]);
            var y = int.Parse(inputs[2]);
            return new Human(id, new Point(x, y));
        }
    }

    public class Map
    {
        private readonly Humans _humans;

        public Map(ControllableHuman player, Humans humans, Zombies zombies)
        {
            _humans = humans;
        }


        public Point DecideNextPosition()
        {
            return _humans.First().Position;
        }
    }

    public class Zombie
    {        
        public Zombie(int id, Point position, Point nextTurnPosition)
        {
            Point = position;
            NextTurnPosition = nextTurnPosition;
        }
        public Point Point { get; }
        public Point NextTurnPosition { get; }
    }


    public class ControllableHuman
    {
        public ControllableHuman(Point point)
        {
            Point = point;
        }

        public Point Point { get; }
    }

    public class Zombies : List<Zombie>
    {
    }

    public class Humans : List<Human>
    {
    }

    public class Human
    {
        public Human(int id, Point position)
        {
            Id = id;
            Position = position;
        }

        public int Id { get; }
        public Point Position { get; }

    }

    public struct Point
    {
        public int X { get; }
        public int Y { get; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

    }
}
