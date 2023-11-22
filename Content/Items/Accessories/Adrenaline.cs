using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace AssortedAdditions.Content.Items.Accessories
{
    internal class Adrenaline : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 12;
            Item.height = 30;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.LightPurple;
            Item.accessory = true;
        }

        // This accessory gives a bigger boost to stats depending on the health of the player
        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            // Increase max run speed manually
            if (player.statLife <= player.statLifeMax2 * 0.7f && player.statLife > player.statLifeMax2 * 0.5f)
            {
                player.maxRunSpeed += 1;
            }
            else if (player.statLife <= player.statLifeMax2 * 0.5f)
            {
                player.maxRunSpeed += 2;
            }

            // The amount of stat increase is tied to health so create a formula:
            // (max health - current health) / max health ===> (missing health / max health)
            // Important to have (float) since statsLifes are int, only one cast required since it forces the rest
            float healthRatio = (player.statLifeMax2 - player.statLife) / (float)player.statLifeMax2;

            // Now multiply the formula by the max increase that the stat can have
            // E.g. max 25% increase in attack speed (0.25)
            player.GetAttackSpeed(DamageClass.Generic) += healthRatio * 0.25f;
            player.moveSpeed += healthRatio * 0.25f;
        }

        // Can't be equipped with some accessories to prevent insane speeds
        public override bool CanAccessoryBeEquippedWith(Item equippedItem, Item incomingItem, Player player)
        {
            if (incomingItem.type == ItemID.PanicNecklace || incomingItem.type == ItemID.SweetheartNecklace)
            {
                return false;
            }

            return incomingItem.type != ModContent.ItemType<Medkit>();
        }
    }
}
