using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace AssortedAdditions.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Face)]
    internal class GrabberFlower : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Draws the accessory over helmets
            ArmorIDs.Face.Sets.DrawInFaceFlowerLayer[Item.faceSlot] = true;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(gold: 1, silver: 30);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.aggro -= 250;
            player.GetDamage(DamageClass.Generic) += 0.03f;
        }
    }
}
