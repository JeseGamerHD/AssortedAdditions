﻿using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Bestiary;
using System;
using Terraria.ModLoader.Utilities;
using Terraria.Audio;
using Terraria.GameContent.ItemDropRules;
using Terraria.Graphics.Shaders;
using AssortedAdditions.Content.Items.Tools;
using AssortedAdditions.Content.Items.Placeables.Banners;
using AssortedAdditions.Content.Projectiles.NPCProj;
using AssortedAdditions.Common.Configs;

namespace AssortedAdditions.Content.NPCs
{
    internal class GoblinBalloonist : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 2;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                PortraitScale = 0.75f,
                PortraitPositionYOverride = -15,
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(NPC.type, value);
        }

        public override void SetDefaults()
        {
            NPC.width = 74;
            NPC.height = 132;
            NPC.damage = 15;
            NPC.defense = 4;
            NPC.lifeMax = 280;
            NPC.knockBackResist = 0.4f; // 60% resist
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath63;
            NPC.value = 1000;
            NPC.aiStyle = 0;

            NPC.noGravity = true;

            Banner = NPC.type;
            BannerItem = ModContent.ItemType<GoblinBalloonistBanner>();
        }

        // Used for movement in AI
        private float speed = 4f;
        private float inertia = 12f;
        public float Timer
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }

        public override void AI()
        {
            Timer++;
            NPC.spriteDirection = 1;
            Player player = Main.player[NPC.target];
            Vector2 targetPos = new(player.position.X, player.position.Y - 200);
            float distanceFromTarget = Vector2.Distance(targetPos, NPC.Center);

            if (distanceFromTarget > 40f)
            {
                Vector2 direction = targetPos - NPC.Center;
                direction.Normalize();
                direction *= speed;
                NPC.velocity = (NPC.velocity * (inertia - 1) + direction) / inertia;
            }

            NPC.rotation = NPC.velocity.X * 0.01f; // Slight lean when moving

            // Used for preventing overlap between NPCs
            float overlapVelocity = 0.04f;

            // Fix overlap:
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

            if (Timer % 500 == 0 && Timer != 0)
            {
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    SoundEngine.PlaySound(SoundID.Item1, NPC.position);
                    Projectile.NewProjectile(NPC.GetSource_FromAI(), new(NPC.Center.X, NPC.Center.Y + 10), NPC.velocity, ModContent.ProjectileType<GoblinBomb>(), 40, 4f, Main.myPlayer);
                }
            }
        }

        private int frame;
        public override void FindFrame(int frameHeight)
        {
            if (NPC.direction == 1)
            {
                frame = 0;
            }
            else
            {
                frame = 1;
            }

            NPC.frame.Y = frameHeight * frame;
        }

		public override float SpawnChance(NPCSpawnInfo spawnInfo)
		{
			if (Main.invasionType == InvasionID.GoblinArmy)
			{
				float multiplier = ServerSidedToggles.Instance.NPCSpawnMultiplier == 1f
					? ServerSidedToggles.Instance.GoblinBalloonistSpawnMultiplier : ServerSidedToggles.Instance.NPCSpawnMultiplier;

				return SpawnCondition.Invasion.Chance * 0.1f * multiplier;
			}

			return 0f;
		}

		public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<BagOfBombs>(), 10));
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 30; i++)
                {
                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Confetti, 0f, 0f, 0, default, 2f);
                    dust.velocity *= 1.4f;
                    dust.shader = GameShaders.Armor.GetShaderFromItemId(ItemID.RedandBlackDye);
				}

                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                   int npc = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X, (int)NPC.position.Y, NPCID.GoblinScout, NPC.whoAmI);
                    NetMessage.SendData(MessageID.SyncNPC, number: npc);
                }
            }
        }

        public override void OnKill()
        {
            Main.invasionProgress += 1;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				
                // Sets the spawning conditions of this NPC that is listed in the bestiary.
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
				
                // Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("The Goblin Army's airforce, they give air support by dropping bombs")
            });
        }
    }
}
