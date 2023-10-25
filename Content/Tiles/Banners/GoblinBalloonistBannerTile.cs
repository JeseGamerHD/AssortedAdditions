using ModdingTutorial.Content.NPCs;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ModdingTutorial.Content.Tiles.Banners
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
