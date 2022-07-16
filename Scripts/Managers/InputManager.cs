using Godot;

namespace Managers
{
    public class InputManager : Node
    {
        public static InputManager Instance;

        [Signal]
        public delegate void OnCancelAction();

        public override void _Ready()
        {
            base._Ready();
            if (Instance != null)
            {
                QueueFree();
            }
            else
            {
                Instance = this;
            }
        }

        public override void _Input(InputEvent @event)
        {
            base._Input(@event);
            if (Input.IsActionJustReleased("ui_cancel"))
            {
                EmitSignal(nameof(OnCancelAction));
            }
        }
    }
}