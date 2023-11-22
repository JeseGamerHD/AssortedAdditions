using AssortedAdditions.Content.NPCs;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Tiles.Banners
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
