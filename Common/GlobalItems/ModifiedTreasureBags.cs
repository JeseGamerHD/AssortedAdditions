using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.ItemDropRules;
using ModdingTutorial.Content.Items.Consumables;

namespace ModdingTutorial.Common.GlobalItems
{
    // Class is used for modifying existing treasure bag loot
    internal class ModifiedTreasureBags : GlobalItem
    {
        public override void ModifyItemLoot(Item item, ItemLoot itemLoot)
        {
            if(item.type == ItemID.WallOfFleshBossBag)
            {
                // This also drops if the player is on normal/journey, but directly from the boss (Added in GlobalNPCS -> ModifiedLootDrops.cs)
                itemLoot.Add(ItemDropRule.Common(ModContent.ItemType<AncientToken>(), 1, 1));
            }
        }
    }
}
