using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AssortedAdditions.Common.Systems
{
	public class DownedBossSystem : ModSystem
	{
		public static bool downedFireDragon = false;
		public static bool downedTheHaunt = false;

		public override void ClearWorld()
		{
			downedFireDragon = false;
			downedTheHaunt = false;
		}

		public override void SaveWorldData(TagCompound tag)
		{
			if(downedFireDragon)
			{
				tag["downedFireDragon"] = true;
			}

			if(downedTheHaunt)
			{
				tag["downedTheHaunt"] = true;
			}
		}

		public override void LoadWorldData(TagCompound tag)
		{
			downedFireDragon = tag.ContainsKey("downedFireDragon");
			downedTheHaunt = tag.ContainsKey("downedTheHaunt");
		}

		public override void NetSend(BinaryWriter writer)
		{
			var flags = new BitsByte();
			flags[0] = downedFireDragon;
			flags[1] = downedTheHaunt;

			writer.Write(flags);

			// REMEMBER - After flags[7] create new BitsByte
		}

		public override void NetReceive(BinaryReader reader)
		{
			BitsByte flags = reader.ReadByte();

			downedFireDragon = flags[0];
			downedTheHaunt= flags[1];
		}
	}
}
