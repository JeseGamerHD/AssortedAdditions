using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace ModdingTutorial.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Face)]
    internal class DandelionFlower : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Draws the accessory over helmets
            ArmorIDs.Face.Sets.DrawInFaceFlowerLayer[Item.faceSlot] = true;
        }
        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 28;
            Item.maxStack = 1;
            Item.value = Item.sellPrice(gold: 1, silver: 30);
            Item.rare = ItemRarityID.Orange;
            Item.accessory = true;
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.moveSpeed += 0.1f;
            player.jumpBoost = true;
        }
    }
}
