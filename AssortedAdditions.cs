using System.Collections.Generic;
using System.IO;
using AssortedAdditions.Content.Items.Misc;
using AssortedAdditions.Content.NPCs;
using AssortedAdditions.Content.Projectiles;
using Microsoft.Xna.Framework;
using Steamworks;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions
{
    public class AssortedAdditions : Mod
    {
        public const string ASSET_PATH = "AssortedAdditions/Assets/";

		internal static Dictionary<ushort, int> vanillaOreTileDrops;

		public override void Load()
		{
			vanillaOreTileDrops = new()
			{
				[TileID.Copper] = ItemID.CopperOre,
				[TileID.Iron] = ItemID.IronOre,
				[TileID.Silver] = ItemID.SilverOre,
				[TileID.Gold] = ItemID.GoldOre,
				[TileID.Tin] = ItemID.TinOre,
				[TileID.Lead] = ItemID.LeadOre,
				[TileID.Tungsten] = ItemID.TungstenOre,
				[TileID.Platinum] = ItemID.PlatinumOre,
				[TileID.Demonite] = ItemID.DemoniteOre,
				[TileID.Crimtane] = ItemID.CrimtaneOre,
				[TileID.Meteorite] = ItemID.Meteorite,
				[TileID.Hellstone] = ItemID.Hellstone,
				[TileID.Cobalt] = ItemID.CobaltOre,
				[TileID.Mythril] = ItemID.MythrilOre,
				[TileID.Adamantite] = ItemID.AdamantiteOre,
				[TileID.Palladium] = ItemID.PalladiumOre,
				[TileID.Orichalcum] = ItemID.OrichalcumOre,
				[TileID.Titanium] = ItemID.TitaniumOre,
				[TileID.Chlorophyte] = ItemID.ChlorophyteOre,
				[TileID.LunarOre] = ItemID.LunarOre
			};
		}

		public override void Unload()
		{
			vanillaOreTileDrops = null;
		}

		/// <summary>
		/// Different net message types for syncing stuff in multiplayer. Accessed using AssortedAdditions.MessageType.namehere
		/// </summary>
		public enum MessageType : byte
		{
			SpawnGenericNPC,

			SpawmTrainingDummy,
			DespawnTrainingDummy,
			SpawnTravellingMerchant,
			SpawnSkeletonMerchant,
			DespawnSkeletonMerchant,

			TeleportDungeon
		}

		public override void HandlePacket(BinaryReader reader, int whoAmI)
		{
			// Using enums makes this much more readable
			MessageType message = (MessageType)reader.ReadByte();

			switch (message)
			{
				case MessageType.SpawnGenericNPC:
					int xPos = reader.ReadInt32();
					int yPos = reader.ReadInt32();
					int npc = reader.ReadInt32();
					NPC.NewNPC(new EntitySource_WorldEvent(), xPos, yPos, npc);
				break;

				// Training dummy spawned from UseItem
				case MessageType.SpawmTrainingDummy:
					xPos = reader.ReadInt32();
					yPos = reader.ReadInt32();
					int player = reader.ReadInt32();
					NPC.NewNPC(new EntitySource_WorldEvent(), xPos, yPos, ModContent.NPCType<TrainingDummyNPC>(), 0, player);
				break;

				// Manually despawning the dummy also needed a sync
				case MessageType.DespawnTrainingDummy:
					int index = reader.ReadInt32();
					TrainingDummy.DespawnDummy(index);
				break;

				// When using merchant invitation to summon the travelling merchant
				case MessageType.SpawnTravellingMerchant:
					xPos = reader.ReadInt32();
					yPos = reader.ReadInt32();
					NPC travellingMerchant = NPC.NewNPCDirect(new EntitySource_WorldEvent(), xPos, yPos, NPCID.TravellingMerchant);
					Chest.SetupTravelShop(); // Without using this the shop would have the same items until a natural spawn occurs
					NetMessage.SendTravelShop(-1); // ^^ Also needs to be synced manually...
					SoundEngine.PlaySound(SoundID.Item8, travellingMerchant.position);
				break;

				case MessageType.SpawnSkeletonMerchant:
					xPos = reader.ReadInt32();
					yPos = reader.ReadInt32();
					NPC.NewNPCDirect(new EntitySource_WorldEvent(), xPos, yPos, NPCID.SkeletonMerchant);
				break;

				case MessageType.DespawnSkeletonMerchant:
					index = reader.ReadInt32();
					SkeletonPotionProj.DespawnSkeletonMerchant(index);
				break;

				case MessageType.TeleportDungeon:
					MysteriousKey.TeleportInMultiplayer(whoAmI);
				break;
			}
		}
	}
}