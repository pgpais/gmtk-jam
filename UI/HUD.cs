using System;
using Godot;

public class HUD : CanvasLayer
{
    public static HUD Instance;

    [Signal]
    public delegate void PlanetInfoTabCloseButtonPressed();
    [Signal]
    public delegate void NextCycleButtonPressed();

    [Export]
    private readonly NodePath NextCycleButtonNodePath;
    [Export]
    private readonly NodePath FoodHintLabelNodePath;
    [Export]
    private readonly NodePath FleetHintLabelNodePath;
    [Export]
    private readonly NodePath MovementHintLabelNodePath;
    [Export]
    private readonly NodePath PlanetInfoTabNodePath;

    private Button nextCycleButton;
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

        nextCycleButton = GetNode<Button>(NextCycleButtonNodePath);
        foodHintLabel = GetNode<Label>(FoodHintLabelNodePath);
        fleetHintLabel = GetNode<Label>(FleetHintLabelNodePath);
        movementHintLabel = GetNode<Label>(MovementHintLabelNodePath);
        planetInfoTab = GetNode<PlanetInfoTab>(PlanetInfoTabNodePath);

        ConnectSignals();
    }

    private void ConnectSignals()
    {
        planetInfoTab.Connect(nameof(PlanetInfoTab.CloseButtonPressed), this, nameof(OnCloseButtonPressed));
    }

    private void OnCloseButtonPressed()
    {
        EmitSignal(nameof(PlanetInfoTabCloseButtonPressed));
    }

    private void OnNextCycleButtonPressed()
    {
        EmitSignal(nameof(NextCycleButtonPressed));
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

    public void GreyOutInteractables()
    {
        nextCycleButton.Disabled = true;
    }

    public void UnGreyOutInteractables()
    {
        nextCycleButton.Disabled = false;
    }
}
