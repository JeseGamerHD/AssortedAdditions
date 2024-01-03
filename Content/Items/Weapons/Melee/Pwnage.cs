using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AssortedAdditions.Content.Projectiles.MeleeProj;

namespace AssortedAdditions.Content.Items.Weapons.Melee
{
	internal class Pwnage : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.ToolTipDamageMultiplier[Type] = 2f;
		}

		public override void SetDefaults()
		{
			Item.width = 38;
			Item.height = 38;
			Item.useAnimation = 40;
			Item.useTime = 40;
			Item.damage = 55;
			Item.knockBack = 7f;
			Item.crit = 4;
			
			Item.noUseGraphic = true;
			Item.channel = true;
			Item.noMelee = true;

			Item.useStyle = ItemUseStyleID.Shoot;
			Item.UseSound = SoundID.Item1;
			Item.rare = ItemRarityID.Pink;
			Item.value = Item.sellPrice(gold: 1, silver: 33);
			Item.shoot = ModContent.ProjectileType<PwnageProj>();
		}

		public override void AddRecipes()
		{
			CreateRecipe()
				.AddIngredient(ItemID.Pwnhammer)
				.AddIngredient(ItemID.HallowedBar, 10)
				.AddIngredient(ItemID.SoulofSight)
				.Register();
		}
	}
}
