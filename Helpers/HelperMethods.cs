using Terraria;

namespace AssortedAdditions.Helpers
{
	public class HelperMethods
	{
		/// <summary>
		/// This method should be used when ownedProjectileCounts does not work properly (it has a one tick delay).
		/// Loops through Main.projectile[] and counts how many projectiles of given type exist. Owner should be player.whoAmI, stopWhenFoundOne = true if only need to check if one exists.
		/// </summary>
		public static int CountProjectiles(int type, int owner, bool stopWhenFoundOne = false)
		{
			int num = 0;

			for(int i = 0; i < Main.maxProjectiles; i++)
			{
				if (Main.projectile[i].type == type && Main.projectile[i].active && Main.projectile[i].owner == owner)
				{
					num++;

					if (stopWhenFoundOne)
					{
						break;
					}
				}
			}

			return num;
		}
	}
}
