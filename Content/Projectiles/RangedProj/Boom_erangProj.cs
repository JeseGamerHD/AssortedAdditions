using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Projectiles.RangedProj
{
    internal class Boom_erangProj : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.width = 24;
            Projectile.height = 32;
            Projectile.aiStyle = 3;
            Projectile.penetrate = -1;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.friendly = true;

            Projectile.DamageType = DamageClass.Ranged;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            ExplosionEffect();
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            ExplosionEffect();
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            ExplosionEffect();
            return base.OnTileCollide(oldVelocity);
        }

        private void ExplosionEffect()
        {
            int explosionArea = 15;
            Vector2 oldSize = Projectile.Size;
            // Resize the projectile hitbox to be bigger.
            Projectile.position = Projectile.Center;
            Projectile.Size += new Vector2(explosionArea);
            Projectile.Center = Projectile.position;

            Projectile.tileCollide = false;
            Projectile.velocity *= 0.01f;
            // Damage enemies inside the hitbox area
            Projectile.Damage();

            //Resize the hitbox to its original size
            Projectile.position = Projectile.Center;
            Projectile.Size = new Vector2(10);
            Projectile.Center = Projectile.position;

            // Sound effect
            SoundEngine.PlaySound(SoundID.Item14, Projectile.position);

            // Smoke Dust
            for (int i = 0; i < 30; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 2f);
                dust.velocity *= 1.4f;
            }

            // Fire Dust
            for (int i = 0; i < 40; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 3f);
                dust.noGravity = true;
                dust.velocity *= 5f;
                dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 2f);
                dust.velocity *= 3f;
                dust.noGravity = true;
            }
        }
    }
}
