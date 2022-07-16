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

            SetupSignals();
        }

        private void CreateMap()
        {
            var layer = new List<Planet>();
            PlanetLayers.Add(layer);
            var planet = PlanetScene.Instance<Planet>();
            AddChild(planet);

            planet.GlobalPosition = firstPlanetPosition.GlobalPosition; //go from 0/2 to 2/2 when numberOfPlanetsInLayer == 3. this is a sort of interpolation between both positions.

            for (int i = 1; i < amountOfLayers; i++)
            {
                layer = new List<Planet>();
                PlanetLayers.Add(layer);
                float numberOfPlanetsInLayer = i * 2 + 1;
                for (float j = 0; j < numberOfPlanetsInLayer; j++)
                {
                    planet = PlanetScene.Instance<Planet>();
                    AddChild(planet);

                    planet.GlobalPosition = firstPlanetPosition.GlobalPosition + new Vector2((j / (numberOfPlanetsInLayer - 1)) * i * planetDistance.x, ((numberOfPlanetsInLayer - 1 - j) / (numberOfPlanetsInLayer - 1)) * i * planetDistance.y); //go from 0/2 to 2/2 when numberOfPlanetsInLayer == 3. this is a sort of interpolation between both positions.
                }
            }
        }

        private void SetupSignals()
        {

        }
    }
}