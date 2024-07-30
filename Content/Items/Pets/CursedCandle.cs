using AssortedAdditions.Content.Projectiles.PetProj;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AssortedAdditions.Content.Buffs.Pets;

namespace AssortedAdditions.Content.Items.Pets
{
    internal class CursedCandle : ModItem
	{
		public override void SetStaticDefaults()
		{

			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(15, 3, true));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 32;
			Item.height = 36;
			Item.rare = ItemRarityID.Master;
			Item.master = true;
			Item.value = Item.sellPrice(gold: 5);

			Item.DefaultToVanitypet(ModContent.ProjectileType<PetHauntling>(), ModContent.BuffType<PetHauntlingBuff>());
		}

		public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
		{
			// Buff applied here, the buff then spawns the pet projectile
			player.AddBuff(Item.buffType, 2);

			return false;
		}
	}
}
