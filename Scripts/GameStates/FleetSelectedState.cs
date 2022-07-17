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

        public FleetSelectedState(GameManager gameManager, Fleet fleet) : base(gameManager)
        {
            this.fleet = fleet;
            stationedPlanet = fleet.StationedPlanet;
        }

        public override void Enter()
        {
            GreyOutFleets();
            GreyOutPlanets();

            fleet.SetSelectable();
            ShowConnectedPlanets();
            ShowPlanetConnections();

            SetupSignals();
            GD.Print("FleetSelectionState.Enter()");
        }


        private void GreyOutPlanets()
        {
            foreach (Planet planet in Planet.Planets)
            {
                planet.SetUnselectable();
            }
        }
        private void ShowConnectedPlanets()
        {
            foreach (Planet connectedPlanet in stationedPlanet.ConnectedPlanets)
            {
                connectedPlanet.SetSelectable();
            }
        }

        private void GreyOutFleets()
        {
            Fleet.Fleets.ForEach(fleet =>
            {
                fleet.SetUnselectable();
            });
        }

        private void ShowPlanetConnections()
        {
            MapManager.Instance.ShowPlanetConnections(stationedPlanet, stationedPlanet.ConnectedPlanets);
        }

        public override void Exit()
        {
            ShowFleets();
            ShowPlanets();
            MapManager.Instance.HidePlanetConnections();

            QueueFree();

            GD.Print("FleetSelectionState.Exit()");
        }

        private void ShowFleets()
        {
            Fleet.Fleets.ForEach(fleet =>
            {
                fleet.SetSelectable();
            });
        }

        private void ShowPlanets()
        {
            Planet.Planets.ForEach(planet =>
            {
                planet.SetSelectable();
            });
        }

        private void SetupSignals()
        {
            Planet.Planets.ForEach(planet =>
            {
                planet.Connect(nameof(Planet.PlanetClicked), this, nameof(OnPlanetSelected));
            });
            InputManager.Instance.Connect(nameof(InputManager.OnCancelAction), this, nameof(OnFleetCanceled));
        }

        private void OnFleetCanceled()
        {
            gameManager.SetState(new ActionSelectionState(gameManager));
        }

        private void OnPlanetSelected(Planet planet)
        {
            if (stationedPlanet.ConnectedPlanets.Contains(planet))
            {
                fleet.MoveFleet(planet);
                gameManager.SetState(new ActionSelectionState(gameManager));
            }
            else
            {
                GD.PrintErr("TODO: Invalid planet selected");
            }
        }
    }
}