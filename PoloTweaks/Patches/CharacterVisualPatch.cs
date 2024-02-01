using System.Runtime.CompilerServices;
using HarmonyLib;
using PoloTweaks.Modules;
using Reptile;
using UnityEngine;

namespace PoloTweaks.Patches;

[HarmonyPatch(typeof(CharacterVisual))]
public class CharacterVisualPatch {
    [HarmonyPostfix]
    [HarmonyPatch("InitVFX")]
    public static void InitVFX(CharacterVisual __instance, PlayerVisualEffects prefabs) {
        var trailModule = Plugin.GetModule<TrailModule>();
        if (trailModule.Config.Enabled.Value) {
            var orig = __instance.VFX.boostpackTrail.GetComponent<TrailRenderer>();
            var leftTrail = __instance.footL.gameObject.AddComponent<TrailRenderer>();
            var rightTrail = __instance.footR.gameObject.AddComponent<TrailRenderer>();

            Setup(
                orig,
                leftTrail,
                trailModule.Config.Color.Value,
                trailModule.Config.Length.Value
            );
            Setup(
                orig,
                rightTrail,
                trailModule.Config.Color.Value,
                trailModule.Config.Length.Value
            );
        }
    }

    private static void Setup(TrailRenderer orig, TrailRenderer @new, Color color, float length) {
        @new.time = orig.time;
        @new.startWidth = orig.startWidth;
        @new.endWidth = orig.endWidth;
        @new.minVertexDistance = orig.minVertexDistance;
        @new.startColor = color;
        @new.endColor = color;
        @new.time = length;
        @new.material = orig.material;
        @new.enabled = true;
        @new.emitting = true;
    }
}
