using System;
using System.Collections.Generic;
using Godot;

namespace Entities
{
    public class Planet : Node2D
    {
        [Export]
        public int PlanetGoal { get; private set; }

        [Export]
        public List<NodePath> FleetPositionPaths { get; private set; }
        [Export]
        public NodePath PlanetGoalLabelPath { get; private set; }

        public List<Planet> ConnectedPlanets { get; private set; } = new List<Planet>();



        public int ShipsInPlanet { get; private set; } = 0;
        List<Position2D> fleetPositions = new List<Position2D>();
        List<Fleet> fleetsInPlanet = new List<Fleet>();
        private Label planetGoalLabel;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            planetGoalLabel = GetNode<Label>(PlanetGoalLabelPath);
            planetGoalLabel.Text = PlanetGoal.ToString();

            foreach (NodePath fleetPositionPath in FleetPositionPaths)
            {
                fleetPositions.Add(GetNode<Position2D>(fleetPositionPath));
            }
        }

        public void AddFleet(Fleet fleet)
        {
            if (fleetsInPlanet.Count >= fleetPositions.Count)
            {
                //TODO: handle error. show something on screen.
                throw new Exception("Too many fleets on planet");
            }
            else
            {
                fleetsInPlanet.Add(fleet);
                fleet.SetStationedPlanet(this);
                ShipsInPlanet += fleet.ShipsInFleet;
                UpdateFleetPositions();
            }
        }

        public void RemoveFleet(Fleet fleet)
        {
            if (fleetsInPlanet.Contains(fleet))
            {
                fleetsInPlanet.Remove(fleet);
                ShipsInPlanet -= fleet.ShipsInFleet;
                UpdateFleetPositions();
            }
            else
            {
                throw new Exception("Fleet not found on planet");
            }
        }

        private void UpdateFleetPositions()
        {
            fleetsInPlanet.Sort((a, b) => a.ShipsInFleet.CompareTo(b.ShipsInFleet));

            for (var i = 0; i < fleetsInPlanet.Count; i++)
            {
                var fleet = fleetsInPlanet[i];
                var position = fleetPositions[i];
                position.AddChild(fleet);
                fleet.Position = Vector2.Zero;
            }
        }
    }
}