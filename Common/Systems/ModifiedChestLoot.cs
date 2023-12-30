using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using AssortedAdditions.Content.Items.Weapons.Ranged;
using AssortedAdditions.Content.Items.Weapons.Magic;
using AssortedAdditions.Content.Items.Accessories;
using AssortedAdditions.Helpers;
using AssortedAdditions.Content.Items.Accessories.Runes;

namespace AssortedAdditions.Common.Systems
{
    // This class is used for adding items to existing chests' loot pools
    internal class ModifiedChestLoot : ModSystem
    {
        public override void PostWorldGen()
        {
            int chestWidth = 36; // 36 is the width of the chest tiles
            // Chests can be found within tiles_21 (TileID.Containers) and tiles_467 (TileID.Containers2) spritesheets
            // See TileStyleID class inside the Helpers folder for the correct IDs

            for (int chestIndex = 0; chestIndex < 8000; chestIndex++)
            {
                Chest chest = Main.chest[chestIndex];
                if(chest == null)
                {
                    continue; // Skip null chests, straight to next index
                }

                Tile chestTile = Main.tile[chest.x, chest.y];

				// Chests
				if (chestTile.TileType == TileID.Containers && chestTile.TileFrameX == TileStyleID.Containers.Chest * chestWidth)
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
                if (chestTile.TileType == TileID.Containers && chestTile.TileFrameX == TileStyleID.Containers.GoldenChest * chestWidth)
                {
                    int [] lootPool = { ModContent.ItemType<StoneWand>(), ModContent.ItemType<RuneOfSpelunking>() };

                    for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
                    {
                        if (chest.item[inventoryIndex].type == ItemID.None)
                        {
							if (Main.rand.NextFloat() <= 0.51)
                            {
                                int lootPoolIndex = Main.rand.Next(0, lootPool.Length);
								chest.item[inventoryIndex].SetDefaults(lootPool[lootPoolIndex]);
                            }

                            break;
                        }
                    }
                }

                // Skyware chests
                if(chestTile.TileType == TileID.Containers && chestTile.TileFrameX == TileStyleID.Containers.SkywareChest * chestWidth)
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
                if (chestTile.TileType == TileID.Containers2 && chestTile.TileFrameX == TileStyleID.Containers2.SandstoneChest * chestWidth)
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
