using AssortedAdditions.Content.Items.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Items.Armor
{
	[AutoloadEquip(EquipType.Legs)]
	internal class GraniteGreaves : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 18;
			Item.height = 18;
			Item.value = Item.sellPrice(silver: 80);
			Item.rare = ItemRarityID.Green;
			Item.defense = 6;
		}

		public override void UpdateEquip(Player player)
		{
			player.whipRangeMultiplier += 0.15f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<GraniteArmorShard>(), 4);
			recipe.AddIngredient(ItemID.Granite, 25);
			recipe.AddRecipeGroup("IronBar", 8);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
