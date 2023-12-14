using AssortedAdditions.Helpers;
using Terraria.ID;
using Terraria;
using Terraria.IO;
using Terraria.WorldBuilding;
using Microsoft.Xna.Framework;

namespace AssortedAdditions.Common.Systems.GenPasses
{
	internal class SpawnStructure : GenPass
	{
		public SpawnStructure(string name, float weight) : base(name, weight) { }

		protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
		{
			progress.Message = "Building spawn";

			// Generates the base which is basically an uneven circle
			Point spawn = new Point(Main.spawnTileX, Main.spawnTileY);
			WorldUtils.Gen(spawn, new Shapes.Circle(13, 4), new Actions.SetTile(TileID.GrayBrick, true));

			// Add some variation to the base, some tiles are stone slab instead
			Point spawnArea = new Point(spawn.X - 13, spawn.Y - 4); // General area around the spawn
			for (int i = 0; i < 27; i++)
			{
				for (int j = 0; j < 9; j++)
				{
					if (Main.tile[spawnArea.X + i, spawnArea.Y + j].TileType == TileID.GrayBrick)
					{
						if (Main.rand.NextBool(3))
						{
							WorldGen.PlaceTile(spawnArea.X + i, spawnArea.Y + j, TileID.StoneSlab, true, true);
						}
					}
				}
			}

			// Add some statues
			WorldGen.PlaceTile(spawn.X - 8, spawn.Y - 4, TileID.Statues, true, true, -1, TileStyleID.Statues.ArmorStatue);
			WorldGen.PlaceTile(spawn.X + 7, spawn.Y - 4, TileID.Statues, true, true, -1, TileStyleID.Statues.ArmorStatue);
			// X is one less since the statue is two tiles wide

			// Make some pillars out of walls
			for (int i = 0; i <= 4; i++)
			{
				WorldGen.PlaceWall(spawn.X - 8, spawn.Y - 4 - i, WallID.StoneSlab, true);
				WorldGen.paintWall(spawn.X - 8, spawn.Y - 4 - i, PaintID.WhitePaint);
			}

			for (int i = 0; i <= 8; i++)
			{
				WorldGen.PlaceWall(spawn.X - 4, spawn.Y - 3 - i, WallID.StoneSlab, true);
				WorldGen.paintWall(spawn.X - 4, spawn.Y - 3 - i, PaintID.WhitePaint);

				if (i == 7) // Middles most pillars have a torch
				{
					WorldGen.PlaceTile(spawn.X - 4, spawn.Y - 3 - i, TileID.Torches, true, false, -1, TileStyleID.Torches.Torch);
				}
			}

			for (int i = 0; i <= 8; i++)
			{
				WorldGen.PlaceWall(spawn.X + 4, spawn.Y - 3 - i, WallID.StoneSlab, true);
				WorldGen.paintWall(spawn.X + 4, spawn.Y - 3 - i, PaintID.WhitePaint);

				if (i == 7)
				{
					WorldGen.PlaceTile(spawn.X + 4, spawn.Y - 3 - i, TileID.Torches, true, false, -1, TileStyleID.Torches.Torch);
				}
			}

			for (int i = 0; i <= 4; i++)
			{
				WorldGen.PlaceWall(spawn.X + 8, spawn.Y - 4 - i, WallID.StoneSlab, true);
				WorldGen.paintWall(spawn.X + 8, spawn.Y - 4 - i, PaintID.WhitePaint);
			}
		}
	}
}
