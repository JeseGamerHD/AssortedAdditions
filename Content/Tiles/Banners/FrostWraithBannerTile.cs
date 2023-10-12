using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using ModdingTutorial.Content.NPCs;

namespace ModdingTutorial.Content.Tiles.Banners
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
