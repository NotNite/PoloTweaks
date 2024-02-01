using BepInEx.Configuration;

namespace PoloTweaks;

public class ModuleConfig(ConfigFile config, string section) {
    public ConfigEntry<bool> Enabled = config.Bind(section, "Enabled", false, "Enable this module.");
    private ConfigFile config = config;
    private string section = section;
}
