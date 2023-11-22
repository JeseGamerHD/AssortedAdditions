using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AssortedAdditions.Content.NPCs;

namespace AssortedAdditions.Content.Tiles.Banners
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
