namespace SpaceBattle.Lib
{
    public class RotateCommand : ICommand
    {
        private readonly IRotatable ObjThatRotate;
        public RotateCommand(IRotatable rotatable)
        {
            ObjThatRotate = rotatable;
        }
        public void Execute()
        {
            ObjThatRotate.Direction += ObjThatRotate.angleVelocty;
        }
    }
}