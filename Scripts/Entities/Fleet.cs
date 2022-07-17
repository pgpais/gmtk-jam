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

    private bool selectable = true;

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

    public void SetUnselectable()
    {
        selectable = false;
        //grey out object
        Modulate = new Color(0.5f, 0.5f, 0.5f, 1);
    }

    public void SetSelectable()
    {
        selectable = true;
        //reset color
        Modulate = new Color(1, 1, 1, 1);
    }

    public override void _InputEvent(Object viewport, InputEvent @event, int shapeIdx)
    {
        base._InputEvent(viewport, @event, shapeIdx);
        if (@event is InputEventMouseButton button && button.Pressed && selectable)
        {
            EmitSignal(nameof(FleetClicked), this);
            GD.Print("Fleet clicked");
        }
    }

    public void MoveFleet(Planet targetPlanet, bool wasPlayerAction)
    {
        bool planetsAreConnect = true;
        if (planetsAreConnect)
        {
            Planet previousPlanet = StationedPlanet;
            SetStationedPlanet(targetPlanet);
            previousPlanet?.RemoveStationedFleet(this);
            StationedPlanet.AddStationedFleet(this);

            if (wasPlayerAction)
            {
                EmitSignal(nameof(FleetMoved), this, targetPlanet);
            }
        }
    }

    public void SetStationedPlanet(Planet planet)
    {
        StationedPlanet = planet;
    }

    public void RollShipsInFleet()
    {
        ShipsInFleet = new System.Random().Next(1, 6);
        dieSprites[ShipsInFleet - 1].Visible = true;
        for (int i = 0; i < dieSprites.Count; i++)
        {
            if (i != ShipsInFleet - 1)
            {
                dieSprites[i].Visible = false;
            }
        }
    }

    public static void GreyOutFleets()
    {
        foreach (Fleet fleet in Fleets)
        {
            fleet.SetUnselectable();
        }
    }

    public static void UngreyFleets()
    {
        foreach (Fleet fleet in Fleets)
        {
            fleet.SetSelectable();
        }
    }
}