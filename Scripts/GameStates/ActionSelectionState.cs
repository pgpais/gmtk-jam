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

    public void OnFleetSelected(Fleet fleet)
    {
        gameManager.SetState(new FleetSelectionState(gameManager, fleet));
    }
}