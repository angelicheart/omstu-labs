using System.Data;
using System.Security.Cryptography;

namespace SpaceBattle.Lib;

public class CreateCommandStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        IMessage message = (IMessage) args[0];

        command = //Resolve
        return command;
    }
}