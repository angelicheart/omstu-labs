namespace SpaceBattle.Lib;

public class EmptyCommand : ICommand {
    public void Execute () {

    }
}

public class ExceptionCommand : ICommand {
    public void Execute () {
        throw new Exception("123");
    }
}
