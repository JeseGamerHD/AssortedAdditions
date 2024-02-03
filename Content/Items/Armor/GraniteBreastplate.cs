using AssortedAdditions.Content.Items.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Items.Armor
{
	[AutoloadEquip(EquipType.Body)]
	internal class GraniteBreastplate : ModItem
	{
		public override void SetStaticDefaults()
		{
			ArmorIDs.Body.Sets.HidesTopSkin[Item.bodySlot] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 20;
			Item.value = Item.sellPrice(silver: 80);
			Item.rare = ItemRarityID.Green;
			Item.defense = 8;
		}

		public override void UpdateEquip(Player player)
		{
			player.maxMinions += 1;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<GraniteArmorShard>(), 12);
			recipe.AddIngredient(ItemID.Granite, 30);
			recipe.AddRecipeGroup("IronBar", 6);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
