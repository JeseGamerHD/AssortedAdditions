using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace AssortedAdditions.Common.Configs
{
    internal class VanillaChangeToggle : ModConfig
    {
        // ConfigScope.ClientSide should be used for client side, usually visual or audio tweaks.
        // ConfigScope.ServerSide should be used for basically everything else, including disabling items or changing NPC behaviours
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Header("VanillaRecipes")] // Headers are like titles in a config. You only need to declare a header on the item it should appear over, not every item in the category.
        [DefaultValue(true)] // This sets the configs default value.
        [ReloadRequired] // Marking it with [ReloadRequired] makes tModLoader force a mod reload if the option is changed. It should be used for things like item toggles, which only take effect during mod loading
        public bool ModifiedRecipesToggle;

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
	}
}
