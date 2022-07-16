using System.Collections.Generic;
using Entities;
using Godot;

namespace Managers
{
    public class MapManager : Node
    {
        [Export]
        public PackedScene PlanetScene { get; private set; }
        [Export]
        public NodePath FirstPlanetPositionNodePath { get; private set; }
        [Export]
        public int amountOfLayers { get; private set; }
        [Export]
        public Vector2 planetDistance { get; private set; }

        public List<List<Planet>> PlanetLayers { get; private set; } = new List<List<Planet>>();

        private Position2D firstPlanetPosition;

        public override void _Ready()
        {
            base._Ready();

            firstPlanetPosition = GetNode<Position2D>(FirstPlanetPositionNodePath);
            CreateMap();
            ConnectPlanets();

            SetupSignals();
        }

        private void CreateMap()
        {
            var layer = new List<Planet>();
            var planet = PlanetScene.Instance<Planet>();
            PlanetLayers.Add(layer);
            layer.Add(planet);
            AddChild(planet);

            planet.GlobalPosition = firstPlanetPosition.GlobalPosition;

            for (int i = 1; i < amountOfLayers; i++)
            {
                layer = new List<Planet>();
                PlanetLayers.Add(layer);
                float numberOfPlanetsInLayer = i * 2 + 1;
                for (float j = 0; j < numberOfPlanetsInLayer; j++)
                {
                    planet = PlanetScene.Instance<Planet>();
                    layer.Add(planet);
                    AddChild(planet);

                    planet.GlobalPosition = firstPlanetPosition.GlobalPosition + new Vector2((j / (numberOfPlanetsInLayer - 1)) * i * planetDistance.x, ((numberOfPlanetsInLayer - 1 - j) / (numberOfPlanetsInLayer - 1)) * i * planetDistance.y); //go from 0/2 to 2/2 when numberOfPlanetsInLayer == 3. this is a sort of interpolation between both positions.
                }
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
                            Line2D line = new Line2D();
                            AddChild(line);
                            line.Points = new Vector2[] { planet.GlobalPosition, neighbour.GlobalPosition };
                        }
                        else
                        {
                            GD.PrintErr("Connected planet is null");
                        }
                    }
                }
            }
        }

        private void SetupSignals()
        {

        }
    }
}