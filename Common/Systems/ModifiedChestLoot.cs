using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using AssortedAdditions.Content.Items.Weapons.Ranged;
using AssortedAdditions.Content.Items.Weapons.Magic;
using AssortedAdditions.Content.Items.Accessories;

namespace AssortedAdditions.Common.Systems
{
    // This class is used for adding items to existing chests' loot pools
    internal class ModifiedChestLoot : ModSystem
    {

        public override void PostWorldGen()
        {

            int chestWidth = 36; // 36 is the width of the chest tiles

            // Chests can be found within tiles_21 (TileID.Containers) and tiles_467 (TileID.Containers2) spritesheets
            // Below are some ID's for different chests, counting begins from zero:

            // Containers
            int chestID = 0;
            int goldChestID = 1;
            int skywareChestID = 13;

            // Containers2
            int SandstoneChestID = 10;


            for (int chestIndex = 0; chestIndex < 8000; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];

                // Chests
                if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == chestID * chestWidth)
                {
                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        // If the spot is empty (no item)
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {
                            // 20% chance to be in a chest
                            if (Main.rand.NextFloat() <= 0.2f)
                            {
                                chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<MinersRing>());
                            }

                            break; // Break to prevent adding the item to other empty slots
                        }
                    }
                }

                // Golden chests
                if (chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == goldChestID * chestWidth)
                {
                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {
                            if (Main.rand.NextFloat() <= 0.2f)
                            {
                                chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<StoneWand>());
                            }

                            break;
                        }
                    }
                }

                // Skyware chests
                if(chest != null && Main.tile[chest.x, chest.y].TileType == TileID.Containers && Main.tile[chest.x, chest.y].TileFrameX == skywareChestID * chestWidth)
                {
                    for(int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {
                            if(Main.rand.NextFloat() <= 0.33f)
                            {
                                chest.item[inventoryIndex].SetDefaults(ModContent.ItemType<HangGlider>());
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
                            if (Main.rand.NextFloat() <= 0.3333f)
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
