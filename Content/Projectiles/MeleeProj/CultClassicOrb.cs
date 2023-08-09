using Terraria;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Projectiles.MeleeProj
{
    internal class CultClassicOrb : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
        }

        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.light = 0.5f;
            Projectile.penetrate = -1;
            Projectile.aiStyle = 0;

            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.friendly = true;

            Projectile.DamageType = DamageClass.MeleeNoSpeed;
        }

        public override void AI()
        {
            // Loop through the sprite frames
            int frameSpeed = 5;
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= frameSpeed)
            {
                Projectile.frameCounter = 0;
                Projectile.frame++;

                if (Projectile.frame >= Main.projFrames[Projectile.type])
                {
                    Projectile.frame = 0;
                }
            }

            // This projectile is spawned inside its parent projectile (CultClassicProj) and it stays on top of it.
            Projectile.Center = Main.projectile[(int)Projectile.ai[0]].Center;

            // If the parent dies, kill this one as well
            if(Main.projectile[(int)Projectile.ai[0]].active == false)
            {
                Projectile.Kill();
            }
        }
    }
}
