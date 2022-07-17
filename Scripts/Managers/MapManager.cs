using System;
using System.Collections.Generic;
using Entities;
using Godot;

namespace Managers
{
    public class MapManager : Node
    {
        public static MapManager Instance;

        [Export]
        private PackedScene PlanetScene;
        [Export]
        private NodePath FirstPlanetPositionNodePath;
        [Export]
        public int AmountOfLayers { get; private set; }
        [Export]
        public Vector2 PlanetDistance { get; private set; }

        public List<List<Planet>> PlanetLayers { get; private set; } = new List<List<Planet>>();
        public Planet GetLastPlanet => PlanetLayers[PlanetLayers.Count - 1][0];

        private Position2D firstPlanetPosition;
        private readonly List<Line2D> planetConnections = new List<Line2D>();

        public override void _Ready()
        {
            base._Ready();

            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                throw new System.Exception("MapManager is a singleton class and can only be instantiated once.");
            }

            firstPlanetPosition = GetNode<Position2D>(FirstPlanetPositionNodePath);

            CreateMap();
            ConnectPlanets();

            SetupSignals();
        }

        private void CreateMap()
        {
            var layer = new List<Planet>();
            PlanetLayers.Add(layer);
            var planet = PlanetScene.Instance<Planet>();
            layer.Add(planet);
            AddChild(planet);

            RandomizePlanetInfo(planet);
            planet.GlobalPosition = firstPlanetPosition.GlobalPosition;

            for (int i = 1; i < AmountOfLayers; i++)
            {
                layer = new List<Planet>();
                PlanetLayers.Add(layer);
                float numberOfPlanetsInLayer = i * 2 + 1;
                for (float j = 0; j < numberOfPlanetsInLayer; j++)
                {
                    planet = PlanetScene.Instance<Planet>();
                    layer.Add(planet);
                    AddChild(planet);

                    RandomizePlanetInfo(planet);
                    planet.GlobalPosition = firstPlanetPosition.GlobalPosition + new Vector2((j / (numberOfPlanetsInLayer - 1)) * i * PlanetDistance.x, ((numberOfPlanetsInLayer - 1 - j) / (numberOfPlanetsInLayer - 1)) * i * PlanetDistance.y); //go from 0/2 to 2/2 when numberOfPlanetsInLayer == 3. this is a sort of interpolation between both positions.
                }
            }

            // Create final planet
            layer = new List<Planet>();
            PlanetLayers.Add(layer);
            planet = PlanetScene.Instance<Planet>();
            layer.Add(planet);
            AddChild(planet);
            planet.GlobalPosition = firstPlanetPosition.GlobalPosition + new Vector2(0.5f * AmountOfLayers * PlanetDistance.x, 0.5f * AmountOfLayers * PlanetDistance.y);
            planet.SetGoal(18);

            foreach (var previousPlanet in PlanetLayers[PlanetLayers.Count - 2])
            {
                planet.AddConnectedPlanet(previousPlanet);
                previousPlanet.AddConnectedPlanet(planet);
            }
        }

        private void RandomizePlanetInfo(Planet planet)
        {
            var random = new Random();

            planet.SetGoal(random.Next(1, 12));
            GameResource rewardResource = (GameResource)random.Next(0, 3);
            switch (rewardResource)
            {
                case GameResource.Food:
                    planet.SetRewards(rewardResource, random.Next(1, 20));
                    break;
                case GameResource.Fleet:
                    planet.SetRewards(rewardResource, random.Next(1, 2));
                    break;
                case GameResource.Movement:
                    planet.SetRewards(rewardResource, random.Next(1, 5));
                    break;
            }
        }

        private void ConnectPlanets()
        {
            for (int i = 0; i < PlanetLayers.Count - 1; i++) //TODO: Hide connections, spawn them when moving fleets
            {
                for (int j = 0; j < PlanetLayers[i].Count; j++)
                {
                    var planet = PlanetLayers[i][j];
                    for (int k = 0; k < 3 && (j + k) < PlanetLayers[i + 1].Count; k++)
                    {
                        var neighbour = PlanetLayers[i + 1][j + k];
                        GD.Print("Connecting " + planet.Name + " to " + neighbour.Name);
                        if (neighbour != null)
                        {
                            planet.AddConnectedPlanet(neighbour);
                            neighbour.AddConnectedPlanet(planet);

                        }
                        else
                        {
                            GD.PrintErr("Connected planet is null");
                        }
                    }
                }
            }
        }

        public void ShowPlanetConnections(Planet planet, List<Planet> connectedPlanets)
        {
            foreach (var connectedPlanet in connectedPlanets)
            {
                Line2D line = new Line2D();
                AddChild(line);
                line.Points = new Vector2[] { planet.GlobalPosition, connectedPlanet.GlobalPosition };
                planetConnections.Add(line);
            }
        }

        public void HidePlanetConnections()
        {
            foreach (var line in planetConnections)
            {
                line.QueueFree();
                RemoveChild(line);
            }
            planetConnections.Clear();
        }

        private void SetupSignals()
        {

        }

    }
}