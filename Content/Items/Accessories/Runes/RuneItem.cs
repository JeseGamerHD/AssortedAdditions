﻿using System.Collections.Generic;
using System.Linq;
using AssortedAdditions.Common.Players;
using Terraria;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace AssortedAdditions.Content.Items.Accessories.Runes
{
	/// <summary>
	/// Base class for rune accessories. CanEquipAccessory returns true only for RuneSlot
	/// Disables prefixes (modifiers/forging) of these items.
	/// Other use is to check if the item is a RuneItem type in RuneSlot.cs
	/// </summary>
	public abstract class RuneItem : ModItem
	{
		public override bool CanEquipAccessory(Player player, int slot, bool modded)
		{
			return modded && slot == ModContent.GetInstance<RuneSlot>().Type; // Can only be equipped in the rune slot
		}

		public override bool? PrefixChance(int pre, UnifiedRandom rand)
		{
			return false;
		}

		// If a rune item adds some unique tooltip modifications by overriding, remember to call base so this gets added as well.
		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			TooltipLine tip = tooltips.FirstOrDefault(tip => tip.Name == "Equipable");
			tip.Text = Language.GetTextValue("Can be equipped in the " + "[c/FFF014:Rune Slot]");
		}
	}
}
