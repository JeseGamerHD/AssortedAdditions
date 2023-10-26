using Terraria;
using Terraria.ModLoader;
using ModdingTutorial.Content.Buffs;
using Microsoft.Xna.Framework;


namespace ModdingTutorial.Content.Projectiles
{
    internal class DraconicArmorProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 6;
        }
        public override void SetDefaults()
        {
            Projectile.width = 32;
            Projectile.height = 32;
            Projectile.tileCollide = false;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.damage = 40;
            Projectile.knockBack = 4f;
            Projectile.alpha = 50;

            Projectile.aiStyle = 0;
        }

        public override bool? CanCutTiles()
        {
            return false; // won't destroy plants, pots, etc...
        }

        public float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void AI()
        {
            Player player = Main.player[Projectile.owner];

            // Check that player is alive and has the buff
            if (!player.dead && player.HasBuff(ModContent.BuffType<DraconicArmorBuff>()))
            {
                // Keep projectile alive
                Projectile.timeLeft = 2;

                // Circles around the player
                Projectile.ai[0] += 0.05f;
                Vector2 Orbit = player.Center + new Vector2(0, 40).RotatedBy(Projectile.ai[0]);
                Projectile.Center = Orbit;

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
            }
            // If player dies or doesnt have the buff kill projectile
            else
            {
                Projectile.Kill();
            }
        }

    }
}
