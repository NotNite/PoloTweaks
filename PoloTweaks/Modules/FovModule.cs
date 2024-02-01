namespace PoloTweaks.Modules;

public class FovModule : Module {
    public Config.ConfigFov Config;

    public FovModule() : base("Fov") {
        this.Config = this.GetConfig<Config.ConfigFov>();
    }
}
