using Terraria;
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
			Item.maxStack = 9999;
			Item.value = Item.sellPrice(gold: 2);
			Item.rare = ItemRarityID.LightRed;
		}
	}
}
