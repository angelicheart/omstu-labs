namespace SpaceBattle.Lib.Test;

public class FindExcHandlerWithTreeStrategyTests
{
    public FindExcHandlerWithTreeStrategyTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }

    [Fact]
    public void SuccessfulExcHandlerWithTreeStrategy()
    {
        // Arrange
        var handler = new Mock<IStrategy>();

        var ExceptionDict = new Mock<IDictionary<Exception, IStrategy>>();
        ExceptionDict.Setup(ed => ed[It.IsAny<Exception>()]).Returns(handler.Object);

        var HandlerDict = new Mock<IDictionary<ICommand, IDictionary<Exception, IStrategy>>>();
        HandlerDict.Setup(hd => hd[It.IsAny<ICommand>()]).Returns(ExceptionDict.Object);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetExceptionTree", (object[] args) => HandlerDict.Object).Execute();

        var command = new Mock<ICommand>();
        var exception = new Mock<Exception>();

        var FindExcHandlerStrategy = new FindHandlerWithTreeStrategy();

        var teststrategy = FindExcHandlerStrategy.Execute(new object[] {command.Object, exception.Object});
        
        // Act
        // Assert
        Assert.NotNull(teststrategy);
    }
}
