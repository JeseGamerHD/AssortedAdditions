using Terraria;
using Terraria.ID;

namespace AssortedAdditions.Content.Items.Accessories.Runes
{
	internal class RuneOfSpelunking : RuneItem
	{
		public override void SetDefaults()
		{
			Item.width = 20;
			Item.height = 22;
			Item.maxStack = 1;
			Item.value = Item.sellPrice(gold: 1);
			Item.rare = ItemRarityID.Orange;
			Item.accessory = true;
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.nightVision = true;
			player.findTreasure = true;
			player.pickSpeed -= 0.20f;

			if(Main.rand.NextBool(10))
			{
				Dust dust = Dust.NewDustDirect(player.position, player.width / 2, player.height, DustID.GoldCoin);
				dust.noGravity = true;
				dust.velocity.Y -= 1f;
				dust.noLightEmittence = true;
			}
		}
	}
}
