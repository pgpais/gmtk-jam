using System;
using Entities;
using Godot;
using Managers;

namespace GameStates
{
    internal class FleetSelectedState : State
    {
        public override string StateName => "FleetSelectedState";
        private readonly Fleet fleet;
        private readonly Planet stationedPlanet;

        public FleetSelectedState()
        {

        }

        public FleetSelectedState(GameManager gameManager, Fleet fleet) : base(gameManager)
        {
            this.fleet = fleet;
            stationedPlanet = fleet.StationedPlanet;
        }

        public override void Enter()
        {
            Fleet.GreyOutFleets();
            Planet.GreyOutPlanets();
            HUD.Instance.GreyOutInteractables();

            fleet.SetSelectable();
            ShowConnectedPlanets();
            ShowPlanetConnections();

            SetupSignals();
            GD.Print("FleetSelectionState.Enter()");
        }

        private void ShowConnectedPlanets()
        {
            foreach (Planet connectedPlanet in stationedPlanet.ConnectedPlanets)
            {
                connectedPlanet.SetSelectable();
            }
        }

        private void ShowPlanetConnections()
        {
            MapManager.Instance.ShowPlanetConnections(stationedPlanet, stationedPlanet.ConnectedPlanets);
        }

        public override void Exit()
        {
            Fleet.UngreyFleets();
            Planet.UngreyPlanets();
            HUD.Instance.UnGreyOutInteractables();
            MapManager.Instance.HidePlanetConnections();

            DisconnectSignals();
            QueueFree();

            GD.Print("FleetSelectionState.Exit()");
        }

        private void SetupSignals()
        {
            Planet.Planets.ForEach(planet =>
            {
                planet.Connect(nameof(Planet.PlanetClicked), this, nameof(OnPlanetSelected));
            });
            InputManager.Instance.Connect(nameof(InputManager.OnCancelAction), this, nameof(OnFleetCanceled));
        }

        private void DisconnectSignals()
        {
            Planet.Planets.ForEach(planet =>
            {
                planet.Disconnect(nameof(Planet.PlanetClicked), this, nameof(OnPlanetSelected));
            });
            InputManager.Instance.Disconnect(nameof(InputManager.OnCancelAction), this, nameof(OnFleetCanceled));
        }

        private void OnFleetCanceled()
        {
            gameManager.SetState(new ActionSelectionState(gameManager));
        }

        private void OnPlanetSelected(Planet planet)
        {
            if (stationedPlanet.ConnectedPlanets.Contains(planet))
            {
                gameManager.ConsumeMovement();
                fleet.MoveFleet(planet, true);
                gameManager.SetState(new ActionSelectionState(gameManager));
            }
            else
            {
                GD.PrintErr("TODO: Invalid planet selected");
            }
        }
    }
}