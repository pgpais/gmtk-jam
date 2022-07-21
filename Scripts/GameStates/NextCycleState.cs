using Entities;
using GameStates;
using Godot;
using Managers;

public class NextCycleState : State
{
    public NextCycleState()
    {

    }

    public NextCycleState(GameManager gameManager) : base(gameManager)
    {

    }

    public override void Enter()
    {
        if (MapManager.Instance.GetLastPlanet.PlanetGoalMet)
        {
            GD.Print("GAME WON");
            HUD.Instance.ShowGameEndScreen(true, "You reached the final planet!");
        }

        ConsumeFood();

        Planet.Planets.ForEach(planet =>
        {
            if (planet.PlanetGoalMet)
            {
                gameManager.ActivatePlanet(planet);
            }
        });

        if (!gameManager.PlayerCanMove)
        {
            // gameManager.SetState(new GameOverState(gameManager));
            GD.Print("GAME OVER: Player can't move");
            HUD.Instance.ShowGameEndScreen(false, "You ran out of movement!");
        }
        else if (!gameManager.PlayerHasFood)
        {
            GD.Print("GAME OVER: You ran out of food!");
            HUD.Instance.ShowGameEndScreen(false, "You ran out of food!");
        }
        else
        {
            gameManager.SetState(new ActionSelectionState(gameManager));
        }

        Fleet.Fleets.ForEach(fleet =>
        {
            fleet.RollShipsInFleet();
        });
    }

    public override void Exit()
    {

    }

    private void ConsumeFood()
    {
        gameManager.ConsumeFood();
    }
}