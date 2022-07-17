using System;
using Entities;
using GameStates;
using Managers;

public class PlanetSelectedState : State
{
    private readonly Planet selectedPlanet;

    public PlanetSelectedState(GameManager gameManager, Planet planet) : base(gameManager)
    {
        selectedPlanet = planet;
    }

    public override void Enter()
    {
        SetupSignals();

        Planet.GreyOutPlanets();
        Fleet.GreyOutFleets();
        selectedPlanet.SetSelectable();
        ShowPlanetScreen();
    }

    private void SetupSignals()
    {
        InputManager.Instance.Connect(nameof(InputManager.OnCancelAction), this, nameof(OnCancelAction));
        HUD.Instance.Connect(nameof(HUD.PlanetInfoTabCloseButtonPressed), this, nameof(OnCancelAction));
    }

    private void DisconnectSignals()
    {
        InputManager.Instance.Disconnect(nameof(InputManager.OnCancelAction), this, nameof(OnCancelAction));
        HUD.Instance.Disconnect(nameof(HUD.PlanetInfoTabCloseButtonPressed), this, nameof(OnCancelAction));
    }

    private void OnCancelAction()
    {
        gameManager.SetState(new ActionSelectionState(gameManager));
    }

    private void ShowPlanetScreen()
    {
        HUD.Instance.ShowPlanetBonus(selectedPlanet.ResourceReward, selectedPlanet.RewardAmount);
    }

    public override void Exit()
    {
        Planet.UngreyPlanets();
        Fleet.UngreyFleets();
        HUD.Instance.HidePlanetBonus();
    }
}