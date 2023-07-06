using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;


namespace ModdingTutorial.Content.Projectiles.MagicProj.ConcoctionsProj;

internal class IchorConcoctionProj : ModProjectile
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

        Projectile.aiStyle = 0;
    }

    public override void AI()
    {
        // Projectile will rotate when thrown
        Projectile.rotation += 0.4f * Projectile.direction;

        if (Main.rand.NextBool(15)) // Chance to spawn dust when in the air
        {
            Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                    DustID.IchorTorch, 0, 0, 100, Color.LightPink, 3f);
            dust.noGravity = false;
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
        target.AddBuff(BuffID.Ichor, 600);
        Projectile.Kill(); // Projectile will break once it hits an enemy
    }

    // When projectile hits something, create an explosion
    public override void Kill(int timeLeft)
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

        SoundEngine.PlaySound(SoundID.Item50, Projectile.position);
        SoundEngine.PlaySound(SoundID.Item34, Projectile.position);
        for (int i = 0; i < 30; i++)
        {
            Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                DustID.YellowTorch, 0, 0, 100, Color.Yellow, 5.5f);
            dust.noGravity = true;
            dust.velocity.Y *= 1f;
            dust.velocity *= 10f;

            dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                DustID.IchorTorch, 0f, 0f, 100, Color.LightPink, 3.5f);
            dust.noGravity = true;

        }

    }
}
