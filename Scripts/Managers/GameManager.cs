using System;
using Entities;
using GameStates;
using Godot;

namespace Managers
{
    public class GameManager : Node2D
    {
        [Export]
        public PackedScene FleetScene;
        [Export]
        public NodePath PlayerNodePath { get; private set; }
        [Export]
        public NodePath HUDNodePath { get; private set; }

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

        internal void SetState(State nextState)
        {
            previousState = currentState;
            currentState = nextState;
            previousState?.Exit();
            currentState.Enter();
        }

        public void ActivatePlanet(Planet planet)
        {
            player.GetRewards(planet.ResourceReward, planet.RewardAmount);
        }

        private void SetupSignals()
        {
            hud.ConnectPlayerSignals(player);
        }
    }
}