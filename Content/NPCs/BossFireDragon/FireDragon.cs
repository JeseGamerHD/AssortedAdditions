﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ModdingTutorial.Content.Items.Consumables.TreasureBags;
using ModdingTutorial.Content.Items.Misc;
using ModdingTutorial.Content.Items.Placeables.Trophies;
using ModdingTutorial.Content.Items.Weapons.Magic;
using ModdingTutorial.Content.Items.Weapons.Melee;
using ModdingTutorial.Content.Items.Weapons.Ranged;
using ModdingTutorial.Content.Items.Weapons.Summon;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

// TODO

namespace ModdingTutorial.Content.NPCs.BossFireDragon // This Boss NPC is built using FireDragonBuilder.cs
{
    [AutoloadBossHead]
    internal class FireDragonHead : WormHead
    {
        public override int BodyType => ModContent.NPCType<FireDragonBody>();
        public override int TailType => ModContent.NPCType<FireDragonTail>();

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Fire Dragon");
            NPCID.Sets.MPAllowedEnemies[Type] = true; // Summoned using an item
            NPCID.Sets.BossBestiaryPriority.Add(Type);

            // Immune to fire debuffs
            NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[] {
                    BuffID.OnFire,
                    BuffID.OnFire3,
                    BuffID.ShadowFlame,
                    BuffID.Burning,

                    BuffID.Confused // Most NPCs have this
				}
            };
            NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);

            // Beastiary stuff
            var drawModifier = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            { // Influences how the NPC looks in the Bestiary
                CustomTexturePath = "ModdingTutorial/Content/NPCs/BossFireDragon/FireDragon_Beastiary",
                PortraitScale = 1.5f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, drawModifier);
        }

        public override void SetDefaults()
        {
            NPC.width = 48;
            NPC.height = 100;
            NPC.value = Item.sellPrice(gold: 10);

            NPC.damage = 50;
            NPC.defense = 12;
            NPC.lifeMax = 10000;
            NPC.knockBackResist = 0f;

            NPC.HitSound = SoundID.NPCHit7;
            NPC.DeathSound = SoundID.DD2_BetsyDeath;
            NPC.SpawnWithHigherTime(30);
            NPC.boss = true;
            NPC.npcSlots = 10f; // Take up open spawn slots, preventing random NPCs from spawning during the fight
            NPC.noTileCollide = true;

            NPC.aiStyle = -1;

            // Music doesnt fit, needs to be changed TODO
/*            if (!Main.dedServ)
            {
                Music = MusicLoader.GetMusicSlot(Mod, "Assets/Music/TheMissingConnection");
            }*/
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld,
                new FlavorTextBestiaryInfoElement("It answers the call of the Ancient Token, however, it does not like being disturbed.")
            });
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.BossBag(ModContent.ItemType<FireDragonBag>())); // Treasure bag
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<FireDragonTrophy>(), 10)); // 10% chance for trophy
            //npcLoot.Add(ItemDropRule.MasterModeCommonDrop(ModContent.ItemType<FireDragonRelic>()));

            // If the player is not playing one expert/master, treasure bag loot drops by itself
            LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert());
            
            // Drops 1 of these:
            notExpert.OnSuccess(ItemDropRule.OneFromOptions(1,
                ModContent.ItemType<DraconicBlade>(),
                ModContent.ItemType<DraconicTome>(),
                ModContent.ItemType<DragonStaff>(),
                ModContent.ItemType<DraconicBow>() ) );
            npcLoot.Add(notExpert); // Register the loot

            // Same goes for Dragon scales
            LeadingConditionRule notExpert2 = new LeadingConditionRule(new Conditions.NotExpert());
            notExpert2.OnSuccess(ItemDropRule.Common(ModContent.ItemType<DragonScale>(), 1, 30, 50));
            npcLoot.Add(notExpert2);
        }

        public override void BossLoot(ref string name, ref int potionType)
        {
            potionType = ItemID.HealingPotion; // Drop healing potions
        }

        public override void Init()
        {
            // Basically how long the Dragon will be
            MinSegmentLength = 40;
            MaxSegmentLength = 40;
            
            CommonWormInit(this); // Segments spawn in FireDragonBuilder.cs
        }

        internal static void CommonWormInit(FireDragonBuilder worm)
        {
            worm.MoveSpeed = 6f;
            worm.Acceleration = 0.7f;
        }

        // Timer for AI
        public float Timer
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }

        bool hasDied = false; // Used for despawning the boss
        int npcCount = 0;

        // Since the Head controls the body and the tail, all AI stuff happens here
        public override void AI()
        {
            // Finding target
            if (NPC.target < 0 || NPC.target == 255 || Main.player[NPC.target].dead || !Main.player[NPC.target].active)
            {
                NPC.TargetClosest();
            }
            Player target = Main.player[NPC.target];

            // Checking if target has died
            if (target.dead)
            {
                if(hasDied == false)
                {
                    Timer = 0;
                    hasDied = true;
                }

                // If the targeted player is dead, flee
                NPC.velocity.Y -= 0.8f;
                MoveSpeed = 20f;

                Timer++; // Count up until despawn (5 seconds)
                if(Timer >= 300) // 5 seconds from current Timer value, otherwise if 300 was already reached it would instantly despawn
                {
                    NPC.active = false; // Other parts will despawn automatically due to FireDragonBuilder.cs
                }
                return;
            }

            Timer++; // Timer used for timing boss actions

            // Then dragon chases the player
            NPC.velocity = (target.Center - NPC.Center).SafeNormalize(Vector2.Zero) * MoveSpeed;

            // Summons minions after 15 seconds
            if(Timer >= 900)
            {
                // Do a rumbling sound when summoning begins / ends
                if (Timer == 900 || Timer == 1320)
                {
                    SoundEngine.PlaySound(new SoundStyle("ModdingTutorial/Assets/Sounds/FireDragonSounds"), NPC.position);
                }

                // Spawn one every second for 7 seconds (spawns 7 minions)
                if (Timer % 60 == 0 && Timer <= 1320)
                {
                    // Checks how many minions exist
                    for (int i = 0; i < Main.npc.Length; i++)
                    {
                        if (Main.npc[i].active && Main.npc[i].type == ModContent.NPCType<FireDrake>())
                        {
                            npcCount++;
                        }
                    }

                    // Only spawn them in if there are less than 7 already
                    if(npcCount < 7)
                    {
                        spawnMinions();
                    }   
                }  
            }

            // Reset Timer
            if (Timer >= 1320)
            {
                npcCount = 0;
                Timer = 1;
            }
        }

        private void spawnMinions()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                // Because we want to spawn minions, and minions are NPCs, we have to do this on the server (or singleplayer, "!= NetmodeID.MultiplayerClient" covers both)
                // This means we also have to sync it after we spawned and set up the minion
                return;
            }

            // Spawn location varies, can be any of the screen corners
            int location = Main.rand.Next(0, 4);
            switch (location)
            {
                case 0:
                    // Top left corner (screenPosition defaults to top left)
                    NPC.NewNPC(NPC.GetSource_FromAI(), (int)Main.screenPosition.X, (int)Main.screenPosition.Y, ModContent.NPCType<FireDrake>());
                    break;
                case 1:
                    // Top right corner (We can get other corner's by adding the screen width/height accordingly)
                    NPC.NewNPC(NPC.GetSource_FromAI(), (int)Main.screenPosition.X + Main.screenWidth, (int)Main.screenPosition.Y, ModContent.NPCType<FireDrake>());
                    break;
                case 2:
                    // Left bottom corner
                    NPC.NewNPC(NPC.GetSource_FromAI(), (int)Main.screenPosition.X, (int)Main.screenPosition.Y + Main.screenHeight, ModContent.NPCType<FireDrake>());
                    break;
                case 3:
                    // Right bottom corner
                    NPC.NewNPC(NPC.GetSource_FromAI(), (int)Main.screenPosition.X + Main.screenWidth, (int)Main.screenPosition.Y + Main.screenHeight, ModContent.NPCType<FireDrake>());
                    break;
            }
            
            // Sync with multiplayer...
        }

        public override bool CanHitPlayer(Player target, ref int cooldownSlot)
        {
            cooldownSlot = ImmunityCooldownID.Bosses; // use the boss immunity cooldown counter, to prevent ignoring boss attacks by taking damage from other sources
            return true;
        }

        public override void HitEffect(int hitDirection, double damage)
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
    // ************************************************************************************************************************************************** //
    internal class FireDragonBody : WormBody
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);

            // Immune to fire debuffs
            NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[] {
                    BuffID.OnFire,
                    BuffID.OnFire3,
                    BuffID.ShadowFlame,
                    BuffID.Burning,

                    BuffID.Confused // Most NPCs have this
				}
            };
            NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);
        }

        public override void SetDefaults()
        {
            NPC.width = 48;
            NPC.height = 45;
            NPC.damage = 25;
            NPC.defense = 12;
            NPC.lifeMax = 10000;
            NPC.HitSound = SoundID.NPCHit7;
            NPC.SpawnWithHigherTime(30);
            NPC.boss = true;
            NPC.noTileCollide = true;
            NPC.aiStyle = -1;
        }

        public override void Init()
        {
            FireDragonHead.CommonWormInit(this);
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

        public override void HitEffect(int hitDirection, double damage)
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
    // ************************************************************************************************************************************************** //
    internal class FireDragonTail : WormTail
    {
        public override void SetStaticDefaults()
        {
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                Hide = true // Hides this NPC from the Bestiary, useful for multi-part NPCs whom you only want one entry.
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);

            // Immune to fire debuffs
            NPCDebuffImmunityData debuffData = new NPCDebuffImmunityData
            {
                SpecificallyImmuneTo = new int[] {
                    BuffID.OnFire,
                    BuffID.OnFire3,
                    BuffID.ShadowFlame,
                    BuffID.Burning,

                    BuffID.Confused // Most NPCs have this
				}
            };
            NPCID.Sets.DebuffImmunitySets.Add(Type, debuffData);
        }

        public override void SetDefaults()
        {
            NPC.width = 50;
            NPC.height = 136;
            NPC.damage = 30;
            NPC.defense = 12;
            NPC.lifeMax = 10000;
            NPC.HitSound = SoundID.NPCHit7;
            NPC.SpawnWithHigherTime(30);
            NPC.boss = true;
            NPC.noTileCollide = true;
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

        public override void HitEffect(int hitDirection, double damage)
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
