using System.Numerics;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace disbot.Modules;

public class ModuleCache
{
    private Dictionary<string, object> _cache = new Dictionary<string, object>();

    private string _file;

    public ModuleCache(string file)
    {
        _file = file;

        Directory.CreateDirectory(new FileInfo(file).DirectoryName!);
        if (!File.Exists(file))
            File.WriteAllText(file, JsonConvert.SerializeObject(_cache, Formatting.Indented));

        string json = File.ReadAllText(file);
        _cache = JsonConvert.DeserializeObject<Dictionary<string, object>>(json)!;
    }

    public void SetDefault(string key, object value)
    {
        if (!_cache.ContainsKey(key))
            this[key] = value;
    }

    public T Get<T>(string key)
    {
        MethodInfo method = typeof(T).GetMethod("Parse", BindingFlags.Static | BindingFlags.Public, new[] { typeof(string), typeof(IFormatProvider) })!;
        if (method != null)
            return (T)method!.Invoke(null, new object?[] { _cache[key].ToString(), null })!;

        if (_cache[key] is JObject o)
            return o.ToObject<T>()!;
            
        return (T)_cache[key];
    }

    public object this[string key]
    {
        get => _cache[key];
        set
        {
            _cache[key] = value;

            string json = JsonConvert.SerializeObject(_cache, Formatting.Indented);
            File.WriteAllText(_file, json);
        }
    }
}