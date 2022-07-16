using System;
using Godot;

public class MapManager : Node
{
    public override void _Ready()
    {
        base._Ready();
        SetupSignals();
    }

    private void SetupSignals()
    {
        Fleet.Fleets.ForEach(fleet =>
        {
            fleet.Connect(nameof(Fleet.FleetStartMovement), this, nameof(OnFleetStartedMovement));
        });
    }

    private void OnFleetStartedMovement(Fleet movingFleet)
    {
        Fleet.Fleets.ForEach(fleet =>
        {
            fleet.Visible = false;
        });

        movingFleet.Visible = true;
    }

    private void CancelFleetMovement()
    {
        bool isInRightState = true; //TODO: do states in GameManager
        if (isInRightState)
        {
            Fleet.Fleets.ForEach(fleet =>
            {
                fleet.Visible = true;
            });
        }
    }
}