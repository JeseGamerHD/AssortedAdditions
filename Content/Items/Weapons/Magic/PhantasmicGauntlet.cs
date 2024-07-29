using AssortedAdditions.Content.Projectiles.MagicProj;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Items.Weapons.Magic
{
	internal class PhantasmicGauntlet : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 32;
			Item.useTime = 35;
			Item.useAnimation = 35;
			Item.damage = 45;
			Item.knockBack = 4f;
			Item.shootSpeed = 4f;
			Item.mana = 12;

			Item.autoReuse = true;
			Item.noMelee = true;
			Item.noUseGraphic = true;

			Item.DamageType = DamageClass.Magic;
			Item.useStyle = ItemUseStyleID.Shoot;
			Item.UseSound = SoundID.Item8 with { Pitch = -0.3f, MaxInstances = 3 };
			Item.rare = ItemRarityID.Lime;
			Item.value = Item.sellPrice(gold: 8);
			Item.shoot = ModContent.ProjectileType<PhantasmicGauntletProj>();
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			float numberProjectiles = 5;
			float rotation = MathHelper.ToRadians(150);

			for (int i = 0; i < numberProjectiles; i++)
			{
				Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1)));
				Projectile.NewProjectile(source, position, perturbedSpeed, type, damage, knockback, player.whoAmI);
			}

			return false;
		}
	}

	public class PhantasmicGauntletPlayer : ModPlayer
	{
		public override void PostUpdateMiscEffects()
		{
			// This is how the dust effect in the Player's hand when holding Nebula Blaze is created:
			if (Player.inventory[Player.selectedItem].type == ModContent.ItemType<PhantasmicGauntlet>() && Player.HandPosition.HasValue)
			{
				Vector2 handPos = Player.HandPosition.Value - Player.velocity;
				for (int i = 0; i < 4; i++)
				{
					Dust dust = Dust.NewDustDirect(Player.Center, 0, 0, DustID.WhiteTorch, Player.direction * 2, 0f, 150, default, 1f);
					dust.position = handPos;
					dust.velocity *= 0f;
					dust.noGravity = true;
					dust.fadeIn = 1f;
					dust.velocity += Player.velocity;

					if (Main.rand.NextBool(2))
					{
						dust.position += Utils.RandomVector2(Main.rand, -4f, 4f);
						dust.scale += Main.rand.NextFloat();
						if (Main.rand.NextBool(2))
						{
							dust.customData = this;
						}
					}
				}
			}
		}
	}
}
