﻿using System.Collections.Generic;
using System.IO;
using AssortedAdditions.Common.Systems;
using AssortedAdditions.Content.Items.Consumables.TreasureBags;
using AssortedAdditions.Content.Items.Misc;
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
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.NPCs.BossFireDragon // This Boss NPC is built using FireDragonBuilder.cs
{
    [AutoloadBossHead]
    internal class FireDragonHead : WormHead
    {
        public override int BodyType => ModContent.NPCType<FireDragonBody>();
        public override int TailType => ModContent.NPCType<FireDragonTail>();

        public override void SetStaticDefaults()
        {
            NPCID.Sets.MPAllowedEnemies[Type] = true; // Summoned using an item
            NPCID.Sets.BossBestiaryPriority.Add(Type);
            NPCID.Sets.CantTakeLunchMoney[Type] = true;

            // Immune to fire debuffs
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.CursedInferno] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.ShadowFlame] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Burning] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;

            // Beastiary stuff
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                CustomTexturePath = "AssortedAdditions/Content/NPCs/BossFireDragon/FireDragon_Bestiary",
                PortraitScale = 1.5f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.width = 48;
            NPC.height = 80;
            NPC.value = 100000;

            NPC.damage = 50;
            NPC.defense = 18;
            NPC.lifeMax = 28500;
            NPC.knockBackResist = 0f;

            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.DD2_BetsyDeath;
            NPC.SpawnWithHigherTime(30);
            NPC.npcSlots = 10f; // Take up open spawn slots, preventing random NPCs from spawning during the fight
           
            NPC.noTileCollide = true;
            NPC.noGravity = true;
			NPC.boss = true;

			NPC.aiStyle = -1;

            if (!Main.dedServ)
            {
                Music = MusicID.OtherworldlyWoF;
            }
        }

        public override void ApplyDifficultyAndPlayerScaling(int numPlayers, float balance, float bossAdjustment)
        {
            // 50% increase in health per player in multiplayer
            // in singleplayer nothing happens
            float multiplier = numPlayers > 1 ? 0.5f : 0;
			NPC.lifeMax = (int)(NPC.lifeMax * (1 + multiplier * (numPlayers - 1)));
		}

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld, // sets the background image
                new FlavorTextBestiaryInfoElement("It lays dormant in the depths of the underworld until it is challenged. It commands an army of Fire Drakes. Tip: kill of its servants whenever they spawn...")
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<FireDragonBag>())); // Treasure bag
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FireDragonTrophy>(), 10)); // 10% chance for trophy
            npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<FireDragonRelic>())); // Master mode relic
            npcLoot.Add(ItemDropRule.MasterModeDropOnAllPlayers(ModContent.ItemType<FireDragonEgg>(), 4)); // 25% chance to drop a pet on master mode

            // If the player is not playing one expert/master, treasure bag loot drops by itself
            LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert());

            // Drops 1 of these:
            notExpert.OnSuccess(ItemDropRule.OneFromOptions(1,
                ModContent.ItemType<DraconicBlade>(),
                ModContent.ItemType<DraconicTome>(),
                ModContent.ItemType<DragonStaff>(),
                ModContent.ItemType<DraconicBow>()));
            npcLoot.Add(notExpert); // Register the loot

            // Same goes for Dragon scales
            LeadingConditionRule notExpert2 = new LeadingConditionRule(new Conditions.NotExpert());
            notExpert2.OnSuccess(ItemDropRule.Common(ModContent.ItemType<DragonScale>(), 1, 30, 50));
            npcLoot.Add(notExpert2);
        }

		public override void OnKill()
		{
            NPC.SetEventFlagCleared(ref DownedBossSystem.downedFireDragon, -1);
		}

		public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.HealingPotion; // Drop healing potions (default is lesser healing)
        }

        public override void Init()
        {
            // How long the Dragon will be
            MinSegmentLength = 60;
            MaxSegmentLength = 60;
            CommonWormInit(this); // Segments spawn in FireDragonBuilder.cs
        }

        // These control the speed/acceleration inside FireDragonBuilder.cs
        internal static void CommonWormInit(FireDragonBuilder worm)
        {
            worm.MoveSpeed = 6.5f;
        }

        private enum State
        {
            Chasing,
            Fleeing,
            Circling  
        }

        private enum SecondaryState
        {
            None,
            SpawnMinions,
            SpawnProjectiles
        }

        public ref float Timer => ref NPC.ai[0]; // Timer used for timing boss states
		public ref float SecondaryTimer => ref NPC.ai[3]; // Used for timing things inside states
		public ref float CurrentState => ref NPC.ai[1];
        public ref float CurrentSecondaryState => ref NPC.ai[2];
        
        bool PlayersAreDead = false; // Used for despawning the boss
        int npcCount = 0; // Used for limiting spawned minion enemies, synced using extra ai

        // Since the Head controls the body and the tail, all AI stuff happens here
        public override void AI()
        {
            GetTargetPlayerOrFlee(); // Attempt to find a player target, if no players are alive state is set to fleeing
            if (CurrentState == (float)State.Fleeing)
            {
                return;
            }
            Timer++;
            NPC.noGravity = true;

            // If the dragon is in chasing state, it can spawn minions every X seconds.
            if (CurrentState == (float)State.Chasing && Timer % HelperMethods.SecondsToTicks(20) == 0)
            {
				SoundEngine.PlaySound(new SoundStyle("AssortedAdditions/Assets/Sounds/NPCSound/FireDragonSounds"), NPC.position);
				CurrentSecondaryState = (float)SecondaryState.SpawnMinions;
            }

            // After X seconds, switch to circling state
            if (Timer == HelperMethods.SecondsToTicks(40))
            {
                CurrentState = (float)State.Circling;
                CurrentSecondaryState = (float)SecondaryState.SpawnProjectiles;
                SecondaryTimer = 0;
                npcCount = 0;

                NPC.netUpdate = true;
            }

			// After circling for a while, return to chasing
			if (Timer >= HelperMethods.SecondsToTicks(70))
            {
                Timer = 1;
                CurrentState = (float)State.Chasing;
                CurrentSecondaryState = (float)SecondaryState.SpawnMinions;
                SecondaryTimer = 0;
				SoundEngine.PlaySound(new SoundStyle("AssortedAdditions/Assets/Sounds/NPCSound/FireDragonSounds"), NPC.position);
			}

            // Movement stuff, what the dragon is chasing
            MovementBasedOnState();

			// Minion spawning, projectile spawning
			AttackBasedOnSecondaryState();
        }

        private void GetTargetPlayerOrFlee()
        {
			// Finding target
			if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
			{
				NPC.TargetClosest();
			}
			Player target = Main.player[NPC.target];
            
			// Checking if target has died
			if (target.dead || !target.active) // If yes, then start fleeing
			{
				if (PlayersAreDead == false)
				{
					Timer = 0;
					PlayersAreDead = true;
                    CurrentState = (float)State.Fleeing;
				}

				// If the targeted player is dead, flee
				NPC.velocity.Y -= 0.8f;
				MoveSpeed = 20f;

				Timer++; // Count up until despawn (5 seconds)
				if (Timer >= 300) // 5 seconds from current Timer value, otherwise if 300 was already reached it would instantly despawn
				{
					NPC.active = false; // Other parts will despawn automatically due to FireDragonBuilder.cs
				}
			}
        }

        private bool setCirclingPoint = false; // Whether a point for circling has been set
        private Vector2 circlingPoint; // Saves the player's center position when state switches to circling, the dragon will circle this point
        private float rotation; // Rotates an imaginary target around the above point
        
        private const int circlingRadius = 550; // The radius of the circle that the dragon moves along
        private int directionForRadius = 1;

        private int HeadHitCooldown = 0; // When the head hits the player (hitbox intersects), this gets set
        // This is to prevent the dragon from latching onto the player
        private void MovementBasedOnState()
        {
            Player target = Main.player[NPC.target];
            Vector2 targetCenter = target.Center;

			if (NPC.Hitbox.Intersects(target.Hitbox))
            {
                HeadHitCooldown = 120;
                NPC.netUpdate = true;
            }

			switch (CurrentState)
			{
				// The dragon will chase the player
				case (float)State.Chasing:

					MoveSpeed = 9f;
					setCirclingPoint = false;
					targetCenter = target.Center;

					break;

				// The dragon will start circling around a point (where player stood when this state began)
				case (float)State.Circling:

					// Set the point once, dragon will start circling the point
					if (!setCirclingPoint)
					{
						SoundEngine.PlaySound(new SoundStyle("AssortedAdditions/Assets/Sounds/NPCSound/FireDragonSounds"), NPC.position);
						circlingPoint = target.Center + target.velocity;
						directionForRadius = target.direction;
						setCirclingPoint = true;
						NPC.netUpdate = true;
					}

					// Speed up so the player cant escape easily (if they got trapped)
					// When circling state is ending slowdown to allow escape (6 seconds until the dragon starts chasing the player)
					// (At 2 minutes the state changes)
					MoveSpeed = Timer < HelperMethods.SecondsToTicks(114) ? 14f : 7f; // These values leave a gap where the player can try to escape
					rotation += Timer < HelperMethods.SecondsToTicks(114) ? 0.026f : 0.013f;

					if (Timer < HelperMethods.SecondsToTicks(114))
					{
						targetCenter = circlingPoint + new Vector2(0, directionForRadius * circlingRadius).RotatedBy(rotation);
					}
					else
					{
						targetCenter = circlingPoint + new Vector2(0, directionForRadius * (circlingRadius + 200)).RotatedBy(rotation);
					}

					break;

				// State should never be something not specified, this is here so that the compiler is happy
				default:
					return;
			}

			// Catch up to the target.
			// (When one player dies and target switches to another one far away, fly to them fast)
			if (NPC.Center.Distance(target.Center) >= 5000) 
            {
                MoveSpeed = 20f;
            }

            // If the cooldown is active, switch homing style to prevent latching onto the player
            // Only apply during non circling states
            if(HeadHitCooldown == 0 || CurrentState == (float)State.Circling)
            {
                if(CurrentState == (float)State.Circling)
                {
					HelperMethods.SmoothHoming(NPC, targetCenter, MoveSpeed, MoveSpeed, null, false);
				}
                else
                {
					HelperMethods.SmoothHoming(NPC, targetCenter, 0.3f, MoveSpeed, null, false);
				}	
			}

            if(HeadHitCooldown <= 60 && HeadHitCooldown != 0 && CurrentState != (float)State.Circling)
            {
				// With these the projectile will move towards the target in a smooth way
				// instead of snapping it moves in a curved way
				float targetPos = (target.Center - NPC.Center).ToRotation();
				float curve = NPC.velocity.ToRotation();
				float maxTurn = MathHelper.ToRadians(3f);

				// Set the velocity and rotation:
				NPC.velocity = NPC.velocity.RotatedBy(MathHelper.WrapAngle(curve.AngleTowards(targetPos, maxTurn)) - curve);
			}
            
            if(HeadHitCooldown != 0)
            {
                HeadHitCooldown--;
            }
            NPC.netUpdate = true;
        }

		private void AttackBasedOnSecondaryState()
        {
			Player targetPlayer = Main.player[NPC.target];

			switch (CurrentSecondaryState)
            {
                case (float)SecondaryState.SpawnMinions:
					
                    // Spawn one every second for 7 seconds (spawns 7 minions)
					if (SecondaryTimer % 60 == 0 && SecondaryTimer <= HelperMethods.SecondsToTicks(37))
					{
						// Checks how many minions exist
						for (int i = 0; i < Main.npc.Length; i++)
						{
							if (Main.npc[i].active && Main.npc[i].type == ModContent.NPCType<FireDrake>())
							{
								npcCount++;
								NPC.netUpdate = true;
							}
						}

						// Only spawn them in if there are less than 7 already
						if (npcCount < 7)
						{
							SpawnMinions();
						}
					}

					// Reset stuff once state is finished
					if (SecondaryTimer >= HelperMethods.SecondsToTicks(37))
					{
						npcCount = 0;
						NPC.netUpdate = true;
						SecondaryTimer = 0;
						CurrentSecondaryState = (float)SecondaryState.None;
						SoundEngine.PlaySound(new SoundStyle("AssortedAdditions/Assets/Sounds/NPCSound/FireDragonSounds"), NPC.position);
					}

				break;

                case (float)SecondaryState.SpawnProjectiles:

                    // If player got trapped by the dragon, projectiles spawn from outside the circle and fly towards the center
                    if (targetPlayer.Center.Distance(circlingPoint) <= circlingRadius)
                    {
                        int modulus = NPC.life < NPC.lifeMax / 3 ? 50 : 75; // When on low health, spawn projectiles faster
                        if (SecondaryTimer % modulus == 0)
                        {
							float randomRotation = Main.rand.NextFloat(0, 6.28318531f);
							Vector2 spawnPosition = circlingPoint + new Vector2(600, 500).RotatedBy(randomRotation);
							Vector2 direction = circlingPoint - spawnPosition;
                            direction.Normalize();

                            if (Main.netMode != NetmodeID.MultiplayerClient)
                            {
								Projectile.NewProjectile(NPC.GetSource_FromAI(), spawnPosition, direction * 8f, ModContent.ProjectileType<FireDragonFireball>(), 35, 4f, Main.myPlayer);
							}
                        }
                    }
                    // If player didnt get trapped or escapes, set the circling point again
                    else
                    {
                        circlingPoint = targetPlayer.Center + targetPlayer.velocity;
                        directionForRadius = targetPlayer.direction;
                        rotation = 0;
                        NPC.netUpdate = true;
                    }

                    break;	
			}

			SecondaryTimer++;
		}

        private void SpawnMinions()
        {
            // Spawn location varies, can be any of the screen corners
            int xPos = 0;
            int yPos = 0;
            int location = Main.rand.Next(0, 4);
            switch (location)
            {
                case 0:
                    // Top left corner (screenPosition defaults to top left)
                    xPos = (int)Main.screenPosition.X;
                    yPos = (int)Main.screenPosition.Y;
                    break;

                case 1:
                    // Top right corner (We can get other corner's by adding the screen width/height accordingly)
                    xPos = (int)Main.screenPosition.X + Main.screenWidth;
                    yPos = (int)Main.screenPosition.Y;
                    break;

                case 2:
                    // Left bottom corner
                    xPos = (int)Main.screenPosition.X;
                    yPos = (int)Main.screenPosition.Y + Main.screenHeight;
                    break;

                case 3:
                    // Right bottom corner
                    xPos = (int)Main.screenPosition.X + Main.screenWidth;
                    yPos = (int)Main.screenPosition.Y + Main.screenHeight;
                    break;
            }

            // Because we want to spawn minions, and minions are NPCs, we have to do this on the server (or singleplayer, "!= NetmodeID.MultiplayerClient" covers both)
            // This means we also have to sync it after we spawned and set up the minion
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                // Spawn the minion at the position when in singleplayer
                NPC.NewNPCDirect(NPC.GetSource_FromAI(), xPos, yPos, ModContent.NPCType<FireDrake>(), NPC.whoAmI);
            }
            else
            { // otherwise server handles it
                var message = Mod.GetPacket();
                message.Write((byte)AssortedAdditions.MessageType.SpawnGenericNPC);
                message.Write(xPos);
                message.Write(yPos);
                message.Write(ModContent.NPCType<FireDrake>());
                message.Send();
            }
        }

		public override void SendExtraAI(BinaryWriter writer)
		{
			writer.Write(npcCount);
            writer.Write(HeadHitCooldown);
		}

        public override void ReceiveExtraAI(BinaryReader reader)
		{
			npcCount = reader.ReadInt32();
            HeadHitCooldown = reader.ReadInt32();
		}

		public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = ImmunityCooldownID.Bosses; // use the boss immunity cooldown counter, to prevent ignoring boss attacks by taking damage from other sources
            return true;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                // Mod.Find can't run on servers, it will crash
                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("FireDragonHead_Gore").Type, 1f);
                }
            }
        }
    }

    internal class FireDragonBody : WormBody
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);

            // Immune to fire debuffs
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.ShadowFlame] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.CursedInferno] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Burning] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;

			NPCID.Sets.CantTakeLunchMoney[Type] = true;

			Main.npcFrameCount[Type] = 2;
        }

        public override void SetDefaults()
        {
            NPC.width = 50;
            NPC.height = 45;
            NPC.damage = 25;
            NPC.defense = 36;
            NPC.lifeMax = 28500;
            NPC.HitSound = SoundID.NPCHit7;
            NPC.SpawnWithHigherTime(30);
            NPC.boss = true;
            NPC.noTileCollide = true;
			NPC.noGravity = true;
			NPC.aiStyle = -1;
        }

		public override void Init()
        {
            FireDragonHead.CommonWormInit(this);
        }

        public override void FindFrame(int frameHeight)
        {
            // Body segment will have a leg every 5th segment
            // Beginning from a little past the first segment
            if (NPC.ai[2] > 2 && (NPC.ai[2] - 3) % 5 == 0)
            {
                NPC.frame.Y = frameHeight * 1;
            }
            else
            {
                NPC.frame.Y = 0;
            }
        }

        public override void AI()
        {
            if (Main.rand.NextBool(15)) // Spawns some fire dust around the dragon
            {
                Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height,
                        DustID.FlameBurst, 3, 3, 180, default, 1f);
                dust.noGravity = true;
            }
		}

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = ImmunityCooldownID.Bosses;
            return true;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                // Mod.Find can't run on servers, it will crash
                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("FireDragonBody_Gore").Type, 1f);
                }
            }
        }
    }

    internal class FireDragonTail : WormTail
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);

            // Immune to fire debuffs
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.ShadowFlame] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.CursedInferno] = true;
			NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Burning] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;

			NPCID.Sets.CantTakeLunchMoney[Type] = true;
		}

        public override void SetDefaults()
        {
            NPC.width = 70;
            NPC.height = 116;
            NPC.damage = 30;
            NPC.defense = 18;
            NPC.lifeMax = 28500;
            NPC.HitSound = SoundID.NPCHit7;
            NPC.SpawnWithHigherTime(30);
            NPC.boss = true;
            NPC.noTileCollide = true;
			NPC.noGravity = true;
			NPC.aiStyle = -1;
        }

		public override void Init()
        {
            FireDragonHead.CommonWormInit(this);
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = ImmunityCooldownID.Bosses;
            return true;
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                // Mod.Find can't run on servers, it will crash
                if (Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("FireDragonTail_Gore").Type, 1f);
                }
            }
        }
    }
}
