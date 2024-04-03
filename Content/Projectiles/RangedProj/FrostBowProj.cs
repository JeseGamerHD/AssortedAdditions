using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Projectiles.RangedProj
{
    internal class FrostBowProj : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; // The length of old position to be recorded
            ProjectileID.Sets.TrailingMode[Projectile.type] = 3;
            // Actual trail is drawn in PreDraw()
        }

        public override void SetDefaults()
        {
            Projectile.width = 46;
            Projectile.height = 18;
            Projectile.light = 1f;
            Projectile.alpha = 50;
            Projectile.aiStyle = 0;

            Projectile.penetrate = 1;
            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.DamageType = DamageClass.Ranged;
        }

        // Timer used for slowing down and adding gravity to the projectile
        public float Timer
        {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }

        public override void AI()
        {
            Timer++; // Count up time

            // Face towards where its going
            Projectile.rotation = Projectile.velocity.ToRotation();

            // Leave a dust trail
            if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                            DustID.Ice, 0, 0, 150, default, 1f);
                dust.noGravity = true;

                Dust dust2 = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                            DustID.IceTorch, 0, 0, 150, default, 2f);
                dust2.noGravity = true;
            }

            // Slows down and falls shortly after firing
            if (Timer >= 15)
            {
                if (Projectile.direction == 1) // If moving to the right, subtract from X
                {
                    Projectile.velocity.X -= 0.2f;
                    if (Projectile.velocity.X < 3f)
                    {
                        Projectile.velocity.X = 3f;
                    }
                }

                if (Projectile.direction == -1) // If moving to the left, add to X
                {
                    Projectile.velocity.X += 0.2f;
                    if (Projectile.velocity.X > -3f)
                    {
                        Projectile.velocity.X = -3f;
                    }
                }

            }

            if (Timer >= 30)
            {
                Projectile.velocity.Y = Projectile.velocity.Y + 0.5f;
                if (Projectile.velocity.Y > 16f) // Increase Y velocity to add gravity until max of 16 is reached
                {
                    Projectile.velocity.Y = 16f;
                }
            }
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(BuffID.Frostburn2, 180);
        }

        public override void OnKill(int timeLeft)
        {
            SoundEngine.PlaySound(SoundID.NPCDeath7, Projectile.position);
            for (int i = 0; i < 30; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.IceTorch, 0f, 0f, 100, default, 2f);
                dust.velocity *= 1.4f;
                dust.noGravity = true;
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
			Asset<Texture2D> texture = TextureAssets.Projectile[Projectile.type];

			// Redraw the projectile with the color not influenced by light
			Vector2 drawOrigin = new Vector2(texture.Value.Width * 0.5f, Projectile.height * 0.5f);
            for (int k = 0; k < Projectile.oldPos.Length; k++)
            {
                Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
                Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
                Main.EntitySpriteDraw(texture.Value, drawPos, null, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
            }

            return true;
        }
    }
}
