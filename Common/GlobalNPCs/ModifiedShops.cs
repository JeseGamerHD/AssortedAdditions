using ModdingTutorial.Common.Systems;
using ModdingTutorial.Content.Items.Consumables;
using ModdingTutorial.Content.Items.Misc;
using ModdingTutorial.Content.Items.Tools;
using ModdingTutorial.Content.Items.Weapons.Ammo;
using ModdingTutorial.Content.Items.Weapons.Magic;
using ModdingTutorial.Content.Items.Weapons.Melee;
using ModdingTutorial.Content.Items.Weapons.Ranged;
using ModdingTutorial.Content.Items.Weapons.Summon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ModdingTutorial.Common.GlobalNPCs
{
    internal class ModifiedShops : GlobalNPC
    {
        public override void ModifyShop(NPCShop shop)
        {
            if(shop.NpcType == NPCID.Cyborg)
            {
                // Add item with custom price
                shop.Add(new Item(ModContent.ItemType<GuidedMissile>())
                {
                    shopCustomPrice = 100,
                });

                // Add just item (its value determines price)
                shop.Add<ControlChip>();
                shop.Add<Battery>();
            }

            if(shop.NpcType == NPCID.Dryad)
            {
                shop.Add(ItemID.FlowerBoots, condition: Condition.DownedSkeletron); // Sells the item after skeletron has been defeated
            }

            if(shop.NpcType == NPCID.PartyGirl)
            {
                shop.Add(ItemID.ShinyRedBalloon);
            }

            if(shop.NpcType == NPCID.Wizard)
            {
                shop.Add(ModContent.ItemType<MagicEssence>());
                shop.Add(ModContent.ItemType<AncientToken>());

                // These are sold after a mysterious key has been used once
                var sellCosmicItems = new Condition("Mods.ModdingTutorial.Conditions.SellCosmicItems", () => ModContent.GetInstance<ItemFlags>().mysteriousKeyWasUsed);
                shop.Add(ModContent.ItemType<CosmicBlade>(), condition: sellCosmicItems);
                shop.Add(ModContent.ItemType<ShootingStar>(), condition: sellCosmicItems);
                shop.Add(ModContent.ItemType<CosmicTome>(), condition: sellCosmicItems);
                shop.Add(ModContent.ItemType<CosmicWhip>(), condition: sellCosmicItems);
                shop.Add(ModContent.ItemType<Telelocator>(), condition: sellCosmicItems);
            }
        }
    }
}
