using AssortedAdditions.Content.Projectiles.RangedProj;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AssortedAdditions.Content.Items.Misc;

namespace AssortedAdditions.Content.Items.Weapons.Ranged
{
	internal class GraniteChakram : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 38;
			Item.damage = 22;
			Item.knockBack = 3f;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.shootSpeed = 12f;
			Item.crit = 4;

			Item.noMelee = true;
			Item.autoReuse = true;
			Item.noUseGraphic = true;

			Item.DamageType = DamageClass.Ranged;
			Item.value = Item.sellPrice(silver: 70);
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shoot = ModContent.ProjectileType<GraniteChakramProj>();
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<GraniteArmorShard>(), 7);
			recipe.AddIngredient(ItemID.Granite, 15);
			recipe.AddRecipeGroup("IronBar", 4);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
