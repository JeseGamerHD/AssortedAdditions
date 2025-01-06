using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Projectiles.NPCProj
{
	public abstract class HauntObjectThrow : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Type] = 7;
			ProjectileID.Sets.TrailingMode[Type] = 3;
		}

		private ref float TargetWhoAmI => ref Projectile.ai[1];
		private ref float HauntWhoAmI => ref Projectile.ai[2];

		private float spinSpeed = 0f;
		private bool velocitySet = false;
		private const float maxSpinSpeed = 22f;
		public override void AI()
		{
			Projectile.rotation += MathHelper.ToRadians(spinSpeed);

			if (spinSpeed < maxSpinSpeed)
			{
				spinSpeed += 0.2f;
				Projectile.velocity = Main.npc[(int)HauntWhoAmI].velocity;
				Projectile.tileCollide = false;
			}

			if (spinSpeed >= maxSpinSpeed && !velocitySet)
			{
				Player player = Main.player[(int)TargetWhoAmI];
				Vector2 direction = player.Center - Projectile.Center;
				direction.Normalize();
				Projectile.velocity = direction * 12f;
				velocitySet = true;
				Projectile.tileCollide = true;
			}

			// When initially spawning, make a dust cloud
			if (Projectile.timeLeft == 300)
			{
				for (int i = 0; i < 35; i++)
				{
					Dust dust = Dust.NewDustDirect(Projectile.position, 25, 25, DustID.Cloud, Projectile.velocity.X, Projectile.velocity.Y, 100, default, 2f);
					dust.noGravity = true;
				}
			}
		}

		public override bool OnTileCollide(Vector2 oldVelocity)
		{
			Collision.HitTiles(Projectile.position, Projectile.velocity, Projectile.width, Projectile.height); // Dust from tile when hit
			SoundEngine.PlaySound(SoundID.Dig, Projectile.Center); // Tile hit sound
			return true;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			Asset<Texture2D> texture = TextureAssets.Projectile[Projectile.type];

			// Use these to limit the trail to one frame, without this the whole spritesheet would draw.
			int frameHeight = texture.Value.Height / Main.projFrames[Projectile.type];
			Rectangle frame = new Rectangle(0, Projectile.frame * frameHeight, texture.Value.Width, frameHeight);

			// Redraw the projectile with the color not influenced by light
			Vector2 drawOrigin = new Vector2(texture.Value.Width * 0.5f, Projectile.height * 0.5f);
			for (int k = Projectile.oldPos.Length - 1; k > 0; k--)
			{
				Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
				Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
				Main.EntitySpriteDraw(texture.Value, drawPos, frame, color, Projectile.rotation, drawOrigin, Projectile.scale, SpriteEffects.None, 0);
			}

			return true;
		}
	}

	public class HauntGravestone : HauntObjectThrow
	{
		public override string Texture => $"Terraria/Images/Item_{ItemID.Gravestone}";

		public override void SetDefaults()
		{
			Projectile.width = 28;
			Projectile.height = 32;
			Projectile.timeLeft = 300;
			Projectile.hostile = true;
			CooldownSlot = ImmunityCooldownID.Bosses;
		}
	}

	public class HauntTombstone: HauntObjectThrow
	{
		public override string Texture => $"Terraria/Images/Item_{ItemID.Tombstone}";

		public override void SetDefaults()
		{
			Projectile.width = 26;
			Projectile.height = 30;
			Projectile.timeLeft = 300;
			Projectile.hostile = true;
			CooldownSlot = ImmunityCooldownID.Bosses;
		}
	}

	public class HauntChair : HauntObjectThrow
	{
		public override string Texture => $"Terraria/Images/Item_{ItemID.WoodenChair}";

		public override void SetDefaults()
		{
			Projectile.width = 16;
			Projectile.height = 32;
			Projectile.timeLeft = 300;
			Projectile.hostile = true;
			CooldownSlot = ImmunityCooldownID.Bosses;
		}
	}

	public class HauntCandelabra : HauntObjectThrow
	{
		public override string Texture => $"Terraria/Images/Item_{ItemID.Candelabra}";

		public override void SetDefaults()
		{
			Projectile.width = 22;
			Projectile.height = 28;
			Projectile.timeLeft = 300;
			Projectile.hostile = true;
			CooldownSlot = ImmunityCooldownID.Bosses;
		}
	}

	public class HauntTrashcan : HauntObjectThrow
	{
		public override string Texture => $"Terraria/Images/Item_{ItemID.TrashCan}";

		public override void SetDefaults()
		{
			Projectile.width = 28;
			Projectile.height = 32;
			Projectile.timeLeft = 300;
			Projectile.hostile = true;
			CooldownSlot = ImmunityCooldownID.Bosses;
		}
	}
}
