using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Common.GlobalItems
{
    // Currently mostly used for buffing/nerfing items, but also editing tooltips
    internal class ModifiedItems : GlobalItem
    {
		public override void SetDefaults(Item entity)
        {
            switch (entity.type)
            {
                case ItemID.GoldenFishingRod:
                    entity.fishingPole = 75; // from 50% to 75%
                    break;

                case ItemID.FrostHelmet:
                    entity.defense = 7; // From 10
                    break;

                case ItemID.FrostBreastplate:
                    entity.defense = 15; // From 20
                    break;

                case ItemID.FrostLeggings:
                    entity.defense = 11; // From 13
                    break;

                default:
                    break;
            }
        }

        public override void UpdateEquip(Item item, Player player)
        {
            switch (item.type)
            {
                case ItemID.FrostHelmet:
                    player.GetDamage(DamageClass.Melee) -= 0.04f; // From 16% to 12%
                    player.GetDamage(DamageClass.Ranged) -= 0.04f;
                    break;

                case ItemID.FrostBreastplate:
                    player.GetCritChance(DamageClass.Melee) -= 0.03f; // From 11% to 8%
                    player.GetCritChance(DamageClass.Ranged) -= 0.03f;
                    break;

                case ItemID.FrostLeggings:
                    player.GetAttackSpeed(DamageClass.Melee) -= 0.02f; // From 10% to 8%
                    break;

                default:
                    break;
            }
        }

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            switch (item.type)
            {
                case ItemID.FrostHelmet:
                    TooltipLine editedTip = tooltips.FirstOrDefault(tip => tip.Name == "Tooltip0" && tip.Mod == "Terraria");
                    if (editedTip != null)
                    {
                        editedTip.Text = "12% increased melee and ranged damage";
                    }
                    break;

                case ItemID.FrostBreastplate:
                    TooltipLine editedTip2 = tooltips.FirstOrDefault(tip => tip.Name == "Tooltip0" && tip.Mod == "Terraria");
                    if (editedTip2 != null)
                    {
                        editedTip2.Text = "8% increased melee and ranged critical strike chance";
                    }
                    break;

                case ItemID.FrostLeggings:
                    TooltipLine editedTip3 = tooltips.FirstOrDefault(tip => tip.Name == "Tooltip1" && tip.Mod == "Terraria");
                    if (editedTip3 != null)
                    {
                        editedTip3.Text = "8% increased melee speed";
                    }
                    break;

                default:
                    break;
            }
        }
    }
}
