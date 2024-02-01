using BepInEx.Logging;

namespace PoloTweaks;

public abstract class Module(string id) {
    public string Id = id;
    public ManualLogSource Log = Logger.CreateLogSource("PoloTweaks " + id);

    public void Init() { }
    public void Update() { }

    public T GetConfig<T>() where T : ModuleConfig => Plugin.GetConfig<T>(this.Id);
}
