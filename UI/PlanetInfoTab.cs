using System;
using Godot;

public class PlanetInfoTab : PanelContainer
{
    [Signal]
    public delegate void CloseButtonPressed();
    [Export]
    private readonly NodePath planetBonusTextureRectPath;
    [Export]
    private readonly NodePath planetBonusTextPath;
    [Export]
    private readonly Texture wheatTexture;
    [Export]
    private readonly Texture movementTexture;
    [Export]
    private readonly Texture fleetTexture;

    private TextureRect planetBonusTextureRect;
    private Label planetBonusText;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Visible = false;
        planetBonusTextureRect = GetNode<TextureRect>(planetBonusTextureRectPath);
        planetBonusText = GetNode<Label>(planetBonusTextPath);
    }

    public void SetPlanetBonus(GameResource resource, int amount)
    {
        switch (resource)
        {
            case GameResource.Food:
                planetBonusTextureRect.Texture = wheatTexture;
                break;
            case GameResource.Movement:
                planetBonusTextureRect.Texture = movementTexture;
                break;
            case GameResource.Fleet:
                planetBonusTextureRect.Texture = fleetTexture;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(resource), resource, null);
        }
        planetBonusText.Text = "+" + amount.ToString();
    }

    public void OnCloseButtonPressed()
    {
        EmitSignal(nameof(CloseButtonPressed));
    }
}
