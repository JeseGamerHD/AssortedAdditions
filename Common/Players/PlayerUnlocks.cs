using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AssortedAdditions.Common.Players
{
	internal class PlayerUnlocks : ModPlayer
	{
		public bool runeSlotUnlocked = false;

		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
		{
			ModPacket packet = Mod.GetPacket();
			packet.Write(runeSlotUnlocked);
			packet.Send(toWho, fromWho);
		}

		public override void SaveData(TagCompound tag)
		{
			tag["runeSlotUnlocked"] = runeSlotUnlocked;
		}
		public override void LoadData(TagCompound tag)
		{
			runeSlotUnlocked = tag.GetBool("runeSlotUnlocked");
		}
	}
}
