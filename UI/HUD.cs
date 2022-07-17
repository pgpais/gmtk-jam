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
    [Export]
    private readonly NodePath EndGamePopupTextNodePath;
    [Export]
    private readonly NodePath EndGamePopupNodePath;

    private Button nextCycleButton;
    private Label foodHintLabel;
    private Label fleetHintLabel;
    private Label movementHintLabel;
    private PlanetInfoTab planetInfoTab;
    private Label endGameText;
    private Popup endGamePopup;

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
        endGameText = GetNode<Label>(EndGamePopupTextNodePath);
        endGamePopup = GetNode<Popup>(EndGamePopupNodePath);

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
        player.Connect(nameof(Player.FoodChanged), this, nameof(OnFoodChanged));
        player.Connect(nameof(Player.FleetChanged), this, nameof(OnFleetChanged));
        player.Connect(nameof(Player.MovementChanged), this, nameof(OnMovementChanged));

        foodHintLabel.Text = "x" + player.FoodAmount;
        fleetHintLabel.Text = "x" + player.FleetAmount;
        movementHintLabel.Text = "x" + player.MovementAmount;
    }

    private void OnMovementChanged(int amount)
    {
        movementHintLabel.Text = "x" + amount.ToString();
    }

    private void OnFleetChanged(int amount)
    {
        fleetHintLabel.Text = "x" + amount.ToString();
    }

    private void OnFoodChanged(int amount)
    {
        foodHintLabel.Text = "x" + amount.ToString();
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

    public void ShowGameEndScreen(bool gameWon, string message)
    {
        string text = "";
        if (gameWon)
        {
            //show win screen
            text = "You won!";
        }
        else
        {
            //show lose screen
            text = "You lost!\n" + message;
        }

        //show game end screen
        endGameText.Text = text;
        endGamePopup.Popup_();
    }

    public void OnQuitButtonPressed()
    {
        GetTree().Quit();
    }
}
