using AssortedAdditions.Content.Projectiles.MeleeProj;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace AssortedAdditions.Content.Items.Weapons.Melee
{
	internal class GraniteYoyo : ModItem
	{
		public override void SetStaticDefaults()
		{
			ItemID.Sets.Yoyo[Item.type] = true;
			ItemID.Sets.GamepadExtraRange[Item.type] = 20;
			ItemID.Sets.GamepadSmartQuickReach[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 30;
			Item.height = 26;
			Item.useTime = 25; // Vanilla yoyo's have this at 25
			Item.useAnimation = 25; // ^^
			Item.damage = 15;
			Item.knockBack = 5f;
			Item.crit = 2;
			Item.shootSpeed = 16f;

			Item.noMelee = true;
			Item.noUseGraphic = true;
			Item.channel = true;

			Item.useStyle = ItemUseStyleID.Shoot;
			Item.DamageType = DamageClass.MeleeNoSpeed;
			Item.rare = ItemRarityID.Green;
			Item.UseSound = SoundID.Item1;
			Item.value = Item.sellPrice(silver: 80);
			Item.shoot = ModContent.ProjectileType<GraniteYoyoProj>();
		}
	}

	public class GraniteYoyoPlayer : ModPlayer
	{
		public override void UpdateEquips()
		{
			if(Player.HeldItem.ModItem is GraniteYoyo)
			{
				Player.stringColor = PaintID.DeepPurplePaint;
			}
		}
	}
}
