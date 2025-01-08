using AssortedAdditions.Content.Items.Accessories;
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
    internal class JungleMimic : ModNPC
    {
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 14;

            // Immune to debuffs:
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.OnFire3] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Poisoned] = true;
            NPCID.Sets.SpecificDebuffImmunity[Type][BuffID.Confused] = true;
        }

        public override void SetDefaults()
        {
            NPC.CloneDefaults(NPCID.BigMimicJungle);
            AnimationType = NPCID.BigMimicJungle;
            AIType = NPCID.BigMimicJungle;
        }

        public override void ModifyNPCLoot(NPCLoot npcLoot)
        {
            npcLoot.Add(ItemDropRule.OneFromOptions(1,
                ModContent.ItemType<PetalBlade>(),
                ModContent.ItemType<SporeRod>(),
                ModContent.ItemType<JungleChakram>(),
                ModContent.ItemType<BlossomLash>()));

            npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<MimicsTongue>(), 3)); // 33% chance
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (NPC.downedMechBossAny && spawnInfo.Player.ZoneJungle && spawnInfo.Player.ZoneRockLayerHeight)
            {
                return SpawnCondition.UndergroundMimic.Chance;
            }
            else if (NPC.downedMechBossAny && spawnInfo.Player.ZoneJungle)
            {
                if(ModLoader.TryGetMod("UltimateSkyblock", out Mod UltimateSkyblock))
                {
                    return 0.015f;
                }
            }

            return 0f;
        }

        public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry)
        {
            // We can use AddRange instead of calling Add multiple times in order to add multiple items at once
            bestiaryEntry.Info.AddRange(new IBestiaryInfoElement[] {
				
                // Sets the spawning conditions of this NPC that is listed in the bestiary.
                BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.UndergroundJungle,
                // Also sets the background image ^^
				
                // Sets the description of this NPC that is listed in the bestiary.
				new FlavorTextBestiaryInfoElement("Mimics overgrown by spores and vines become even more dangerous")
            });
        }
    }
}
