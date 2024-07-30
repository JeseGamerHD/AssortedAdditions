using AssortedAdditions.Content.Projectiles.MeleeProj;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;

namespace AssortedAdditions.Content.Items.Weapons.Melee
{
	internal class PhantasmicBlade : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 60;
			Item.height = 62;
			Item.damage = 84;
			Item.knockBack = 5f;
			Item.useAnimation = 16;
			Item.useTime = 16;
			Item.scale = 1f;

			Item.noMelee = true; // This is set the sword itself doesn't deal damage (only the projectile does).
			Item.shootsEveryUse = true; // This makes sure Player.ItemAnimationJustStarted is set when swinging.
			Item.autoReuse = true;

			Item.value = Item.sellPrice(gold: 8);
			Item.UseSound = SoundID.Item1;
			Item.rare = ItemRarityID.Lime;
			Item.DamageType = DamageClass.Melee;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shoot = ModContent.ProjectileType<PhantasmicBladeTrail>();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float adjustedItemScale = player.GetAdjustedItemScale(Item); // Get the melee scale of the player and item.
			Projectile.NewProjectile(source, player.MountedCenter, new Vector2(player.direction, 0f), type, damage, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax, adjustedItemScale);
			NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, player.whoAmI); // Sync the changes in multiplayer.

			return base.Shoot(player, source, position, velocity, type, damage, knockback);
		}
	}
}
