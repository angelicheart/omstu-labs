namespace SpaceBattle.Lib;

class SenderAdapter : ISender
{
    ICommand cmd; 

    public SenderAdapter(ICommand cmd) => this.cmd = cmd;

    public ICommand Send()
    {
        queue.Send();
    }
}