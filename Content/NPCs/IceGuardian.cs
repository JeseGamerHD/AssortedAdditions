using AssortedAdditions.Content.Items.Misc;
using AssortedAdditions.Content.Items.Placeables.Banners;
using AssortedAdditions.Content.Projectiles.NPCProj;
using Microsoft.Xna.Framework;
using AssortedAdditions.Content.Projectiles;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;
using System;

namespace AssortedAdditions.Content.NPCs
{
    internal class IceGuardian : ModNPC // TODO
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 4;

            // Immune to debuffs:
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn2] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
        }
        public override void SetDefaults()
        {
            NPC.width = 48;
            NPC.height = 48;
            NPC.damage = 50;
            NPC.defense = 20;
            NPC.lifeMax = 250;
            NPC.knockBackResist = 0.5f;
            NPC.HitSound = SoundID.NPCHit5;
            NPC.DeathSound = SoundID.NPCDeath7;
            NPC.value = 500;

            NPC.noGravity = true;
            NPC.noTileCollide = true;

            NPC.aiStyle = 10;

            Banner = NPC.type;
            BannerItem = ModContent.ItemType<IceGuardianBanner>();
        }

        private int timer;

        public override bool PreAI()
        {
            Player target = Main.player[NPC.target];

            Lighting.AddLight(NPC.Center, TorchID.Blue);

            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height,
                    DustID.Frost, 0, 0, 150, default, 1.5f);
                dust.noGravity = true;
            }

            if (timer % 25 == 0 && timer <= 100)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    int npc = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, ModContent.NPCType<IceGuardianShield>(), 0, NPC.whoAmI);
					NetMessage.SendData(MessageID.SyncNPC, number: npc);
				}
            }
            timer++;

            if (timer % 180 == 0)
            {
                Vector2 direction = target.Center - NPC.Center;
                direction.Normalize();
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    SoundEngine.PlaySound(SoundID.Item28, NPC.position);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), NPC.Center, direction * 10f, ModContent.ProjectileType<IceGuardianProj>(), 20, 4f, Main.myPlayer);
                }
            }
            return true;
        }

        private int frame;
        public override void FindFrame(int frameHeight)
        {
            // Switch frames
            if (NPC.frameCounter % 10 == 0)
            {
                frame++;
            }
            // When over the last frame, back to start
            // (frames start from 0)
            if (frame == 4)
            {
                frame = 0;
            }
            NPC.frameCounter++;

            // Set the frame
            NPC.frame.Y = frameHeight * frame;
        }

        public override void OnKill()
        {
            for (int i = 0; i < 60; i++)
            {
                Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height,
                    DustID.Frost, 0, 0, 150, default, 1f);
                dust.noGravity = true;
                dust.velocity *= 3f;
            }
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.downedMechBossAny && spawnInfo.Player.ZoneSnow && spawnInfo.Player.ZoneRockLayerHeight)
            {
                return SpawnCondition.Cavern.Chance * 0.05f;
            }

            return 0f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.FrostCore, 6, 1, 1));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<IceEssence>(), 10, 1, 1));
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				
                // Sets the spawning conditions of this NPC that is listed in the bestiary.
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundSnow,
				
                // Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("These magical creatures guard the depths of the ice caverns from anyone looking to plunder their treasures")
            });
        }
    }
}
