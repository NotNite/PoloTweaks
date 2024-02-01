using System.Collections.Generic;
using UnityEngine;

namespace PoloTweaks.Modules;

public class TrailModule() : Module("trail") {
    public TrailConfig Config = new();

    public class TrailConfig : ModuleConfig {
        public List<Trail> Trails { get; set; } = new();
        public bool AllPlayers { get; set; } = false;
    }

    public class Trail {
        public string Bone { get; set; } = null!;
        public Color Color { get; set; } = Color.white;
        public float Length { get; set; } = 1;
        public float? MinSpeed { get; set; } = null;
    }
}
