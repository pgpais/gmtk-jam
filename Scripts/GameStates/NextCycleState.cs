using Entities;
using GameStates;
using Managers;

public class NextCycleState : State
{
    public NextCycleState(GameManager gameManager) : base(gameManager)
    {

    }

    public override void Enter()
    {
        Planet.Planets.ForEach(planet =>
        {
            if (planet.PlanetGoalMet)
            {
                gameManager.ActivatePlanet(planet);
            }
        });
        Fleet.Fleets.ForEach(fleet =>
        {
            fleet.RollShipsInFleet();
        });
        gameManager.SetState(new ActionSelectionState(gameManager));
    }

    public override void Exit()
    {

    }
}