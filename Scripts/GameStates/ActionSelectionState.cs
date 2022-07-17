using System;
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
        HUD.Instance.Connect(nameof(HUD.NextCycleButtonPressed), this, nameof(OnNextCycleButtonPressed));
    }

    public override void Exit()
    {
        QueueFree();
        GD.Print("ActionSelectionState.Exit()");
    }

    private void DisconnectSignals()
    {
        Fleet.Fleets.ForEach(fleet =>
        {
            fleet.Disconnect(nameof(Fleet.FleetClicked), this, nameof(OnFleetSelected));
        });
        Planet.Planets.ForEach(planet =>
        {
            planet.Disconnect(nameof(Planet.PlanetClicked), this, nameof(OnPlanetSelected));
        });
        HUD.Instance.Disconnect(nameof(HUD.NextCycleButtonPressed), this, nameof(OnNextCycleButtonPressed));
    }

    private void OnFleetSelected(Fleet fleet)
    {
        gameManager.SetState(new FleetSelectedState(gameManager, fleet));
    }

    private void OnPlanetSelected(Planet planet)
    {
        gameManager.SetState(new PlanetSelectedState(gameManager, planet));
    }

    private void OnNextCycleButtonPressed()
    {
        gameManager.SetState(new NextCycleState(gameManager));
    }
}