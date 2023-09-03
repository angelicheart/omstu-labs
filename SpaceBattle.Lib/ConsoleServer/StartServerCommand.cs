namespace SpaceBattle.Lib;

public class StartServerCommand : ICommand
{
    private int n_threads;

    public StartServerCommand(int n_threads)
    {
        this.n_threads = n_threads;
    }

    public void Execute()
    {
        try
        {
            for (int id = 0; id < n_threads; id++)
            {
                string thread_id = Convert.ToString(id);
                IoC.Resolve<ICommand>("Game.Threads.CreateAndStart", thread_id).Execute();
            }
        }
        catch (Exception e)
        {
            IoC.Resolve<ICommand>("CatchException", "Start Thread, " + e.Message).Execute();
        }
    }
}
