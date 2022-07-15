using Godot;

public class Player : Node
{
    [Signal]
    public delegate void FoodChanged(int foodAmount);
    [Signal]
    public delegate void FleetChanged(int fleetAmount);
    [Signal]
    public delegate void MovementChanged(int movementAmount);

    public int FoodAmount { get; private set; }
    public int FleetAmount { get; private set; }
    public int MovementAmount { get; private set; }

    public override void _Ready()
    {
        FoodAmount = 10;
        FleetAmount = 3;
        MovementAmount = 1;
    }
}