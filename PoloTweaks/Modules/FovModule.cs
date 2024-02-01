namespace PoloTweaks.Modules;

public class FovModule() : Module("fov") {
    public FovConfig Config = new();

    public class FovConfig : ModuleConfig {
        public float Fov { get; set; } = 90f;
    }
}
