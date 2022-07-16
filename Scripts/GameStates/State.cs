namespace GameStates
{
    public abstract class State
    {
        public virtual string Name { get; private set; } = "State";

        private readonly GameManager gameManager;

        public State(GameManager gameManager)
        {
            this.gameManager = gameManager;
        }

        public abstract void Enter();
        public abstract void Exit();
    }
}