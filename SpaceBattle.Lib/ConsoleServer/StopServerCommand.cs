namespace SpaceBattle.Lib;

public class StopServerCommand : ICommand
{
    public int n_threads;

    public StopServerCommand(int n_threads)
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
                IoC.Resolve<ICommand>("Game.Senders.Send", thread_id, IoC.Resolve<ICommand>("Game.Threads.SoftStop", thread_id)).Execute();
            }
        }
        catch (Exception e)
        {
            IoC.Resolve<ICommand>("CatchException", "Soft Stop Thread, " + e.Message).Execute();
        }
    }
}
