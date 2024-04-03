using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Projectiles.RangedProj
{
	internal class GraniteChakramProj : ModProjectile
	{
		public override string Texture => "AssortedAdditions/Content/Items/Weapons/Ranged/GraniteChakram";

		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Type] = 10;
			ProjectileID.Sets.TrailingMode[Type] = 3;
		}

		public override void SetDefaults()
		{
			Projectile.width = 38;
			Projectile.height = 38;
			Projectile.penetrate = -1;

			Projectile.ignoreWater = true;
			Projectile.tileCollide = true;
			Projectile.friendly = true;

			Projectile.DamageType = DamageClass.Ranged;

			Projectile.aiStyle = ProjAIStyleID.Boomerang;
			AIType = ProjectileID.LightDisc;
		}

		public override void PostAI()
		{
			if (Main.rand.NextBool(4))
			{
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Granite);
				dust.noGravity = true;
				dust.velocity *= 1.5f;
			}

			Lighting.AddLight((int)Projectile.position.X / 16, (int)Projectile.position.Y / 16, TorchID.Purple, 0.25f);
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Asset<Texture2D> texture = TextureAssets.Projectile[Projectile.type];

			// Use these to limit the trail to one frame, without this the whole spritesheet would draw.
			int frameHeight = texture.Value.Height / Main.projFrames[Projectile.type];
			Rectangle frame = new Rectangle(0, Projectile.frame * frameHeight, texture.Value.Width, frameHeight);

			// Redraw the projectile with the color not influenced by light
			Vector2 drawOrigin = new Vector2(texture.Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = 0; k < Projectile.oldPos.Length; k++)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture.Value, drawPos, frame, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}

			return true;
		}
	}
}
