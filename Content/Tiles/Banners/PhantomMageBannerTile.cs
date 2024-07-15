using AssortedAdditions.Content.NPCs;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace AssortedAdditions.Content.Tiles.Banners
{
	internal class PhantomMageBannerTile : MonsterBanners
	{
		public override void SetStaticDefaults()
		{
			Color = Color.Gray;
			BuffNPC = ModContent.NPCType<PhantomMage>();
			base.SetStaticDefaults();
		}
	}
}
