using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AssortedAdditions.Content.NPCs;

namespace AssortedAdditions.Content.Tiles.Banners
{
    internal class GoblinBalloonistBannerTile : MonsterBanners
    {
        public override void SetStaticDefaults()
        {
            Color = Color.OrangeRed;
            BuffNPC = ModContent.NPCType<GoblinBalloonist>();
            base.SetStaticDefaults();
        }
    }
}
