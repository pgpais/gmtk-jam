using System;
using Godot;

namespace Managers
{
    public class MapManager : Node
    {
        public override void _Ready()
        {
            base._Ready();
            SetupSignals();
        }

        private void SetupSignals()
        {

        }
    }
}