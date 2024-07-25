using Terraria.ModLoader;
using AssortedAdditions.Content.Items.Placeables.Relics;

namespace AssortedAdditions.Content.Tiles.Relics
{
	internal class HauntRelicTile : BossRelicTile
	{
		public override string RelicTextureName => "AssortedAdditions/Content/Tiles/Relics/HauntRelicTile";

		public override int RelicItemType => ModContent.ItemType<HauntRelic>();

		public override void SetStaticDefaults()
		{
			base.SetStaticDefaults();
		}
	}
}
