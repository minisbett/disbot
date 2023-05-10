namespace disbot;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public sealed class CommandHandlerAttribute : Attribute
{
  public string[] CommandPath { get; }

  public CommandHandlerAttribute(params string[] commandPath)
  {
    ArgumentNullException.ThrowIfNull(commandPath);

    CommandPath = commandPath;
  }
}