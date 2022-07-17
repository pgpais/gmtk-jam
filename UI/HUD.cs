using System;
using Godot;

public class HUD : CanvasLayer
{
    public static HUD Instance;

    [Export]
    private readonly NodePath FoodHintLabelNodePath;
    [Export]
    private readonly NodePath FleetHintLabelNodePath;
    [Export]
    private readonly NodePath MovementHintLabelNodePath;
    [Export]
    private readonly NodePath PlanetInfoTabNodePath;

    private Label foodHintLabel;
    private Label fleetHintLabel;
    private Label movementHintLabel;
    private PlanetInfoTab planetInfoTab;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            throw new Exception("HUD is a singleton class and can only be instantiated once.");
        }

        foodHintLabel = GetNode<Label>(FoodHintLabelNodePath);
        fleetHintLabel = GetNode<Label>(FleetHintLabelNodePath);
        movementHintLabel = GetNode<Label>(MovementHintLabelNodePath);
        planetInfoTab = GetNode<PlanetInfoTab>(PlanetInfoTabNodePath);
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

    public void ShowPlanetBonus(GameResource resource, int amount)
    {
        planetInfoTab.Visible = true;
        planetInfoTab.SetPlanetBonus(resource, amount);
    }

    public void HidePlanetBonus()
    {
        planetInfoTab.Visible = false;
    }
}
