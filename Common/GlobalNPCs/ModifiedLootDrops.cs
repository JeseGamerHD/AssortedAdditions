using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using ModdingTutorial.Content.Items.Placeables.Ores;
using ModdingTutorial.Content.Items.Misc;
using ModdingTutorial.Content.Items.Weapons.Magic;
using ModdingTutorial.Content.Items.Weapons.Ranged;
using ModdingTutorial.Content.Items.Weapons.Melee;
using ModdingTutorial.Content.Items.Weapons.Summon;
using ModdingTutorial.Content.Items.Accessories;

namespace ModdingTutorial.Common.GlobalNPCs
{
    // This class is for adding loot drops to existing NPCs
    internal class ModifiedLootDrops : GlobalNPC
    {
        public override void ModifyNPCLoot(NPC npc, NPCLoot npcLoot)
        {
            /* BOSSES HERE: */
            /* -------------------------------------------------------------------------------------------------- */
            if(npc.type == NPCID.CultistBoss)
            {
                npcLoot.Add(ItemDropRule.BossBag(ItemID.CultistBossBag)); // The cultist boss now drops its treasure bag since this mod adds some items to it

                // These would drop from the treasure bag on expert mode
                LeadingConditionRule notExpert = new LeadingConditionRule(new Conditions.NotExpert()); // New condition for when its not expert mode
                
                notExpert.OnSuccess(ItemDropRule.OneFromOptions(1, ModContent.ItemType<DeathRay>(), // Drops one of these
                    ModContent.ItemType<ScifiBlaster>(),
                    ModContent.ItemType<CultClassic>(),
                    ModContent.ItemType<Motivator>())); 
                npcLoot.Add(notExpert);

                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EchoChamber>(), 2, 1, 1));
            }

            /* REGULAR ENEMIES UNDER HERE: */
            /* -------------------------------------------------------------------------------------------------- */
            
            // Demon eyes (only positive ids since the negatives are variants of the positives)
            if(npc.type == NPCID.DemonEye || npc.type == NPCID.CataractEye || npc.type == NPCID.SleepyEye
                || npc.type == NPCID.DialatedEye || npc.type == NPCID.GreenEye || npc.type == NPCID.PurpleEye)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<EyeBow>(), 40, 1, 1)); // 2.5% chance to drop
            }
            
            if (npc.type == NPCID.TombCrawlerHead)
            {
                // 33% chance to drop 4-7 dune ores after crimson/corruption boss has been defeated
                npcLoot.Add(ItemDropRule.ByCondition(Condition.DownedEowOrBoc.ToDropCondition(ShowItemDropInUI.Always),
                    ModContent.ItemType<DuneOre>(), 3, 4, 7));
            }

            if (npc.type == NPCID.Antlion)
            {
                // ~15% chance to drop 2-4 dune ores after crimson/corruption boss has been defeated
                npcLoot.Add(ItemDropRule.ByCondition(Condition.DownedEowOrBoc.ToDropCondition(ShowItemDropInUI.Always),
                    ModContent.ItemType<DuneOre>(), 7, 2, 4));
            }

            // Ice biome enemies have a 10% chance to drop Ice Essence after one of the mech bosses have been defeated
            // Alternatively could have checked if the player is in the ice biome zone and make all enemies drop the essence
            if(npc.type == NPCID.IceBat || npc.type == NPCID.SnowFlinx ||  npc.type == NPCID.UndeadViking || npc.type == NPCID.CyanBeetle || npc.type == NPCID.SpikedIceSlime
                || npc.type == NPCID.ArmoredViking || npc.type == NPCID.IceTortoise || npc.type == NPCID.IceElemental || npc.type == NPCID.IcyMerman || npc.type == NPCID.IceMimic)
            {
                npcLoot.Add(ItemDropRule.ByCondition(Condition.DownedMechBossAny.ToDropCondition(ShowItemDropInUI.Always), 
                    ModContent.ItemType<IceEssence>(), 10, 1, 1));
            }

            // Add armor polish to other armored enemies as well
            // (not to rusty armored bones since its armor is rusty and having it drops makes less sense)
            if(npc.type == NPCID.ArmoredViking || npc.type == NPCID.HellArmoredBones)
            {
                npcLoot.Add(ItemDropRule.Common(ItemID.ArmorPolish, 40, 1, 1));
            }

            if(npc.type == NPCID.Shark)
            {
                // 25% chance to drop 1-3 Shark Tooth
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<SharkTooth>(), 4, 1, 3));
            }

            if(npc.type == NPCID.ThePossessed || npc.type == NPCID.Fritz)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<Adrenaline>(), 40, 1, 1)); // 2.5% chance to drop
            }

            // Add vitamins to more enemy loot pools so its less rare
            if(npc.type == NPCID.Herpling || npc.type == NPCID.IchorSticker || npc.type == NPCID.Clinger || npc.type == NPCID.SeekerHead)
            {
                npcLoot.Add(ItemDropRule.Common(ItemID.Vitamins, 40, 1, 1)); // 2.5% chance to drop
            }

            // Bats drop antidote during hardmode
            if(npc.type == NPCID.CaveBat || npc.type == NPCID.JungleBat || npc.type == NPCID.GiantBat || npc.type == NPCID.SporeBat || npc.type == NPCID.GiantFlyingFox)
            {
                npcLoot.Add(ItemDropRule.ByCondition(Condition.Hardmode.ToDropCondition(ShowItemDropInUI.Always), ModContent.ItemType<Antidote>(), 40, 1)); // 2.5 % chance
            }

            if(npc.type == NPCID.Dandelion)
            {
                npcLoot.Add(ItemDropRule.Common(ModContent.ItemType<DandelionFlower>(), 3, 1, 1)); // 33% chance
            }
        }
    }
}
