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
        if (fovModule.Config.Enabled) {
            __instance.cam.fieldOfView = fovModule.Config.Fov;
        }
    }
}
