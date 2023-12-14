using Terraria.ModLoader;
using Terraria.WorldBuilding;
using System.Collections.Generic;
using AssortedAdditions.Common.Systems.GenPasses;
using Terraria;
using Terraria.ID;

namespace AssortedAdditions.Common.Systems
{
    // This class is used for inserting GenPasses to the world generation
    internal class WorldSystem : ModSystem
    {
        public override void ModifyWorldGenTasks(List<GenPass> tasks, ref double totalWeight)
        {
            // Adds some ores:
            int shiniesIndex = tasks.FindIndex(t => t.Name.Equals("Shinies"));
            if (shiniesIndex != -1)
            {
                tasks.Insert(shiniesIndex + 1, new OreGenPreHard("Prehardmode modded GenPass", 320f)); // The float value affects the loading bar (how much completing this task moves it)
            }

            // Adds a secret room for the dungeon:
            int DungeonIndex = tasks.FindIndex(GenPass => GenPass.Name.Equals("Dungeon"));
            if (DungeonIndex != -1)
            {
                tasks.Insert(DungeonIndex + 1, new SecretRoomDungeon("Secret room in the dungeon", 1f));
            }

            int spawnIndex = tasks.FindIndex(GenPass => GenPass.Name.Equals("Spawn Point"));
            if (spawnIndex != -1)
            {
                tasks.Insert(spawnIndex + 1, new SpawnStructure("Guide", 15f));

            }
        }
    }

    // Guide would spawn inside the custom spawn structure so no clip him out of there
    internal class StopGuideFromSpawningInsideTile : GlobalNPC
    {
		public override bool PreAI(NPC npc)
		{
            if(npc.type == NPCID.Guide)
            {
                if (Collision.SolidCollision(npc.position, npc.width, npc.height))
                {
                    npc.position.Y -= 5;
                }
			}

            return base.PreAI(npc);
		}
	}
}
