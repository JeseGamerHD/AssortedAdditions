using Terraria;
using Terraria.ModLoader;
using Terraria.IO;
using Terraria.WorldBuilding;
using Terraria.ID;
using AssortedAdditions.Content.Tiles;

namespace AssortedAdditions.Common.Systems.GenPasses
{
    internal class OreGenPreHard : GenPass // Ore generation for Pre-hardmore
    {
        public OreGenPreHard(string name, float weight) : base(name, weight) { }

        protected override void ApplyPass(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Spicing up ore generation";

            //Coal Chunk
            int maxtoSpawn = (int)(Main.maxTilesX * Main.maxTilesY * 0.0003);
            for (int i = 0; i < maxtoSpawn; i++)
            {
                int x = WorldGen.genRand.Next(100, Main.maxTilesX - 100);
                int y = WorldGen.genRand.Next((int)GenVars.worldSurface + 30, Main.maxTilesY - 300);

                WorldGen.TileRunner(x, y, WorldGen.genRand.Next(2, 7), WorldGen.genRand.Next(3, 6), ModContent.TileType<CoalChunk>());
            }

            // Dune Ore
            maxtoSpawn = (int)(Main.maxTilesX * Main.maxTilesY * 0.006);
            for (int i = 0; i < maxtoSpawn; i++)
            {
                int x = WorldGen.genRand.Next(150, Main.maxTilesX - 150); // 150 should be outside oceans
                int y = WorldGen.genRand.Next((int)GenVars.worldSurface, Main.maxTilesY - 300);

                Tile tile = Framing.GetTileSafely(x, y); // This ore only spawns in the desert underground
                if (tile.TileType == TileID.HardenedSand || tile.TileType == TileID.DesertFossil)
                {
                    WorldGen.TileRunner(x, y, WorldGen.genRand.Next(3, 9), WorldGen.genRand.Next(4, 9), ModContent.TileType<DuneOre>());
                }
            }
        }
    }
}
