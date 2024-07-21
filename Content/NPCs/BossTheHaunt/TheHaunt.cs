﻿using System.Collections.Generic;
using System.IO;
using AssortedAdditions.Content.Projectiles.NPCProj;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.NPCs.BossTheHaunt
{
	[AutoloadBossHead]
	internal class TheHaunt : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 5;
			
			NPCID.Sets.TrailCacheLength[NPC.type] = 6;
			NPCID.Sets.TrailingMode[NPC.type] = 3;

			NPCID.Sets.BossBestiaryPriority.Add(Type);
			NPCID.Sets.MPAllowedEnemies[Type] = true;
			NPCID.Sets.CantTakeLunchMoney[Type] = true;
			NPCID.Sets.ImmuneToRegularBuffs[Type] = true;
		}

		public override void SetDefaults()
		{
			NPC.width = 128;
			NPC.height = 162;

			NPC.value = 100000;

			NPC.damage = 65;
			NPC.defense = 25;
			NPC.lifeMax = 29500;
			NPC.knockBackResist = 0f;

			NPC.HitSound = SoundID.NPCHit7;
			NPC.DeathSound = SoundID.DD2_BetsyDeath;
			NPC.SpawnWithHigherTime(30);
			NPC.npcSlots = 6f;

			NPC.noTileCollide = true;
			NPC.noGravity = true;
			NPC.boss = true;

			NPC.aiStyle = -1;

			if (!Main.dedServ)
			{
				Music = MusicID.OtherworldlyPlantera;
				// maybe 79 (underground hallow otherwordly)
			}
		}

		public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
		{
			// 20% increase in health per player in multiplayer
			// in singleplayer nothing happens
			float multiplier = numPlayers > 1 ? 0.2f : 0;
			NPC.lifeMax = (int)(NPC.lifeMax * (1 + multiplier * (numPlayers - 1)));
		}

		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.GreaterHealingPotion; // Drop healing potions (default is lesser healing)
		}

		private float speed = 4f;
		private float inertia = 10f;
		private int timer = 0;

		// All the different main states the boss has
		private enum States
		{
			Chase,
			Dash,
			Flee
		}

		public ref float State => ref NPC.ai[0];

		public override void AI()
		{
			// TEST CODE TODO PROPER MOVEMENT
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
			{
				NPC.TargetClosest();
			}
			Player target = Main.player[NPC.target];
			float distanceFromTarget = Vector2.Distance(target.Center, NPC.Center);

			// Attack, fly towards the player
			if (distanceFromTarget > 40f && State != (float)States.Dash)
			{
				// The immediate range around the target (so it doesn't latch onto it when close)
				Vector2 direction = target.Center - NPC.Center;
				direction.Normalize();
				direction *= speed;

				NPC.velocity = (NPC.velocity * (inertia - 1) + direction) / inertia;
				NPC.spriteDirection = target.Center.X < NPC.Center.X ? -1 : 1;
			}

			if (timer >= 900)
			{
				if(timer == 900)
				{
					dashStateJustStarted = true;
					State = (float)States.Dash;
				}
				//ThrowFurniture(target);
				DashState(target);
			}
			timer++;

			if (timer >= 1800)
			{
				if(timer == 1800)
				{
					dashStateJustEnded = true;
				}

				timer = 0;
				State = (float)States.Chase;
				NPC.velocity = Vector2.Zero;
			}

			if (dashStateJustEnded && NPC.alpha > 0)
			{
				NPC.alpha -= 5;

				if(NPC.alpha < 0)
				{
					NPC.alpha = 0;
				}
			}
			else if (dashStateJustEnded && NPC.alpha == 0)
			{
				dashStateJustEnded = false;
			}
		}

		// All the different types of furniture the boss can throw at the player
		private readonly int[] furniture = {
			ModContent.ProjectileType<HauntGravestone>(),
			ModContent.ProjectileType<HauntTombstone>(),
			ModContent.ProjectileType<HauntChair>(),
			ModContent.ProjectileType<HauntCandelabra>(),
			ModContent.ProjectileType<HauntTrashcan>()
		};

		private int projectileSpawnTimer = 0;
		private void ThrowFurniture(Player target)
		{
			if (projectileSpawnTimer % 60 == 0)
			{
				float angle = 0;
				switch (projectileSpawnTimer)
				{
					case 0:
						angle = MathHelper.ToRadians(300);
						break;

					case 60:
						angle = MathHelper.ToRadians(0);
						break;

					case 120:
						angle = MathHelper.ToRadians(60);
						break;

					case 180:
						angle = MathHelper.ToRadians(330);
						break;

					case 240:
						angle = MathHelper.ToRadians(30);
						break;
				}

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					int type = furniture[Main.rand.Next(0, furniture.Length)];
					Vector2 spawnPos = new Vector2(NPC.Center.X, NPC.Center.Y - 100).RotatedBy(angle, NPC.Center);
					Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, Vector2.Zero, type, 45, 5f, Main.myPlayer, 0, target.whoAmI, NPC.whoAmI);
				}
			}

			if (projectileSpawnTimer >= 300)
			{
				projectileSpawnTimer = 0;
				timer = 0;
				return;
				// STOP THROWING STATE TODO
			}

			projectileSpawnTimer++;
		}

		private float randomRotation = 0;
		private int dashTimer = 0;
		private bool dashStateJustStarted = false;
		private bool dashStateJustEnded = false;
		private void DashState(Player target)
		{
			// When Dash State begins, slow the haunt down and make it turn invisible
			if(dashStateJustStarted)
			{
				NPC.alpha += 5;
				NPC.velocity *= 0.7f;
			}

			// Once fully invisible, dashing can start
			if(dashStateJustStarted && NPC.alpha >= 255)
			{
				NPC.alpha = 255;
				dashStateJustStarted = false;
			}

			// Otherwise keep increasing alpha
			if(dashStateJustStarted && NPC.alpha < 255)
			{
				return;
			}

			if (dashTimer == 0)
			{
				NPC.velocity = Vector2.Zero;
				randomRotation = Main.rand.NextFloat(0, 6.2f);
				NPC.netUpdate = true;

				// start position is essentially a random spot along a circle which center is the player
				// The npc will teleport to this position
				Vector2 dashStartPos = new Vector2(target.Center.X + 500, target.Center.Y).RotatedBy(randomRotation, target.Center);
				NPC.Center = dashStartPos;

				NPC.spriteDirection = target.Center.X < NPC.Center.X ? -1 : 1;
			}

			dashTimer++;

			if (dashTimer == 30)
			{
				Vector2 dashDirection = target.Center - NPC.Center;
				NPC.velocity = dashDirection.SafeNormalize(Vector2.Zero) * 12f;
			}

			if (dashTimer >= 30)
			{
				NPC.alpha += 2;
			}
			else
			{
				NPC.alpha -= 9;
			}

			// Dash ends, new one begins
			if (dashTimer >= 180)
			{
				dashTimer = 0;
			}

			// The NPC.alpha can be given values below 0 and over 255
			// limit the alpha to the intented range
			if (NPC.alpha < 0)
			{
				NPC.alpha = 0;
			}

			if (NPC.alpha > 255)
			{
				NPC.alpha = 255;
			}
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(randomRotation);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			randomRotation = reader.ReadSingle();
		}

		private int currentFrame = 0;
		public override void FindFrame(int frameHeight)
		{
			int frameSpeed = 12;
			NPC.frameCounter++;

			if (NPC.frameCounter % frameSpeed == 0)
			{
				currentFrame++;
			}

			if (currentFrame > 4)
			{
				currentFrame = 0;
			}

			NPC.frame.Y = frameHeight * currentFrame;
		}

		public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Asset<Texture2D> texture = TextureAssets.Npc[NPC.type];

			int frameHeight = texture.Value.Height / Main.npcFrameCount[NPC.type];
			Rectangle frame = new Rectangle(0, currentFrame * frameHeight, texture.Value.Width, frameHeight);

			// Redraw the projectile with the color not influenced by light
			Vector2 drawOrigin = new Vector2(texture.Value.Width * 0.5f, NPC.height * 0.5f);
			for (int k = 0; k < NPC.oldPos.Length; k++)
			{
				Vector2 drawPos = NPC.oldPos[k] - Main.screenPosition + drawOrigin + new Vector2(0f, NPC.gfxOffY);
				Color color = NPC.GetAlpha(drawColor) * ((NPC.oldPos.Length - k) / (float)NPC.oldPos.Length);

				SpriteEffects spriteDirection = SpriteEffects.None;
				if(State != (float)States.Dash)
				{
					spriteDirection = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
				}
				else if (State == (float)States.Dash)
				{
					spriteDirection = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
				}
				
				Main.EntitySpriteDraw(texture.Value, drawPos, frame, color * 0.85f, NPC.rotation, drawOrigin, NPC.scale, spriteDirection, 0);
			}

			return true;
		}

		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			// When dash state begins, the haunt turns invisible, turn the glowmask off until dash state is fully ready
			if (!dashStateJustStarted) 
			{
				int frames = 5; // Amount of frames in the sprite
				Asset<Texture2D> glowMask = ModContent.Request<Texture2D>("AssortedAdditions/Content/NPCs/BossTheHaunt/TheHaunt_Glow");
				Vector2 drawPosition = NPC.position + new Vector2(0, 4) - Main.screenPosition;
				Rectangle frame = new Rectangle(0, NPC.frame.Y, glowMask.Value.Width, glowMask.Value.Height / frames);

				SpriteEffects spriteDirection = SpriteEffects.None;
				if (State != (float)States.Dash)
				{
					spriteDirection = NPC.spriteDirection == 1 ? SpriteEffects.FlipHorizontally : SpriteEffects.None;
				}
				else if (State == (float)States.Dash)
				{
					spriteDirection = NPC.spriteDirection == -1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
				}
				spriteBatch.Draw(glowMask.Value, drawPosition, frame, Color.White, NPC.rotation, Vector2.Zero, 1f, spriteDirection, 0f);
			}

			HandleDarknessDrawing(spriteBatch);
		}

		private float strength = 0f; // Controls the amount of darkness during dash state
		private void HandleDarknessDrawing(SpriteBatch spriteBatch)
		{
			// DASH ATTACK PART
			float maxStrength = Main.IsItDay() ? 0.85f : 0.55f; // Limit the amount of darkness, during night 0.85f would be too dark.
			// TODO better check for day/night, should become 0.55f during dusk
			if (State == (float)States.Dash || strength > 0) // Dashing state or darkness has not yet fully lifted
			{
				// strenght is initially 0f, increase it to the full when the state begins
				if (strength < maxStrength && State == (float)States.Dash)
				{
					strength += 0.015f;
				}

				// Dashing state has ended, decrease darkness until normal
				if (State != (float)States.Dash)
				{
					strength -= 0.015f;
				}

				// Limit the strenght between 0 and maxStrenght
				if (strength < 0)
				{
					strength = 0;
				}

				if (strength > maxStrength)
				{
					strength = maxStrength;
				}

				// This is how the obstruction debuff obstructs the player's vision
				Color color = Color.Black * strength;
				int num = TextureAssets.Extra[49].Width();
				int num2 = 10;
				Rectangle rect = Main.player[Main.myPlayer].getRect();
				rect.Inflate((num - rect.Width) / 2, (num - rect.Height) / 2 + num2 / 2);
				rect.Offset(-(int)Main.screenPosition.X, -(int)Main.screenPosition.Y + (int)Main.player[Main.myPlayer].gfxOffY - num2);
				Rectangle destinationRectangle = Rectangle.Union(new Rectangle(0, 0, 1, 1), new Rectangle(rect.Right - 1, rect.Top - 1, 1, 1));
				Rectangle destinationRectangle2 = Rectangle.Union(new Rectangle(Main.screenWidth - 1, 0, 1, 1), new Rectangle(rect.Right, rect.Bottom - 1, 1, 1));
				Rectangle destinationRectangle3 = Rectangle.Union(new Rectangle(Main.screenWidth - 1, Main.screenHeight - 1, 1, 1), new Rectangle(rect.Left, rect.Bottom, 1, 1));
				Rectangle destinationRectangle4 = Rectangle.Union(new Rectangle(0, Main.screenHeight - 1, 1, 1), new Rectangle(rect.Left - 1, rect.Top, 1, 1));
				spriteBatch.Draw(TextureAssets.MagicPixel.Value, destinationRectangle, new Rectangle(0, 0, 1, 1), color);
				spriteBatch.Draw(TextureAssets.MagicPixel.Value, destinationRectangle2, new Rectangle(0, 0, 1, 1), color);
				spriteBatch.Draw(TextureAssets.MagicPixel.Value, destinationRectangle3, new Rectangle(0, 0, 1, 1), color);
				spriteBatch.Draw(TextureAssets.MagicPixel.Value, destinationRectangle4, new Rectangle(0, 0, 1, 1), color);
				spriteBatch.Draw(TextureAssets.Extra[49].Value, rect, color);
			}
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Graveyard,
                new FlavorTextBestiaryInfoElement("This being is a tormented and restless soul. It has haunted these lands for an eternity and awaits a challenger to face it in battle.")
			});
		}
	}
}
