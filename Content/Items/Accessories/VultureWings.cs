using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;

namespace AssortedAdditions.Content.Items.Accessories
{
    [AutoloadEquip(EquipType.Wings)]
    internal class VultureWings : ModItem
    {
        public override void SetStaticDefaults()
        {
            // 30 is the fly time in ticks so 0.5 seconds
            ArmorIDs.Wing.Sets.Stats[Item.wingSlot] = new WingStats(30, -1, 1f);
        }

        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 28;
            Item.value = Item.sellPrice(silver: 15);
            Item.rare = ItemRarityID.Green;
            Item.accessory = true;
        }

        public override bool WingUpdate(Player player, bool inUse)
        {
            // Make the player lean towards their direction a little while in the air
            // Either while flying up or when gliding
            if(inUse || (player.controlJump && player.wingTime == 0 && player.velocity.Y != 0))
            {
                player.fullRotation = player.velocity.X * 0.01f;
            }
            else
            { // Otherwise reset rotation
                player.fullRotation = default;
            }

            return false;
        }

        public override void VerticalWingSpeeds(Player player, ref float ascentWhenFalling, ref float ascentWhenRising, ref float maxCanAscendMultiplier, ref float maxAscentMultiplier, ref float constantAscend)
        {
            ascentWhenFalling = 0.85f;
            ascentWhenRising = 0.15f;
            maxCanAscendMultiplier = 1f;
            maxAscentMultiplier = 1.5f;
            constantAscend = 0.135f;
        }
    }
}
