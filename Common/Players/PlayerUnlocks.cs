using System.IO;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace AssortedAdditions.Common.Players
{
	internal class PlayerUnlocks : ModPlayer
	{
		public bool runeSlotUnlocked;

		public override void SyncPlayer(int toWho, int fromWho, bool newPlayer)
		{
			ModPacket packet = Mod.GetPacket();
			packet.Write((byte)AssortedAdditions.MessageType.SyncPlayerUnlocks);
			packet.Write((byte)Player.whoAmI);
			packet.Write(runeSlotUnlocked);
			packet.Send(toWho, fromWho);
		}

		public void ReceivePlayerSync(BinaryReader reader)
		{
			runeSlotUnlocked = reader.ReadBoolean();
		}

		public override void CopyClientState(ModPlayer targetCopy)
		{
			PlayerUnlocks clone = (PlayerUnlocks)targetCopy;
			clone.runeSlotUnlocked = runeSlotUnlocked;
		}

		public override void SendClientChanges(ModPlayer clientPlayer)
		{
			PlayerUnlocks clone = (PlayerUnlocks)clientPlayer;

			if(runeSlotUnlocked != clone.runeSlotUnlocked)
			{
				SyncPlayer(toWho: -1, fromWho: Main.myPlayer, newPlayer: false);
			}
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
