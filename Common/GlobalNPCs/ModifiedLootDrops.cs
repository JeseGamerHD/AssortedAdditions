using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using ModdingTutorial.Common.ItemDropRules;
using ModdingTutorial.Content.Items.Consumables;
using ModdingTutorial.Content.Items.Placeables.Ores;
using ModdingTutorial.Content.Items.Misc;
using ModdingTutorial.Content.Items.Weapons.Magic;
using ModdingTutorial.Content.Items.Weapons.Ranged;

namespace ModdingTutorial.Common.GlobalNPCs
{
    // This class is for adding loot drops to existing NPCs
    internal class ModifiedLootDrops : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            /* DROP CONDITIONS HERE: */
            /* -------------------------------------------------------------------------------------------------- */
            LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert());
            LeadingConditionRule WormOrBrainDefeated = new LeadingConditionRule(new WormOrBrainDefeated());
            LeadingConditionRule AnyMechBossDefeated = new LeadingConditionRule(new AnyMechBossDefeated());

            /* BOSSES HERE: */
            /* -------------------------------------------------------------------------------------------------- */
            if (npc.type == NPCID.WallofFlesh)
            {
                // Drops if playing on normal mode / Journey mode otherwise obtained from treasure bag
                notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<AncientToken>(), 1, 1));
                npcLoot.Add(notExpert);
            }

            if(npc.type == NPCID.CultistBoss)
            {
                npcLoot.Add(ItemDropRule.Common(ItemID.CultistBossBag, 1)); // The cultist boss now drops its treasure bag since this mod adds some items to it

                // These would drop from the treasure bag on expert mode
                notExpert.OnSuccess(ItemDropRule.OneFromOptions(1, ModContent.ItemType<DeathRay>(),
                    ModContent.ItemType<ScifiBlaster>())); 
                npcLoot.Add(notExpert);
            }

            /* REGULAR ENEMIES UNDER HERE: */
            /* -------------------------------------------------------------------------------------------------- */
            if (npc.type == NPCID.TombCrawlerHead)
            {
                // 33% chance to drop 4-7 dune ores after crimson/corruption boss has been defeated
                WormOrBrainDefeated.OnSuccess(ItemDropRule.Common(ModContent.ItemType<DuneOre>(), 3, 4, 7));
                npcLoot.Add(WormOrBrainDefeated);
            }

            if (npc.type == NPCID.Antlion)
            {
                // ~15% chance to drop 2-4 dune ores after crimson/corruption boss has been defeated
                WormOrBrainDefeated.OnSuccess(ItemDropRule.Common(ModContent.ItemType<DuneOre>(), 7, 2, 4));
                npcLoot.Add(WormOrBrainDefeated);
            }

            // Ice biome enemies have a 10% chance to drop Ice Essence after one of the mech bosses have been defeated
            // Alternatively could have checked if the player is in the ice biome zone and make all enemies drop the essence
            if(npc.type == NPCID.IceBat || npc.type == NPCID.SnowFlinx ||  npc.type == NPCID.UndeadViking || npc.type == NPCID.CyanBeetle || npc.type == NPCID.SpikedIceSlime
                || npc.type == NPCID.ArmoredViking || npc.type == NPCID.IceTortoise || npc.type == NPCID.IceElemental || npc.type == NPCID.IcyMerman || npc.type == NPCID.IceMimic)
            {
                AnyMechBossDefeated.OnSuccess(ItemDropRule.Common(ModContent.ItemType<IceEssence>(), 10, 1, 1));
                npcLoot.Add(AnyMechBossDefeated);
            }
        }
    }
}
