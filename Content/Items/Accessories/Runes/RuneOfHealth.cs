using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using AssortedAdditions.Content.Tiles.CraftingStations;
using AssortedAdditions.Content.Items.Misc;

namespace AssortedAdditions.Content.Items.Accessories.Runes
{
	internal class RuneOfHealth : RuneItem
	{
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 28;
			Item.maxStack = 1;
			Item.value = Item.sellPrice(gold: 2);
			Item.rare = ItemRarityID.Pink;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			// Only increase based on base health without buffs etc so they won't stack
			player.statLifeMax2 = (int)(player.statLifeMax * 1.2f);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<BlankRune>());
			recipe.AddIngredient(ModContent.ItemType<MagicEssence>(), 10);
			recipe.AddIngredient(ItemID.LifeCrystal, 3);
			recipe.AddTile(ModContent.TileType<MagicWorkbenchTile>());
			recipe.Register();
		}
	}
}
