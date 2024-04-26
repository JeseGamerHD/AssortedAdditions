using AssortedAdditions.Content.Items.Accessories.Runes;
using Terraria;
using Terraria.ModLoader;

namespace AssortedAdditions.Common.Players
{
	internal class RuneSlot : ModAccessorySlot
	{
		public override string FunctionalBackgroundTexture => "Terraria/Images/Inventory_Back10";
		public override string FunctionalTexture => "AssortedAdditions/Assets/UI/RuneIcon";
		
		// This slot does not have dye or vanity slots with it
		public override bool DrawDyeSlot => false;
		public override bool DrawVanitySlot => false;

		public override void OnMouseHover(AccessorySlotType context)
		{
			Main.hoverItemName = "Runes";
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
	}
}
