using AssortedAdditions.Common.Configs;
using AssortedAdditions.Common.Systems;
using AssortedAdditions.Helpers;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace AssortedAdditions.Common.GlobalTiles
{
	// The hardmode ore progression is different.
	// Palladium/Cobalt after wall of flesh
	// Orichalcum/Mythril after defeating one mech boss or the fire dragon
	// Adamantite/Titanium after any two mech bosses
	internal class HardmodeOreProgression : GlobalTile
	{
		private int popupMsg = -1;
		public override bool CanKillTile(int i, int j, int type, ref bool blockDamaged)
		{
			// The player can toggle this on/off if they want to use vanilla progression
			if (ModContent.GetInstance<ServerSidedToggles>().HardmodeOreProgressionToggle)
			{
				switch (type)
				{
					case TileID.Mythril:
					case TileID.Orichalcum:
						if (NPC.downedMechBossAny || DownedBossSystem.downedFireDragon)
						{
							return true;
						}
						else
						{
							if(popupMsg != -1)
							{
								Main.popupText[popupMsg].active = false;
							}
							popupMsg = PopupText.NewText(new AdvancedPopupRequest
							{
								Text = Language.GetTextValue("Mods.AssortedAdditions.PopupMessages.HardmodeOre"),
								Color = Color.Red,
								DurationInFrames = 90,
								Velocity = Vector2.Zero
							}, new Vector2(i * 16, j * 16));

							return false;
						}

					case TileID.Adamantite:
					case TileID.Titanium:
						if (HelperMethods.AtLeastTrue(2, NPC.downedMechBoss1, NPC.downedMechBoss2, NPC.downedMechBoss3))
						{
							return true;
						}
						else
						{
							string message = NPC.downedMechBossAny 
								? Language.GetTextValue("Mods.AssortedAdditions.PopupMessages.HardmodeOre") 
								: Language.GetTextValue("Mods.AssortedAdditions.PopupMessages.HardmodeOreAlt");

							if (popupMsg != -1)
							{
								Main.popupText[popupMsg].active = false;
							}
							popupMsg = PopupText.NewText(new AdvancedPopupRequest
							{
								Text = message,
								Color = Color.Red,
								DurationInFrames = 90,
								Velocity = Vector2.Zero
							}, new Vector2(i * 16, j * 16));

							return false;
						}


					default:
						break;
				}
			}

			return base.CanKillTile(i, j, type, ref blockDamaged);
		}
	}

	public class HardmodeOreProgressionMessage : GlobalNPC
	{
		public override bool AppliesToEntity(NPC entity, bool lateInstantiation) => entity.type == NPCID.WallofFlesh;

		public override void OnKill(NPC npc)
		{
			if(!Main.hardMode && ServerSidedToggles.Instance.HardmodeOreProgressionToggle)
			{
				ChatHelper.BroadcastChatMessage(NetworkText.FromKey("Mods.AssortedAdditions.ChatMessages.HardmodeOres"), Color.DarkOrange);
			}
		}
	}
}
