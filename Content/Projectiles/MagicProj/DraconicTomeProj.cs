using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Projectiles.MagicProj
{
    internal class DraconicTomeProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 16;
        }

        public override void SetDefaults()
        {
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.timeLeft = 3600;
            Projectile.alpha = 0;
            Projectile.light = 0.5f;
            Projectile.scale = 2f;

            Projectile.DamageType = DamageClass.Magic;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.penetrate = -1;
            Projectile.hostile = false;
            Projectile.friendly = true;

            Projectile.aiStyle = 0;
        }

        private bool setOnce = false;
        public override void AI()
        {
            // Loop through the sprite frames
            int frameSpeed = 2;
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

            // Faces the right way
            if (setOnce == false)
            {
                Projectile.spriteDirection = Main.player[Projectile.owner].direction;
                setOnce = true; // Only set once when it spawns
            }


            Lighting.AddLight(Projectile.position, TorchID.Torch); // Emits some orange light

            // Fade out projectile once player dies, or when reaching the end of its lifetime
            if (Main.player[Projectile.owner].dead || Projectile.timeLeft == 130)
            {
                Projectile.timeLeft = 127;

                if (Projectile.timeLeft <= 127)
                {
                    Projectile.alpha += 2;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.OnFire, 120);
        }
    }
}
