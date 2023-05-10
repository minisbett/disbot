using disbot.Commands;
using Discord;
using Discord.WebSocket;

namespace disbot.Modules;

public abstract class Module
{
  /// <summary>
  /// The DiscordSocketClient instance used by the bot this module belongs to.
  /// </summary>
  public DiscordSocketClient Bot { get; internal set; } = null!;

  /// <summary>
  /// The data cache of this module. Can hold objects that are
  /// being serialized as JSON and stored as a file on the disk.
  /// </summary>
  public ModuleCache Cache { get; internal set; } = null!;

  /// <summary>
  /// A unique identifier for the module.
  /// </summary>
  public abstract string Id { get; }

  /// <summary>
  /// Method called after Initialize() which returns all commands that will be linked to the module.
  /// </summary>
  public virtual IEnumerable<SlashCommandBuilder> CreateCommands() { return Enumerable.Empty<SlashCommandBuilder>(); }

  /// <summary>
  /// Method called after the DiscordSocketClient connected to the Discord gateway.
  /// Can be used to initializes certain stuff like cache values.
  /// </summary>
  /// <returns></returns>
  public virtual Task Initialize() { return Task.CompletedTask; }
}