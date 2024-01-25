using AssortedAdditions.Content.NPCs;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedAdditions.Content.Tiles.Banners
{
	internal class GraniteGruntBannerTile : MonsterBanners
	{
		public override void SetStaticDefaults()
		{
			Color = Color.Navy;
			BuffNPC = ModContent.NPCType<GraniteGrunt>();
			base.SetStaticDefaults();
		}
	}
}
