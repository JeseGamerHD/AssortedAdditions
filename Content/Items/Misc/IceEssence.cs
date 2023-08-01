using Terraria.DataStructures;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ModdingTutorial.Content.Items.Misc
{
    internal class IceEssence : ModItem
    {
        public override void SetStaticDefaults()
        {
            
            Main.RegisterItemAnimation(Item.type, new DrawAnimationVertical(5, 5)); 
            ItemID.Sets.AnimatesAsSoul[Item.type] = true; // Makes the item have an animation while in world (not held.). Use in combination with RegisterItemAnimation
            ItemID.Sets.ItemNoGravity[Item.type] = true;

            Item.ResearchUnlockCount = 25;
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = Item.CommonMaxStack;
            Item.value = Item.sellPrice(silver: 4);
            Item.rare = ItemRarityID.Orange;
        }

        public override void PostUpdate()
        {
            Lighting.AddLight(Item.Center, Color.LightBlue.ToVector3() * 0.55f * Main.essScale); // Makes this item glow when thrown out of inventory.
        }
    }
}
