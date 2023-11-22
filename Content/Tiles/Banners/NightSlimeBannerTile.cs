using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AssortedAdditions.Content.NPCs;

namespace AssortedAdditions.Content.Tiles.Banners
{
    internal class NightSlimeBannerTile : MonsterBanners
    {
        public override void SetStaticDefaults()
        {
            Color = Color.MidnightBlue;
            BuffNPC = ModContent.NPCType<NightSlime>();
            base.SetStaticDefaults();
        }
    }
}
