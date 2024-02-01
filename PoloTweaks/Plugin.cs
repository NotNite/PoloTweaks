using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using Tomlyn;
using Tomlyn.Model;
using UnityEngine;

namespace PoloTweaks;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInProcess("Bomb Rush Cyberfunk.exe")]
public class Plugin : BaseUnityPlugin {
    public static ManualLogSource Log = null!;
    public static Harmony Harmony = null!;
    public static List<Module> Modules = new();

    public static T GetModule<T>() where T : class => (Modules.First(module => module is T) as T)!;

    private void Awake() {
        Log = this.Logger;

        var cfgPath = this.Config.ConfigFilePath.Replace(".cfg", ".toml");
        var cfg = File.Exists(cfgPath) ? Toml.ToModel(File.ReadAllText(cfgPath)) : new TomlTable();

        var modules = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => type.IsSubclassOf(typeof(Module)) && !type.IsAbstract)
            .ToList();
        foreach (var module in modules) {
            var instance = Activator.CreateInstance(module) as Module;
            Modules.Add(instance!);
        }

        var modelOptions = new TomlModelOptions {
            ConvertToModel = (obj, type) => {
                if (type == typeof(Color))
                    return ColorUtility.TryParseHtmlString((string) obj, out var color) ? color : null;
                return null;
            },
            ConvertToToml = obj => {
                if (obj is Color color) return ColorUtility.ToHtmlStringRGBA(color);
                return null;
            }
        };

        foreach (var module in Modules) {
            if (cfg.TryGetValue(module.Id, out var moduleConfig)) {
                // Never give me C# reflection
                var cfgType = module.GetType().GetField("Config")!.FieldType;
                var moduleConfigStr = Toml.FromModel(moduleConfig);
                var toModel = typeof(Toml)
                    .GetMethods()
                    .First(method => method.Name == "ToModel"
                                     && method.IsGenericMethod
                                     && method.GetParameters().First().Name == "text")
                    .MakeGenericMethod(cfgType);

                var cfgInstance = toModel.Invoke(null, [moduleConfigStr, cfgPath, modelOptions])!;
                module.GetType().GetField("Config")!.SetValue(module, cfgInstance);

                if (((ModuleConfig) cfgInstance).Enabled) module.Init();
            }
        }

        // Really wish I could patch by category
        Harmony = new Harmony(PluginInfo.PLUGIN_GUID);
        Harmony.PatchAll();
    }


    private void Update() {
        foreach (var module in Modules) module.Update();
    }
}
