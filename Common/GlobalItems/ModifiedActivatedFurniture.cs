using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Common.GlobalItems
{
    // This class is used for modifying furniture items that grant a buff
    // E.g Sharpening Station, Ammo Box...
    // These items can still be used normally as well
    internal class ModifiedActivatedFurniture : GlobalItem
    {
        // Make these items give the buff when the item is in player's inventory
        public override void UpdateInventory(Item item, Player player)
        {
            if(item.type == ItemID.AmmoBox)
            {
                player.AddBuff(BuffID.AmmoBox, 2);
            }

            if(item.type == ItemID.CrystalBall)
            {
                player.AddBuff(BuffID.Clairvoyance, 2);
            }

            if(item.type == ItemID.SharpeningStation)
            {
                player.AddBuff(BuffID.Sharpened, 2);
            }

            if (item.type == ItemID.BewitchingTable)
            {
                player.AddBuff(BuffID.Bewitched, 2);
            }
        }

        // Add a tooltip to tell the player about this change
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.AmmoBox)
            {
                tooltips.Add(new(Mod, "Tooltip0", "[c/FFF014:Gives the Ammo Box buff while in any personal inventory]"));
            }

            if (item.type == ItemID.CrystalBall)
            {
                tooltips.Add(new(Mod, "Tooltip0", "[c/FFF014:Gives the Clairvoyance buff while in any personal inventory]"));
            }

            if (item.type == ItemID.SharpeningStation)
            {
                tooltips.Add(new(Mod, "Tooltip0", "[c/FFF014:Gives the Sharpened buff while in any personal inventory]"));
            }

            if (item.type == ItemID.BewitchingTable)
            {
                tooltips.Add(new(Mod, "Tooltip0", "[c/FFF014:Gives the Bewitched buff while in any personal inventory]"));
            }
        }
    }
}
