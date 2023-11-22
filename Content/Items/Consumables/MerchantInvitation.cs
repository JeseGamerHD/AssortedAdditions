using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Terraria.Audio;

namespace AssortedAdditions.Content.Items.Consumables
{
    internal class MerchantInvitation : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 34;
            Item.height = 26;
            Item.maxStack = 999;
            Item.useAnimation = 15;
            Item.useTime = 15;

            Item.consumable = true;

            Item.value = Item.sellPrice(gold: 2);
            Item.rare = ItemRarityID.Quest;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.UseSound = SoundID.Item92;
        }

        // Invitation works if the merchant is not already present
        public override bool CanUseItem(Player player)
        {
            for (int i = 0; i < Main.npc.Length; i++)
            {
                if (Main.npc[i].active && Main.npc[i].type == NPCID.TravellingMerchant)
                {
                    return false;
                }
            }

            return base.CanUseItem(player);
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                return true;
            }

            return false;
        }

        public override bool ConsumeItem(Player player)
        {
            // Dust + sound effect:
            for (int i = 0; i < 30; i++)
            {
                int dust = Dust.NewDust(player.Center, player.width, player.height, DustID.WaterCandle, 0, 0, 150, default, 3.5f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].velocity *= 3f;
            }
            SoundEngine.PlaySound(SoundID.Item8, player.position);

            // This resets the shop items
            Chest.SetupTravelShop(); // Without using this the shop would have the same items until a natural spawn occurs

            // NPC spawning should not be done on multiplayer clients
            if (Main.netMode != NetmodeID.MultiplayerClient)
            {
                NPC.NewNPC(player.GetSource_ItemUse(Item), (int)player.Center.X, (int)player.Center.Y, NPCID.TravellingMerchant);
            }

            return base.ConsumeItem(player);
        }
    }
}
