using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using ModdingTutorial.Content.Items.Placeables.Banners;
using Microsoft.Xna.Framework;
using Terraria.GameContent.Bestiary;
using System;
using Terraria.ModLoader.Utilities;

namespace ModdingTutorial.Content.NPCs
{
    internal class GoblinBalloonist : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 2;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;

            NPCID.Sets.NPCBestiaryDrawModifiers value = new NPCID.Sets.NPCBestiaryDrawModifiers(0)
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
            NPC.lifeMax = 300;
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

        public override void AI()
        {
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

        public float npcCounter
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Main.invasionType == InvasionID.GoblinArmy)
            {
                return SpawnCondition.Invasion.Chance * 0.1f;
            }

            return 0f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            // TODO
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                for (int i = 0; i < 30; i++)
                {
                    Dust dust = Dust.NewDustDirect(NPC.position, NPC.width, NPC.height, DustID.Confetti, 0f, 0f, 0, Color.Red, 1.5f);
                    dust.velocity *= 1.4f;
                }

                if(Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.position.X, (int)NPC.position.Y, NPCID.GoblinScout, NPC.whoAmI);
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
