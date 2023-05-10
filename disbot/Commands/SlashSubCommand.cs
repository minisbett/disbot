using Discord;

namespace disbot.Commands;

public class SlashSubCommand : SlashCommandOptionBuilder
{
  public SlashSubCommand AddSubCommand(string name, string description, Action<SlashCommandOptionBuilder>? builder = null)
  {
    ArgumentException.ThrowIfNullOrEmpty(name);

    SlashSubCommand subCommand = new SlashSubCommand();
    builder?.Invoke(subCommand);
    subCommand.Name = name;
    subCommand.Description = description;
    AddOption(subCommand);

    return this;
  }

  public SlashSubCommand AddStringOption(string name, string description, Action<SlashCommandOptionBuilder>? builder = null)
  {
    ArgumentException.ThrowIfNullOrEmpty(name);

    SlashCommandOptionBuilder option = new SlashCommandOptionBuilder();
    builder?.Invoke(option);
    option.Name = name;
    option.Description = description;
    option.Type = ApplicationCommandOptionType.String;
    AddOption(option);

    return this;
  }

  public SlashSubCommand AddIntegerOption(string name, string description, Action<SlashCommandOptionBuilder>? builder = null)
  {
    ArgumentException.ThrowIfNullOrEmpty(name);

    SlashCommandOptionBuilder option = new SlashCommandOptionBuilder();
    builder?.Invoke(option);
    option.Name = name;
    option.Description = description;
    option.Type = ApplicationCommandOptionType.Integer;
    AddOption(option);

    return this;
  }


  public SlashSubCommand AddBoolOption(string name, string description, Action<SlashCommandOptionBuilder>? builder = null)
  {
    ArgumentException.ThrowIfNullOrEmpty(name);

    SlashCommandOptionBuilder option = new SlashCommandOptionBuilder();
    builder?.Invoke(option);
    option.Name = name;
    option.Description = description;
    option.Type = ApplicationCommandOptionType.Boolean;
    AddOption(option);

    return this;
  }

  public SlashSubCommand AddUserOption(string name, string description, Action<SlashCommandOptionBuilder>? builder = null)
  {
    ArgumentException.ThrowIfNullOrEmpty(name);

    SlashCommandOptionBuilder option = new SlashCommandOptionBuilder();
    builder?.Invoke(option);
    option.Name = name;
    option.Description = description;
    option.Type = ApplicationCommandOptionType.User;
    AddOption(option);

    return this;
  }

  public SlashSubCommand AddChannelOption(string name, string description, Action<SlashCommandOptionBuilder>? builder = null)
  {
    ArgumentException.ThrowIfNullOrEmpty(name);

    SlashCommandOptionBuilder option = new SlashCommandOptionBuilder();
    builder?.Invoke(option);
    option.Name = name;
    option.Description = description;
    option.Type = ApplicationCommandOptionType.Channel;
    AddOption(option);

    return this;
  }

  public SlashSubCommand AddRoleOption(string name, string description, Action<SlashCommandOptionBuilder>? builder = null)
  {
    ArgumentException.ThrowIfNullOrEmpty(name);

    SlashCommandOptionBuilder option = new SlashCommandOptionBuilder();
    builder?.Invoke(option);
    option.Name = name;
    option.Description = description;
    option.Type = ApplicationCommandOptionType.Role;
    AddOption(option);

    return this;
  }

  public SlashSubCommand AddMentionableOption(string name, string description, Action<SlashCommandOptionBuilder>? builder = null)
  {
    ArgumentException.ThrowIfNullOrEmpty(name);

    SlashCommandOptionBuilder option = new SlashCommandOptionBuilder();
    builder?.Invoke(option);
    option.Name = name;
    option.Description = description;
    option.Type = ApplicationCommandOptionType.Mentionable;
    AddOption(option);

    return this;
  }

  public SlashSubCommand AddDecimalOption(string name, string description, Action<SlashCommandOptionBuilder>? builder = null)
  {
    ArgumentException.ThrowIfNullOrEmpty(name);

    SlashCommandOptionBuilder option = new SlashCommandOptionBuilder();
    builder?.Invoke(option);
    option.Name = name;
    option.Description = description;
    option.Type = ApplicationCommandOptionType.Number;
    AddOption(option);

    return this;
  }

  public SlashSubCommand AddAttachmentOption(string name, string description, Action<SlashCommandOptionBuilder>? builder = null)
  {
    ArgumentException.ThrowIfNullOrEmpty(name);

    SlashCommandOptionBuilder option = new SlashCommandOptionBuilder();
    builder?.Invoke(option);
    option.Name = name;
    option.Description = description;
    option.Type = ApplicationCommandOptionType.Attachment;
    AddOption(option);

    return this;
  }
}