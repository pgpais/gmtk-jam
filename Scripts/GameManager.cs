using Godot;

public class GameManager : Node2D
{
    [Export]
    public NodePath PlayerNodePath { get; private set; }
    [Export]
    public NodePath HUDNodePath { get; private set; }

    private Player player;
    private HUD hud;

    //TODO: think about how to handle  different states of game (selecting fleet, selecting planet to move, etc.)
    // I am thinking that a state machine would be a good idea.

    public override void _Ready()
    {
        player = GetNode<Player>(PlayerNodePath);
        hud = GetNode<HUD>(HUDNodePath);

        SetupSignals();
    }

    private void SetupSignals()
    {
        hud.ConnectPlayerSignals(player);
    }
}