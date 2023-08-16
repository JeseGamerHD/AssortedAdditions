﻿using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using ModdingTutorial.Content.Items.Consumables;
using ModdingTutorial.Content.Items.Weapons.Magic;
using ModdingTutorial.Content.Items.Weapons.Ranged;
using ModdingTutorial.Content.Items.Weapons.Melee;
using ModdingTutorial.Content.Items.Weapons.Summon;
using ModdingTutorial.Content.Items.Accessories;

namespace ModdingTutorial.Common.GlobalItems
{
    // Class is (mostly) used for modifying existing treasure bag loot
    internal class ModifiedTreasureBags : GlobalItem
    {
        // These items also drop from the bosses when not in expert. This is set in ModifiedLootDrops.cs
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if(item.type == ItemID.CultistBossBag)
            {
                // Always drop one of these:
                itemLoot.Add(ItemDropRule.OneFromOptions(1, ModContent.ItemType<DeathRay>(),
                    ModContent.ItemType<ScifiBlaster>(),
                    ModContent.ItemType<CultClassic>(),
                    ModContent.ItemType<Motivator>()
                    ));

                // Also a 25% chance to get an accessory
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<EchoChamber>(), 4, 1, 1));
            }
        }

        public override void SetDefaults(Item entity)
        {
            // Since cultist bag is normally not obtainable, journey mode research count needs to be added
            if(entity.type == ItemID.CultistBossBag)
            {
                entity.ResearchUnlockCount = 3;
            }  
        }
    }
}
