using HarmonyLib;
using PoloTweaks.Modules;
using Reptile;

namespace PoloTweaks.Patches;

[HarmonyPatch(typeof(GameplayCamera))]
public class GameplayCameraPatch {
    [HarmonyPostfix]
    [HarmonyPatch("Awake")]
    private static void Awake(GameplayCamera __instance) {
        var fovModule = Plugin.GetModule<FovModule>();
        if (Plugin.GetConfig(fovModule.Id).Enabled.Value) {
            __instance.cam.fieldOfView = fovModule.Config.Fov.Value;
        }
    }
}
