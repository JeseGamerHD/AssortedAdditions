﻿using Terraria.Audio;
using Microsoft.Xna.Framework;
using ModdingTutorial.Content.Projectiles.NPCProj;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent.Bestiary;

namespace ModdingTutorial.Content.NPCs.BossFireDragon
{
    internal class FireDrake : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 5; // The amount of frames the NPC has

            // Influences how the NPC looks in the Bestiary
            NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Velocity = 1f, // Draws the NPC in the bestiary as if its moving +1 tiles in the x direction
                Direction = 1 // -1 is left and 1 is right.
            };

            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
        }

        public override void SetDefaults()
        {
            NPC.width = 38;
            NPC.height = 22;
            NPC.noTileCollide = true;
            NPC.noGravity = true;
            
            NPC.lifeMax = 50;
            NPC.defense = 5;
            NPC.damage = 25;
            NPC.knockBackResist = 0.5f;

            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;

            NPC.aiStyle = 0;
        }

        // Used for movement in AI
        private float speed = 15f;
        private float inertia = 30f;

        public override void AI()
        {
            Player target = Main.player[NPC.target];
            float distanceFromTarget = Vector2.Distance(target.Center, NPC.Center);

            if(target.dead)
            {
                Vector2 direction = Main.screenPosition - NPC.position;
                direction.Normalize();
                direction *= speed;
                NPC.velocity = (NPC.velocity * (inertia - 1) + direction) / inertia;
                NPC.spriteDirection = -1 * NPC.direction;

                NPC.EncourageDespawn(5);
            }
            else
            {
                Movement(target, distanceFromTarget);
            }

        }

        private void Movement(Player target, float distanceFromTarget)
        {
            // The dragon will attack with fire first until it gets closer
            if (distanceFromTarget <= 400f && distanceFromTarget >= 350f && Main.netMode != NetmodeID.MultiplayerClient)
            {
                if (Main.rand.NextBool(3))
                {
                    Vector2 direction = target.Center - NPC.Center;
                    direction.Normalize();
                    int type = ModContent.ProjectileType<FireDrakeProj>();
                    
                    // Spawn the projectile, check netmode
                    if(Main.netMode != NetmodeID.MultiplayerClient)
                    {
                        Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, direction * 10f, type, 15, 0f, Main.myPlayer);
                    }
                    SoundEngine.PlaySound(SoundID.Item20, NPC.Center);
                }
            }

            // Minion has a target: attack (here, fly towards the enemy)
            if (distanceFromTarget > 40f)
            {
                // The immediate range around the target (so it doesn't latch onto it when close)
                Vector2 direction = target.Center - NPC.Center;
                direction.Normalize();
                direction *= speed;

                NPC.velocity = (NPC.velocity * (inertia - 1) + direction) / inertia;
            }
        }

        // Visual animation happens here
        public override void FindFrame(int frameHeight)
        {
            Player target = Main.player[NPC.target];
            float distanceFromTarget = Vector2.Distance(target.Center, NPC.Center);
            int frameSpeed = 5;
            NPC.frameCounter++;

            NPC.spriteDirection = -1 * NPC.direction; // Faces left/right correctly

            if (NPC.frameCounter >= frameSpeed)
            {
                NPC.frameCounter = 0;
                NPC.frame.Y += frameHeight;

                // When the dragon is doing its fire attack it has a unique frame
                if (distanceFromTarget < 400f && distanceFromTarget > 200f)
                {
                    NPC.frame.Y = 4 * frameHeight;
                }
                else if (NPC.frame.Y == 4 * frameHeight) // Ensures that fire attack frame only plays when actually attacking
                {
                    NPC.frame.Y = frameHeight;
                }

                if(NPC.frame.Y == 5 * frameHeight)
                {
                    NPC.frame.Y = 0;
                }
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				
                // Sets the spawning conditions of this NPC that is listed in the bestiary.
                // Also sets the background to match the biome
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,

				// Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("A loyal minion of the Fire Dragon. It will protect its master by spewing fire and biting any aggressors.")
            });
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if(NPC.life <= 0)
            {
                // Mod.Find can't run on servers, it will crash
                if(Main.netMode != NetmodeID.Server)
                {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("FireDrakeWing").Type, 1f);
                    Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("FireDrakeHead").Type, 1f);
                }
            }
        }
    }
}
