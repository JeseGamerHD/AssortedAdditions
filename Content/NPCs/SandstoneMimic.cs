﻿using AssortedAdditions.Common.Configs;
using AssortedAdditions.Content.Items.Weapons.Magic;
using AssortedAdditions.Content.Items.Weapons.Melee;
using AssortedAdditions.Content.Items.Weapons.Ranged;
using AssortedAdditions.Content.Items.Weapons.Summon;
using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.ItemDropRules;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.Utilities;

namespace AssortedAdditions.Content.NPCs
{
    internal class SandstoneMimic : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 6; // Sprite has 6 frames

            // Immune to debuffs:
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Frostburn2] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
        }

        public override void SetDefaults()
        {
            NPC.width = 32;
            NPC.height = 46;
            NPC.damage = 80;
            NPC.defense = 30;
            NPC.lifeMax = 500;
            NPC.knockBackResist = 0.3f; // 70 resist
            NPC.HitSound = SoundID.NPCHit4;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.value = 10000; // 10 gold

            AnimationType = NPCID.IceMimic;
            NPC.aiStyle = 25; // Copy behaviour and animation from Mimics
            AIType = NPCID.IceMimic;

            Banner = NPC.type;
            BannerItem = ItemID.MimicBanner;
        }

        // Should only spawn during hardmode and in the underground desert
        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            float baseChance = 0.05f;
			float multiplier = ServerSidedToggles.Instance.NPCSpawnMultiplier == 1f
                ? ServerSidedToggles.Instance.SandstoneMimicSpawnMultiplier : ServerSidedToggles.Instance.NPCSpawnMultiplier;
			if (Main.hardMode && spawnInfo.Player.ZoneUndergroundDesert)
            {
				return SpawnCondition.DesertCave.Chance * baseChance * multiplier;
            }
            else if(Main.hardMode && spawnInfo.Player.ZoneDesert)
            {
                if(ModLoader.TryGetMod("UltimateSkyblock", out Mod UltimateSkyblock))
                {
                    return baseChance * multiplier;
                }
            }

            return 0f;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.OneFromOptions(1,
                        ModContent.ItemType<DesertsFury>(),
                        ModContent.ItemType<DustbringerStaff>(),
                        ModContent.ItemType<DesertBlaster>(),
                        ModContent.ItemType<SunScepter>()
                        ));
        }

        // Spawns cloud gores upon death
        public override void HitEffect(NPC.HitInfo hit)
        {
            if (NPC.life <= 0)
            {
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, GoreID.Smoke1, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, GoreID.Smoke2, 1f);
                Gore.NewGore(NPC.GetSource_Death(), NPC.position, NPC.velocity, GoreID.Smoke3, 1f);
            }
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				
                // Sets the spawning conditions of this NPC that is listed in the bestiary.
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundDesert,
				
                // Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("Talking to a chest doesn't cause craziness, but if the chest answers back, it may cause death! It still contains rare treasure, regardless!")
            });
        }
    }
}
