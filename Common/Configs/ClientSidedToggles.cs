using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace AssortedAdditions.Common.Configs
{
    internal class ClientSidedToggles : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;

        [Header("VisualToggle")]
        [DefaultValue(true)]
        public bool FancyWingFlightToggle;
    }
}
