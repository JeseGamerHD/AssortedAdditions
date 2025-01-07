using AssortedAdditions.Content.Tiles;
using AssortedAdditions.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.WorldBuilding;

namespace AssortedAdditions.Common.GlobalNPCs
{
    // This class is used for generating permafrost after defeating one of the mech bosses
    internal class PermafrostGeneration : GlobalNPC
    {
        public override bool AppliesToEntity(NPC entity, bool lateInstantiation)
        {
            // Check if the entity is one of the mech bosses
            if (entity.type == NPCID.Retinazer || entity.type == NPCID.Spazmatism || entity.type == NPCID.TheDestroyer || entity.type == NPCID.SkeletronPrime)
            {
                return true;
            }

            return false;
        }

        public override void OnKill(NPC npc)
        {
            // If none of the mech bosses have been defeated yet, generate Permafrost
            if (!NPC.downedMechBossAny)
            {
                // Special check for the twins to prevent permafrost generating twice
                if(npc.type == NPCID.Retinazer || npc.type == NPCID.Spazmatism)
                {
                    // Basically check if the other twin is still alive, works in combination with the downedMechBossAny
                    int otherTwin = npc.type == NPCID.Retinazer ? NPCID.Spazmatism : NPCID.Retinazer;
					Main.NewText(otherTwin);
					if (HelperMethods.CountNPCs(otherTwin, 1) > 0)
                    {
                        return;
                    }
                }

                int maxtoSpawn = (int)(Main.maxTilesX * Main.maxTilesY * 0.0006); // Adjusting this last number increases/decreases amount
                for (int i = 0; i < maxtoSpawn; i++)
                {
                    int x = WorldGen.genRand.Next(150, Main.maxTilesX);
                    int y = WorldGen.genRand.Next((int)GenVars.rockLayerHigh, Main.maxTilesY - 300);

                    Tile tile = Framing.GetTileSafely(x, y); // Permafrost only generates in the snow so check for ice/snow
                    if (tile.TileType == TileID.IceBlock || tile.TileType == TileID.CorruptIce || tile.TileType == TileID.HallowedIce || tile.TileType == TileID.FleshIce ||
                        tile.TileType == TileID.SnowBlock)
                    {
                        WorldGen.TileRunner(x, y, WorldGen.genRand.Next(4, 8), WorldGen.genRand.Next(4, 8), ModContent.TileType<PermafrostTile>());
                    }
                }

                Main.NewText(Language.GetTextValue("Mods.AssortedAdditions.ChatMessages.Permafrost"), Color.Blue);
            }
        }
    }
}
