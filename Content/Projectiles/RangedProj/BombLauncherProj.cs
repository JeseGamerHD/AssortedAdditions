using Terraria.Audio;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ModdingTutorial.Content.Projectiles.RangedProj
{
    internal class BombLauncherProj : ModProjectile
    {
        public override string Texture => "ModdingTutorial/Content/Projectiles/NPCProj/GoblinBomb";
        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 30;
            Projectile.light = 0.1f;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.friendly = true;

            Projectile.DamageType = DamageClass.Ranged;

            Projectile.aiStyle = -1;
        }

        public override void AI()
        {
            if(Projectile.direction == 1)
            {
                Projectile.rotation += 0.4f;
            } 
            else {
                Projectile.rotation -= 0.4f;
            }

            Projectile.velocity.Y = Projectile.velocity.Y + 0.18f;
            if (Projectile.velocity.Y > 16f)
            {
                Projectile.velocity.Y = 16f;
            }
        }

        public override void OnKill(int timeLeft)
        {
            int explosionArea = 80;
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
                dust.velocity *= 1.2f;
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
