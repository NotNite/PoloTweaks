using HarmonyLib;
using PoloTweaks.Modules;
using Reptile;
using UnityEngine;

namespace PoloTweaks.Patches;

[HarmonyPatch(typeof(Player))]
public class PlayerPatch {
    [HarmonyPostfix]
    [HarmonyPatch("SetCharacter")]
    public static void SetCharacter(Player __instance, Characters setChar, int setOutfit = 0) {
        var trailModule = Plugin.GetModule<TrailModule>();
        if (trailModule.Config.Enabled.Value) {
            if (!trailModule.Config.AllPlayers.Value && __instance.isAI) {
                return;
            }

            trailModule.Log.LogDebug("Setting up trail for player.");

            var characterVisual = __instance.characterVisual;
            var orig = characterVisual.VFX.boostpackTrail.GetComponent<TrailRenderer>();
            var leftTrail = characterVisual.footL.gameObject.AddComponent<TrailRenderer>();
            var rightTrail = characterVisual.footR.gameObject.AddComponent<TrailRenderer>();

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
        @new.startWidth = orig.startWidth;
        @new.endWidth = orig.endWidth;
        @new.minVertexDistance = orig.minVertexDistance;
        @new.time = length;
        @new.enabled = true;
        @new.emitting = true;

        var tex = new Texture2D(2, 1);
        tex.SetPixel(0, 0, color);
        tex.SetPixel(1, 0, Color.clear);
        tex.Apply();
        @new.material = new Material(orig.material);
        @new.material.mainTexture = tex;
    }
}
