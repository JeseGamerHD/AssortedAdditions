using AssortedAdditions.Common.Players;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Items.Misc
{
	internal class BlankRune : ModItem
	{
		public override void SetStaticDefaults()
		{
			Item.ResearchUnlockCount = 3;
		}
		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 22;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.maxStack = 9999;
			Item.value = Item.sellPrice(gold: 2);
			Item.rare = ItemRarityID.LightRed;
			Item.useStyle = ItemUseStyleID.HoldUp;
			Item.UseSound = new SoundStyle("AssortedAdditions/Assets/Sounds/Misc/RuneSlotUnlock") with { Pitch = -0.2f };
		}

		public override bool CanUseItem(Player player)
		{
			return !player.GetModPlayer<PlayerUnlocks>().runeSlotUnlocked;
		}

		public override bool? UseItem(Player player)
		{
			if(player.whoAmI == Main.myPlayer)
			{
				player.GetModPlayer<PlayerUnlocks>().runeSlotUnlocked = true;
			}

			return base.UseItem(player);
		}
	}
}
