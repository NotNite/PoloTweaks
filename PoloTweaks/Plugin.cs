using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using MonoMod.Utils;

namespace PoloTweaks;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInProcess("Bomb Rush Cyberfunk.exe")]
public class Plugin : BaseUnityPlugin {
    public static ManualLogSource Log = null!;
    public static Harmony Harmony = null!;

    public static Config ConfigFile = null!;
    public static Dictionary<string, ModuleConfig> ConfigObjects = new();
    public static List<Module> Modules = new();

    public static T GetModule<T>() where T : class => (Modules.First(module => module is T) as T)!;
    public static T GetConfig<T>(string name) where T : ModuleConfig => (ConfigObjects[name] as T)!;
    public static ModuleConfig GetConfig(string name) => ConfigObjects[name];

    private void Awake() {
        Log = this.Logger;

        ConfigFile = new Config(this.Config);
        foreach (var field in ConfigFile.GetType().GetFields()) {
            ConfigObjects[field.Name] = (field.GetValue(ConfigFile) as ModuleConfig)!;
        }

        var modules = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => type.IsSubclassOf(typeof(Module)) && !type.IsAbstract)
            .ToList();
        foreach (var module in modules) {
            var instance = Activator.CreateInstance(module) as Module;
            Modules.Add(instance!);
        }

        foreach (var module in Modules.Where(x => GetConfig(x.Id).Enabled.Value)) module.Init();

        // Really wish I could patch by category
        Harmony = new Harmony(PluginInfo.PLUGIN_GUID);
        Harmony.PatchAll();
    }


    private void Update() {
        foreach (var module in Modules) module.Update();
    }
}
