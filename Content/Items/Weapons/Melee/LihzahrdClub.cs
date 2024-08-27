using AssortedAdditions.Content.Projectiles.MeleeProj;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using AssortedAdditions.Helpers;
using AssortedAdditions.Content.Items.Placeables.Ores;

namespace AssortedAdditions.Content.Items.Weapons.Melee
{
	internal class LihzahrdClub : ModItem
	{
		public override void SetDefaults()
		{
			Item.useAnimation = 25;
			Item.useTime = 25;
			Item.damage = 85;
			Item.knockBack = 8f;
			Item.width = 46;
			Item.height = 46;

			Item.autoReuse = true;
			Item.useTurn = true;

			Item.value = Item.sellPrice(gold: 6);
			Item.UseSound = SoundID.Item1;
			Item.rare = ItemRarityID.Lime;
			Item.DamageType = DamageClass.Melee;
			Item.useStyle = ItemUseStyleID.Swing;
		}

		public override void OnHitNPC(Player player, NPC target, NPC.HitInfo hit, int damageDone)
		{
			if (Main.myPlayer == player.whoAmI)
			{
				Projectile.NewProjectile(player.GetSource_ItemUse(Item), target.Center, Vector2.Zero, ModContent.ProjectileType<LihzahrdClubProj>(), 75, 8f, player.whoAmI);
				SoundEngine.PlaySound(SoundID.Item74, target.position);
			}

			target.AddBuff(BuffID.OnFire3, HelperMethods.SecondsToTicks(7));
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ItemID.LunarTabletFragment, 6);
			recipe.AddIngredient(ModContent.ItemType<CoalChunk>(), 6);
			recipe.AddIngredient(ItemID.MeteoriteBar, 6);
			recipe.AddTile(TileID.MythrilAnvil);
			recipe.Register();
		}
	}
}
