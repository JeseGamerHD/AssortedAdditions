using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Common.Players
{
    // Used for giving player the buff from items that give a buff inside the inventory
    // when loading back into a world
    internal class SaveInventoryBuffs : ModPlayer
    {
        public override void PreUpdateBuffs()
        {
            if (Player.HasItemInAnyInventory(ItemID.SharpeningStation))
            {
                Player.AddBuff(BuffID.Sharpened, 2);
            }

            if (Player.HasItemInAnyInventory(ItemID.BewitchingTable))
            {
                Player.AddBuff(BuffID.Bewitched, 2);
            }

            if (Player.HasItemInAnyInventory(ItemID.AmmoBox))
            {
                Player.AddBuff(BuffID.AmmoBox, 2);
            }

            if (Player.HasItemInAnyInventory(ItemID.CrystalBall))
            {
                Player.AddBuff(BuffID.Clairvoyance, 2);
            }
        }
    }
}
