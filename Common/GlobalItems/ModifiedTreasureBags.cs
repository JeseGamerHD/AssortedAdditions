using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using AssortedAdditions.Content.Items.Consumables;
using AssortedAdditions.Content.Items.Weapons.Melee;
using AssortedAdditions.Content.Items.Weapons.Ranged;
using AssortedAdditions.Content.Items.Weapons.Magic;
using AssortedAdditions.Content.Items.Weapons.Summon;
using AssortedAdditions.Content.Items.Accessories;

namespace AssortedAdditions.Common.GlobalItems
{
    // Class is (mostly) used for modifying existing treasure bag loot
    internal class ModifiedTreasureBags : GlobalItem
    {
        // These items also drop from the bosses when not in expert. This is set in ModifiedLootDrops.cs
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if (item.type == ItemID.CultistBossBag)
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
            if (entity.type == ItemID.CultistBossBag)
            {
                entity.ResearchUnlockCount = 3;
            }
        }
    }
}
