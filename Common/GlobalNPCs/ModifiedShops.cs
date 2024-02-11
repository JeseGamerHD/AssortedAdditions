using AssortedAdditions.Common.Systems;
using AssortedAdditions.Content.Items.Accessories.Runes;
using AssortedAdditions.Content.Items.Consumables;
using AssortedAdditions.Content.Items.Misc;
using AssortedAdditions.Content.Items.Pets;
using AssortedAdditions.Content.Items.Tools;
using AssortedAdditions.Content.Items.Weapons.Ammo;
using AssortedAdditions.Content.Items.Weapons.Magic;
using AssortedAdditions.Content.Items.Weapons.Melee;
using AssortedAdditions.Content.Items.Weapons.Ranged;
using AssortedAdditions.Content.Items.Weapons.Summon;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Common.GlobalNPCs
{
    internal class ModifiedShops : GlobalNPC
    {
        public override void ModifyShop(NPCShop shop)
        {
            if (shop.NpcType == NPCID.Merchant)
            {
                shop.Add<MerchantInvitation>();
            }

            if (shop.NpcType == NPCID.Cyborg)
            {
                // Add item with custom price
                shop.Add(new Item(ModContent.ItemType<GuidedMissile>())
                {
                    shopCustomPrice = 100,
                });

                // Add just item (its value determines price)
                shop.Add<ControlChip>();
                shop.Add<Battery>();
                shop.Add<ToyCarRemote>();
            }

            if (shop.NpcType == NPCID.Dryad)
            {
                shop.Add(ItemID.FlowerBoots, condition: Condition.DownedSkeletron); // Sells the item after skeletron has been defeated
            }

            if (shop.NpcType == NPCID.PartyGirl)
            {
                shop.Add(ItemID.ShinyRedBalloon);
            }

            if (shop.NpcType == NPCID.Wizard)
            {
                shop.Add(ModContent.ItemType<MagicEssence>());
                shop.Add(ModContent.ItemType<BlankRune>());
                shop.Add(ModContent.ItemType<AncientToken>());

                // These are sold after a mysterious key has been used once
                var sellCosmicItems = new Condition("Mods.AssortedAdditions.Conditions.SellCosmicItems", () => ModContent.GetInstance<ItemFlags>().mysteriousKeyWasUsed);
                shop.Add(ModContent.ItemType<CosmicBlade>(), condition: sellCosmicItems);
                shop.Add(ModContent.ItemType<ShootingStar>(), condition: sellCosmicItems);
                shop.Add(ModContent.ItemType<CosmicTome>(), condition: sellCosmicItems);
                shop.Add(ModContent.ItemType<CosmicWhip>(), condition: sellCosmicItems);
                shop.Add(ModContent.ItemType<Telelocator>(), condition: sellCosmicItems);
            }

            if (shop.NpcType == NPCID.SkeletonMerchant)
            {
                shop.Add(ModContent.ItemType<StoneWand>());
                shop.Add(new Item(ModContent.ItemType<RuneOfSpelunking>()) { shopCustomPrice = 80000});
            }

            if(shop.NpcType == NPCID.WitchDoctor)
            {
                shop.Add(new Item(ItemID.GuideVoodooDoll) { shopCustomPrice = 4000 }, condition: Condition.DownedSkeletron);
				shop.Add(new Item(ItemID.ClothierVoodooDoll) { shopCustomPrice = 4000 }, condition: Condition.DownedSkeletron);
			}

            if(shop.NpcType == NPCID.Steampunker)
            {
                shop.Add(ModContent.ItemType<RuneOfRelocation>());
            }
        }
    }
}
