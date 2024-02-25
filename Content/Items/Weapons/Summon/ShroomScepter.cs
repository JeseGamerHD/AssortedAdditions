using AssortedAdditions.Content.Buffs;
using AssortedAdditions.Content.Projectiles.SummonProj;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace AssortedAdditions.Content.Items.Weapons.Summon
{
	internal class ShroomScepter : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 54;
			Item.height = 54;
			Item.useTime = 36;
			Item.useAnimation = 36;
			Item.mana = 10;
			Item.damage = 12;
			Item.knockBack = 2f;

			Item.noMelee = true;

			Item.rare = ItemRarityID.Green;
			Item.DamageType = DamageClass.Summon;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.UseSound = SoundID.Item2;
			Item.value = Item.sellPrice(silver: 85);
			Item.buffType = ModContent.BuffType<ShroomScepterBuff>();
			Item.shoot = ModContent.ProjectileType<ShroomScepterMinion>();
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
	}
}
