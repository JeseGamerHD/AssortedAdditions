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
        }
    }
}
