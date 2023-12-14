using AssortedAdditions.Content.Buffs;
using Microsoft.Xna.Framework;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace AssortedAdditions.Content.Items.Consumables
{
	internal class EvasionPotion : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 20;

			// Dust that will appear in these colors when the item with ItemUseStyleID.DrinkLiquid is used
			ItemID.Sets.DrinkParticleColors[Type] = new Color[3] {
				new Color(171, 19, 9),
				new Color(196, 95, 88),
				new Color(82, 18, 14)
			};
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 32;
			Item.useAnimation = 15;
			Item.useTime = 15;
			Item.maxStack = 9999;

			Item.consumable = true;
			Item.useTurn = true;

			Item.useStyle = ItemUseStyleID.DrinkLiquid;
			Item.UseSound = SoundID.Item3;
			Item.rare = ItemRarityID.Blue;
			Item.value = Item.sellPrice(silver: 6);
			Item.buffType = ModContent.BuffType<EvasionBuff>();
			Item.buffTime = 10800; // 3 minutes
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.SwiftnessPotion);
			recipe.AddIngredient(ItemID.Bone, 5);
			recipe.AddIngredient(ItemID.GlowingMushroom);
			recipe.AddTile(TileID.Bottles);
			recipe.Register();
		}
	}
}
