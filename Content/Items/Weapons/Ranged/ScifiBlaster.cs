using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.Audio;
using AssortedAdditions.Content.Projectiles.RangedProj;
using AssortedAdditions.Content.Items.Weapons.Ammo;

namespace AssortedAdditions.Content.Items.Weapons.Ranged
{
    internal class ScifiBlaster : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 68;
            Item.height = 32;
            Item.damage = 80;
            Item.knockBack = 4f;
            Item.scale = 0.9f;
            Item.useAnimation = 15;
            Item.useTime = 15;
            Item.shootSpeed = 12;

            Item.noMelee = true;
            Item.autoReuse = true;

            Item.DamageType = DamageClass.Ranged;
            Item.rare = ItemRarityID.Yellow;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.UseSound = new SoundStyle("AssortedAdditions/Assets/Sounds/WeaponSound/ScifiBlasterSound")
                with
            { Volume = 0.5f, PitchVariance = 0.1f };
            Item.value = Item.sellPrice(gold: 8);
            Item.useAmmo = ModContent.ItemType<Battery>();
            Item.shoot = ModContent.ProjectileType<ScifiBlasterProj>();
        }

        public override Vector2? HoldoutOffset() => new(-5, 0); // Used for alligning the sprite

        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            // Needs to be set here since Battery converts to plasmacarbineproj by default
            type = ModContent.ProjectileType<ScifiBlasterProj>();

            Vector2 muzzleOffset = Vector2.Normalize(velocity) * 25f;
            if (Collision.CanHit(position, 0, 0, position + muzzleOffset, 0, 0))
            {
                position += muzzleOffset; // Projectiles come out of the muzzle properly using this
            }
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float numberOfProjs = 2;
            float rotation = MathHelper.ToRadians(25);

            for (int i = 0; i < numberOfProjs; i++)
            {
                Vector2 perturbedSpeed = velocity.RotatedBy(MathHelper.Lerp(-rotation, rotation, i / (numberOfProjs - 1))); // Watch out for dividing by 0 if there is only 1 projectile.
                Projectile.NewProjectile(source, position, perturbedSpeed, ModContent.ProjectileType<ScifiBlasterHoming>(), damage, knockback, player.whoAmI);
            }

            return true;
        }

        public override bool CanConsumeAmmo(Item ammo, Player player)
        {
            return Main.rand.NextBool(2, 10);
        }
    }
}
