using Godot;
using Managers;

namespace GameStates
{
    public abstract class State : Node
    {
        public virtual string StateName { get; private set; } = "State";

        protected readonly GameManager gameManager;

        public State(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        public abstract void Enter();
        public abstract void Exit();
    }
}