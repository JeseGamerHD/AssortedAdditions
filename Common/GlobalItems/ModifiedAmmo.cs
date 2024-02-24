using AssortedAdditions.Content.Projectiles.RangedProj;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace AssortedAdditions.Common.GlobalItems
{
	// From wiki:
	// "Note that using vanilla items to make new items runs the risk of conflict with other mods,
	// so use it sparingly to maintain compatibility and to prevent your players from getting confused."
	internal class ModifiedAmmo : GlobalItem
	{
		public override void SetDefaults(Item entity)
		{
			if (entity.type == ItemID.GlowingMushroom)
			{
				entity.ammo = ItemID.GlowingMushroom;
				entity.shoot = ModContent.ProjectileType<ShroomzookaProj>();
			}
		}
	}
}
