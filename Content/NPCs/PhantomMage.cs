using AssortedAdditions.Common.Configs;
using AssortedAdditions.Content.Items.Consumables;
using AssortedAdditions.Content.Items.Placeables.Banners;
using AssortedAdditions.Content.Projectiles.NPCProj;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.NPCs
{
	internal class PhantomMage : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 6;

			NPCID.Sets.TrailCacheLength[NPC.type] = 6;
			NPCID.Sets.TrailingMode[NPC.type] = 3;

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() { Velocity = 1, SpriteDirection = 1 };
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
			
			NPCID.Sets.ImmuneToRegularBuffs[Type] = true;
		}

		public override void SetDefaults()
		{
			NPC.width = 48;
			NPC.height = 66;
			NPC.damage = 35;
			NPC.defense = 5;
			NPC.lifeMax = 150;
			NPC.alpha = 85;
			NPC.knockBackResist = 0.7f;
			NPC.HitSound = new SoundStyle("AssortedAdditions/Assets/Sounds/NPCSound/PhantomMageHit");
			NPC.DeathSound = SoundID.NPCDeath6;
			NPC.value = 1000;
			NPC.aiStyle = -1;

			NPC.noGravity = true;
			NPC.noTileCollide = true;

			Banner = NPC.type;
			BannerItem = ModContent.ItemType<PhantomMageBanner>();
		}

		// ai[0], ai[1], ai[2] used by movement code
		// which is taken from vanilla (not sure how to name them better)
		private ref float ShotTimer => ref NPC.ai[3];
		private ref float SoundTimer => ref NPC.localAI[0];
		public override void AI()
		{
			Movement();

			ShotTimer++;
			Player target = Main.player[NPC.target];

			// Fire a projectile every 3 to 5 seconds
			if (ShotTimer >= Main.rand.Next(180, 301))
			{
				Vector2 direction = target.Center - NPC.Center;
				direction.Normalize();
				int type = ModContent.ProjectileType<PhantomMageProj>();

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, direction * 6f, type, 40, 4f, Main.myPlayer);
				}
				SoundEngine.PlaySound(new SoundStyle("AssortedAdditions/Assets/Sounds/NPCSound/PhantomMageShoot"), NPC.position);

				ShotTimer = 0; // Reset after each shot
			}

			if (Main.rand.NextBool(10))
			{
				Dust dust = Dust.NewDustDirect(NPC.position - NPC.velocity, NPC.width, NPC.height, DustID.SteampunkSteam, 1, 1, 85, Color.Gray, 1.25f);
				dust.velocity *= 1.2f;
				dust.noGravity = true;
			}

			if (SoundTimer % 300 == 0)
			{
				SoundEngine.PlaySound(new SoundStyle("AssortedAdditions/Assets/Sounds/NPCSound/PhantomMageSound"), NPC.position);
			}
			SoundTimer++;
		}

		// This is taken from vanilla aiStyle 22, specifically parts relating to the ghost enemy (NPCID 316)
		private void Movement()
		{
			if (NPC.justHit)
			{
				NPC.ai[2] = 0f;
			}

			bool flag20 = false;
			bool flag21 = false;

			if (Main.player[NPC.target].dead || Vector2.Distance(NPC.Center, Main.player[NPC.target].Center) > 3000f)
			{
				NPC.TargetClosest();
				if (Main.player[NPC.target].dead || Vector2.Distance(NPC.Center, Main.player[NPC.target].Center) > 3000f)
				{
					NPC.EncourageDespawn(10);
					flag20 = true;
					flag21 = true;
				}
			}

			if (flag21)
			{
				if (NPC.velocity.X == 0f)
				{
					NPC.velocity.X = Main.rand.Next(-1, 2) * 1.5f;
					NPC.netUpdate = true;
				}
			}
			else if (NPC.ai[2] >= 0f)
			{
				int num827 = 16;
				bool flag22 = false;
				bool flag23 = false;
				if (NPC.position.X > NPC.ai[0] - num827 && NPC.position.X < NPC.ai[0] + num827)
				{
					flag22 = true;
				}
				else if ((NPC.velocity.X < 0f && NPC.direction > 0) || (NPC.velocity.X > 0f && NPC.direction < 0))
				{
					flag22 = true;
				}
				num827 += 24;
				if (NPC.position.Y > NPC.ai[1] - num827 && NPC.position.Y < NPC.ai[1] + num827)
				{
					flag23 = true;
				}
				if (flag22 && flag23)
				{
					NPC.ai[2] += 1f;
					if (NPC.ai[2] >= 30f && num827 == 16)
					{
						flag20 = true;
					}
					if (NPC.ai[2] >= 60f)
					{
						NPC.ai[2] = -200f;
						NPC.direction *= -1;
						NPC.velocity.X *= -1f;
						NPC.collideX = false;
					}
				}
				else
				{
					NPC.ai[0] = NPC.position.X;
					NPC.ai[1] = NPC.position.Y;
					NPC.ai[2] = 0f;
				}
				NPC.TargetClosest();
			}
			else
			{
				NPC.ai[2] += 1f;
				if (Main.player[NPC.target].position.X + (Main.player[NPC.target].width / 2) > NPC.position.X + (NPC.width / 2))
				{
					NPC.direction = -1;
				}
				else
				{
					NPC.direction = 1;
				}
			}

			int xTilePosition = (int)((NPC.position.X + (NPC.width / 2)) / 16f) + NPC.direction * 2;
			int yTilePosition = (int)((NPC.position.Y + NPC.height) / 16f);
			bool flag25 = true;
			int maxTileGap = 8; // How far the npc can move up without being inside a tile

			if (NPC.position.Y + NPC.height > Main.player[NPC.target].position.Y)
			{
				for (int yTileCheck = yTilePosition; yTileCheck < yTilePosition + maxTileGap; yTileCheck++)
				{
					if ((Main.tile[xTilePosition, yTileCheck].HasUnactuatedTile && Main.tileSolid[Main.tile[xTilePosition, yTileCheck].TileType]) || Main.tile[xTilePosition, yTileCheck].LiquidAmount > 0)
					{
						flag25 = false;
						break;
					}
				}
			}

			if (Main.player[NPC.target].npcTypeNoAggro[NPC.type])
			{
				bool validTile = false;
				for (int yTileCheck = yTilePosition; yTileCheck < yTilePosition + maxTileGap - 2; yTileCheck++)
				{
					if ((Main.tile[xTilePosition, yTileCheck].HasUnactuatedTile && Main.tileSolid[Main.tile[xTilePosition, yTileCheck].TileType]) || Main.tile[xTilePosition, yTileCheck].LiquidAmount > 0)
					{
						validTile = true;
						break;
					}
				}
				NPC.directionY = (!validTile).ToDirectionInt();
			}

			if (flag20)
			{
				flag25 = true;
			}

			if (flag25)
			{
				NPC.velocity.Y += 0.1f;
				if (flag21)
				{
					NPC.velocity.Y -= 0.05f;
					if (NPC.velocity.Y > 6f)
					{
						NPC.velocity.Y = 6f;
					}
				}
				else if (NPC.velocity.Y > 3f)
				{
					NPC.velocity.Y = 3f;
				}
			}
			else
			{
				if (NPC.directionY < 0 && NPC.velocity.Y > 0f)
				{
					NPC.velocity.Y -= 0.1f;
				}
				if (NPC.velocity.Y < -4f)
				{
					NPC.velocity.Y = -4f;
				}
			}

			if (NPC.collideX)
			{
				NPC.velocity.X = NPC.oldVelocity.X * -0.4f;
				if (NPC.direction == -1 && NPC.velocity.X > 0f && NPC.velocity.X < 1f)
				{
					NPC.velocity.X = 1f;
				}
				if (NPC.direction == 1 && NPC.velocity.X < 0f && NPC.velocity.X > -1f)
				{
					NPC.velocity.X = -1f;
				}
			}
			if (NPC.collideY)
			{
				NPC.velocity.Y = NPC.oldVelocity.Y * -0.25f;
				if (NPC.velocity.Y > 0f && NPC.velocity.Y < 1f)
				{
					NPC.velocity.Y = 1f;
				}
				if (NPC.velocity.Y < 0f && NPC.velocity.Y > -1f)
				{
					NPC.velocity.Y = -1f;
				}
			}

			float xFriction = 2f;
			if (NPC.direction == -1 && NPC.velocity.X > 0f - xFriction)
			{
				NPC.velocity.X -= 0.1f;
				if (NPC.velocity.X > xFriction)
				{
					NPC.velocity.X -= 0.1f;
				}
				else if (NPC.velocity.X > 0f)
				{
					NPC.velocity.X += 0.05f;
				}
				if (NPC.velocity.X < 0f - xFriction)
				{
					NPC.velocity.X = 0f - xFriction;
				}
			}
			else if (NPC.direction == 1 && NPC.velocity.X < xFriction)
			{
				NPC.velocity.X += 0.1f;
				if (NPC.velocity.X < 0f - xFriction)
				{
					NPC.velocity.X += 0.1f;
				}
				else if (NPC.velocity.X < 0f)
				{
					NPC.velocity.X -= 0.05f;
				}
				if (NPC.velocity.X > xFriction)
				{
					NPC.velocity.X = xFriction;
				}
			}

			float yFriction = 1.5f;
			if (NPC.directionY == -1 && NPC.velocity.Y > 0f - yFriction)
			{
				NPC.velocity.Y -= 0.04f;
				if (NPC.velocity.Y > yFriction)
				{
					NPC.velocity.Y -= 0.05f;
				}
				else if (NPC.velocity.Y > 0f)
				{
					NPC.velocity.Y += 0.03f;
				}
				if (NPC.velocity.Y < 0f - yFriction)
				{
					NPC.velocity.Y = 0f - yFriction;
				}
			}
			else if (NPC.directionY == 1 && NPC.velocity.Y < yFriction)
			{
				NPC.velocity.Y += 0.04f;
				if (NPC.velocity.Y < 0f - yFriction)
				{
					NPC.velocity.Y += 0.05f;
				}
				else if (NPC.velocity.Y < 0f)
				{
					NPC.velocity.Y -= 0.03f;
				}
				if (NPC.velocity.Y > yFriction)
				{
					NPC.velocity.Y = yFriction;
				}
			}
		}

		private int currentFrame = 0;
		public override void FindFrame(int frameHeight)
		{
			// Switch frames
			if (NPC.frameCounter % 30 == 0)
			{
				currentFrame++;
			}
			NPC.frameCounter++;

			// If final frame was reached, reset to frame 1 (0 is only for idle)
			if (currentFrame > 5)
			{
				currentFrame = 0;
			}

			// Set the frame whether its the idle one or one of the walking frames
			NPC.frame.Y = frameHeight * currentFrame;
			NPC.spriteDirection = -NPC.direction;
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (NPC.life <= 0)
			{
				for (int i = 0; i < 30; i++)
				{
					Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.SteampunkSteam, 1, 1, 60, Color.Gray, 1.25f);
					dust.noGravity = true;
					dust.velocity *= 1.25f;
				}
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GraveFlowers>(), 20));
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			float multiplier = ServerSidedToggles.Instance.NPCSpawnMultiplier;
			
			if (Main.hardMode && spawnInfo.Player.ZoneGraveyard)
			{
				return 0.09f * multiplier;
			}
			else if(Main.hardMode && (spawnInfo.Player.ZoneMarble || spawnInfo.Marble))
			{
				return 0.05f * multiplier;
			}

			return 0f;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				
                // Sets the spawning conditions of this NPC that is listed in the bestiary.
				// Last one will be displayed as the bestiary background
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Marble,
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Graveyard,
				
                // Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("An angry spirit of a once great mage left to haunt those who tread these lands.")
			});
		}

		// Trail drawing
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

				SpriteEffects spriteDirection = NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
				Main.EntitySpriteDraw(texture.Value, drawPos, frame, color, NPC.rotation, drawOrigin, NPC.scale, spriteDirection, 0);
			}

			return true;
		}

		// Glowmask drawing
		public override void PostDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
		{
			Asset<Texture2D> glowMask = ModContent.Request<Texture2D>("AssortedAdditions/Content/NPCs/PhantomMage_Glow");
			Vector2 drawPosition = NPC.position + new Vector2(0, 4) - Main.screenPosition;
			Rectangle frame = new Rectangle(0, NPC.frame.Y, glowMask.Value.Width, glowMask.Value.Height / 6); // Frame(s) of the sprite, divided by 6 here since there are six frames

			SpriteEffects spriteDirection = NPC.direction == 1 ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
			spriteBatch.Draw(glowMask.Value, drawPosition, frame, Color.White * 0.75f, NPC.rotation, Vector2.Zero, 1f, spriteDirection, 0f);
		}
	}
}

