using HarmonyLib;
using PoloTweaks.Components;
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
        if (trailModule.Config.Enabled) {
            if (!trailModule.Config.AllPlayers && __instance.isAI) {
                return;
            }

            var characterVisual = __instance.characterVisual;
            var orig = characterVisual.VFX.boostpackTrail.GetComponent<TrailRenderer>();
            foreach (var trail in trailModule.Config.Trails) {
                var bone = characterVisual.root.FindRecursive(trail.Bone);
                if (bone == null) {
                    trailModule.Log.LogWarning($"Bone {trail.Bone} not found.");
                    continue;
                }

                var @new = bone.gameObject.AddComponent<TrailRenderer>();
                @new.startWidth = orig.startWidth;
                @new.endWidth = orig.endWidth;
                @new.minVertexDistance = orig.minVertexDistance;
                @new.time = trail.Length;
                @new.enabled = true;
                @new.emitting = true;

                var tex = new Texture2D(2, 1);
                tex.SetPixel(0, 0, trail.Color);
                tex.SetPixel(1, 0, Color.clear);
                tex.Apply();
                @new.material = new Material(orig.material);
                @new.material.mainTexture = tex;

                if (trail.MinSpeed.HasValue) {
                    bone.gameObject.AddComponent<TrailSpeedChecker>().SpeedBarrier = trail.MinSpeed.Value;
                }
            }
        }
    }
}
