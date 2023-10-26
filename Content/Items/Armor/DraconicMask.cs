using ModdingTutorial.Content.Buffs;
using ModdingTutorial.Content.Items.Misc;
using ModdingTutorial.Content.Projectiles;
using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using ModdingTutorial.Common.Players;

namespace ModdingTutorial.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    internal class DraconicMask : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = false;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.value = Item.sellPrice(gold: 2, silver: 40);
            Item.rare = ItemRarityID.LightRed;
            Item.defense = 5;

            Item.shoot = ModContent.ProjectileType<DraconicArmorProj>();
            Item.buffType = ModContent.BuffType<DraconicArmorBuff>();
        }

        // 12% increased magic damage and speed
        // 80 more maximum mana
        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Magic) += 0.12f;
            player.GetAttackSpeed(DamageClass.Magic) += 0.12f;
            player.statManaMax2 += 80;
        }

        public override bool IsArmorSet(Item head, Item body, Item legs)
        {
            bool bodyMatch = body.type == ModContent.ItemType<DraconicChestplate>();
            bool legsMatch = legs.type == ModContent.ItemType<DraconicGreaves>();

            return bodyMatch && legsMatch;
        }

        public override void UpdateArmorSet(Player player)
        {
            player.setBonus = "Summons protective flames";
            player.AddBuff(ModContent.BuffType<DraconicArmorBuff>(), 2);
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<DragonScale>(), 6);
            recipe.AddIngredient(ItemID.PalladiumBar, 12);
            recipe.AddTile(TileID.MythrilAnvil);
            recipe.Register();

            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ModContent.ItemType<DragonScale>(), 6);
            recipe2.AddIngredient(ItemID.CobaltBar, 12);
            recipe2.AddTile(TileID.MythrilAnvil);
            recipe2.Register();
        }
    }
}
