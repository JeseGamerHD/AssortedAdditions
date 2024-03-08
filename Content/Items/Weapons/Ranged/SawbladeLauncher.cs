using AssortedAdditions.Content.Projectiles.RangedProj;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace AssortedAdditions.Content.Items.Weapons.Ranged
{
	internal class SawbladeLauncher : ModItem
	{
		public override void SetDefaults() 
		{ 
			Item.width = 56;
			Item.height = 34;
			Item.damage = 82;
			Item.knockBack = 2f;
			Item.useTime = 30;
			Item.useAnimation = 30;
			Item.shootSpeed = 16f;
			Item.crit = 4;

			Item.noMelee = true;
			Item.autoReuse = true;

			Item.value = Item.sellPrice(gold: 8);
			Item.rare = ItemRarityID.Lime;
			Item.UseSound = new SoundStyle("AssortedAdditions/Assets/Sounds/WeaponSound/SawbladeLauncherSound") with { PitchVariance = 0.2f, Pitch = -0.5f };
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.shoot = ModContent.ProjectileType<SawbladeLauncherProj>();
		}

		public override Vector2? HoldoutOffset() => new(-10, -3);

		public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
		{
			Vector2 muzzleOffset = Vector2.Normalize(velocity) * 25f;
			if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
			{
				position += muzzleOffset;
			}
		}
	}
}
