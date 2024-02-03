using Terraria;
using AssortedAdditions.Content.Projectiles.SummonProj;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using AssortedAdditions.Content.Buffs;
using AssortedAdditions.Content.Items.Misc;

namespace AssortedAdditions.Content.Items.Weapons.Summon
{
	internal class GraniteStaff : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 54;
			Item.height = 54;
			Item.useTime = 36;
			Item.useAnimation = 36;
			Item.mana = 10;
			Item.damage = 15;
			Item.knockBack = 2f;

			Item.noMelee = true;

			Item.rare = ItemRarityID.Green;
			Item.DamageType = DamageClass.Summon;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.UseSound = SoundID.Item44;
			Item.value = Item.sellPrice(silver: 80);
			Item.buffType = ModContent.BuffType<GraniteStaffBuff>();
			Item.shoot = ModContent.ProjectileType<GraniteStaffMinion>();
		}

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			position = Main.MouseWorld; // Spawns at cursor position
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			// This is needed so the buff that keeps your minion alive and allows you to despawn it properly applies
			player.AddBuff(Item.buffType, 2);

			// Minions have to be spawned manually, then have originalDamage assigned to the damage of the summon item
			var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
			projectile.originalDamage = Item.damage;

			// Since we spawned the projectile manually already, we do not need the game to spawn it for ourselves anymore, so return false
			return false;
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
