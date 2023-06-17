using ModdingTutorial.Content.Items.Misc;
using ModdingTutorial.Content.Items.Weapons.Ammo;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Common.GlobalNPCs
{
    internal class ModifiedShops : GlobalNPC
    {
        public override void SetupShop(int type, Chest shop, ref int nextSlot)
        {
            if (type == NPCID.Cyborg)
            {
                shop.item[nextSlot].SetDefaults(ModContent.ItemType<GuidedMissile>());
                shop.item[nextSlot].value = 100;
                nextSlot++;

                shop.item[nextSlot].SetDefaults(ModContent.ItemType<ControlChip>());
                nextSlot++;

                shop.item[nextSlot].SetDefaults(ModContent.ItemType<Battery>());
                nextSlot++;
            }
        }
    }
}
