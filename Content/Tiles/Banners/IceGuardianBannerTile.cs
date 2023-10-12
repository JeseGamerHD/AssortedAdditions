using Microsoft.Xna.Framework;
using ModdingTutorial.Content.NPCs;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Tiles.Banners
{
    internal class IceGuardianBannerTile : MonsterBanners
    {
        public override void SetStaticDefaults()
        {
            Color = Color.Aqua;
            BuffNPC = ModContent.NPCType<IceGuardian>();
            base.SetStaticDefaults();
        }
    }
}
