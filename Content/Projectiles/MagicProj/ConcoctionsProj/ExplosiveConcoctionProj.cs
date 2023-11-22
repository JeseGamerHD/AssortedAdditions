using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;


namespace AssortedAdditions.Content.Projectiles.MagicProj.ConcoctionsProj;

internal class ExplosiveConcoctionProj : ModProjectile // Projectile for the Explosive Concoction weapon
{
    public override void SetDefaults()
    {
        Projectile.width = 32;
        Projectile.height = 32;

        Projectile.friendly = true;
        Projectile.timeLeft = 600;
        Projectile.DamageType = DamageClass.Magic;
        Projectile.ignoreWater = true;
        Projectile.tileCollide = true;
        Projectile.light = 0.25f;
        Projectile.stepSpeed = 10;

        Projectile.aiStyle = 2; // Thrown style
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        target.AddBuff(BuffID.OnFire, 180);
        Projectile.Kill(); // Projectile will break once it hits an enemy
    }

    // When projectile hits something, create an explosion
    public override void OnKill(int timeLeft)
    {
        // If the projectile dies without hitting an enemy, crate a small explosion that hits all enemies in the area.
        if (Projectile.penetrate == 1)
        {
            // Makes the projectile hit all enemies as it circunvents the penetrate limit.
            Projectile.maxPenetrate = -1;
            Projectile.penetrate = -1;

            int explosionArea = 60;
            Vector2 oldSize = Projectile.Size;
            // Resize the projectile hitbox to be bigger.
            Projectile.position = Projectile.Center;
            Projectile.Size += new Vector2(explosionArea);
            Projectile.Center = Projectile.position;

            Projectile.tileCollide = false;
            Projectile.velocity *= 0.01f;
            // Damage enemies inside the hitbox area
            Projectile.Damage();
            Projectile.scale = 0.01f;

            //Resize the hitbox to its original size
            Projectile.position = Projectile.Center;
            Projectile.Size = new Vector2(10);
            Projectile.Center = Projectile.position;
        }

        SoundEngine.PlaySound(SoundID.DD2_BetsyFireballShot, Projectile.position);
        for (int i = 0; i < 30; i++)
        {
            Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                DustID.OrangeTorch, 0, 0, 100, Color.Orange, 5.5f);
            dust.noGravity = true;
            dust.velocity *= 3f;


            dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
            DustID.OrangeTorch, 0, 0, 100, Color.Orange, 5.5f);
            dust.noGravity = true;
            dust.velocity *= 7f;


            dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                DustID.OrangeTorch, 0f, 0f, 100, Color.Red, 3.5f);
            dust.noGravity = true;

        }

    }
}
