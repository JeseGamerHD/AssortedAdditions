using AssortedAdditions.Common.Players;
using Terraria;
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
			return slot == ModContent.GetInstance<RuneSlot>().Type; // Can only be equipped in the rune slot
		}

		public override bool? PrefixChance(int pre, UnifiedRandom rand)
		{
			return false;
		}
	}
}
