using System;
using Entities;
using GameStates;
using Godot;

namespace Managers
{
    public class GameManager : Node2D
    {
        [Export]
        private readonly PackedScene FleetScene;
        [Export]
        private readonly NodePath PlayerNodePath;
        [Export]
        private readonly NodePath HUDNodePath;

        public bool PlayerCanMove => player.MovementAmount > 0;
        public bool PlayerHasFood => player.FoodAmount >= 0;

        private Player player;
        private HUD hud;

        private State previousState;
        private State currentState;

        public override void _Ready()
        {
            player = GetNode<Player>(PlayerNodePath);
            hud = GetNode<HUD>(HUDNodePath);

            SetupSignals();

            for (int i = 0; i < 3; i++)
            {
                Fleet fleet = FleetScene.Instance<Fleet>();
                fleet.MoveFleet(Planet.Planets[0], false);
                fleet.RollShipsInFleet();
            }
            SetState(new ActionSelectionState(this));
        }

        internal void ConsumeFood()
        {
            player.ConsumeFood(Fleet.NumberOfShipsInFleets);
        }

        internal void ConsumeMovement()
        {
            player.ConsumeMovement();
        }

        internal void SetState(State nextState)
        {
            previousState = currentState;
            currentState = nextState;
            previousState?.Exit();
            currentState.Enter();
        }

        public void ActivatePlanet(Planet planet)
        {
            if (planet.ResourceReward == GameResource.Fleet)
            {
                Fleet fleet = FleetScene.Instance<Fleet>();
                fleet.MoveFleet(planet, false);
                fleet.RollShipsInFleet();
            }
            player.GetRewards(planet.ResourceReward, planet.RewardAmount);
        }

        private void SetupSignals()
        {
            hud.ConnectPlayerSignals(player);
        }
    }
}