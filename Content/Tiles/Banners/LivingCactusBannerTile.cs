using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AssortedAdditions.Content.NPCs;

namespace AssortedAdditions.Content.Tiles.Banners
{
    internal class LivingCactusBannerTile : MonsterBanners
    {
        public override void SetStaticDefaults()
        {
            Color = Color.LimeGreen;
            BuffNPC = ModContent.NPCType<LivingCactus>();
            base.SetStaticDefaults();
        }
    }
}
