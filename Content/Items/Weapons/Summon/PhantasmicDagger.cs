using AssortedAdditions.Content.Buffs;
using AssortedAdditions.Content.Projectiles.SummonProj;
using AssortedAdditions.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Items.Weapons.Summon
{
	internal class PhantasmicDagger : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 34;
			Item.height = 42;
			Item.useTime = 20;
			Item.useAnimation = 20;
			Item.mana = 60;

			Item.noMelee = true;

			Item.rare = ItemRarityID.Lime;
			Item.DamageType = DamageClass.Summon;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.UseSound = SoundID.Zombie54 with { Pitch = -0.1f };
			Item.value = Item.sellPrice(gold: 8);
			Item.buffType = ModContent.BuffType<PhantasmicDaggerDebuff>();
		}

		public override bool CanUseItem(Player player)
		{
			return !player.HasBuff(ModContent.BuffType<PhantasmicDaggerDebuff>());
		}

		public override bool? UseItem(Player player)
		{
			if (player.whoAmI == Main.myPlayer)
			{
				player.AddBuff(Item.buffType, HelperMethods.MinutesToTicks(2));

				int healAmount = 0;
				foreach (var projectile in Main.ActiveProjectiles)
				{
					if (projectile.minion && projectile.owner == Main.myPlayer)
					{
						Vector2 direction = Main.player[projectile.owner].Center - projectile.Center;
						direction.Normalize();
						Projectile.NewProjectile(player.GetSource_ItemUse(Item), projectile.position, direction * 2f, ModContent.ProjectileType<PhantasmicDaggerSoul>(), 0, 0, Main.myPlayer);

						healAmount += (int)(projectile.minionSlots * 40f);
						projectile.Kill();
					}
				}

				player.Heal(healAmount);
				SoundEngine.PlaySound(SoundID.DD2_DarkMageHealImpact, player.position);
			}

			return true;
		}
	}
}
