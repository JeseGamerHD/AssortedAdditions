using AssortedAdditions.Common.Configs;
using AssortedAdditions.Content.Items.Accessories;
using AssortedAdditions.Content.Items.Placeables.Banners;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace AssortedAdditions.Content.NPCs
{
    internal class GrabberPlant : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 8;
        }

        public override void SetDefaults()
        {
            NPC.width = 50;
            NPC.height = 40;
            NPC.damage = 15;
            NPC.defense = 6;
            NPC.lifeMax = 100;
            NPC.knockBackResist = 0;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.value = 200;

            NPC.aiStyle = 0;

            Banner = NPC.type;
            BannerItem = ModContent.ItemType<GrabberPlantBanner>();
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
			float multiplier = ServerSidedToggles.Instance.NPCSpawnMultiplier == 1f 
                ? ServerSidedToggles.Instance.GrabberPlantSpawnMultiplier : ServerSidedToggles.Instance.NPCSpawnMultiplier;

			// Surface
			if (spawnInfo.Player.ZoneJungle && !spawnInfo.Player.ZoneRockLayerHeight && !spawnInfo.Water)
            {
                if (!Main.hardMode)
                {
                    return SpawnCondition.SurfaceJungle.Chance * multiplier;
                }
                else
                {
                    return SpawnCondition.SurfaceJungle.Chance * 0.4f * multiplier;
                }
            }
            // Underground
            else if (spawnInfo.Player.ZoneJungle && spawnInfo.Player.ZoneRockLayerHeight && !spawnInfo.Water)
            {
                if (!Main.hardMode)
                {
                    return SpawnCondition.UndergroundJungle.Chance * multiplier;
                }
                else
                {
                    return SpawnCondition.UndergroundJungle.Chance * 0.4f * multiplier;
                }
            }

            return 0f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<GrabberFlower>(), 15, 1));
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            // Create gore when the NPC is killed.
            if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, Vector2.Zero, Mod.Find<ModGore>("GrabberPlantGore1").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, Vector2.Zero, Mod.Find<ModGore>("GrabberPlantGore2").Type, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, Vector2.Zero, Mod.Find<ModGore>("GrabberPlantGore3").Type, 1f);
            }
        }

        float State
        {
            get => NPC.ai[0];
            set => NPC.ai[0] = value;
        }
        private Player trappedPlayer { get; set; }
        public override void AI()
        {
            // When in idle
            if (State == 0)
            {
                return; // Does nothing
            }
            else
            {
                // When player is grabbed they can move very slowly still
                // and escape using teleportation items
                if (trappedPlayer.Hitbox.Intersects(NPC.Hitbox) && !trappedPlayer.dead)
                {
                    trappedPlayer.velocity = Vector2.Zero;
                }
                else // If player dies/escapes, reset state
                {
                    State = 0;
                }
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            // Change state to 1
            State = 1;
            trappedPlayer = target;

            // Grab the player
            if (target.Center != NPC.Center)
            {
                target.Center = NPC.Center;
            }

            if (!target.HasBuff(BuffID.Poisoned))
            {
                target.AddBuff(BuffID.Poisoned, 300);
            }
        }

        private int frame = 0;
        public override void FindFrame(int frameHeight)
        {
            // When in idle
            if (State == 0)
            {
                // Loop through the first four frames
                if (NPC.frameCounter % 20 == 0)
                {
                    frame++;
                    if (frame > 3)
                    {
                        frame = 0;
                    }
                }
                NPC.frameCounter++;
            }
            else // Otherwise play the last three and stay on the last one
            {
                // Ensure that the grab will begin at frame 4 below by setting it to three first
                if (frame < 3)
                {
                    frame = 3;
                }

                // Loop through the last frames until until 2nd last is reached
                // After that loop the two last frames
                if (NPC.frameCounter % 10 == 0)
                {
                    if (frame < 7)
                    {
                        frame++;
                    }
                    else if (frame == 7)
                    {
                        frame--;
                    }
                }
                NPC.frameCounter++;
            }

            // Set the frame
            NPC.frame.Y = frameHeight * frame;
        }

        // Draw the NPC over the player
        public override void DrawBehind(int index)
        {
            Main.instance.DrawCacheNPCsOverPlayers.Add(index);
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				
                // Sets the spawning conditions of this NPC that is listed in the bestiary.
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Jungle,
				
                // Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("This poisonous plant will grab any traveller who touches it. Keep your distance.")
            });
        }
    }
}
