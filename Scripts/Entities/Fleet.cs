using System.Collections.Generic;
using Godot;

public class Fleet : Node2D
{
    /// <summary>
    /// Value in die. How many ships are in the fleet.
    /// </summary>
    [Export(PropertyHint.Range, "1,6,1")]
    public int ShipsInFleet { get; private set; }

    [Export]
    public List<NodePath> DieSpritesPath { get; private set; }

    readonly List<Sprite> dieSprites = new List<Sprite>();

    public override void _Ready()
    {
        foreach (NodePath dieSpritePath in DieSpritesPath)
        {
            var dieSprite = GetNode<Sprite>(dieSpritePath);
            dieSprites.Add(dieSprite);

            dieSprite.Visible = false;
        }

        dieSprites[ShipsInFleet - 1].Visible = true;
    }
}