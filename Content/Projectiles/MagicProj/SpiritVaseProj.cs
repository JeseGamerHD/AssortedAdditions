using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Graphics.Shaders;
using Terraria.Graphics;

namespace AssortedAdditions.Content.Projectiles.MagicProj
{
	internal class SpiritVaseProj : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			// For shader trail
			ProjectileID.Sets.TrailCacheLength[Type] = 28;
			ProjectileID.Sets.TrailingMode[Type] = 3;
		}

		public override void SetDefaults()
		{
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.aiStyle = 0;
			Projectile.light = 0.25f;
			Projectile.alpha = 255;
			Projectile.timeLeft = 300;
			Projectile.penetrate = -1;

			Projectile.DamageType = DamageClass.Magic;

			Projectile.usesIDStaticNPCImmunity = true;
			Projectile.tileCollide = false;
			Projectile.friendly = true;
			Projectile.ignoreWater = true;
		}

		public override void AI()
		{

			// After hitting a target the projectile will fly off in the direction it was headed when the hit occurred (ai[0] is increased)
			// If ai[1] is not 0, then the projectile's timeLeft is running out
			if (Projectile.ai[0] == 0 && Projectile.ai[1] == 0)
			{
				// Since the projectile follows the mouse, only the owner can set the value
				if (Main.myPlayer == Projectile.owner)
				{
					Projectile.ai[2] = (Main.MouseWorld - Projectile.Center).ToRotation();
					Projectile.netUpdate = true; // Value needs to be synced even though it it using the ai array
				}

				// With these the projectile will move towards the target in a smooth way
				float curve = Projectile.velocity.ToRotation();
				float maxTurn = MathHelper.ToRadians(5f); // Lower values mean faster turning
				Projectile.velocity = Projectile.velocity.RotatedBy(MathHelper.WrapAngle(curve.AngleTowards(Projectile.ai[2], maxTurn)) - curve);
			}

			// Keep projectile alive until the trail has faded enough
			if (Projectile.timeLeft == 1 && Projectile.ai[1] <= 60)
			{
				Projectile.timeLeft = 2;
				Projectile.ai[1]++;
			}

			// Slow down the projectile to fade out the trail when the projectile starts to die
			if (Projectile.ai[1] != 0)
			{
				Projectile.velocity = 0.95f * Projectile.velocity;
			}

			Projectile.rotation = Projectile.velocity.ToRotation();
			Lighting.AddLight(Projectile.Center, TorchID.Blue);
		}

		public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
		{
			Projectile.ai[0]++; // Fly off after hitting a target
		}

		public override void OnHitPlayer(Player target, Player.HurtInfo info)
		{
			Projectile.ai[0]++;
		}

		public override bool? CanHitNPC(NPC target)
		{
			if (Projectile.ai[0] > 3) // Can only hit three times
			{
				return false;
			}

			return base.CanHitNPC(target);
		}

		public override void OnKill(int timeLeft)
		{
			for(int i = 0; i < 30 ; i++)
			{
				Dust dust = Dust.NewDustDirect(Projectile.Center, Projectile.width * 2, Projectile.height * 2, DustID.GemDiamond, 0f, 0f, 75, Color.Cyan, 1f);
				dust.velocity *= 1.4f;
				dust.noGravity = true;
			}
		}

		private static readonly VertexStrip _vertexStrip = new();

		public override bool PreDraw(ref Color lightColor)
		{
			MiscShaderData miscShaderData = GameShaders.Misc["MagicMissile"];
			miscShaderData.UseSaturation(-2.8f);
			miscShaderData.UseOpacity(4f);
			miscShaderData.Apply();
			_vertexStrip.PrepareStripWithProceduralPadding(Projectile.oldPos, Projectile.oldRot, StripColors, StripWidth, -Main.screenPosition + Projectile.Size / 2, true);
			_vertexStrip.DrawTrail();
			Main.pixelShader.CurrentTechnique.Passes[0].Apply();

			return true;
		}

		private Color StripColors(float progressOnStrip)
		{
			Color result = Color.Lerp(Color.Cyan, Color.GhostWhite, Utils.GetLerpValue(-0.2f, 0.5f, progressOnStrip, clamped: true)) * (1f - Utils.GetLerpValue(0f, 0.98f, progressOnStrip));
			result.A = 0;
			return result;
		}

		private float StripWidth(float progressOnStrip)
		{
			float num = 3.5f;
			float lerpValue = Utils.GetLerpValue(0f, 0.7f, progressOnStrip, clamped: true);
			num *= 1f - (1f - lerpValue) * (1f - lerpValue);
			return MathHelper.Lerp(16f, 32f, num);
		}
	}
}
