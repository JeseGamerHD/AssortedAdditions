using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using ModdingTutorial.Content.NPCs;

namespace ModdingTutorial.Content.Tiles.Banners
{
    internal class CursedPickaxeBannerTile : MonsterBanners
    {
        public override void SetStaticDefaults()
        {
            Color = Color.MediumTurquoise;
            BuffNPC = ModContent.NPCType<CursedPickaxe>();
            base.SetStaticDefaults();
        }
    }
}
