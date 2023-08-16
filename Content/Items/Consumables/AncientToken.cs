using Terraria.GameContent.Creative;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using ModdingTutorial.Content.NPCs.BossFireDragon;
using Terraria.Audio;
using ModdingTutorial.Content.Items.Misc;

namespace ModdingTutorial.Content.Items.Consumables
{
    internal class AncientToken : ModItem
    {
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Summons the Fire Dragon");

            CreativeItemSacrificesCatalog.Instance.SacrificeCountNeededByItemId[Type] = 3;
            ItemID.Sets.SortingPriorityBossSpawns[Type] = 12; // This helps sort inventory know that this is a boss summoning Item.
        }

        public override void SetDefaults()
        {
            Item.width = 32;
            Item.height = 32;
            Item.maxStack = 99;
            Item.value = Item.sellPrice(gold: 1);
            Item.rare = ItemRarityID.Blue;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.consumable = false; // Normally would be, but I want to make boss summon items non consumable
        }

        public override bool CanUseItem(Player player)
        {
            // Checks if boss already is active and that the player is in the underworld since this boss is fought there
            return !NPC.AnyNPCs(ModContent.NPCType<FireDragonHead>()) && player.ZoneUnderworldHeight;
        }

        public override bool? UseItem(Player player)
        {
            if (player.whoAmI == Main.myPlayer)
            {
                // The roaring sound when using summon items
                SoundEngine.PlaySound(SoundID.Roar, player.position);

                // Spawns the head which spawns the rest
                int type = ModContent.NPCType<FireDragonHead>();

                // Needed so multiplayer works
                if (Main.netMode != NetmodeID.MultiplayerClient)
                {
                    NPC.SpawnOnPlayer(player.whoAmI, type);
                }
                else
                {
                    NetMessage.SendData(MessageID.SpawnBossUseLicenseStartEvent, number: player.whoAmI, number2: type);
                }
            }

            return true;
        }

        public override void AddRecipes()
        {
            Recipe recipe = CreateRecipe();
            recipe.AddIngredient(ModContent.ItemType<DragonScale>(), 5);
            recipe.AddIngredient(ItemID.GoldBar, 5);
            recipe.AddTile(TileID.Anvils);
            recipe.Register();

            Recipe recipe2 = CreateRecipe();
            recipe2.AddIngredient(ModContent.ItemType<DragonScale>(), 5);
            recipe2.AddIngredient(ItemID.PlatinumBar, 5);
            recipe2.AddTile(TileID.Anvils);
            recipe2.Register();
        }
    }
}
