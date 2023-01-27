using Discord;

namespace disbot.Commands;

public class SlashCommand : SlashCommandBuilder
{
    public SlashCommand(string name, string description = "", bool isNsfw = false, bool isDMEnabled = false, GuildPermission? defaultPermissions = null)
    {
        Name = name;
        Description = description;
        IsNsfw = isNsfw;
        IsDMEnabled = isDMEnabled;
        DefaultMemberPermissions = defaultPermissions;
    }

    public SlashCommand AddSubCommand(string name, string description, Action<SlashSubCommand>? builder = null)
    {
        ArgumentException.ThrowIfNullOrEmpty(name);
        
        SlashSubCommand subCommand = new SlashSubCommand();
        builder?.Invoke(subCommand);
        subCommand.Name = name;
        subCommand.Description = description;
        subCommand.Type = ApplicationCommandOptionType.SubCommand;
        AddOption(subCommand);

        return this;
    }

    public SlashCommand AddStringOption(string name, string description, Action<SlashCommandOptionBuilder>? builder = null)
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

    public SlashCommand AddIntegerOption(string name, string description, Action<SlashCommandOptionBuilder>? builder = null)
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
    

    public SlashCommand AddBoolOption(string name, string description, Action<SlashCommandOptionBuilder>? builder = null)
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
    

    public SlashCommand AddUserOption(string name, string description, Action<SlashCommandOptionBuilder>? builder = null)
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
    

    public SlashCommand AddChannelOption(string name, string description, Action<SlashCommandOptionBuilder>? builder = null)
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
    

    public SlashCommand AddRoleOption(string name, string description, Action<SlashCommandOptionBuilder>? builder = null)
    {
        SlashCommandOptionBuilder option = new SlashCommandOptionBuilder();
        builder?.Invoke(option);
        option.Name = name;
        option.Description = description;
        option.Type = ApplicationCommandOptionType.Role;
        AddOption(option);

        return this;
    }

    public SlashCommand AddMentionableOption(string name, string description, Action<SlashCommandOptionBuilder> builder)
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

    public SlashCommand AddDecimalOption(string name, string description, Action<SlashCommandOptionBuilder>? builder = null)
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

    public SlashCommand AddAttachmentOption(string name, string description, Action<SlashCommandOptionBuilder>? builder = null)
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

