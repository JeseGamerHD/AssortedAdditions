using Terraria.ModLoader;
using Terraria.ID;
using Terraria;
using AssortedAdditions.Content.Items.Weapons.Ranged;
using AssortedAdditions.Content.Items.Weapons.Magic;
using AssortedAdditions.Content.Items.Accessories;
using AssortedAdditions.Helpers;
using AssortedAdditions.Content.Items.Accessories.Runes;
using AssortedAdditions.Content.Items.Consumables;
using AssortedAdditions.Content.Items.Misc;
using AssortedAdditions.Content.Items.Weapons.Melee;
using System.Collections.Generic;

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
							int[] options = { 
                                ModContent.ItemType<StoneWand>(), 
                                ModContent.ItemType<RuneOfSpelunking>() 
                            };
							AddItemToChestFromOptions(chest, options, 0.51f);
						break;

                        case TileStyleID.Containers.LockedGoldenChest * chestWidth:
                            AddItemToChest(chest, ModContent.ItemType<RuneOfMovement>(), 0.2f);
                        break;

                        case TileStyleID.Containers.SkywareChest * chestWidth:
                            AddItemToChest(chest, ModContent.ItemType<HangGlider>(), 0.33f);
                        break;

                        case TileStyleID.Containers.GraniteChest * chestWidth:
                           
                            RemoveVanillaLoot(chest); // First empty all vanilla loots

                            // Next create a lootPool
                            // These arrays are for the pool, to make it slightly less awful to read
							int[] modPotions = {
								ModContent.ItemType<BerserkerPotion>(),
								ModContent.ItemType<WardingPotion>()
							};

							int[] vanillaPotions = {
								ItemID.NightOwlPotion,
								ItemID.IronskinPotion,
								ItemID.TeleportationPotion,
								ItemID.TrapsightPotion
							};

							int[] weapons = {
								ModContent.ItemType<GraniteYoyo>(),
								ModContent.ItemType<GraniteChakram>(),
								ModContent.ItemType<GeodeScepter>()
							};

							List<ChestLoot> lootPool = new List<ChestLoot> { // TODO maybe clean this up
                                
                                // Secondary items
                                new ChestLoot(ItemID.GoldCoin, 1f, Main.rand.Next(1, 3)),
								new ChestLoot(ModContent.ItemType<GraniteArmorShard>(), 1f, Main.rand.Next(1, 5)),
								new ChestLoot(modPotions, 0.67f, Main.rand.Next(1, 3)),
								new ChestLoot(ItemID.Granite, 0.5f, Main.rand.Next(25, 51)),
								new ChestLoot(ItemID.Geode, 0.33f, Main.rand.Next(1, 5)),
								new ChestLoot(ItemID.HealingPotion, 0.5f, Main.rand.Next(3, 6)),
								new ChestLoot(ItemID.Spaghetti, 0.15f, Main.rand.Next(1, 3)),
								new ChestLoot(ItemID.Bomb, 0.33f, Main.rand.Next(20, 31)),
								new ChestLoot(vanillaPotions, 0.66f, Main.rand.Next(1, 3)),
								new ChestLoot(ItemID.Dynamite, 0.33f, Main.rand.Next(2, 7)),

                                // Primary items
								new ChestLoot(ItemID.NightVisionHelmet, 0.15f),
								new ChestLoot(weapons, 1f)
							};

							lootPool.Shuffle(); // Shuffle the loot pool to randomize the order that the loot may appear in
                            AddItemsToChestFromLootPool(chest, lootPool); // Once a pool has been made, try adding stuff from it to the chest

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
		private void AddItemToChest(Chest chest, int itemToAdd, float chance, int amount = 1)
        {
			for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
			{
				if (chest.item[inventoryIndex].type == ItemID.None)
				{
					if (Main.rand.NextFloat() <= chance)
					{
						chest.item[inventoryIndex].SetDefaults(itemToAdd);
						chest.item[inventoryIndex].stack = amount;
					}
					break;
				}
			}
		}

		/// <summary>
		/// Tries to add a single item from given options to the chest
        /// Item is picked randomly from the options array
		/// </summary>
		private void AddItemToChestFromOptions(Chest chest, int[] options, float chance, int amount = 1 )
        {
			for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
			{
				if (chest.item[inventoryIndex].type == ItemID.None)
				{
					if (Main.rand.NextFloat() <= chance)
					{
						int i = Main.rand.Next(0, options.Length);
						chest.item[inventoryIndex].SetDefaults(options[i]);
						chest.item[inventoryIndex].stack = amount;
					}
					break;
				}
			}
		}

		/// <summary>
		/// Attempts to add each entry from the loot pool (each entry has its own chance to be in a chest)
		/// If the entry contains multiple items, one is picked from the entry
		/// </summary>
		private void AddItemsToChestFromLootPool(Chest chest, List<ChestLoot> lootPool)
        {
            int inventoryIndex = 0;

            foreach(ChestLoot loot in lootPool)
            {
                while(inventoryIndex < 40)
                {
                    if (chest.item[inventoryIndex].type == ItemID.None)
                    {
                        if(Main.rand.NextFloat() <= loot.Chance)
                        {
                            chest.item[inventoryIndex].SetDefaults(loot.GetItem());
                            chest.item[inventoryIndex].stack = loot.Amount;
						}
                        break;
					}
					inventoryIndex++;
				}
                
                if(inventoryIndex >= 40)
                {
                    break;
                }
            }
        }

		/// <summary>
		/// Removes vanilla items from the chest
		/// </summary>
		private void RemoveVanillaLoot(Chest chest)
		{
			for (int inventoryIndex = 0; inventoryIndex < 40; inventoryIndex++)
			{
				if (chest.item[inventoryIndex].type != ItemID.None)
				{
					// If the item is vanilla item ModItem is null
					// Only remove vanilla items from the chest
					if (chest.item[inventoryIndex].ModItem == null)
					{
						chest.item[inventoryIndex].TurnToAir();
					}
				}
				else
				{ // Reached the end of items (only empty slots left), stop here
					break;
				}
			}
		}
	}

	/// <summary>
	/// Helper class for creating chest loot pools.
	/// Can be created with a single item or multiple
	/// </summary>
	public class ChestLoot
    {
		private int[] ItemsToAdd;
		private int ItemToAdd;
        public float Chance { get; set; }
        public int Amount { get; set; }

        public ChestLoot(int[] itemsToAdd, float chance, int amount = 1)
        {
            ItemsToAdd = itemsToAdd;
            Chance = chance;
            Amount = amount;
			ItemToAdd = 0;
        }

		public ChestLoot(int itemToAdd, float chance, int amount = 1)
		{
			ItemToAdd = itemToAdd;
			Chance = chance;
			Amount = amount;
			ItemsToAdd = null;
		}

		/// <summary>
		/// Gets an item that is stored in this instance.
		/// If there are multiple items, one is picked randomly. Otherwise the only stored item is returned.
		/// </summary>
		public int GetItem()
        {
			if(ItemsToAdd == null)
			{
				return ItemToAdd;
			}

            return ItemsToAdd[Main.rand.Next(ItemsToAdd.Length)];
        }
    }
}
