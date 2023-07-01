using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using ModdingTutorial.Content.Items.Placeables;
using ModdingTutorial.Common.ItemDropRules;
using ModdingTutorial.Content.Items.Consumables;

namespace ModdingTutorial.Common.GlobalNPCs
{
    // This class is for adding loot drops to existing NPCs
    internal class ModifiedLootDrops : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            // BOSSES HERE:
            if(npc.type == NPCID.WallofFlesh)
            {
                // Drops if playing on normal mode / Journey mode otherwise obtained from treasure bag
                LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert());
                notExpert.OnSuccess(ItemDropRule.Common(ModContent.ItemType<AncientToken>(), 1, 1));
                npcLoot.Add(notExpert);
            }


            // REGULAR ENEMIES UNDER HERE:
            if(npc.type == NPCID.TombCrawlerHead)
            {
                // 33% chance to drop 4-7 dune ores aftercrimson/corruption boss has been defeated
                npcLoot.Add(new ItemDropWithConditionRule(ModContent.ItemType<DuneOre>(), 3, 4, 7, new WormOrBrainDefeated(), 1));
            }

            if (npc.type == NPCID.Antlion)
            {
                // ~15% chance to drop 2-4 dune ores after crimson/corruption boss has been defeated
                npcLoot.Add(new ItemDropWithConditionRule(ModContent.ItemType<DuneOre>(), 7, 2, 4, new WormOrBrainDefeated(), 1));
            }

        }
    }
}
