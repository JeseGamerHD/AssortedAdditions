using Terraria;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Items.Accessories.Runes
{
	/// <summary>
	/// Base class for rune accessories. Sets CanEquipAccessory to return true for modded slots.
	/// Other use is to check if the item is a RuneItem type in RuneSlot.cs
	/// </summary>
	public abstract class RuneItem : ModItem 
	{
		public override bool CanEquipAccessory(Player player, int slot, bool modded)
		{
			return modded; // Can only be equipped in the rune slot
		}
	}
}
