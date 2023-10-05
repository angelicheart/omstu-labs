namespace SpaceBattle.Lib;

public class InterpretationTest
{
    public InterpretationTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        /*
        Реализована команда, которая извлекает не блокируемым образом из очереди сообщение, создает команду интерпретации сообщения и складывает ее в очередь соответствующей игры.
        Команда из пункта 1 вызывается как часть обработки каждой игры, чтобы обеспечить хорошую реакцию сервера на входящие сообщения.
        Команда интерпретации должна создавать команду, соответствующую сообщению и ставить эту команду в очередь игры.
        */

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "PushInQueue", (object[] args) => new InQueueStrategy().Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetQueue", (object[] args) => new GetQueueStrategy().Execute(args)).Execute();
    }
}