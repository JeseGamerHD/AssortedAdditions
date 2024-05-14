using AssortedAdditions.Content.Projectiles.MagicProj;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace AssortedAdditions.Content.Items.Weapons.Magic
{
	internal class ShroomPouch : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 30;
			Item.useAnimation = 40;
			Item.useTime = 40;
			Item.damage = 22;
			Item.knockBack = 0f;
			Item.shootSpeed = 2f;
			Item.mana = 12;

			Item.autoReuse = true;
			Item.noMelee = true;

			Item.value = Item.sellPrice(silver: 85);
			Item.UseSound = SoundID.Item1;
			Item.rare = ItemRarityID.Green;
			Item.DamageType = DamageClass.Magic;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shoot = ModContent.ProjectileType<ShroomPouchProj>();
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.GlowingMushroom, 20);
			recipe.AddIngredient(ItemID.Rope);
			recipe.AddIngredient(ItemID.Silk, 8);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
