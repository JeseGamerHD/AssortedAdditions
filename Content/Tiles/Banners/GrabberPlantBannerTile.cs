using AssortedAdditions.Content.NPCs;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Tiles.Banners
{
    internal class GrabberPlantBannerTile : MonsterBanners
    {
        public override void SetStaticDefaults()
        {
            Color = Color.Green;
            BuffNPC = ModContent.NPCType<GrabberPlant>();
            base.SetStaticDefaults();
        }
    }
}
