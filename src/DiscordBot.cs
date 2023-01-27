using disbot.Commands;
using disbot.Modules;
using Disbot;
using Discord;
using Discord.WebSocket;
using System.Linq;
using System.Reflection;
using Module = disbot.Modules.Module;

namespace disbot;

public class DiscordBot
{
    /// <summary>
    /// Discord.NET's socket client internally used by this instance.
    /// </summary>
    public DiscordSocketClient Client { get; } = new DiscordSocketClient();

    /// <summary>
    /// The module resolver used to add modules to this DiscordBot instance.
    /// </summary>
    public ModuleResolver ModuleResolver { get; } = new ModuleResolver();

    private string _token;

    private bool _running;

    private Func<DiscordSocketClient, Task>? _configurator;

    private CancellationTokenSource _runCancellationToken = new CancellationTokenSource();

    private Dictionary<string[], Func<SocketSlashCommand, Task>> _commandHandlerLinks = new Dictionary<string[], Func<SocketSlashCommand, Task>>();

    /// <summary>
    /// Creates a new DiscordBot instance with the specified bot token.
    /// </summary>
    /// <param name="token">The authentication token of the discord bot.</param>
    private DiscordBot(string token)
    {
        _token = token;
    }

    /// <summary>
    /// Allows to provide a configuration delegate that is ran after the underlying DiscordSocketClient is connected.
    /// This can include stuff such as configuring the bot's initial activity status on startup.
    /// </summary>
    /// <param name="configurator">The configuration delegate to run.</param>
    public void Configure(Func<DiscordSocketClient, Task> configurator)
    {
        _configurator = configurator;
    }

    /// <summary>
    /// Runs the Discord bot and blocks the thread until the bot is no longer running.
    /// </summary>
    public async Task Run()
    {
        if (_running)
            throw new InvalidOperationException("This DiscordBot instance is already running.");

        // Subscribe to default events the wrapper needs to work.
        Client.Log += Log;
        Client.Ready += Ready;
        Client.SlashCommandExecuted += SlashCommandExecuted;

        // Log into the client and start the discord bot.
        await Client.LoginAsync(TokenType.Bot, _token);
        await Client.StartAsync();
        _running = true;

        // Wait until the client is no longer running
        await Task.Delay(-1, _runCancellationToken.Token);
    }

    /// <summary>
    /// Stops the Discord bot.
    /// </summary>
    public async void Stop()
    {
        if (!_running)
            throw new InvalidOperationException("This DiscordBot instance is not running.");

        // Stop the discord bot.
        await Client.StopAsync();
        _running = false;

        // Release the block on the Run() method.
        _runCancellationToken.Cancel();
    }

    /// <summary>
    /// Event triggered when the DiscordSocketClient logs a message.
    /// </summary>
    /// <param name="msg">The message being logged from the DiscordSocketClient.</param>
    private Task Log(LogMessage msg)
    {
        string timestamp = DateTime.Now.ToLongTimeString();

        if (msg.Exception != null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{timestamp} [{msg.Source}/Exception]: {msg.Exception.Message}");
            Console.WriteLine(msg.Exception.StackTrace);
            Console.ForegroundColor = ConsoleColor.Gray;
        }
        else
            Console.WriteLine($"{timestamp} [{msg.Source}/{msg.Severity.ToString()}]: {msg.Message}");

        return Task.CompletedTask;
    }

    /// <summary>
    /// Event triggered whenever the DiscordSocketClient established 
    /// a connection successfully and loaded the guild and user data.
    /// </summary>
    private async Task Ready()
    {
        // Run the configurator if one has been specified.
        await (_configurator?.Invoke(Client) ?? Task.CompletedTask);

        // Unregister all commands from this bot before re-registering them.
        foreach (SocketApplicationCommand command in await Client.GetGlobalApplicationCommandsAsync())
            await command.DeleteAsync();

        // Perform the initial processing of all modules.
        foreach (Module module in ModuleResolver.Modules)
        {
            // Pass objects like the DiscordSocketClient to the module so it's code can access it.
            module.Bot = Client;
            module.Cache = new ModuleCache(Path.Combine(".data", "modules", module.Id.ToLower()));

            // Initialize the module.
            await module.Initialize();

            // Register all commands from the module and save them to link them to the handlers afterwards.
            List<SocketApplicationCommand> commands = new List<SocketApplicationCommand>();
            foreach (SlashCommandBuilder builder in module.CreateCommands())
                commands.Add(await Client.CreateGlobalApplicationCommandAsync(builder.Build()));

            // Go through all command handlers defined in the module and link them to their command.
            foreach (MethodInfo method in module.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance))
            {
                // Check whether the method has a command handler attribute.
                CommandHandlerAttribute? attrib = method.GetCustomAttribute<CommandHandlerAttribute>();
                if (attrib == null)
                    continue;

                // Checke whether the method has the correct signature. If not, throw an exception.
                if (method.ReturnType != typeof(Task) ||
                    method.GetParameters().Length != 1 ||
                    method.GetParameters()[0].ParameterType != typeof(SocketSlashCommand))
                    throw new InvalidCastException($"The command handler '{module.GetType().Name}.{method.Name}' has an invalid signature.");

                // Add the handler and it's command path to the link collection.
                _commandHandlerLinks.Add(attrib.CommandPath, Utils.CreateCommandHandler(method, module));
            }
        }

        // Unsubscribe from the event as stuff only needs to be initialized once
        // and Ready is being raised multiple times through-out the life-cycle
        // as Discord's servers sometimes request reconnects from bots.
        Client.Ready -= Ready;
    }

    /// <summary>
    /// Event triggered whenever a slash command registered by this bot was executed.
    /// </summary>
    private async Task SlashCommandExecuted(SocketSlashCommand command)
    {
        // Get the command path of the executed command.
        IEnumerable<string> path = Utils.GetCommandPath(command);

        // Look for a handler for the command and execute it.
        Func<SocketSlashCommand, Task> handler = _commandHandlerLinks.FirstOrDefault(x => Enumerable.SequenceEqual(x.Key, path)).Value;
        await (handler?.Invoke(command) ?? Task.CompletedTask);
    }

    /// <summary>
    /// Creates a new DiscordBot instance with the specified bot token.
    /// </summary>
    /// <param name="token">The authentication token of the discord bot.</param>
    public static DiscordBot FromToken(string token)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(token);

        return new DiscordBot(token);
    }

    /// <summary>
    /// Creates a new DiscordBot instance with a token stored in plain text in the specified file.
    /// </summary>
    /// <param name="tokenFile">The file containing The authentication token of the discord bot in plain text.</param>
    public static DiscordBot FromTokenFile(string tokenFile)
    {
        if (!File.Exists(tokenFile))
            throw new FileNotFoundException("The token file could not be found.");

        return FromToken(File.ReadAllText(tokenFile));
    }
}