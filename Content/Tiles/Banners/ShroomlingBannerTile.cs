using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AssortedAdditions.Content.NPCs;

namespace AssortedAdditions.Content.Tiles.Banners
{
    internal class ShroomlingBannerTile : MonsterBanners
    {
        public override void SetStaticDefaults()
        {
            Color = Color.Gainsboro;
            BuffNPC = ModContent.NPCType<Shroomling>();
            base.SetStaticDefaults();
        }
    }
}
