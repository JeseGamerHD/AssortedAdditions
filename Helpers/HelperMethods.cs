using Terraria;

namespace AssortedAdditions.Helpers
{
	/// <summary>
	/// Contains useful methods that can be used from anywhere
	/// </summary>
	public class HelperMethods
	{
		/// <summary>
		/// Converts seconds to ticks
		/// </summary>
		public static int SecondsToTicks(int seconds)
		{
			return seconds * 60;
		}

		/// <summary>
		/// Converts minutes to ticks
		/// </summary>
		public static int MinutesToTicks(int minutes)
		{
			return SecondsToTicks(minutes) * 60;
		}

		/// <summary>
		/// This method should be used when ownedProjectileCounts does not work properly (it has a one tick delay).
		/// Loops through Main.projectile[] and counts how many projectiles of given type exist. Owner should be player.whoAmI, stopWhenFoundOne = true if only need to check if one exists.
		/// </summary>
		/// <returns>the number of projectiles matching the type (or 1 if stopWhenFoundOne = true)</returns>
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
