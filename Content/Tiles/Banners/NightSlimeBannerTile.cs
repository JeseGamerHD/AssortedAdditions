using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using ModdingTutorial.Content.NPCs;

namespace ModdingTutorial.Content.Tiles.Banners
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
