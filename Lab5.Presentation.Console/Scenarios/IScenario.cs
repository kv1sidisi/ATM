namespace Itmo.ObjectOrientedProgramming.Lab5.Presentation.Scenaries;

public interface IScenario
{
    string Name { get; }

    void Run();
}