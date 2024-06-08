using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using AssortedAdditions.Content.Projectiles;
using Terraria.Graphics.Shaders;
using Terraria.Audio;
using System.Collections.Generic;
using AssortedAdditions.Common.Systems;
using System.Linq;
using AssortedAdditions.Content.Buffs;
using AssortedAdditions.Content.Tiles.CraftingStations;
using AssortedAdditions.Content.Items.Misc;
using Microsoft.Xna.Framework.Graphics;
using Terraria.GameContent;
using ReLogic.Content;

namespace AssortedAdditions.Content.Items.Accessories.Runes
{
	internal class RuneOfShadows : RuneItem
	{
		public override void SetStaticDefaults()
		{
			Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(9, 5));
			ItemID.Sets.AnimatesAsSoul[Item.type] = true;
		}

		public override void SetDefaults()
		{
			Item.width = 24;
			Item.height = 32;
			Item.maxStack = 1;
			Item.scale = 0.5f;
			Item.value = Item.sellPrice(gold: 5);
			Item.rare = ItemRarityID.LightPurple;
			Item.accessory = true;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			base.ModifyTooltips(tooltips); // Call base so RuneItem can replace the "Equipable" tip with a custom one
			tooltips.Insert(tooltips.FindLastIndex(tip => tip.Name.StartsWith("Tooltip")) + 1, 
				new(Mod, "Tooltip3", "Press " + CustomKeyBinds.RuneAbility.GetAssignedKeys().FirstOrDefault("<Unbound>") + " to use an ability"));
		}

		public override void UpdateAccessory(Player player, bool hideVisual)
		{
			player.GetModPlayer<RuneOfShadowsPlayer>().isWearingRuneOfShadows = true;

			// If the player is wearing the rune, no projectiles already exist
			if (player.GetModPlayer<RuneOfShadowsPlayer>().canSpawnProjectile && player.ownedProjectileCounts[ModContent.ProjectileType<RuneOfShadowsProj>()] < 1 
				&& Main.myPlayer == player.whoAmI)
			{
				Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<RuneOfShadowsProj>(), 65, 4f, player.whoAmI, 0f);
				Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<RuneOfShadowsProj>(), 65, 4f, player.whoAmI, 3f);
				Projectile.NewProjectile(player.GetSource_Accessory(Item), player.Center, Vector2.Zero, ModContent.ProjectileType<RuneOfShadowsProj>(), 65, 4f, player.whoAmI, 6f);

				// Won't spawn more unless previous ones die
				player.GetModPlayer<RuneOfShadowsPlayer>().canSpawnProjectile = false;
				// RuneOfShadowsProj.cs sets this to true when the projectile dies
				// ownedProjectileCounts ensures that all projectiles have died before new ones can spawn

				// Sound & dust
				SoundEngine.PlaySound(SoundID.Item8);
				for(int i = 0; i < 40; i++)
				{
					Dust dust = Dust.NewDustDirect(player.position, player.width, player.height, DustID.Shadowflame, 0, 0, 0, default, 1.2f);
					dust.noGravity = true;
					dust.velocity *= 1.5f;
					dust.shader = GameShaders.Armor.GetShaderFromItemId(ItemID.ShadowflameHadesDye);
				}
			}
		}

		public override void AddRecipes()
		{
			Recipe recipe = CreateRecipe();
			recipe.AddIngredient(ModContent.ItemType<BlankRune>());
			recipe.AddIngredient(ItemID.ShadowOrb);
			recipe.AddIngredient(ItemID.SoulofMight);
			recipe.AddIngredient(ItemID.SoulofSight);
			recipe.AddIngredient(ItemID.SoulofFright);
			recipe.AddTile(ModContent.TileType<MagicWorkbenchTile>());
			recipe.Register();
		}

		// The rune icon would be too big when dropped and Item.scale did not fix this
		// If the sprite's size is reduced then the icon is too small in the inventory so either way drawing would be required..
		public override bool PreDrawInWorld(SpriteBatch spriteBatch, Color lightColor, Color alphaColor, ref float rotation, ref float scale, int whoAmI)
		{
			Asset<Texture2D> texture = TextureAssets.Item[Item.type];

			Rectangle? source = null;
			if (Main.itemAnimations[Type] != null)
			{
				// The current frame of the animation, null check for items that have one frame
				source = Main.itemAnimations[Type].GetFrame(texture.Value);
			}

			// Draw item with 0.5 scale
			Main.spriteBatch.Draw(texture.Value, Item.position - Main.screenPosition, source, lightColor, 0, Vector2.Zero, scale * 0.5f, SpriteEffects.None, 0f);

			return false;
		}
	}

	public class RuneOfShadowsPlayer : ModPlayer
	{
		public bool isWearingRuneOfShadows = false;
		public bool canSpawnProjectile = true; // Resets when projectiles have died

		public override void ResetEffects()
		{
			isWearingRuneOfShadows = false;
		}
	}

	public class RuneOfShadowsNPC : GlobalNPC
	{
		public override void OnHitByItem(NPC npc, Player player, Item item, NPC.HitInfo hit, int damageDone)
		{
			if (player.GetModPlayer<RuneOfShadowsPlayer>().isWearingRuneOfShadows)
			{
				npc.AddBuff(BuffID.ShadowFlame, 300);
			}
		}

		public override void OnHitByProjectile(NPC npc, Projectile projectile, NPC.HitInfo hit, int damageDone)
		{
			Player player = Main.player[projectile.owner];
			if (player.GetModPlayer<RuneOfShadowsPlayer>().isWearingRuneOfShadows)
			{
				npc.AddBuff(BuffID.ShadowFlame, 300);
			}
		}
	}
}
