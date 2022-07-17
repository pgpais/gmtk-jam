using Entities;
using GameStates;
using Godot;
using Managers;

public class ActionSelectionState : State
{
    public override string StateName => "ActionSelectionState";

    public ActionSelectionState(GameManager gameManager) : base(gameManager)
    {
    }

    public override void Enter()
    {
        SetupSignals();

        GD.Print("ActionSelectionState.Enter()");
    }

    private void SetupSignals()
    {
        Fleet.Fleets.ForEach(fleet =>
        {
            fleet.Connect(nameof(Fleet.FleetClicked), this, nameof(OnFleetSelected));
        });
        Planet.Planets.ForEach(planet =>
        {
            planet.Connect(nameof(Planet.PlanetClicked), this, nameof(OnPlanetSelected));
        });
    }

    public override void Exit()
    {
        Fleet.Fleets.ForEach(fleet =>
        {
            fleet.Disconnect(nameof(Fleet.FleetClicked), this, nameof(OnFleetSelected));
        });
        QueueFree();

        GD.Print("ActionSelectionState.Exit()");
    }

    private void OnFleetSelected(Fleet fleet)
    {
        gameManager.SetState(new FleetSelectedState(gameManager, fleet));
    }

    private void OnPlanetSelected(Planet planet)
    {
        gameManager.SetState(new PlanetSelectedState(gameManager, planet));
    }
}