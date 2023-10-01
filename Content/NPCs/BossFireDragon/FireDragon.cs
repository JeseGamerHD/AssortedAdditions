using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ModdingTutorial.Content.Items.Consumables.TreasureBags;
using ModdingTutorial.Content.Items.Misc;
using ModdingTutorial.Content.Items.Pets;
using ModdingTutorial.Content.Items.Placeables.Relics;
using ModdingTutorial.Content.Items.Placeables.Trophies;
using ModdingTutorial.Content.Items.Weapons.Magic;
using ModdingTutorial.Content.Items.Weapons.Melee;
using ModdingTutorial.Content.Items.Weapons.Ranged;
using ModdingTutorial.Content.Items.Weapons.Summon;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.NPCs.BossFireDragon // This Boss NPC is built using FireDragonBuilder.cs
{
    [AutoloadBossHead]
    internal class FireDragonHead : WormHead
    {
        public override int BodyType => ModContent.NPCType<FireDragonBody>();
        public override int TailType => ModContent.NPCType<FireDragonTail>();

        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Fire Dragon");
            NPCID.Sets.MPAllowedEnemies[Type] = true; // Summoned using an item
            NPCID.Sets.BossBestiaryPriority.Add(Type);

            // Immune to fire debuffs
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.ShadowFlame] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Burning] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;

            // Beastiary stuff
            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
            {
                CustomTexturePath = "ModdingTutorial/Content/NPCs/BossFireDragon/FireDragon_Bestiary",
                PortraitScale = 1.5f
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.width = 48;
            NPC.height = 100;
            NPC.value = 450000;

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

            if (!Main.dedServ)
            {
                Music = MusicID.OtherworldlyInvasion;
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            bestiaryEntry.Info.AddRange(new List<IBestiaryInfoElement> {
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.TheUnderworld, // sets the background image
                new FlavorTextBestiaryInfoElement("It lays dormant in the depths of the underworld until it is challenged. It commands an army of Fire Drakes")
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
                ModContent.ItemType<DraconicBow>() ) );
            npcLoot.Add(notExpert); // Register the loot

            // Same goes for Dragon scales
            LeadingConditionRule notExpert2 = new LeadingConditionRule(new Conditions.NotExpert());
            notExpert2.OnSuccess(ItemDropRule.Common(ModContent.ItemType<DragonScale>(), 1, 30, 50));
            npcLoot.Add(notExpert2);
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
            worm.MoveSpeed = 7f;
        }

        // Timers for AI
        public float Timer
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }

        bool hasDied = false; // Used for despawning the boss
        int npcCount = 0; // Used for limiting spawned minion enemies

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
            if (target.dead || !target.active)
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

            // ********* //
            Timer++;
            // Timer used for timing boss actions

            // Set the dragon to chase the player using these:
            // when hitting the player it will continue for a bit and then smoothly turn back to chase
            float moveTo = Utils.ToRotation(target.Center - NPC.Center);
            float curve = Utils.ToRotation(NPC.velocity);
            float maxTurn = MathHelper.ToRadians(4f);
            NPC.velocity = Utils.RotatedBy(NPC.velocity, MathHelper.WrapAngle(Utils.AngleTowards(curve, moveTo, maxTurn)) - curve).SafeNormalize(Vector2.Zero) * MoveSpeed;

            // Summons minions after 15 seconds
            if(Timer >= 900)
            {
                // Do a rumbling sound when summoning begins / ends
                if (Timer == 900 || Timer == 1320)
                {
                    SoundEngine.PlaySound(new SoundStyle("ModdingTutorial/Assets/Sounds/NPCSound/FireDragonSounds"), NPC.position);
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
                        SpawnMinions();
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

        private void SpawnMinions()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
            {
                // Because we want to spawn minions, and minions are NPCs, we have to do this on the server (or singleplayer, "!= NetmodeID.MultiplayerClient" covers both)
                // This means we also have to sync it after we spawned and set up the minion
                return;
            }

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

            // Spawn the minion at the position selected
            NPC minion = NPC.NewNPCDirect(NPC.GetSource_FromAI(), xPos, yPos, ModContent.NPCType<FireDrake>(), NPC.whoAmI);
            
            // Sync up
            if(Main.netMode == NetmodeID.Server)
            {
                NetMessage.SendData(MessageID.SyncNPC, number: minion.whoAmI);
            }
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
    // ************************************************************************************************************************************************** //
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
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Burning] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
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
    // ************************************************************************************************************************************************** //
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
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Burning] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
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
