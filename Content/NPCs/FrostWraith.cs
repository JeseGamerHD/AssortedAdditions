using ModdingTutorial.Content.Items.Misc;
using ModdingTutorial.Content.Items.Placeables.Banners;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace ModdingTutorial.Content.NPCs
{
    internal class FrostWraith : ModNPC
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
            NPC.width = 30;
            NPC.height = 50;
            NPC.damage = 70;
            NPC.defense = 16;
            NPC.lifeMax = 180;
            NPC.knockBackResist = 0.7f; // 30% resist

            NPC.HitSound = SoundID.NPCHit54; // TODO custom sounds?
            NPC.DeathSound = SoundID.NPCDeath52;
            NPC.value = Item.sellPrice(silver: 1);

            NPC.noTileCollide = true;
            NPC.noGravity = true;

            // AI and animation copy the Wraith NPC
            AnimationType = NPCID.Wraith;
            NPC.aiStyle = 22;
            AIType = NPCID.Wraith;

            Banner = NPC.type;
            BannerItem = ModContent.ItemType<FrostWraithBanner>();
        }

        // Tried making a flying AI and it kinda worked
        // The problem was that it floated up and down too much
        // Also sprite direction flipping when being close to the player
/*        public override void AI()
        {
            NPC.TargetClosest();
            Vector2 targetPos = Main.player[NPC.target].position;

            // IF the target is to the left and max velocity hasn't been reached
            if (targetPos.X < NPC.position.X && NPC.velocity.X > -8)
            {
                NPC.velocity.X -= 0.22f; // accelerate to the left
            }

            // IF the target is to the right and max velocity hasn't been reached
            if (targetPos.X > NPC.position.X && NPC.velocity.X < 8)
            {
                NPC.velocity.X += 0.22f; // accelerate to the right
            }

            // IF the target is higher
            // (Y grows when going down in the world)
            if (targetPos.Y < NPC.position.Y)
            {
                // If already moving up and not at max up velocity
                if(NPC.velocity.Y < 0 && NPC.velocity.Y > -1)
                {
                    NPC.velocity.Y -= 0.022f; // accelerate up
                }
                else // Otherwise begin moving up at a faster rate
                {
                    NPC.velocity.Y -= 0.03f;
                }
            }

            // IF the target is lower
            if (targetPos.Y > NPC.position.Y)
            {
                if(NPC.velocity.Y > 0 && NPC.velocity.Y < 1)
                {
                    NPC.velocity.Y += 0.022f;
                }
                else
                {
                    NPC.velocity.Y += 0.03f;
                }
            }
        }*/

        // Add some lighting and dust to the NPC
        public override void PostAI()
        {
            Lighting.AddLight(NPC.Center, TorchID.Ice);

            if (Main.rand.NextBool(3))
            {
                Dust dust = Dust.NewDustDirect(NPC.position - NPC.velocity, NPC.width, NPC.height,
                    DustID.IcyMerman, 0, 0, 150, default, 1.5f);
                dust.noGravity = true;
            }
        }

        // Should only spawn after one of the mech bosses have been defeated
        // and only in the underground ice biome
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.downedMechBossAny && spawnInfo.Player.ZoneSnow && spawnInfo.Player.ZoneRockLayerHeight)
            {
                return SpawnCondition.Cavern.Chance * 0.1f;
            }

            return 0f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.Common(ItemID.FastClock, 20, 1, 1));
            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<IceEssence>(), 10, 1, 1));
        }

        // Spawns cloud gores upon death
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 11, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 12, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, 13, 1f);
            }
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo hurtInfo)
        {
            // 10% chance to get chilled
            if (Main.rand.NextBool(10))
            {
                target.AddBuff(BuffID.Chilled, 360);
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				
                // Sets the spawning conditions of this NPC that is listed in the bestiary.
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundSnow,
				
                // Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("Soul lost to the icy depths unable to move on.")
            });
        }
    }
}
