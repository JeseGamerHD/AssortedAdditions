using AssortedAdditions.Content.Projectiles.MagicProj;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using AssortedAdditions.Content.Tiles.CraftingStations;

namespace AssortedAdditions.Content.Items.Weapons.Magic
{
	internal class SpiritVase : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 34;
			Item.damage = 45;
			Item.knockBack = 2f;
			Item.useAnimation = 70;
			Item.useTime = 70;
			Item.shootSpeed = 18;
			Item.mana = 10;

			Item.autoReuse = true;
			Item.noMelee = true;

			Item.value = Item.sellPrice(silver: 15);
			Item.useStyle = ItemUseStyleID.Guitar;
			Item.holdStyle = ItemHoldStyleID.HoldGuitar;
			Item.DamageType = DamageClass.Magic;
			Item.rare = ItemRarityID.Yellow;
			Item.UseSound = SoundID.Zombie53;
			Item.shoot = ModContent.ProjectileType<SpiritVaseProj>();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{

			float numberOfProjs = 4;
			float rotation = MathHelper.ToRadians(55);

			for (int i = 0; i < numberOfProjs; i++)
			{
				Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberOfProjs - 1))); // Watch out for dividing by 0 if there is only 1 projectile.
				Projectile.NewProjectile(source, position, perturbedSpeed, ModContent.ProjectileType<SpiritVaseProj>(), damage, knockback, player.whoAmI);
			}

			for (int i = 0; i < 10; i++)
			{
				Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.GemDiamond, 0f, 0f, 100, default, 1f);
				dust.velocity *= 1.4f;
				dust.noGravity = true;
			}

			return false;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.Ectoplasm, 10);
			recipe.AddRecipeGroup("AssortedAdditions:DungeonVases");
			recipe.AddIngredient(ItemID.Marble, 30);
			recipe.AddTile(ModContent.TileType<MagicWorkbenchTile>());
			recipe.Register();
		}
	}
}
