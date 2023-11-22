using AssortedAdditions.Content.Items.Armor;
using AssortedAdditions.Content.Items.Placeables.Banners;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace AssortedAdditions.Content.NPCs
{
    internal class Shroomling : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 15;
        }

        public override void SetDefaults()
        {
            NPC.width = 33;
            NPC.height = 36;
            NPC.damage = 20;
            NPC.defense = 4;
            NPC.lifeMax = 45;
            NPC.knockBackResist = 0.5f;
            NPC.HitSound = new SoundStyle("AssortedAdditions/Assets/Sounds/NPCSound/ShroomlingHit");
            NPC.DeathSound = new SoundStyle("AssortedAdditions/Assets/Sounds/NPCSound/ShroomlingDeath");
            NPC.value = 80;

            NPC.aiStyle = 0;

            Banner = NPC.type; // Each enemy has their own banner
            BannerItem = ModContent.ItemType<ShroomlingBanner>();
        }

        Player target;

        public override bool PreAI()
        {
            // Enemy starts off in idle position
            // then switches to fighter ai
            if (NPC.aiStyle == 0)
            {
                // Find a target
                if (!NPC.HasValidTarget)
                {
                    NPC.TargetClosest();
                    target = Main.player[NPC.target];
                }

                // Check if the target is close enough or the NPC gets hit
                // Then switch aiStyle to beging attacking
                if (target.Distance(NPC.Center) < 150f || NPC.velocity != Vector2.Zero)
                {
                    // It also makes a noise when disturbed
                    SoundStyle shroomlingAngry = new SoundStyle("AssortedAdditions/Assets/Sounds/NPCSound/ShroomlingAngry");
                    SoundEngine.PlaySound(shroomlingAngry, NPC.position);

                    NPC.aiStyle = 38; // Snowman AI, works well enough
                }
                return false; // If aiStyle stays at 0, keep idling
            }

            return true; // If aiStyle is 38, start using the aiStyle
        }

        // Animation happens here:
        private int frame = 0;
        public override void FindFrame(int frameHeight)
        {
            // When target gets close ai switches to fighter ai
            // Frame won't change from 0 if aiStyle is still 0 (idle)
            if (NPC.aiStyle == 38)
            {
                // Switch frames
                if (NPC.frameCounter % 3 == 0)
                {
                    frame++;
                }
                NPC.frameCounter++;

                // If final frame was reached, reset to frame 1 (0 is only for idle)
                if (frame > 14)
                {
                    frame = 1;
                }
            }

            // Set the frame whether its the idle one or one of the walking frames
            NPC.frame.Y = frameHeight * frame;
            NPC.spriteDirection = NPC.direction;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            // Only spawns in the forest biome during the day
            if (spawnInfo.Player.ZonePurity)
            {
                return SpawnCondition.OverworldDaySlime.Chance * 0.15f;
            }

            return 0f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.Mushroom, 1, 1, 5));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<ShroomHat>(), 10, 1, 1));
        }

        public override void HitEffect(NPC.HitInfo hit)
        {
            // Create gore when the NPC is killed.
            if (Main.netMode != NetmodeID.Server && NPC.life <= 0)
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, Mod.Find<ModGore>("ShroomlingGore").Type, 1f);
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				
                // Sets the spawning conditions of this NPC that is listed in the bestiary.
				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Times.DayTime,
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,
                // Also sets the background image ^^
				
                // Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("These mushroom creatures are quite territorial and get jumpy at tresspassers")
            });
        }
    }
}
