﻿using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using AssortedAdditions.Content.Items.Misc;
using AssortedAdditions.Content.Projectiles;
using AssortedAdditions.Content.Buffs;

namespace AssortedAdditions.Content.Items.Armor
{
    [AutoloadEquip(EquipType.Head)]
    internal class DraconicHat : ModItem
    {
        public override void SetStaticDefaults()
        {
            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 1;
            ArmorIDs.Head.Sets.DrawHead[Item.headSlot] = true;
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

        // 12% increased randged damage and speed
        // 7% increased ranged crit chance
        public override void UpdateEquip(Player player)
        {
            player.GetDamage(DamageClass.Ranged) += 0.12f;
            player.GetAttackSpeed(DamageClass.Ranged) += 0.12f;
            player.GetCritChance(DamageClass.Ranged) += 0.07f;
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
            recipe.AddTile(TileID.Anvils);
            recipe.Register();

            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ModContent.ItemType<DragonScale>(), 6);
            recipe2.AddIngredient(ItemID.CobaltBar, 12);
            recipe2.AddTile(TileID.Anvils);
            recipe2.Register();
        }
    }
}
