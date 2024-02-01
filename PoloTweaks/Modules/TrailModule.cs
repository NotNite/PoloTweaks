namespace PoloTweaks.Modules;

public class TrailModule : Module {
    public Config.ConfigTrail Config;

    public TrailModule() : base("Trail") {
        this.Config = this.GetConfig<Config.ConfigTrail>();
    }
}
