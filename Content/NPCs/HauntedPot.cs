using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using Terraria.GameContent.Bestiary;
using Terraria.Audio;
using AssortedAdditions.Common.Configs;
using Terraria.ModLoader.Utilities;

namespace AssortedAdditions.Content.NPCs
{
	internal class HauntedPot : ModNPC
	{
		public override void SetStaticDefaults()
		{
			Main.npcFrameCount[Type] = 5;

			NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers() { Velocity = 1, Frame = 3, SpriteDirection = 1 };
			NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);

			NPCID.Sets.ImmuneToRegularBuffs[Type] = true;
		}

		public override void SetDefaults()
		{
			NPC.width = 32;
			NPC.height = 42;
			NPC.damage = 15;
			NPC.defense = 2;
			NPC.lifeMax = 55;
			NPC.value = 80;
			NPC.aiStyle = 0;

			NPC.HitSound = NPC.HitSound = new SoundStyle("AssortedAdditions/Assets/Sounds/NPCSound/PhantomMageHit");
			NPC.DeathSound = SoundID.Shatter;
		}

		private enum State
		{
			Idle,
			Chase
		}

		private ref float CurrentState => ref NPC.ai[0];
		private ref float PeekTimer => ref NPC.ai[1]; // Used in the animation to make the npc peek out of the pot
		public override void AI()
		{
			PeekTimer++;

			// Finding target
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
			{
				NPC.TargetClosest();
			}
			Player target = Main.player[NPC.target];
			float distanceFromTarget = Vector2.Distance(target.Center, NPC.Center);

			// If the player is dead, flee
			if (target.dead || !target.active)
			{
				Vector2 direction = Main.screenPosition - NPC.position;
				direction.Normalize();
				direction *= speed;
				NPC.velocity = (NPC.velocity * (inertia - 1) + direction) / inertia;
				NPC.spriteDirection = NPC.direction;

				NPC.EncourageDespawn(10);
			}

			if (CurrentState == (float)State.Idle)
			{
				NPC.noTileCollide = false;
				NPC.noGravity = false;
			}

			if (CurrentState == (float)State.Chase)
			{
				NPC.noTileCollide = true;
				NPC.noGravity = true;
				Movement(target, distanceFromTarget);
			}

			if (distanceFromTarget < 400)
			{
				CurrentState = (float)State.Chase;
			}

			NPC.rotation = NPC.velocity.X * 0.06f; // lean when moving
		}

		private float speed = 11f;
		private float inertia = 45f;
		private void Movement(Player target, float distanceFromTarget)
		{
			// Attack, fly towards the player
			if (distanceFromTarget > 40f)
			{
				// The immediate range around the target (so it doesn't latch onto it when close)
				Vector2 direction = target.Center - NPC.Center;
				direction.Normalize();
				direction *= speed;

				NPC.velocity = (NPC.velocity * (inertia - 1) + direction) / inertia;
			}

			// Used for preventing multiple fire drakes from overlapping
			float overlapVelocity = 0.04f;

			// Fix overlap with other NPCs
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC other = Main.npc[i];

				if (i != NPC.whoAmI && other.active && Math.Abs(NPC.position.X - other.position.X) + Math.Abs(NPC.position.Y - other.position.Y) < NPC.width)
				{
					if (NPC.position.X < other.position.X)
					{
						NPC.velocity.X -= overlapVelocity;
					}
					else
					{
						NPC.velocity.X += overlapVelocity;
					}

					if (NPC.position.Y < other.position.Y)
					{
						NPC.velocity.Y -= overlapVelocity;
					}
					else
					{
						NPC.velocity.Y += overlapVelocity;
					}
				}
			}
		}

		private int frame = 0;
		private bool peekReverse = false;
		private const int peekStartTime = 600;
		public override void FindFrame(int frameHeight)
		{

			if (CurrentState == (float)State.Idle)
			{
				NPC.frameCounter++;
				// The npc will peek out of the pot every now and then
				if (PeekTimer > peekStartTime && NPC.frameCounter % 12 == 0)
				{
					// First the npc peeks its head out of the pot, then it goes back in
					if (peekReverse)
					{
						frame--;
					}
					else
					{
						frame++;
					}

					if (peekReverse && frame < 0)
					{
						// Done peeking, back inside the pot
						peekReverse = false;
						PeekTimer = 0;
						frame = 0;
					}

					if (frame > 3 && !peekReverse)
					{
						// Fully peeked out
						peekReverse = true;
						frame = 3;
					}
				}
				else
				{
					// Not peeking, stays inside the pot
					if (PeekTimer <= peekStartTime)
					{
						frame = 0;
					}
				}
			}

			if (CurrentState == (float)State.Chase)
			{
				NPC.frameCounter++;
				// Animation has reached last two frames, keep switching between them
				if (NPC.frameCounter % 15 == 0)
				{
					frame++;

					if (frame > 4)
					{
						frame = 3;
					}
				}

				// NPC hasnt fully came out of the pot yet, animate until frame 3.
				if (frame < 3 && NPC.frameCounter % 12 == 0)
				{
					frame++;
				}
			}

			NPC.frame.Y = frameHeight * frame;
			NPC.spriteDirection = NPC.direction;
		}

		public override void OnHitByItem(Player player, Item item, NPC.HitInfo hit, int damageDone)
		{
			CurrentState = (float)State.Chase;
		}

		public override void OnHitByProjectile(Projectile projectile, NPC.HitInfo hit, int damageDone)
		{
			CurrentState = (float)State.Chase;
		}

		public override void HitEffect(NPC.HitInfo hit)
		{
			// Create gore when the NPC is killed.
			if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
			{
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, Vector2.Zero, 51, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, Vector2.Zero, 52, 1f);
				Gore.NewGore(NPC.GetSource_Death(), NPC.position, Vector2.Zero, 53, 1f);

				for (int i = 0; i < 25; i++)
				{
					Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Cloud, 0.5f, 1f, 85, default, 1.15f);
					dust.noGravity = true;
					dust.velocity.Y *= 1.3f;
				}

				SoundEngine.PlaySound(SoundID.NPCDeath6, NPC.position);
			}
		}

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			float multiplier = ServerSidedToggles.Instance.NPCSpawnMultiplier;

			if(spawnInfo.Player.ZoneRockLayerHeight)
			{
				return SpawnCondition.Cavern.Chance * 0.15f * multiplier;
			}
			else if(spawnInfo.Player.ZoneDirtLayerHeight)
			{
				return SpawnCondition.Underground.Chance * 0.4f * multiplier;
			}

			return 0;
		}

		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
		{
			// We can use AddRange instead of calling Add multiple times in order to add multiple items at once
			bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				
                // Sets the spawning conditions of this NPC that is listed in the bestiary.
				// Last one will be displayed as the bestiary background
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Underground,
				
                // Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("These small ghosts take refuge inside pots. They like to be left alone and get angry if distrurbed. " +
				"When left alone the ghost will stay inside its pot, occasionally peeking out.")
			});
		}
	}
}
