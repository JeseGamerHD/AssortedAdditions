﻿using AssortedAdditions.Content.Items.Accessories.Runes;
using Terraria;
using Terraria.ModLoader;

namespace AssortedAdditions.Common.Players
{
	internal class RuneSlot : ModAccessorySlot
	{
		public override string FunctionalBackgroundTexture => "Terraria/Images/Inventory_Back10";

		// If the slot is locked, display a lock icon instead of the rune icon
		public override string FunctionalTexture => Player.GetModPlayer<PlayerUnlocks>().runeSlotUnlocked ? "AssortedAdditions/Assets/UI/RuneIcon" : "AssortedAdditions/Assets/UI/RuneIconLocked";

		// This slot does not have dye or vanity slots with it
		public override bool DrawDyeSlot => false;
		public override bool DrawVanitySlot => false;

		public override void OnMouseHover(AccessorySlotType context)
		{
			if (Player.GetModPlayer<PlayerUnlocks>().runeSlotUnlocked)
			{
				Main.hoverItemName = "Runes";
			}
			else
			{
				Main.hoverItemName = "Locked - Blank Rune required";
			}	
		}

		public override bool CanAcceptItem(Item checkItem, AccessorySlotType context)
		{
			return checkItem.ModItem is RuneItem;
		}

		public override bool ModifyDefaultSwapSlot(Item item, int accSlotToSwapTo)
		{
			return item.ModItem is RuneItem;
		}

		public override bool IsEnabled()
		{
			return Player.GetModPlayer<PlayerUnlocks>().runeSlotUnlocked;
		}

		public override bool IsVisibleWhenNotEnabled()
		{
			return true;
		}
	}
}
