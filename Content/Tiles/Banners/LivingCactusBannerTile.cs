using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using ModdingTutorial.Content.NPCs;

namespace ModdingTutorial.Content.Tiles.Banners
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
