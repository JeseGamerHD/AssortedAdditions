using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;


namespace AssortedAdditions.Content.Projectiles.MagicProj.ConcoctionsProj;

internal class LightConcoctionProj : ModProjectile
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
        Projectile.light = 1f;
        Projectile.stepSpeed = 10;

        Projectile.aiStyle = 0;
    }

    public override void AI()
    {
        // Projectile will rotate when thrown
        Projectile.rotation += 0.4f * Projectile.direction;

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

        SoundEngine.PlaySound(SoundID.Item50, Projectile.position);
        SoundEngine.PlaySound(SoundID.Item43, Projectile.position);

        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ProjectileID.PrincessWeapon, Projectile.damage, Projectile.knockBack, Projectile.owner);

        for (int i = 0; i < 30; i++)
        {
            Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                DustID.GolfPaticle, 0, 0, 100, Color.Pink, 2.5f);
            dust.noGravity = true;
            dust.velocity.Y *= 1f;
            dust.velocity *= 10f;
            dust.noLight = false;
        }

    }
}
