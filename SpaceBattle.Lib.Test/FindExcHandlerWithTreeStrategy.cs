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

        var ExceptionDict = new Mock<IDictionary<Int32, IStrategy>>();
        ExceptionDict.Setup(ed => ed[It.IsAny<Int32>()]).Returns(handler.Object);

        var HandlerDict = new Mock<IDictionary<Int32, IDictionary<Int32, IStrategy>>>();
        HandlerDict.Setup(hd => hd[It.IsAny<Int32>()]).Returns(ExceptionDict.Object);

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetExceptionTree", (object[] args) => HandlerDict.Object).Execute();

        var CommandHash = new Mock<ICommand>().GetHashCode();
        var ExceptionHash = new Mock<Exception>().GetHashCode();

        var FindExcHandlerStrategy = new FindHandlerWithTreeStrategy();

        var teststrategy = FindExcHandlerStrategy.Execute(new object[] {CommandHash, ExceptionHash});

        // Act
        // Assert
        Assert.NotNull(teststrategy);
    }
}
