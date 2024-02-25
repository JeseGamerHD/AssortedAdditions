using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using AssortedAdditions.Content.Buffs;

namespace AssortedAdditions.Content.Projectiles.SummonProj
{
	internal class ShroomScepterMinion : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			Main.projFrames[Projectile.type] = 6;
			Main.projPet[Projectile.type] = true;
			ProjectileID.Sets.MinionSacrificable[Projectile.type] = true;
			ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true;
		}

		public override void SetDefaults()
		{
			Projectile.width = 44;
			Projectile.height = 34;
			Projectile.minionSlots = 1;
			Projectile.penetrate = -1;

			Projectile.friendly = true;
			Projectile.minion = true;

			Projectile.DamageType = DamageClass.Summon;
			Projectile.aiStyle = ProjAIStyleID.Pet;
			AIType = ProjectileID.BabySlime;
		}

		// Despawn minion if buff is removed. Otherwise keep minion alive
		public override bool PreAI()
		{
			Player owner = Main.player[Projectile.owner];
			if (CheckActive(owner))
			{
				return true;
			}

			return false;
		}

		public override void PostAI()
		{
			if(Main.rand.NextBool(8))
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.FungiHit, 1, 1, 0, default, 1f);
				dust.noGravity = true;
			}
		}

		private bool CheckActive(Player owner)
		{
			if (owner.dead || !owner.active)
			{
				owner.ClearBuff(ModContent.BuffType<ShroomScepterBuff>()); // If not, minion despawns
				return false;
			}

			if (owner.HasBuff(ModContent.BuffType<ShroomScepterBuff>()))
			{
				Projectile.timeLeft = 2;
			}

			return true;
		}

		public override bool? CanCutTiles()
		{
			return false;
		}

		// Minion won't deal damage without this (unless it fires projectiles)
		public override bool MinionContactDamage()
		{
			return true;
		}
	}
}
