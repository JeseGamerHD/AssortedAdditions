using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using ModdingTutorial.Content.Items.Weapons.Ranged;
using ModdingTutorial.Content.Items.Accessories;

namespace ModdingTutorial.Common.Systems
{
    // This class is used for adding items to existing chests' loot pools
    internal class ModifiedChestLoot : ModSystem
    {

        public override void PostWorldGen()
        {

            int chestWidth = 36; // 36 is the width of the chest tiles
     
            // Chests can be found within tiles_21 (TileID.Containers) and tiles_467 (TileID.Containers2) spritesheets
            // Below are ID's for different chests, counting begins from zero:
            
            // Containers
            int ChestID = 0;

            // Containers2
            int SandstoneChestID = 10;
            

            for (int chestIndex = 0; chestIndex < 8000; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];

                // Regular chests
                if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == ChestID * chestWidth)
                {
                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {
                            // 20% chance to be in a chest
                            if (Main.rand.NextFloat() <= 0.2f)
                            {
                                chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<MinersRing>());
                            }

                            break;
                        }
                    }
                }

                // Sandstone chests
                if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers2 && Main.tile[chest.x, chest.y].TileFrameX == SandstoneChestID * chestWidth)
                {
                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {
                            // 33% chance to be in a chest
                            if(Main.rand.NextFloat() <= 0.3333f)
                            {
                                chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<Dunerang>());
                            }

                            break;
                        }
                    }
                }

                // ...
            }
        }

    }
}
