using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AssortedAdditions.Content.NPCs;

namespace AssortedAdditions.Content.Tiles.Banners
{
    internal class FrostWraithBannerTile : MonsterBanners
    {
        public override void SetStaticDefaults()
        {
            Color = Color.DarkTurquoise;
            BuffNPC = ModContent.NPCType<FrostWraith>();
            base.SetStaticDefaults();
        }
    }
}
