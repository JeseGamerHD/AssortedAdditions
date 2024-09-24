using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;
using Terraria.ID;
using Terraria.ModLoader;
using AssortedAdditions.Helpers;
using System;

namespace AssortedAdditions.Content.Projectiles.RangedProj
{
    internal class ShootingStarProj : ModProjectile
    {
        public override void SetStaticDefaults() // These are needed for shader trail
        {
            ProjectileID.Sets.TrailCacheLength[Type] = 45;
            ProjectileID.Sets.TrailingMode[Type] = 3;
        }

        public override void SetDefaults()
        {
            Projectile.width = 22;
            Projectile.height = 24;
            Projectile.light = 1f;
            Projectile.alpha = 50;
            Projectile.aiStyle = 0;
            Projectile.timeLeft = 300;

            Projectile.tileCollide = true;
            Projectile.ignoreWater = true;
            Projectile.friendly = true;

            Projectile.DamageType = DamageClass.Ranged;
        }

        public ref float Timer => ref Projectile.ai[0];


        public override void AI()
        {
            NPC target = HelperMethods.FindClosestNPC(Projectile.Center, 1000f);

			Timer++;
			if (Timer < 120f && Timer > 15f && target != null)
			{
				float speed = Projectile.velocity.Length();
				Vector2 direction = target.Center - Projectile.Center;
				direction.Normalize();
				direction *= speed;

				Projectile.velocity = (Projectile.velocity * 5f + direction) / 30f;
				Projectile.velocity.Normalize();
				Projectile.velocity *= speed;
            }

            if(target == null)
            {
				Projectile.velocity.Y = Projectile.velocity.Y + 0.3f; // 0.1f for arrow gravity, 0.4f for knife gravity
				if (Projectile.velocity.Y > 16f)
				{
					Projectile.velocity.Y = 16f;
				}
			}

            if (Projectile.velocity.Length() < 12f)
            {
                Projectile.velocity *= 1.03f;
            }

			Projectile.spriteDirection = Projectile.direction;
			if (Projectile.direction < 0)
			{
				Projectile.rotation = (float)Math.Atan2(0f - Projectile.velocity.Y, 0f - Projectile.velocity.X);
			}
			else
			{
				Projectile.rotation = (float)Math.Atan2(Projectile.velocity.Y, Projectile.velocity.X);
			}

			// Leave a dust trail
			if (Main.rand.NextBool(2))
            {
                Dust dust = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                            DustID.CorruptTorch, 0, 0, 150, default, 1f);
                dust.noGravity = true;

                Dust dust2 = Dust.NewDustDirect(Projectile.position - Projectile.velocity, Projectile.width, Projectile.height,
                            DustID.PlatinumCoin, 0, 0, 150, default, 2f);
                dust2.noGravity = true;
            }

        }

        public override void OnKill(int timeLeft)
        {
            for (int i = 0; i < 30; i++)
            {
                Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.PurpleTorch, 0f, 0f, 100, default, 2f);
                dust.velocity *= 1.4f;
                dust.noGravity = true;
            }
        }

        // The cool trail effect happens under here:

        private static readonly VertexStrip _vertexStrip = new();

        public override bool PreDraw(ref Color lightColor)
        {
            MiscShaderData miscShaderData = GameShaders.Misc["MagicMissile"]; // Basically a copy of magic missile with its colors changed
            miscShaderData.UseSaturation(-2.8f);
            miscShaderData.UseOpacity(4f);
            miscShaderData.Apply();
            _vertexStrip.PrepareStripWithProceduralPadding(Projectile.oldPos, Projectile.oldRot, StripColors, StripWidth, -Main.screenPosition + Projectile.Size / 2);
            _vertexStrip.DrawTrail();
            Main.pixelShader.CurrentTechnique.Passes[0].Apply();

            return true;
        }

        private Color StripColors(float progressOnStrip)
        {
            Color result = Color.Lerp(Color.Indigo, Color.Indigo, Utils.GetLerpValue(-0.2f, 0.5f, progressOnStrip, clamped: true)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip));
            result.A = 0;
            return result;
        }

        private float StripWidth(float progressOnStrip)
        {
            float num = 1f;
            float lerpValue = Utils.GetLerpValue(0f, 0.2f, progressOnStrip, clamped: true);
            num *= 1f - (1f - lerpValue) * (1f - lerpValue);
            return MathHelper.Lerp(0f, 32f, num);
        }
    }
}
