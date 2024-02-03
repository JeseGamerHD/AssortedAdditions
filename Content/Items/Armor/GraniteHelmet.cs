using AssortedAdditions.Content.Items.Misc;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Items.Armor
{
	[AutoloadEquip(EquipType.Head)]
	internal class GraniteHelmet : ModItem
	{
		public override void SetStaticDefaults()
		{
			ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
		}

		public override void SetDefaults()
		{
			Item.width = 26;
			Item.height = 24;
			Item.value = Item.sellPrice(silver: 80);
			Item.rare = ItemRarityID.Green;
			Item.defense = 6;
		}

		public override void UpdateEquip(Player player)
		{
			player.GetDamage(DamageClass.Summon) += 0.05f;
		}

		public override bool IsArmorSet(Item head, Item body, Item legs)
		{
			bool bodyMatch = body.type == ModContent.ItemType<GraniteBreastplate>();
			bool legsMatch = legs.type == ModContent.ItemType<GraniteGreaves>();

			return bodyMatch && legsMatch;
		}

		public override void UpdateArmorSet(Player player)
		{
			player.setBonus = "Increases summon damage by 5% and whip speed by 10%";
			player.GetDamage(DamageClass.Summon) += 0.05f;
			player.GetAttackSpeed(DamageClass.SummonMeleeSpeed) += 0.1f;
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<GraniteArmorShard>(), 6);
			recipe.AddIngredient(ItemID.Granite, 20);
			recipe.AddRecipeGroup("IronBar", 3);
			recipe.AddTile(TileID.Anvils);
			recipe.Register();
		}
	}
}
