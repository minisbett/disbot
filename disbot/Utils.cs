using System.Globalization;
using System.Reflection;
using disbot.Modules;
using Discord;
using Discord.WebSocket;
using Module = disbot.Modules.Module;

namespace disbot;

internal static class Utils
{
  public static Func<SocketSlashCommand, Task> CreateCommandHandler(MethodInfo method, Module module)
  {
    return (Func<SocketSlashCommand, Task>) Delegate.CreateDelegate(typeof(Func<SocketSlashCommand, Task>), module, method);
  }

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