using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameContent;
using Microsoft.Xna.Framework;
using AssortedAdditions.Content.Buffs.Pets;

namespace AssortedAdditions.Content.Projectiles.PetProj
{
	internal class PetHauntling : ModProjectile
	{
		public override void SetStaticDefaults()
		{
			ProjectileID.Sets.TrailCacheLength[Projectile.type] = 5; // The length of old position to be recorded
			ProjectileID.Sets.TrailingMode[Projectile.type] = 3; // The recording mode

			Main.projFrames[Projectile.type] = 2;
			Main.projPet[Projectile.type] = true;

			ProjectileID.Sets.CharacterPreviewAnimations[Projectile.type] = ProjectileID.Sets.SimpleLoop(0, Main.projFrames[Projectile.type], 20)
			.WithOffset(-10, -20f) // Offset the sprite so it looks good
			.WithSpriteDirection(-1) // Direction it faces
			.WhenNotSelected(0, 0) // Stops animation when not hovering over the icon
			.WithCode(DelegateMethods.CharacterPreview.Float);
		}

		public override void SetDefaults()
		{
			Projectile.width = 40;
			Projectile.height = 40;

			Projectile.friendly = true;
			Projectile.ignoreWater = true;
			Projectile.tileCollide = false;
		}

		private float speed = 7.5f;
		private float inertia = 15f;

		// When the pet teleports to the player, the trail messes up since it uses the oldPos which was the position before teleporting
		// use this to give a cooldown before starting the trail again
		private ref float JustTeleported => ref Projectile.ai[0]; 
		public override void AI()
		{
			Player player = Main.player[Projectile.owner];
			float distanceFromTarget = Vector2.Distance(player.Center, Projectile.Center);
			
			// Keep the projectile from disappearing as long as the player isn't dead and has the pet buff.
			if (!player.dead && player.HasBuff(ModContent.BuffType<PetHauntlingBuff>()))
			{
				Projectile.timeLeft = 2;
			}

			if(JustTeleported > 0)
			{
				JustTeleported--;
			}

			if (distanceFromTarget > 60f)
			{
				speed = 7.5f;
				Vector2 direction;
				if (player.direction == 1)
				{
					direction = new Vector2(player.position.X, player.position.Y - 40) - Projectile.position;
				}
				else
				{
					direction = new Vector2(player.position.X, player.position.Y - 40) - Projectile.position;
				}

				direction.Normalize();
				direction *= speed;

				Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia;
			}
			else
			{
				Projectile.velocity *= 0.8f;
			}

			if(distanceFromTarget > 1500f)
			{
				Projectile.position = player.position;
				Projectile.velocity *= 0.1f;
				JustTeleported = 3;
				Projectile.netUpdate = true;
			}

			if(Projectile.velocity.HasNaNs())
			{
				Projectile.velocity = player.velocity;
			}

			// Loop through the sprite frames
			int frameSpeed = 20;
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
			Projectile.spriteDirection = -Projectile.direction;
		}

		public override Color? GetAlpha(Color lightColor)
		{
			return Color.White;
		}

		public override bool PreDraw(ref Color lightColor)
		{
			if(JustTeleported <= 0)
			{
				Asset<Texture2D> texture = TextureAssets.Projectile[Projectile.type];

				// Use these to limit the trail to one frame, without this the whole spritesheet would draw
				int frameHeight = texture.Value.Height / Main.projFrames[Projectile.type];
				Rectangle frame = new Rectangle(0, Projectile.frame * frameHeight, texture.Value.Width, frameHeight);

				// Redraw the projectile with the color not influenced by light
				Vector2 drawOrigin = new Vector2(texture.Value.Width * 0.5f, Projectile.height * 0.5f);
				for (int k = 0; k < Projectile.oldPos.Length; k++)
				{
					Vector2 drawPos = Projectile.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, Projectile.gfxOffY);
					Color color = Projectile.GetAlpha(lightColor) * ((Projectile.oldPos.Length - k) / (float)Projectile.oldPos.Length);
					SpriteEffects spriteDir = Projectile.direction == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
					Main.EntitySpriteDraw(texture.Value, drawPos, frame, color, Projectile.rotation, drawOrigin, Projectile.scale, spriteDir, 0);
				}
			}

			return true;
		}
	}
}
