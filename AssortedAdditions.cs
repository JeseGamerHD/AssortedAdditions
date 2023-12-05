using System.IO;
using AssortedAdditions.Content.Items.Misc;
using AssortedAdditions.Content.NPCs;
using Steamworks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace AssortedAdditions
{
    public class AssortedAdditions : Mod
    {
        public const string ASSET_PATH = "AssortedAdditions/Assets/";

		/// <summary>
		/// Different net message types for syncing stuff in multiplayer. Accessed using AssortedAdditions.MessageType.namehere
		/// </summary>
		public enum MessageType : byte
		{
			SpawmTrainingDummy,
			DespawnTrainingDummy
		}

		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			// Using enums makes this much more readable
			MessageType message = (MessageType)reader.ReadByte();

			switch (message)
			{
				// Training dummy spawned from UseItem
				case MessageType.SpawmTrainingDummy:
					int xPos = reader.ReadInt32();
					int yPos = reader.ReadInt32();
					int player = reader.ReadInt32();
					NPC.NewNPC(new EntitySource_WorldEvent(), xPos, yPos, ModContent.NPCType<TrainingDummyNPC>(), 0, player);
				break;

				// Manually despawning the dummy also needed a sync
				case MessageType.DespawnTrainingDummy:
					int index = reader.ReadInt32();
					TrainingDummy.DespawnDummy(index);
				break;
			}
		}
	}
}