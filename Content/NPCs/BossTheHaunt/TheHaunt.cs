using System.Collections.Generic;
using System.IO;
using AssortedAdditions.Common.Systems;
using AssortedAdditions.Content.Items.Consumables.TreasureBags;
using AssortedAdditions.Content.Items.Pets;
using AssortedAdditions.Content.Items.Placeables.Relics;
using AssortedAdditions.Content.Items.Placeables.Trophies;
using AssortedAdditions.Content.Items.Weapons.Magic;
using AssortedAdditions.Content.Items.Weapons.Melee;
using AssortedAdditions.Content.Items.Weapons.Ranged;
using AssortedAdditions.Content.Items.Weapons.Summon;
using AssortedAdditions.Content.Projectiles.NPCProj;
using AssortedAdditions.Helpers;
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

			NPC.damage = 50;
			NPC.defense = 28;
			NPC.lifeMax = 28500;
			NPC.knockBackResist = 0f;

			NPC.HitSound = SoundID.NPCHit7;
			NPC.DeathSound = new SoundStyle("AssortedAdditions/Assets/Sounds/NPCSound/HauntDeath");
			NPC.SpawnWithHigherTime(30);
			NPC.npcSlots = 6f;

			NPC.noTileCollide = true;
			NPC.noGravity = true;
			NPC.boss = true;

			NPC.aiStyle = -1;

			if (!Main.dedServ)
			{
				Music = MusicID.OtherworldlyPlantera;
			}
		}

		public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
		{
			// 40% increase in health per player in multiplayer
			// in singleplayer nothing happens
			float multiplier = numPlayers > 1 ? 0.4f : 0;
			NPC.lifeMax = (int)(NPC.lifeMax * (1 + multiplier * (numPlayers - 1)));
		}

		public override void BossLoot(ref string name, ref int potionType)
		{
			potionType = ItemID.GreaterHealingPotion; // Drop healing potions (default is lesser healing)
		}


		// All the different states the boss has
		private enum States
		{
			Chase, // Chase is the base state, haunt chases the player and shoots projectiles
			ThrowFurniture, // Haunt will slowdown and start throwing furniture at the player
			SummonGhosts, // The haunt will wander around the player and summon Hauntlings
			SummonProjectiles,
			
			Dash, // Haunt will stop and turn invisible, then teleport to a spot, turn back visible and dash at the player, repeat
			Flee, // Haunt will vanish, all players are dead
		}

		public ref float State => ref NPC.ai[0]; // The Current active state of the Haunt
		public ref float PreviousState => ref NPC.ai[1]; // The previous active state (other than Chase since its the default)
		public ref float Timer => ref NPC.ai[2];
		public ref float SecondaryTimer => ref NPC.ai[3];


		public override void AI()
		{
			// Targetting
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
			{
				NPC.TargetClosest();
			}
			Player target = Main.player[NPC.target];
			float distanceFromTarget = Vector2.Distance(target.Center, NPC.Center);

			// Check if all the players are dead (targetClosest didnt return alive player)
			if(target.dead || !target.active)
			{
				State = (float)States.Flee;
			}

			// Keep running the timer, each state resets this
			Timer++; 

			// ** STATE BEHAVIOUR **
			// Each state works independently at least mostly (Dash has a transition with the alpha value)
			if (State == (float)States.Flee)
			{
				NPC.alpha++;

				if (NPC.alpha >= 255)
				{
					NPC.active = false;
				}

				return;
			}

			if (State == (float)States.Chase)
			{
				ChaseState(target, distanceFromTarget);
				BasicMovement(target, distanceFromTarget, 8.5f, 30f);
			}

			if (State == (float)States.Dash)
			{
				DashState(target);
			}
			
			if(State == (float)States.ThrowFurniture)
			{
				ThrowFurniture(target);
				BasicMovement(target, distanceFromTarget, 2f);
			}

			if (State == (float)States.SummonGhosts)
			{
				SummonGhosts(target);
				//SummonGhostsMovement(target, distanceFromTarget, 3f);
				RandomMovementAroundPlayer(target, 7f);
			}

			if(State == (float)States.SummonProjectiles)
			{
				SummonProjectiles(target);
				RandomMovementAroundPlayer(target, 5.5f);
			}

			// Choose next state (if in the default chase state)
			// Each state runs SecondaryTimer and once the state ends it resets it along with the Timer
			// So the haunt returns to Chase state and can then choose the next state...
			if (Timer >= 800 && State == (float)States.Chase)
			{
				// Prevent choosing the same state twice in a row
				int choice = Main.rand.Next(1, 5);
				if(choice == PreviousState)
				{
					if(choice != 4)
					{
						choice++;
					}
					else if(choice != 1)
					{
						choice--;
					}	
				}

				switch (choice)
				{
					case 1:
						State = (float)States.ThrowFurniture;
						PreviousState = (float)States.ThrowFurniture;
						break;
					
					case 2:
						State = (float)States.SummonGhosts;
						PreviousState = (float)States.SummonGhosts;
						break;

					case 3:
						State = (float)States.SummonProjectiles;
						PreviousState = (float)States.SummonProjectiles;
						break;

					case 4:
						State = (float)States.Dash;
						PreviousState = (float)States.Dash;
						break;
				}

				SecondaryTimer = 0; // Safety reset incase some state did not reset this properly (Chase state)
				NPC.netUpdate = true;

				SoundStyle stateChange = new SoundStyle("AssortedAdditions/Assets/Sounds/NPCSound/HauntStateChange");
				SoundEngine.PlaySound(stateChange with { Pitch = -0.7f, Volume = 1.3f }, NPC.position);
			}

			// Dash state messes with the alpha, return the haunt to normal alpha
			// when dash has ended
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

		private void BasicMovement(Player target, float distanceFromTarget, float speed = 6f, float inertia = 17f)
		{
			if (distanceFromTarget > 80f) // The immediate range around the target (so it doesn't latch onto it when close)
			{
				// Set the velocity, and sprite direction
				Vector2 direction = target.Center - NPC.Center;
				direction.Normalize();
				direction *= speed;

				NPC.velocity = (NPC.velocity * (inertia - 1) + direction) / inertia;
				NPC.spriteDirection = target.Center.X < NPC.Center.X ? -1 : 1;
			}
		}

		private void ChaseState(Player target, float distanceFromTarget)
		{

			if(SecondaryTimer == 0)
			{
				SoundStyle chaseStart = new SoundStyle("AssortedAdditions/Assets/Sounds/NPCSound/HauntChaseStart");
				SoundEngine.PlaySound(chaseStart with { Pitch = -0.3f }, NPC.position);
			}
			SecondaryTimer++;

			// During the chase state, the haunt will attempt to spawn three projectiles
			// if the player is far enough away (max every 3 seconds). 
			if (distanceFromTarget > 350f && SecondaryTimer % 180 == 0 && SecondaryTimer != 0)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					// Basic straight line velocity
					Vector2 direction = target.Center - NPC.Center;
					direction.Normalize();
					direction *= 4f;

					
					float numberProjectiles = 6; // Spawn up to X
					float rotation = MathHelper.ToRadians(180); // rotation for start velocity

					// Spawn the projectiles
					for (int i = 0; i < numberProjectiles; i++)
					{
						Vector2 perturbedSpeed = direction.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1)));
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + NPC.velocity, perturbedSpeed, ModContent.ProjectileType<TheHauntGhost>(), 25, 5f, Main.myPlayer);
					}
				}
				SoundEngine.PlaySound(SoundID.Item8 with { Pitch = 0.5f }, NPC.position);
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

		private void ThrowFurniture(Player target)
		{
			if (SecondaryTimer % 60 == 0)
			{
				float angle = 0;
				switch (SecondaryTimer)
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
					Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, Vector2.Zero, type, 50, 5f, Main.myPlayer, 0, target.whoAmI, NPC.whoAmI);
				}

				// Sound effect when the furniture projectile spawns
				SoundStyle furnitureSpawn = new SoundStyle("AssortedAdditions/Assets/Sounds/NPCSound/HauntFurnitureSpawn");
				furnitureSpawn = furnitureSpawn with { Pitch = 1f, PitchVariance = 0.1f, Volume = 1.5f };
				SoundEngine.PlaySound(furnitureSpawn, NPC.position);
			}

			if (SecondaryTimer >= 300)
			{
				Timer = 0;
				SecondaryTimer = 0;
				State = (float)States.Chase;
				NPC.netUpdate = true;
				return;
			}

			SecondaryTimer++;
		}

		private float randomRotation = 0;
		private int dashTimer = 0;
		private bool dashStateJustStarted = false;
		private bool dashStateJustEnded = false;
		private void DashState(Player target)
		{
			if(SecondaryTimer == 0)
			{
				dashStateJustStarted = true;
			}

			// When Dash State begins, slow the haunt down and make it turn invisible
			if (dashStateJustStarted)
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
			if (dashStateJustStarted && NPC.alpha < 255)
			{
				return;
			}

			// Dash state ends
			if (SecondaryTimer >= 900)
			{
				dashStateJustEnded = true;
				Timer = 0;
				SecondaryTimer = 0;
				State = (float)States.Chase;
				NPC.velocity = Vector2.Zero;
				NPC.netUpdate = true;
				
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
			SecondaryTimer++;

			if (dashTimer == 30)
			{
				Vector2 dashDirection = target.Center - NPC.Center;
				NPC.velocity = dashDirection.SafeNormalize(Vector2.Zero) * 12f;
				SoundStyle dashSound = new SoundStyle("AssortedAdditions/Assets/Sounds/NPCSound/HauntDash");
				dashSound = dashSound with { Pitch = 0f, PitchVariance = 0.1f };
				SoundEngine.PlaySound(dashSound, NPC.position);
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

		private const int maxHauntlings = 5; // A maximum of 5 can be active at the same time
		private void SummonGhosts(Player target)
		{
			int type = ModContent.NPCType<Hauntling>();
			if (SecondaryTimer % 100 == 0 && HelperMethods.CountNPCs(type) < maxHauntlings && SecondaryTimer < 800)
			{
				randomRotation = Main.rand.NextFloat(0, 6.2f);
				Vector2 spawnPos = new Vector2(target.Center.X + 1000, target.Center.Y).RotatedBy(randomRotation, target.Center);
				int xPos = (int)spawnPos.X;
				int yPos = (int)spawnPos.Y; // int since the sync message requires these as int (im too lazy to switch to floats instead)

				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					// Spawn the hauntling at the position when in singleplayer
					NPC.NewNPCDirect(NPC.GetSource_FromAI(), xPos, yPos, type, NPC.whoAmI);
				}
				else
				{ // otherwise server handles it
					var message = Mod.GetPacket();
					message.Write((byte)AssortedAdditions.MessageType.SpawnGenericNPC);
					message.Write(xPos);
					message.Write(yPos);
					message.Write(type);
					message.Send();
				}
			}

			if(SecondaryTimer % 120 == 0)
			{
				if(Main.netMode != NetmodeID.MultiplayerClient)
				{
					Vector2 spawnPos = new Vector2(target.Center.X + 1000, target.Center.Y).RotatedBy(Main.rand.NextFloat(0, 6.2f), target.Center);
					Vector2 direction = target.Center - spawnPos;
					direction.Normalize();
					Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, direction * 2f, ModContent.ProjectileType<TheHauntGhost>(), 30, 5f, Main.myPlayer);
				}
			}

			SecondaryTimer++;

			// Stop this state after 15 seconds
			if (SecondaryTimer >= 900)
			{
				State = (float)States.Chase;
				Timer = 0;
				SecondaryTimer = 0;
				NPC.netUpdate = true;
			}
		}

		private void SummonProjectiles(Player target)
		{
			// Summon projectiles from around the player:
			if (SecondaryTimer % 45 == 0)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					int xOffSet = Main.rand.Next(-600, 600);
					Vector2 spawnPos = new Vector2(target.Center.X + xOffSet, target.Center.Y + 800);
					Vector2 direction = new Vector2(target.Center.X + xOffSet, target.Center.Y) - spawnPos;
					direction.Normalize();
					Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPos, direction * 5f, ModContent.ProjectileType<TheHauntProj>(), 30, 0f, Main.myPlayer);
				}
			}

			SecondaryTimer++;

			if (SecondaryTimer % 240 == 0)
			{
				if (Main.netMode != NetmodeID.MultiplayerClient)
				{
					// Basic straight line velocity
					Vector2 direction = target.Center - NPC.Center;
					direction.Normalize();
					direction *= 3f;

					// Spawn up to three, rotate each ones basic velocity
					float numberProjectiles = 3;
					float rotation = MathHelper.ToRadians(75);

					// Spawn the projectiles
					for (int i = 0; i < numberProjectiles; i++)
					{
						Vector2 perturbedSpeed = direction.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberProjectiles - 1)));
						Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center + NPC.velocity, perturbedSpeed, ModContent.ProjectileType<TheHauntGhost>(), 30, 6f, Main.myPlayer);
					}
					SoundEngine.PlaySound(SoundID.Item8 with { Pitch = 0.5f }, NPC.position);
				}
			}

			if (SecondaryTimer > 1920)
			{
				SecondaryTimer = 0;
				Timer = 0;
				State = (float)States.Chase;
				NPC.netUpdate = true;
			}
		}

		private float randomXPos;
		private float randomYPos;
		private void RandomMovementAroundPlayer(Player target, float speed = 6f, float inertia = 17f)
		{
			if (SecondaryTimer % 120 == 0)
			{
				randomXPos = Main.rand.Next(-600, 600);
				randomYPos = Main.rand.Next(-500, 500);
				NPC.netUpdate = true; // ^^ Values need to be synced
			}

			// Basically moves around randomly within the ranges given above
			Vector2 direction = new Vector2(target.Center.X + randomXPos, target.Center.Y + randomYPos) - NPC.Center;
			direction.Normalize();
			direction *= speed;

			NPC.velocity = (NPC.velocity * (inertia - 1) + direction) / inertia;
			NPC.spriteDirection = target.Center.X < NPC.Center.X ? -1 : 1;
		}

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(randomRotation);
			writer.Write(randomXPos);
			writer.Write(randomYPos);
		}

		public override void ReceiveExtraAI(BinaryReader reader)
		{
			randomRotation = reader.ReadSingle();
			randomXPos = reader.ReadSingle();
			randomYPos = reader.ReadSingle();
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot)
		{
			cooldownSlot = ImmunityCooldownID.Bosses; // use the boss immunity cooldown counter, to prevent ignoring boss attacks by taking damage from other sources
			return true;
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
			{
				for(int i = 0; i < 5; i++)
				{
					Gore.NewGore(NPC.GetSource_Death(), NPC.Center, Vector2.Zero, 11, Main.rand.NextFloat(1f, 2f));
					Gore.NewGore(NPC.GetSource_Death(), NPC.Center, Vector2.Zero, 12, Main.rand.NextFloat(1f, 2f));
					Gore.NewGore(NPC.GetSource_Death(), NPC.Center, Vector2.Zero, 13, Main.rand.NextFloat(1f, 2f));
				}
			}
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
		{
			npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<HauntBag>()));
			npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<HauntTrophy>(), 10)); // 10% chance for trophy
			npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<HauntRelic>())); // Master mode relic
			npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<CursedCandle>(), 4));

			// non expert version
			LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert());
			notExpert.OnSuccess(ItemDropRule.OneFromOptions(1,
				ModContent.ItemType<PhantasmicBow>(),
				ModContent.ItemType<PhantasmicBlade>(),
				ModContent.ItemType<PhantasmicGauntlet>(),
				ModContent.ItemType<PhantasmicDagger>()));
			npcLoot.Add(notExpert);
		}

		public override void OnKill()
		{
			NPC.SetEventFlagCleared(ref DownedBossSystem.downedTheHaunt, -1);
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
