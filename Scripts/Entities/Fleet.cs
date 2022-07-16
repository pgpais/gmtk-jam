using System.Collections.Generic;
using Entities;
using Godot;

public class Fleet : Area2D
{
    public static List<Fleet> Fleets = new List<Fleet>();
    [Signal]
    public delegate void FleetClicked(Fleet fleet);
    [Signal]
    public delegate void FleetMoved(Fleet fleet, Planet planet);

    [Export(PropertyHint.Range, "1,6,1")]
    public int ShipsInFleet { get; private set; } = 1;

    [Export]
    private readonly List<NodePath> DieSpritesPath;

    public Planet StationedPlanet { get; private set; }

    readonly List<Sprite> dieSprites = new List<Sprite>();

    private Planet startingPlanet;

    public Fleet()
    {

    }

    public override void _EnterTree()
    {
        base._EnterTree();
        Fleets.Add(this);
    }

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

    override public void _ExitTree()
    {
        base._ExitTree();
        Fleets.Remove(this);
    }

    public override void _InputEvent(Object viewport, InputEvent @event, int shapeIdx)
    {
        base._InputEvent(viewport, @event, shapeIdx);
        if (@event is InputEventMouseButton button && button.Pressed)
        {
            EmitSignal(nameof(FleetClicked), this);
            GD.Print("Fleet clicked");
        }
    }

    public void MoveFleet(Planet targetPlanet)
    {
        bool planetsAreConnect = true;
        if (planetsAreConnect)
        {
            Planet previousPlanet = StationedPlanet;
            SetStationedPlanet(targetPlanet);
            previousPlanet?.RemoveStationedFleet(this);
            StationedPlanet.AddStationedFleet(this);
        }
    }

    public void SetStationedPlanet(Planet planet)
    {
        StationedPlanet = planet;
    }
}