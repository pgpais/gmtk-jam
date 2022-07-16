using System;
using System.Collections.Generic;
using Godot;
using Object = Godot.Object;

namespace Entities
{
    public class Planet : Area2D
    {
        public static List<Planet> Planets = new List<Planet>();

        [Signal]
        public delegate void PlanetClicked(Planet planet);

        [Export]
        public int PlanetGoal { get; private set; }

        [Export]
        public List<NodePath> FleetPositionPaths { get; private set; }
        [Export]
        public NodePath PlanetGoalLabelPath { get; private set; }
        [Export]
        public List<NodePath> ConnectedPlanetsNodePaths { get; private set; } = new List<NodePath>();

        public List<Planet> ConnectedPlanets { get; private set; } = new List<Planet>();
        public int ShipsInPlanet { get; private set; } = 0;

        readonly List<Position2D> fleetPositions = new List<Position2D>();
        readonly List<Fleet> fleetsInPlanet = new List<Fleet>();
        private Label planetGoalLabel;

        public override void _EnterTree()
        {
            base._EnterTree();
            planetGoalLabel = GetNode<Label>(PlanetGoalLabelPath);
            foreach (NodePath fleetPositionPath in FleetPositionPaths)
            {
                fleetPositions.Add(GetNode<Position2D>(fleetPositionPath));
            }
            foreach (var ConnectedPlanetNodePath in ConnectedPlanetsNodePaths)
            {
                ConnectedPlanets.Add(GetNode<Planet>(ConnectedPlanetNodePath));
            }

            Planets.Add(this);
        }

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            planetGoalLabel.Text = PlanetGoal.ToString();
        }

        override public void _ExitTree()
        {
            base._ExitTree();
            Planets.Remove(this);
        }

        public override void _InputEvent(Object viewport, InputEvent @event, int shapeIdx)
        {
            base._InputEvent(viewport, @event, shapeIdx);
            if (@event is InputEventMouseButton button && button.Pressed)
            {
                EmitSignal(nameof(PlanetClicked), this);
                GD.Print("Planet clicked");
            }
        }

        public void AddStationedFleet(Fleet fleet)
        {
            if (fleetsInPlanet.Count > fleetPositions.Count)
            {
                //TODO: handle error. show something on screen.
                GD.PrintErr("Too many fleets in planet");
            }
            else
            {
                fleetsInPlanet.Add(fleet);
                fleet.Connect(nameof(Fleet.FleetMoved), this, nameof(MoveStationedFleet));

                ShipsInPlanet += fleet.ShipsInFleet;
                UpdateFleetPositions();
            }
        }

        internal void AddConnectedPlanet(Planet neighbour)
        {
            ConnectedPlanets.Add(neighbour);
        }

        public void MoveStationedFleet(Fleet fleet, Planet targetPlanet)
        {
            fleet.MoveFleet(targetPlanet);
        }

        public void RemoveStationedFleet(Fleet fleet)
        {
            if (fleetsInPlanet.Contains(fleet))
            {
                fleetsInPlanet.Remove(fleet);
                fleet.Disconnect(nameof(Fleet.FleetMoved), this, nameof(MoveStationedFleet));

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
                if (fleet.StationedPlanet == this)
                {
                    fleet.GetParent()?.RemoveChild(fleet);
                    position.AddChild(fleet);
                    fleet.Position = Vector2.Zero;
                }
                else
                {
                    position.RemoveChild(fleet);
                }
            }
        }
    }
}