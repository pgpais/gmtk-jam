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

        private int shipsInPlanet = 0;
        List<Position2D> fleetPositions = new List<Position2D>();
        List<Fleet> fleets = new List<Fleet>();
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
    }
}