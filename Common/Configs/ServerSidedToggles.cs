using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace AssortedAdditions.Common.Configs
{
    internal class ServerSidedToggles : ModConfig
    {
        public static ServerSidedToggles Instance; // Easier access to values when combined with getters
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Header("VanillaRecipes")]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool ModifiedRecipesToggle;

        [DefaultValue(true)]
        [ReloadRequired]
        public bool MagicWorkBenchCraftingToggle;
        // Whether or not to use the magic workbench over some other crafting tiles
        // Check MagicWorkBenchCrafting.cs

        [Header("Items")]
        [DefaultValue(true)]
        [ReloadRequired]
        public bool NoConsumeBossSummon;

        [DefaultValue(true)]
        [ReloadRequired]
        public bool NoConsumeEventSummon;

        [Header("OreProgression")]
        [DefaultValue(true)]
        public bool HardmodeOreProgressionToggle;

        [Header("WorldGeneration")]
        [DefaultValue(true)]
        public bool spawnStructureToggle;

        // *** NPC SPAWN CHANCE STUFF ***

        private const float MaxSpawnMultiplier = 2f;
        [Header("ModNPCSpawning")]
        [Range(0f, MaxSpawnMultiplier)]
        [Increment(0.1f)]
        [DefaultValue(1f)]
		[BackgroundColor(72, 0, 148, 128)]
		[SliderColor(126, 7, 210, 128)]
		public float NPCSpawnMultiplier { get; set; } // Adjusts all Mod npcs spawn chances and overrides any individual ones

        [Range(0f, MaxSpawnMultiplier)]
        [Increment(0.1f)]
        [DefaultValue(1f)]
        public float CursedPickaxeSpawnMultiplier { get; set; }

        [Range(0f, MaxSpawnMultiplier)]
        [Increment(0.1f)]
        [DefaultValue(1f)]
        public float FrostWraithSpawnMultiplier { get; set; }

        [Range(0f, MaxSpawnMultiplier)]
        [Increment(0.1f)]
        [DefaultValue(1f)]
        public float GoblinBalloonistSpawnMultiplier { get; set; }

        [Range(0f, MaxSpawnMultiplier)]
        [Increment(0.1f)]
        [DefaultValue(1f)]
        public float GrabberPlantSpawnMultiplier { get; set; }

        [Range(0f, MaxSpawnMultiplier)]
        [Increment(0.1f)]
        [DefaultValue(1f)]
        public float GraniteGruntSpawnMultiplier { get; set; }

        [Range(0f, MaxSpawnMultiplier)]
        [Increment(0.1f)]
        [DefaultValue(1f)]
        public float IceGuardianSpawnMultiplier { get; set; }

        [Range(0f, MaxSpawnMultiplier)]
        [Increment(0.1f)]
        [DefaultValue(1f)]
        public float LivingCactusSpawnMultiplier { get; set; }

        [Range(0f, MaxSpawnMultiplier)]
        [Increment(0.1f)]
        [DefaultValue(1f)]
        public float NightSlimeSpawnMultiplier { get; set; }

        [Range(0f, MaxSpawnMultiplier)]
        [Increment(0.1f)]
        [DefaultValue(1f)]
        public float SandstoneMimicSpawnMultiplier { get; set; }

        [Range(0f, MaxSpawnMultiplier)]
        [Increment(0.1f)]
        [DefaultValue(1f)]
        public float ShroomlingSpawnMultiplier { get; set; }
	}
}
