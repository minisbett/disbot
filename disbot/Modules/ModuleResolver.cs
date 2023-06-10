using System.Reflection;
using System.Runtime.CompilerServices;
using disbot.Modules;
using Module = disbot.Modules.Module;

namespace Disbot;

/// <summary>
/// Responsible for resolving the modules of the Discord bot using reflection.
/// </summary>
public class ModuleResolver
{
  /// <summary>
  /// Internal collection of all resolved module.
  /// </summary>
  internal List<Module> Modules { get; } = new List<Module>();

  /// <summary>
  /// Resolves all types inheriting from IModule from the namespace (and optionally sub-namespaces)
  /// that have not been added yet via reflection and adds an instance of them to this resolver.
  /// </summary>
  /// <param name="namesp">The namespace to check for.</param>
  /// <param name="subNamespaces">Bool whether types from sub-namespaces should also be resolved.</param>
  public void ResolveFromNamespace(string namesp, bool subNamespaces = true)
  {
    // Get all types in the calling assembly that meet the namespace requirement.
    IEnumerable<Type> types = Assembly.GetCallingAssembly().GetTypes()
                                .Where(x => x.Namespace != null)
                                .Where(x => x.Namespace == namesp || (subNamespaces && x.Namespace!.StartsWith(namesp)));

    // Get all types that are instantiable, are not compiler generated and have not been resolved yet.
    types = types.Where(x => x is { IsClass: true, IsAbstract: false, IsNested: false }
                          && x.GetConstructors().Any()
                          && x.GetCustomAttribute(typeof(CompilerGeneratedAttribute)) == null
                          && !Modules.Any(m => x.IsInstanceOfType(m)));

    // Go through each resolvec type and check whether a module with the same id already
    // exists and if not, add an instance of all resolved types to the module collection.
    foreach (Type type in types)
    {
      // Create a new module instance.
      Module module = (Module) Activator.CreateInstance(type)!;

      // Check whether a module with the same id already exists.
      if (Modules.Any(x => x.Id.ToLower() == module.Id.ToLower()))
        throw new InvalidOperationException($"A module with Id '{module.Id}' was already resolved.");

      // Add the module.
      Modules.Add(module);
    }
  }

  /// <summary>
  /// Adds an instance of the specified generic type representing a module to this resolver.
  /// All module types must inherit from IModule and cannot be added multiple times.
  /// </summary>
  /// <param name="type">The type of the module to add.</param>
  public ModuleResolver AddModule<T>() where T : disbot.Modules.Module, new()
  {
    // Check whether a module of this type already exists.
    if (Modules.Any(x => typeof(T).IsInstanceOfType(x)))
      throw new InvalidOperationException($"The module {typeof(T).Name} was already resolved.");

    // Create a new module instance.
    Module module = Activator.CreateInstance<T>();

    // Check whether a module with the same id already exists.
    if (Modules.Any(x => x.Id.ToLower() == module.Id.ToLower()))
      throw new InvalidOperationException($"A module with Id '{module.Id}' was already resolved.");

    // Add the module.
    Modules.Add(module);

    return this;
  }
}