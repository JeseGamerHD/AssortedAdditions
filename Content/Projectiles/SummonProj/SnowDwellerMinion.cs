using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using AssortedAdditions.Content.Buffs;

namespace AssortedAdditions.Content.Projectiles.SummonProj
{
    internal class SnowDwellerMinion : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projPet[Projectile.type] = true; // Denotes that this projectile is a pet or minion
            Main.projFrames[Projectile.type] = 15;

            ProjectileID.Sets.MinionSacrificable[Projectile.type] = true; // This is needed so your minion can properly spawn when summoned and replaced when other minions are summoned
            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true; // Make the cultist resistant to this projectile, as it's resistant to all homing projectiles.
        }

        public override void SetDefaults()
        {
            Projectile.width = 36;
            Projectile.height = 40;
            Projectile.penetrate = -1;
            Projectile.minionSlots = 1f;

            Projectile.tileCollide = true;
            Projectile.friendly = true;
            Projectile.minion = true;

            Projectile.DamageType = DamageClass.Summon;
            Projectile.aiStyle = 67; // Pirate staff
            AIType = ProjectileID.OneEyedPirate;
            // The downside to using this is that the minion poops
            // although it sort of fits the creature...
        }

        // Despawn minion if buff is removed
        // Otherwise keep minion alive
        public override bool PreAI()
        {
            Player owner = Main.player[Projectile.owner];

            if (CheckActive(owner))
            {
                return true;
            }

            return false;
        }

		private bool CheckActive(Player owner)
		{
			if (owner.dead || !owner.active)
			{
				owner.ClearBuff(ModContent.BuffType<SnowDwellerBuff>()); // If not, minion despawns
				return false;
			}

			if (owner.HasBuff(ModContent.BuffType<SnowDwellerBuff>()))
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
