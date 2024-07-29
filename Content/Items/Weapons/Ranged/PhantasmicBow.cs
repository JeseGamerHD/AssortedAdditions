using AssortedAdditions.Content.Projectiles.RangedProj;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;

namespace AssortedAdditions.Content.Items.Weapons.Ranged
{
	internal class PhantasmicBow : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 28;
			Item.height = 42;
			Item.damage = 55;
			Item.knockBack = 4;
			Item.shootSpeed = 12;
			Item.useTime = 5;
			Item.useAnimation = 15;
			Item.reuseDelay = 30;

			Item.noMelee = true;
			Item.autoReuse = true;
			Item.consumeAmmoOnLastShotOnly = true;

			Item.rare = ItemRarityID.Lime;
			Item.DamageType = DamageClass.Ranged;
			Item.useStyle = ItemUseStyleID.Shoot;
			//Item.UseSound = SoundID.Item5;
			Item.value = Item.sellPrice(gold: 8);
			Item.useAmmo = AmmoID.Arrow;
			Item.shoot = ModContent.ProjectileType<PhantasmicBowProj>();
		}

		public override Vector2? HoldoutOffset() => new(-4, 0); // Alligns the sprite properly

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * -20f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}

			// Always shoots custom projectile no matter which arrow was used
			type = ModContent.ProjectileType<PhantasmicBowProj>();
		}
	}
}
