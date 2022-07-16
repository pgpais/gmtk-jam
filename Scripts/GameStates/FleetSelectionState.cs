using Entities;
using Godot;
using Managers;

namespace GameStates
{
    internal class FleetSelectionState : State
    {
        public override string StateName => "FleetSelectionState";
        private readonly Fleet fleet;

        public FleetSelectionState(GameManager gameManager, Fleet fleet) : base(gameManager)
        {
            this.fleet = fleet;
        }

        public override void Enter()
        {
            Fleet.Fleets.ForEach(fleet =>
            {
                fleet.Visible = false;
            });
            fleet.Visible = true;


            SetupSignals();
            GD.Print("FleetSelectionState.Enter()");
        }

        public override void Exit()
        {
            Fleet.Fleets.ForEach(fleet =>
            {
                fleet.Visible = true;
            });

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

        private void OnFleetCanceled()
        {
            gameManager.SetState(new ActionSelectionState(gameManager));
        }

        private void OnPlanetSelected(Planet planet)
        {
            fleet.MoveFleet(planet);
            gameManager.SetState(new ActionSelectionState(gameManager));
        }
    }
}