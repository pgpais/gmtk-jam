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
        private readonly NodePath SpriteNodePath;
        [Export]
        private readonly List<NodePath> FleetPositionPaths;
        [Export]
        private readonly NodePath PlanetGoalLabelPath;
        [Export]
        private readonly List<NodePath> ConnectedPlanetsNodePaths = new List<NodePath>();

        public GameResource ResourceReward { get; private set; }
        public int RewardAmount { get; private set; }
        public List<Planet> ConnectedPlanets { get; private set; } = new List<Planet>();
        public bool PlanetGoalMet => ShipsInPlanet == PlanetGoal;
        public int ShipsInPlanet { get; private set; } = 0;

        private Sprite sprite;
        readonly List<Position2D> fleetPositions = new List<Position2D>();
        readonly List<Fleet> fleetsInPlanet = new List<Fleet>();
        private Label planetGoalLabel;

        private bool selectable = true;

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
            sprite = GetNode<Sprite>(SpriteNodePath);
        }

        public void SetRewards(GameResource resource, int amount)
        {
            ResourceReward = resource;
            RewardAmount = amount;
        }

        override public void _ExitTree()
        {
            base._ExitTree();
            Planets.Remove(this);
        }

        public override void _InputEvent(Object viewport, InputEvent @event, int shapeIdx)
        {
            base._InputEvent(viewport, @event, shapeIdx);
            if (@event is InputEventMouseButton button && button.Pressed && selectable)
            {
                EmitSignal(nameof(PlanetClicked), this);
                GD.Print("Planet clicked");
            }
        }

        internal void SetGoal(int planetGoal)
        {
            PlanetGoal = planetGoal;
            planetGoalLabel.Text = planetGoal.ToString();
        }

        public void SetUnselectable()
        {
            selectable = false;
            //grey out object
            sprite.Modulate = new Color(0.5f, 0.5f, 0.5f, 1);
        }

        public void SetSelectable()
        {
            selectable = true;
            //reset color
            sprite.Modulate = new Color(1, 1, 1, 1);
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

                ShipsInPlanet += fleet.ShipsInFleet;
                UpdateFleetPositions();
            }
        }

        internal void AddConnectedPlanet(Planet neighbour)
        {
            ConnectedPlanets.Add(neighbour);
        }

        public void RemoveStationedFleet(Fleet fleet)
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

        public static void GreyOutPlanets()
        {
            foreach (var planet in Planet.Planets)
            {
                planet.SetUnselectable();
            }
        }

        public static void UngreyPlanets()
        {
            foreach (var planet in Planet.Planets)
            {
                planet.SetSelectable();
            }
        }
    }
}