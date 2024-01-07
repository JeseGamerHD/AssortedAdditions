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
            const int chestWidth = 36; // 36 is the width of the chest tiles
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

                if(chestTile.TileType == TileID.Containers)
                {
                    switch (chestTile.TileFrameX)
                    {
                        case TileStyleID.Containers.Chest * chestWidth:
                            AddItemToChest(chest, ModContent.ItemType<MinersRing>(), 0.2f);
                        break;

                        case TileStyleID.Containers.GoldenChest * chestWidth:
							int[] options = { ModContent.ItemType<StoneWand>(), ModContent.ItemType<RuneOfSpelunking>() };
							AddItemToChestFromOptions(chest, options, 0.51f);
						break;

                        case TileStyleID.Containers.LockedGoldenChest * chestWidth:
                            AddItemToChest(chest, ModContent.ItemType<RuneOfMovement>(), 0.2f);
                        break;

                        case TileStyleID.Containers.SkywareChest * chestWidth:
                            AddItemToChest(chest, ModContent.ItemType<HangGlider>(), 0.33f);
                        break;
					}
                }

                if (chestTile.TileType == TileID.Containers2)
                {
                    switch(chestTile.TileFrameX) 
                    {
                        case TileStyleID.Containers2.SandstoneChest * chestWidth:
                            AddItemToChest(chest, ModContent.ItemType<Dunerang>(), 0.3333f);
                        break;
                    }
                }
            }
        }

		/// <summary>
        /// Tries to add a single item to a chest
		/// </summary>
		private void AddItemToChest(Chest chest, int itemToAdd, float chance)
        {
			for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
			{
				if (chest.item[inventoryIndex].type == ItemID.None)
				{
					if (Main.rand.NextFloat() <= chance)
					{
						chest.item[inventoryIndex].SetDefaults(itemToAdd);
					}

					break;
				}
			}
		}

		/// <summary>
		/// Tries to add a single item from given options to the chest
        /// Item is picked randomly from the options array
		/// </summary>
		private void AddItemToChestFromOptions(Chest chest, int[] options, float chance)
        {
			for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
			{
				if (chest.item[inventoryIndex].type == ItemID.None)
				{
					if (Main.rand.NextFloat() <= chance)
					{
						int i = Main.rand.Next(0, options.Length);
						chest.item[inventoryIndex].SetDefaults(options[i]);
					}

					break;
				}
			}
		}

        // TODO: Method for adding multiple items to a chest
        // Pick a random amount depending on lootpool size
        // keep trying to add from the lootpool until enough is added
        // somehow also need to keep track of items which already have been added so there are no duplicates
    }
}
