using Itmo.ObjectOrientedProgramming.Lab5.Core.States;

namespace Itmo.ObjectOrientedProgramming.Lab5.Core.Commands;

public abstract class ICommand
{
    public IState State { get; private set; }
    
}