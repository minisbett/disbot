using System.Globalization;
using System.Reflection;
using disbot.Modules;
using Discord;
using Discord.WebSocket;
using Module = disbot.Modules.Module;

namespace disbot;

/// <summary>
/// Internal class for utility methods.
/// </summary>
internal static class Utils
{
  /// <summary>
  /// Creates a comamnd handler delegate from the specified module and handler MethodInfo.
  /// </summary>
  /// <param name="method"></param>
  /// <param name="module"></param>
  /// <returns>The command handler delegate.</returns>
  public static Func<SocketSlashCommand, Task> CreateCommandHandler(MethodInfo method, Module module)
  {
    return (Func<SocketSlashCommand, Task>) Delegate.CreateDelegate(typeof(Func<SocketSlashCommand, Task>), module, method);
  }


  /// <summary>
  /// Gets the command path of the specified slash command.
  /// This consists of the main command, as well as possible sub commands.
  /// </summary>
  /// <param name="command">The slash command object.</param>
  /// <returns>The path of the slash command.</returns>
  public static IEnumerable<string> GetCommandPath(SocketSlashCommand command)
  {
    // Get the path of the executed command. (base command name followed by all subcommand names)
    // by initially starting with the base command name and stepping further into the nested
    // options until an option that is not of type SubCommand has been found.
    yield return command.CommandName;
    SocketSlashCommandDataOption? option = command.Data.Options.FirstOrDefault(x => x.Type == ApplicationCommandOptionType.SubCommand);
    while (option != null)
    {
      yield return option.Name;
      option = option.Options.FirstOrDefault(x => x.Type == ApplicationCommandOptionType.SubCommand);
    }
  }
}