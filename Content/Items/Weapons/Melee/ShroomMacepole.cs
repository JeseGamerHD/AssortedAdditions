using AssortedAdditions.Content.Projectiles.MeleeProj;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace AssortedAdditions.Content.Items.Weapons.Melee
{
	internal class ShroomMacepole : ModItem
	{
		public override void SetDefaults()
		{
			Item.width = 54;
			Item.height = 54;
			Item.useAnimation = 30;
			Item.useTime = 30;
			Item.damage = 30;
			Item.knockBack = 6f;
			Item.shootSpeed = 1f;

			Item.autoReuse = true;
			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.channel = true;

			Item.value = Item.sellPrice(silver: 85);
			Item.UseSound = SoundID.Item1;
			Item.rare = ItemRarityID.Green;
			Item.DamageType = DamageClass.Melee;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.shoot = ModContent.ProjectileType<ShroomMacepoleProj>();
		}

		public override bool CanUseItem(Player player)
		{
			return player.ownedProjectileCounts[Item.shoot] < 1;
		}
	}
}
