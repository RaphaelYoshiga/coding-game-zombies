using System;
using System.Collections.Generic;
using System.Linq;

namespace CodingGame.Zombies.OldDotNet
{
    class Player
    {
        static void Main(string[] args)
        {
            while (true)
            {
                var map = ParseInputs();

                var first = map.DecideNextPosition();

                Console.WriteLine(first.ToCommand()); // Your destination coordinates        
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
            return new ControllableHuman(new Position(x, y));
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
            return new Zombie(zombieId, new Position(zombieX, zombieY), new Position(zombieXNext, zombieYNext));
        }

        private static Human ParseHuman(string[] inputs)
        {
            var id = int.Parse(inputs[0]);
            var x = int.Parse(inputs[1]);
            var y = int.Parse(inputs[2]);
            return new Human(id, new Position(x, y));
        }
    }

    public class Map
    {
        private readonly Humans _humans;
        private ControllableHuman _player;
        private Zombies _zombies;

        public Map(ControllableHuman player, Humans humans, Zombies zombies)
        {
            _zombies = zombies;
            _player = player;
            _humans = humans;
        }

        public Position DecideNextPosition()
        {
            var humanPositionToGo = _humans.FirstOrDefault(IsSalvable) ?? _humans.First();
            return humanPositionToGo.Position;
        }

        private bool IsSalvable(Human human)
        {
            if (human.Position.Equals(_player.Position))
                return false;

            var humanPosition = human.Position;

            var closestZombie = _zombies.GetClosestZombie(humanPosition);
            var distance = closestZombie.Position.CalculateDistance(humanPosition);
            var distanceInTurns = distance / 400;

            var turnsToSaveHuman = _player.TurnsToSaveHuman(humanPosition);

            var isSalvable = turnsToSaveHuman <= distanceInTurns;
            Console.Error.WriteLine($"HumanId: {human.Id} IsSalvable: {isSalvable} TurnsToSaveHuman: {turnsToSaveHuman} TurnsForZombieToReachIt: {distanceInTurns}");


            return isSalvable;
        }
    }

    public class Zombie
    {
        public Zombie(int id, Position position, Position nextTurnPosition)
        {
            Position = position;
            NextTurnPosition = nextTurnPosition;
        }
        public Position Position { get; }
        public Position NextTurnPosition { get; }
    }


    public class ControllableHuman
    {
        private const int ShootingRange = 2000;
        private const int MovementDistance = 1000;

        public ControllableHuman(Position position)
        {
            Position = position;
        }

        public Position Position { get; }

        public int TurnsToSaveHuman(Position humanPosition)
        {
            var distance = Position.CalculateDistance(humanPosition);
            Console.Error.WriteLine($"Distance {distance}");
            return distance / (MovementDistance + ShootingRange);
        }
    }

    public class Zombies : List<Zombie>
    {
        public Zombie GetClosestZombie(Position humanPosition)
        {
            return this.OrderBy(p => p.Position.CalculateDistance(humanPosition)).First();
        }
    }

    public class Humans : List<Human>
    {
    }

    public class Human
    {
        public Human(int id, Position position)
        {
            Id = id;
            Position = position;
        }

        public int Id { get; }
        public Position Position { get; }

    }

    public struct Position
    {
        public int X { get; }
        public int Y { get; }

        public Position(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int CalculateDistance(Position anotherPosition)
        {
            var xDistance = EnsurePositive(X - anotherPosition.X);
            var yDistance = EnsurePositive(Y - anotherPosition.Y);

            return xDistance + yDistance;
        }

        private int EnsurePositive(int value)
        {
            return value > 0 ? value : value * -1;
        }

        public string ToCommand()
        {
            return X + " " + Y;
        }

        private sealed class XYEqualityComparer : IEqualityComparer<Position>
        {
            public bool Equals(Position x, Position y)
            {
                return x.X == y.X && x.Y == y.Y;
            }

            public int GetHashCode(Position obj)
            {
                unchecked
                {
                    return (obj.X * 397) ^ obj.Y;
                }
            }
        }

        public static IEqualityComparer<Position> XYComparer { get; } = new XYEqualityComparer();
    }
}
