using System;
using Godot;

public class HUD : CanvasLayer
{
    [Export]
    public NodePath FoodHintLabelNodePath { get; private set; }
    [Export]
    public NodePath FleetHintLabelNodePath { get; private set; }
    [Export]
    public NodePath MovementHintLabelNodePath { get; private set; }

    private Label foodHintLabel;
    private Label fleetHintLabel;
    private Label movementHintLabel;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        foodHintLabel = GetNode<Label>(FoodHintLabelNodePath);
        fleetHintLabel = GetNode<Label>(FleetHintLabelNodePath);
        movementHintLabel = GetNode<Label>(MovementHintLabelNodePath);
    }

    public void ConnectPlayerSignals(Player player)
    {
        player.Connect("FoodChanged", this, nameof(OnFoodChanged));
        player.Connect("FleetChanged", this, nameof(OnFleetChanged));
        player.Connect("MovementChanged", this, nameof(OnMovementChanged));

        foodHintLabel.Text = "x" + player.FoodAmount;
        fleetHintLabel.Text = "x" + player.FleetAmount;
        movementHintLabel.Text = "x" + player.MovementAmount;
    }

    private void OnMovementChanged(int amount)
    {
        movementHintLabel.Text = amount.ToString();
    }

    private void OnFleetChanged(int amount)
    {
        fleetHintLabel.Text = amount.ToString();
    }

    private void OnFoodChanged(int amount)
    {
        foodHintLabel.Text = amount.ToString();
    }
}
