using ModdingTutorial.Content.Projectiles.MeleeProj;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using ModdingTutorial.Content.Projectiles.RangedProj;

namespace ModdingTutorial.Content.Items.Weapons.Melee
{
    internal class CosmicBlade : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 72;
            Item.height = 74;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.damage = 70;
            Item.knockBack = 3f;
            Item.shootSpeed = 6f;

            Item.noMelee = true; // This is set the sword itself doesn't deal damage (only the projectile does).
            Item.shootsEveryUse = true; // This makes sure Player.ItemAnimationJustStarted is set when swinging.
            Item.autoReuse = true;

            Item.value = Item.sellPrice(gold: 10);
            Item.UseSound = SoundID.Item1;
            Item.rare = ItemRarityID.Lime;
            Item.DamageType = DamageClass.Melee;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.shoot = ModContent.ProjectileType<CosmicBladeProj>();
        }

        private int swingCount = 0;
        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {

            float adjustedItemScale = player.GetAdjustedItemScale(Item); // Get the melee scale of the player and item.
            Projectile.NewProjectile(source, player.MountedCenter, new Vector2(player.direction, 0f), type, damage, knockback, player.whoAmI, player.direction * player.gravDir, player.itemAnimationMax, adjustedItemScale);
            NetMessage.SendData(MessageID.PlayerControls, -1, -1, null, player.whoAmI); // Sync the changes in multiplayer.

            float numberOfProjs = 3;
            float rotation = MathHelper.ToRadians(25);

            if(swingCount == 0)
            {
                for (int i = 0; i < numberOfProjs; i++)
                {
                    Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberOfProjs - 1))); // Watch out for dividing by 0 if there is only 1 projectile.
                    Projectile.NewProjectile(source, position, perturbedSpeed, ModContent.ProjectileType<CosmicBladeHoming>(), damage, knockback, player.whoAmI);
                }
            }
            swingCount++;

            if(swingCount == 4)
            {
                swingCount = 0;
            }

            return true;
        }
    }
}
