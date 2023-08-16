using ModdingTutorial.Content.Items.Consumables;
using ModdingTutorial.Content.Items.Misc;
using ModdingTutorial.Content.Items.Weapons.Ammo;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Common.GlobalNPCs
{
    internal class ModifiedShops : GlobalNPC
    {
        public override void ModifyShop(NPCShop shop)
        {
            if(shop.NpcType == NPCID.Cyborg)
            {
                // Add item with custom price
                shop.Add(new Item(ModContent.ItemType<GuidedMissile>())
                {
                    shopCustomPrice = 100,
                });

                // Add just item (its value determines price)
                shop.Add<ControlChip>();
                shop.Add<Battery>();
            }

            if(shop.NpcType == NPCID.Dryad)
            {
                shop.Add(ItemID.FlowerBoots, condition: Condition.DownedSkeletron); // Sells the item after skeletron has been defeated
            }

            if(shop.NpcType == NPCID.PartyGirl)
            {
                shop.Add(ItemID.ShinyRedBalloon);
            }

            if(shop.NpcType == NPCID.Wizard)
            {
                shop.Add(ModContent.ItemType<AncientToken>());
            }
        }
    }
}
