using BepInEx.Configuration;
using UnityEngine;

namespace PoloTweaks;

public class Config(ConfigFile config) {
    public ConfigFov Fov = new(config);
    public ConfigTrail Trail = new(config);

    public class ConfigFov(ConfigFile config) : ModuleConfig(config, "Fov") {
        public ConfigEntry<float> Fov = config.Bind("Fov", "Fov", 90f, "The field of view to use.");
    }
    
    public class ConfigTrail(ConfigFile config) : ModuleConfig(config, "Trail") {
        public ConfigEntry<float> Length = config.Bind("Trail", "Length", 1f, "The length of the trail.");
        public ConfigEntry<Color> Color = config.Bind("Trail", "Color", UnityEngine.Color.white, "The color of the trail.");
    }
}
