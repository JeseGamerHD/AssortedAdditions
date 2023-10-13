using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;


namespace ModdingTutorial.Content.Projectiles.MagicProj.ConcoctionsProj;

internal class FrozenConcoctionProj : ModProjectile
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

        Projectile.aiStyle = 0;
    }

    public override void AI()
    {
        // Projectile will rotate when thrown
        Projectile.rotation += 0.4f * Projectile.direction;

        if (Main.rand.NextBool(5)) // Dust has 20% chance to spawn when the projectile is in the air (trail effect)
        {
            Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                    DustID.PortalBolt, 0, 0, 100, Color.LightBlue, 3f);
            dust.noGravity = true;
        }

        Projectile.ai[0] += 1f; // Timer that starts up gravity after 30 ticks
        if (Projectile.ai[0] >= 30f)
        {
            Projectile.ai[0] = 30f;
            Projectile.velocity.Y = Projectile.velocity.Y + 0.3f;
        }

        if (Projectile.velocity.Y > 16f) // Max downwards velocity for gravity
        {
            Projectile.velocity.Y = 16f;
        }
    }

    public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
    {
        target.AddBuff(BuffID.Frostburn, 120);
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

        SoundEngine.PlaySound(SoundID.Item107, Projectile.position);
        for (int i = 0; i < 30; i++)
        {
            Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                DustID.ApprenticeStorm, 0, 0, 100, Color.LightBlue, 2.5f);
            dust.noGravity = true;
            dust.velocity *= 6f;

            dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                DustID.WhiteTorch, 0f, 0f, 100, Color.LightBlue, 3.5f);
            dust.noGravity = true;

        }
    }
}
