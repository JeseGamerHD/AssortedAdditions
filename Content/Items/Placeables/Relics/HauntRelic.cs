using Terraria;
using AssortedAdditions.Content.Tiles.Relics;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Items.Placeables.Relics
{
	internal class HauntRelic : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 33;
			Item.maxStack = 999;
			Item.rare = ItemRarityID.Master;
			Item.master = true; // This makes sure that "Master" displays in the tooltip, as the rarity only changes the item name color
			Item.value = Item.sellPrice(gold: 5);

			Item.DefaultToPlaceableTile(ModContent.TileType<HauntRelicTile>(), 0);
		}
	}
}
