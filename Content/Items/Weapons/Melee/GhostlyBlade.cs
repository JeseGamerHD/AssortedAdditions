using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using AssortedAdditions.Content.Projectiles.MeleeProj;

namespace AssortedAdditions.Content.Items.Weapons.Melee
{
	internal class GhostlyBlade : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 42;
			Item.height = 36;
			Item.useAnimation = 23;
			Item.useTime = 23;
			Item.damage = 30;
			Item.knockBack = 2f;
			Item.alpha = 55;
			Item.scale = 1.25f;

			Item.autoReuse = true;

			Item.value = Item.sellPrice(silver: 50);
			Item.UseSound = SoundID.Item1;
			Item.rare = ItemRarityID.Green;
			Item.DamageType = DamageClass.Melee;
			Item.useStyle = ItemUseStyleID.Swing;
		}

		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			if(target.life <= 0)
			{
				for(int i = 0; i < 10; i++)
				{
					Dust dust = Dust.NewDustDirect(target.Center, target.width, target.height, DustID.WhiteTorch, 0f, 0f, 75, default, 1f);
					dust.velocity *= 1.4f;
					dust.noGravity = true;
				}

				if(Main.myPlayer == player.whoAmI)
				{
					Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center, Vector2.Zero, ModContent.ProjectileType<GhostlyBladeProj>(), 30, 2, player.whoAmI);
				}			
			}
		}
	}
}
