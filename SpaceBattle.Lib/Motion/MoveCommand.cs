namespace SpaceBattle.Lib
{
    public class MoveCommand : ICommand
    {
        private readonly IMovable ObjThatMove;
        public MoveCommand(IMovable movable)
        {
            ObjThatMove = movable;
        }
        public void Execute()
        {
            ObjThatMove.position += ObjThatMove.velocity;
        }
    }
}