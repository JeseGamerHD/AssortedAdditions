using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using ModdingTutorial.Content.Items.Weapons.Ranged;

namespace ModdingTutorial.Common.Systems
{
    internal class ModifiedChestLoot : ModSystem
    { // This class is used for adding items to existing chest loot pools

        public override void PostWorldGen()
        {
            for (int chestIndex = 0; chestIndex < 8000; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];
                int chestID = 10; //Sandstone Chest, actual SubID is one lower since counting begins from 0
                int chestWidth = 36; // 36 is the width of the tile

                if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers2 && Main.tile[chest.x, chest.y].TileFrameX == chestID * chestWidth)
                {
                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {
                            // 33% chance to be in the chest
                            if(Main.rand.NextFloat() <= 0.3333f)
                            {
                                chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Dunerang>());
                            }

                            break;
                        }
                    }
                }
            }

            // Additional chests here
        }

    }
}
