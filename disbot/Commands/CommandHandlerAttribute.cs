namespace disbot;

/// <summary>
/// Attribute for declaring methods as command handlers for slash commands.
/// </summary>
[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public sealed class CommandHandlerAttribute : Attribute
{
  /// <summary>
  /// The path of the command, consisting of the main command and possible sub commands.
  /// </summary>
  public string[] CommandPath { get; }

  /// <summary>
  /// Delcare the method as a command handler for a slash command with the specified path.
  /// </summary>
  /// <param name="commandPath">The slash command path, consisting of the main command and possible sub commands.</param>
  public CommandHandlerAttribute(params string[] commandPath)
  {
    ArgumentNullException.ThrowIfNull(commandPath);

    CommandPath = commandPath;
  }
}