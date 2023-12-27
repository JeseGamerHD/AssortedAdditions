using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Common.GlobalItems
{
	internal class NewShimmers : GlobalItem
	{
		public override void SetDefaults(Item entity)
		{
			if(entity.type == ItemID.CrimsonHeart)
			{
				ItemID.Sets.ShimmerTransformToItem[entity.type] = ItemID.ShadowOrb;
			}

			if(entity.type == ItemID.ShadowOrb)
			{
				ItemID.Sets.ShimmerTransformToItem[entity.type] = ItemID.CrimsonHeart;
			}
		}
	}
}
