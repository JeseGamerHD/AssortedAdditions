using AssortedAdditions.Content.Projectiles.RangedProj;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.Audio;
using Microsoft.Xna.Framework;

namespace AssortedAdditions.Content.Items.Weapons.Ranged
{
	internal class Shroomzooka : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 58;
			Item.height = 30;
			Item.useAnimation = 27;
			Item.useTime = 27;
			Item.damage = 35;
			Item.knockBack = 6f;
			Item.shootSpeed = 11f;
			Item.crit = 4;

			Item.autoReuse = true;
			Item.noMelee = true;

			Item.value = Item.sellPrice(silver: 85);
			Item.UseSound = new SoundStyle("AssortedAdditions/Assets/Sounds/WeaponSound/ShroomzookaSound") with { PitchVariance = 0.1f };
			Item.rare = ItemRarityID.Green;
			Item.DamageType = DamageClass.Ranged;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ModContent.ProjectileType<ShroomzookaProj>();
			Item.useAmmo = ItemID.GlowingMushroom; // AmmoID is basically the same as ItemID (values are the same, names different)
												   // This works only when Item.ammo = ItemID.GlowingMushroom has been set for the Glowing Mushroom
												   // This is set in ModifiedAmmo.cs (GlobalItem class)
		}

		public override Vector2? HoldoutOffset() => new(-10, 0); // Alligns the sprite properly

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 25f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset; // Projectiles come out of the muzzle properly using this
			}
		}

		public override void PickAmmo(Item weapon, Player player, ref int type, ref float speed, ref StatModifier damage, ref float knockback)
		{
			base.PickAmmo(weapon, player, ref type, ref speed, ref damage, ref knockback);
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.GlowingMushroom, 20);
			recipe.AddIngredient(ItemID.TinBar, 8);
			recipe.AddRecipeGroup("Wood", 12);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();

			Recipe recipe2 = CreateRecipe();
			recipe2.AddIngredient(ItemID.GlowingMushroom, 20);
			recipe2.AddIngredient(ItemID.CopperBar, 8);
			recipe2.AddRecipeGroup("Wood", 12);
			recipe2.AddTile(TileID.Anvils);
			recipe2.Register();
		}
	}
}
