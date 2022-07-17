using System;
using Entities;
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
        MovementAmount = 6;
    }

    public void AddFood(int amount)
    {
        FoodAmount += amount;
        EmitSignal(nameof(FoodChanged), FoodAmount);
    }

    public void AddFleet(int amount)
    {
        FleetAmount += amount;
        EmitSignal(nameof(FleetChanged), FleetAmount);
    }

    public void AddMovement(int amount)
    {
        MovementAmount += amount;
        EmitSignal(nameof(MovementChanged), MovementAmount);
    }

    internal void GetRewards(GameResource resourceReward, int rewardAmount)
    {
        switch (resourceReward)
        {
            case GameResource.Food:
                AddFood(rewardAmount);
                break;
            case GameResource.Fleet:
                AddFleet(rewardAmount);
                break;
            case GameResource.Movement:
                AddMovement(rewardAmount);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(resourceReward), resourceReward, null);
        }
    }

    internal void ConsumeFood(int numberOfShipsInFleets)
    {
        FoodAmount -= numberOfShipsInFleets;
        EmitSignal(nameof(FoodChanged), FoodAmount);
        if (FoodAmount < 0)
        {
            GD.Print("GAME OVER: You ran out of food!");
            HUD.Instance.ShowGameEndScreen(false, "You ran out of food!");
        }
    }

    internal void ConsumeMovement()
    {
        MovementAmount--;
        EmitSignal(nameof(MovementChanged), MovementAmount);
    }
}