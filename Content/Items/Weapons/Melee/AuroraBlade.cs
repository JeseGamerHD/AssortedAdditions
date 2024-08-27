using AssortedAdditions.Content.Items.Misc;
using AssortedAdditions.Content.Items.Placeables.Ores;
using AssortedAdditions.Content.Projectiles.MeleeProj;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Content.Items.Weapons.Melee;

public class AuroraBlade : ModItem
{
    public override void SetDefaults()
    {
        Item.useAnimation = 16;
        Item.useTime = 16;
        Item.damage = 70;
        Item.knockBack = 4.5f;
        Item.width = 72;
        Item.height = 72;
        Item.scale = 1f;

        Item.noMelee = true; // This is set the sword itself doesn't deal damage (only the projectile does).
        Item.shootsEveryUse = true; // This makes sure Player.ItemAnimationJustStarted is set when swinging.
        Item.autoReuse = true;

        Item.value = Item.sellPrice(gold: 5);
        Item.UseSound = SoundID.Item1;
        Item.rare = ItemRarityID.Pink;
        Item.DamageType = DamageClass.Melee;
        Item.useStyle = ItemUseStyleID.Swing;
        Item.shoot = ModContent.ProjectileType<AuroraBladeProj>();

    }

    public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
    {
        float adjustedItemScale = player.GetAdjustedItemScale(Item); // Get the melee scale of the player and item.
        Projectile.NewProjectile(source, player.MountedCenter, new Vector2(player.direction, 0f), type, damage, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax, adjustedItemScale);
        NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, player.whoAmI); // Sync the changes in multiplayer.

        return base.Shoot(player, source, position, velocity, type, damage, knockback);
    }

    public override void AddRecipes()
    {
        Recipe recipe = CreateRecipe();
        recipe.AddIngredient(ModContent.ItemType<FrostBar>(), 15);
        recipe.AddIngredient(ModContent.ItemType<IceEssence>(), 8);
        recipe.AddTile(TileID.IceMachine);
        recipe.Register();
    }
}