using Microsoft.Xna.Framework;
using ModdingTutorial.Content.Items.Accessories;
using ModdingTutorial.Content.Tiles.Blocks;
using ModdingTutorial.Content.Tiles.Furniture;
using Terraria;
using Terraria.ID;
using Terraria.IO;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace ModdingTutorial.Common.Systems.GenPasses
{
    // This mod adds a secret room below the dungeon entrance
    // The generation happens here
    internal class SecretRoomDungeon : GenPass
    {
        public SecretRoomDungeon(string name, float loadWeight) : base(name, loadWeight)
        {
            // Constructor required for this to function
        }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Adding a secret room";

            // This point is 13 blocks from roughly the middle of the dungeon entrance towards the door
            // and 13 blocks down:
            Point roomWalls = new Point(Main.dungeonX - 13, Main.dungeonY + 13);

            // Create a rectangle from that point
            WorldUtils.Gen(roomWalls, new Shapes.Rectangle(30, 20), new Actions.SetTile((ushort)ModContent.TileType<MysteriousBrickTile>()));

            // This carves out the rectangle making it hollow (creates a room basically)
            // Just basic looping through the x coordinate and then the y coordinate
            for (int i = 1; i <= 28; i++)
            {
                for (int j = 1; j <= 18; j++)
                {
                    WorldGen.KillTile(roomWalls.X + i, roomWalls.Y + j, false, false, true);
                }
            }

            // Place walls inside the room
            WorldUtils.Gen(roomWalls, new Shapes.Rectangle(30, 20), new Actions.PlaceWall(WallID.BlueStarryGlassWall));

            // Next generate some arches at the top corners of the room
            Point roomArchLeft = new Point(roomWalls.X + 1, roomWalls.Y + 1);
            GenerateArches(roomArchLeft, TileID.GraniteBlock, false);

            Point roomArchRight = new Point(roomWalls.X + 28, roomWalls.Y + 1);
            GenerateArches(roomArchRight, TileID.GraniteBlock, true);

            // Place a platform and a candle on both sides below the arches
            // Note that platforms are crammed in one TileID so a style needs to be specified to pick the correct platform
            // In this case style 10 places Brass Shelves
            WorldGen.PlaceTile(roomArchLeft.X, roomArchLeft.Y + 15, TileID.Platforms, true, true, -1, 10);
            WorldGen.PlaceTile(roomArchRight.X, roomArchRight.Y + 15, TileID.Platforms, true, true, -1, 10);

            WorldGen.PlaceTile(roomArchLeft.X, roomArchLeft.Y + 14, TileID.WaterCandle, true, true);
            WorldGen.PlaceTile(roomArchRight.X, roomArchRight.Y + 14, TileID.WaterCandle, true, true);

            // Next generate a sort of altar in the middle of the room
            Point altarStart = new Point(roomArchLeft.X + 5, roomArchRight.Y + 17);
            GenerateAltar(altarStart, TileID.Titanstone, TileID.GraniteBlock);

            // Place a chest on top of that altar...
            WorldGen.PlaceTile(altarStart.X + 8, altarStart.Y - 4, ModContent.TileType<MysteriousChestTile>(), true, true, -1, 1);

            // ...  and some loot inside
            for (int chestIndex = 0; chestIndex < 8000; chestIndex++) // loop through chests in the world
            {
                Chest chest = Main.chest[chestIndex]; // Set the index each loop
                if (chest != null && Main.tile[chest.x, chest.y].TileType == ModContent.TileType<MysteriousChestTile>()
                    && Main.tile[chest.x, chest.y].TileFrameX == 1 * 36) // Check that the tile is mysteriouschestile (locked = 1, unlocked = 0), 36 is chest width
                {
                    // Chest contains a weapon for each class TODO
                    // and a recall potion so players won't get stuck in the room
                    chest.item[0].SetDefaults(ModContent.ItemType<MinersRing>());
                    chest.item[1].SetDefaults(ModContent.ItemType<MinersRing>());
                    chest.item[2].SetDefaults(ModContent.ItemType<MinersRing>());
                    chest.item[3].SetDefaults(ModContent.ItemType<MinersRing>());
                    chest.item[4].SetDefaults(ItemID.RecallPotion);
                }
            }

            // Finally some finishing touches...
            // generate some beams:

            // Left side of the room
            for (int i = 0; i <= 12; i++)
            {
                WorldGen.PlaceTile(roomArchLeft.X + 2, roomArchLeft.Y + 17 - i, TileID.WoodenBeam, true, true);
                WorldGen.paintTile(roomArchLeft.X + 2, roomArchLeft.Y + 17 - i, 22); // This paints the tile with deep purple paint
            }

            // Right side...
            for (int i = 0; i <= 12; i++)
            {
                WorldGen.PlaceTile(roomArchRight.X - 2, roomArchLeft.Y + 17 - i, TileID.WoodenBeam, true, true);
                WorldGen.paintTile(roomArchRight.X - 2, roomArchLeft.Y + 17 - i, 22); // This paints the tile with deep purple paint
            }


            // Middle (this is an arch shape instead of a pillar)
            for (int i = 0; i <= 6; i++)
            {
                WorldGen.PlaceTile(roomArchLeft.X + 11, roomArchLeft.Y + 14 - i, TileID.WoodenBeam, true, true);
                WorldGen.paintTile(roomArchLeft.X + 11, roomArchLeft.Y + 14 - i, PaintID.DeepPurplePaint); // This paints the tile with deep purple paint
            }

            WorldGen.PlaceTile(roomArchLeft.X + 12, roomArchLeft.Y + 14 - 7, TileID.WoodenBeam, true, true);
            WorldGen.paintTile(roomArchLeft.X + 12, roomArchLeft.Y + 14 - 7, PaintID.DeepPurplePaint);

            for (int i = 0; i <= 1; i++)
            {
                WorldGen.PlaceTile(roomArchLeft.X + 13 + i, roomArchLeft.Y + 14 - 8, TileID.WoodenBeam, true, true);
                WorldGen.paintTile(roomArchLeft.X + 13 + i, roomArchLeft.Y + 14 - 8, PaintID.DeepPurplePaint);
            }

            WorldGen.PlaceTile(roomArchLeft.X + 15, roomArchLeft.Y + 14 - 7, TileID.WoodenBeam, true, true);
            WorldGen.paintTile(roomArchLeft.X + 15, roomArchLeft.Y + 14 - 7, PaintID.DeepPurplePaint);

            for (int i = 0; i <= 6; i++)
            {
                WorldGen.PlaceTile(roomArchLeft.X + 16, roomArchLeft.Y + 14 - i, TileID.WoodenBeam, true, true);
                WorldGen.paintTile(roomArchLeft.X + 16, roomArchLeft.Y + 14 - i, PaintID.DeepPurplePaint); // This paints the tile with deep purple paint
            }


            // Finally add two lamps
            WorldGen.PlaceTile(roomArchLeft.X + 9, roomArchLeft.Y + 15, TileID.Lamps, true, true, -1, 29);
            WorldGen.PlaceTile(roomArchRight.X - 9, roomArchRight.Y + 15, TileID.Lamps, true, true, -1, 29);
        }

        // This method generates arches for the room, set a starting point (either the top left or right corner)
        // give a tile and whether or not its the left or right arch
        private void GenerateArches(Point start, int tile, bool rightSide)
        {
            int direction = 1;
            if (rightSide)
            {
                direction = -1;
            }

            // Generation happens one row at a time, moving down each time
            for (int i = 0; i <= 9; i++)
            {
                WorldGen.PlaceTile(start.X + direction * i, start.Y, tile, true, true);
            }
            for (int i = 0; i <= 6; i++)
            {
                WorldGen.PlaceTile(start.X + direction * i, start.Y + 1, tile, true, true);
            }
            for (int i = 0; i <= 4; i++)
            {
                WorldGen.PlaceTile(start.X + direction * i, start.Y + 2, tile, true, true);
            }
            for (int i = 0; i <= 3; i++)
            {
                WorldGen.PlaceTile(start.X + direction * i, start.Y + 3, tile, true, true);
            }
            for (int i = 0; i <= 2; i++)
            {
                WorldGen.PlaceTile(start.X + direction * i, start.Y + 4, tile, true, true);
            }
            for (int i = 0; i <= 1; i++)
            {
                WorldGen.PlaceTile(start.X + direction * i, start.Y + 5, tile, true, true);
            }
            for (int i = 0; i <= 1; i++)
            {
                WorldGen.PlaceTile(start.X + direction * i, start.Y + 5, tile, true, true);
            }
            for (int i = 0; i <= 1; i++)
            {
                WorldGen.PlaceTile(start.X + direction * i, start.Y + 6, tile, true, true);
            }
            // Last bit is just one block wide so just spam this three times
            WorldGen.PlaceTile(start.X, start.Y + 7, tile, true, true);
            WorldGen.PlaceTile(start.X, start.Y + 8, tile, true, true);
            WorldGen.PlaceTile(start.X, start.Y + 9, tile, true, true);
        }

        private void GenerateAltar(Point start, int borderTile, int innerTile)
        {
            // Altar shape (borders):
            //     __----__
            //__---        ---__

            // The altar has an outer layer of different tiles than the ones on the inside
            for (int i = 0; i <= 17; i++)
            {
                if (i <= 1 || i >= 16)
                {
                    WorldGen.PlaceTile(start.X + i, start.Y, borderTile, true, true);
                }
                else
                {
                    WorldGen.PlaceTile(start.X + i, start.Y, innerTile, true, true);
                }
            }

            for (int i = 0; i <= 13; i++)
            {
                if (i <= 2 || i >= 11)
                {
                    WorldGen.PlaceTile(start.X + 2 + i, start.Y - 1, borderTile, true, true);
                }
                else
                {
                    WorldGen.PlaceTile(start.X + 2 + i, start.Y - 1, innerTile, true, true);
                }
            }

            for (int i = 0; i <= 7; i++)
            {
                if (i <= 1 || i >= 6)
                {
                    WorldGen.PlaceTile(start.X + 5 + i, start.Y - 2, borderTile, true, true);
                }
                else
                {
                    WorldGen.PlaceTile(start.X + 5 + i, start.Y - 2, innerTile, true, true);
                }
            }

            for (int i = 0; i <= 3; i++)
            {
                WorldGen.PlaceTile(start.X + 7 + i, start.Y - 3, borderTile, true, true);
            }
        }
    }
}
