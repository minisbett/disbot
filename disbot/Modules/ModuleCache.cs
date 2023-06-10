using System.Numerics;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace disbot.Modules;

/// <summary>
/// Class representing the cache for a module.
/// </summary>
public class ModuleCache
{
  /// <summary>
  /// The cache of the module, with strings as keys and objects as values.
  /// </summary>
  private Dictionary<string, object> _cache = new Dictionary<string, object>();

  /// <summary>
  /// The file of the cache.
  /// </summary>
  private string _file;

  /// <summary>
  /// Create a new module cache instance with the specified file path.
  /// </summary>
  /// <param name="file">The file path of the cache file.</param>
  public ModuleCache(string file)
  {
    _file = file;

    // Ensure the cache file exists.
    Directory.CreateDirectory(new FileInfo(file).DirectoryName!);
    if (!File.Exists(file))
      File.WriteAllText(file, JsonConvert.SerializeObject(_cache, Formatting.Indented));

    // Read the cache file and load it into the dictionary.
    string json = File.ReadAllText(file);
    _cache = JsonConvert.DeserializeObject<Dictionary<string, object>>(json)!;
  }

  /// <summary>
  /// Ensure the specified key is initialized and if not, set it to the specified default value.
  /// </summary>
  /// <param name="key">The key.</param>
  /// <param name="value">The default vlaue.</param>
  public void SetDefault(string key, object value)
  {
    if (!_cache.ContainsKey(key))
      this[key] = value;
  }

  /// <summary>
  /// Get the value of the specified key.
  /// </summary>
  /// <typeparam name="T">The type of the value.</typeparam>
  /// <param name="key">The key.</param>
  /// <returns>The value related to the key.</returns>
  public T Get<T>(string key)
  {
    // Try to find a Parse method to use it for conversion.
    MethodInfo method = typeof(T).GetMethod("Parse", BindingFlags.Static | BindingFlags.Public, new[] { typeof(string), typeof(IFormatProvider) })!;
    if (method != null)
      return (T) method!.Invoke(null, new object?[] { _cache[key].ToString(), null })!;

    // If it's a JObject, convert it to an object of the specified type.
    if (_cache[key] is JObject o)
      return o.ToObject<T>()!;

    // Otherwise do a simple casting.
    return (T) _cache[key];
  }

  /// <summary>
  /// Get or set the value of the specified key.
  /// </summary>
  /// <param name="key">The key.</param>
  /// <returns>The value related to the key.</returns>
  public object this[string key]
  {
    get => _cache[key];
    set
    {
      _cache[key] = value;

      // Save the config file.
      string json = JsonConvert.SerializeObject(_cache, Formatting.Indented);
      File.WriteAllText(_file, json);
    }
  }
}