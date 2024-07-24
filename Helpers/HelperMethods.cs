using System;
using Microsoft.Xna.Framework;
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

			for (int i = 0; i < Main.maxProjectiles; i++)
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

		/// <summary>
		/// Loops through the NPCs and checks if the given npc overlaps with another one. If it does, its velocity will be adjusted using the strength.
		/// </summary>
		public static void StopNPCOverlap(NPC npc, float strength = 0.04f)
		{
			// Fix overlap:
			for (int i = 0; i < Main.maxNPCs; i++)
			{
				NPC other = Main.npc[i];

				if (i != npc.whoAmI && other.active && Math.Abs(npc.position.X - other.position.X) + Math.Abs(npc.position.Y - other.position.Y) < npc.width)
				{
					if (npc.position.X < other.position.X)
					{
						npc.velocity.X -= strength;
					}
					else
					{
						npc.velocity.X += strength;
					}

					if (npc.position.Y < other.position.Y)
					{
						npc.velocity.Y -= strength;
					}
					else
					{
						npc.velocity.Y += strength;
					}
				}
			}
		}

		/// <summary>
		/// Goes through Main.ActiveNPCs and checks how many of the given type of npc are active.
		/// If you only need to check if one or a specific amount exists, you can specify the stopWhenFoundAmount.
		/// </summary>
		/// <returns>the number of NPCs matching the type (or up until the stopWhenFoundAmount)</returns>
		public static int CountNPCs(int type, int stopWhenFoundAmount = -1)
		{
			int num = 0;

			foreach(var npc in Main.ActiveNPCs)
			{
				if (npc.type == type)
				{
					num++;
				}

				if(num == stopWhenFoundAmount)
				{
					break;
				}
			}

			return num;
		}

		// The following method is taken from the tsorcRevamp repository under GPL 3.0: https://github.com/Zeodexic/tsorcRevamp/blob/main/tsorcRevampUtils.cs#L774
		// Modifications made: incorporated the GenerateTargetingVector into this method, cleaned up the doc comments.
		///<summary> 
		///<para> Note: This method is licensed under GPL 3.0 from the <see href="https://github.com/Zeodexic/tsorcRevamp/blob/main/tsorcRevampUtils.cs#L774">tsorcRevamp</see> repository with minor tweaks </para> 
		/// Accelerates an entity toward a target in a smooth way. Returns a Vector2 with length 'acceleration' that points in the optimal direction to accelerate the NPC toward the target.
		/// If the target is moving, then it accounts for that (No, unfortunately the optimal direction is not actually a straight line most of the time).
		/// Accelerates until the NPC is moving fast enough that the acceleration can *just* slow it down in time, then does so.
		///</summary>
		///<param name="actor">The entity moving</param>
		///<param name="target">The target point it is aiming for</param>
		///<param name="acceleration">The rate at which it can accelerate</param>
		///<param name="topSpeed">The max speed of the entity</param>
		///<param name="targetVelocity">The velocity of its target, defaults to 0</param>
		///<param name="bufferZone">Should it smoothly slow down on approach?</param>
		public static void SmoothHoming(Entity actor, Vector2 target, float acceleration, float topSpeed, Vector2? targetVelocity = null, bool bufferZone = true, float bufferStrength = 0.1f)
		{
			//If the target has a velocity then factor it in
			Vector2 velTarget = Vector2.Zero;
			if (targetVelocity != null)
			{
				velTarget = targetVelocity.Value;
			}

			//Get the difference between the center of both entities
			Vector2 posDifference = target - actor.Center;

			//Get the distance between them
			float distance = posDifference.Length();

			//Get the difference of velocities
			//This shifts the reference frame of the calculations, from here on out we are looking at the problem as if Entity 1 was still and Entity 2 had the velocity of both entities combined
			//The formulas below calculate where it will be in the future and then the entity is accelerated toward that point on an intercept trajectory
			Vector2 vTarget = velTarget - actor.velocity;

			//Normalize posDifference to get the direction of it, ignoring the length
			posDifference.Normalize();

			//Use a dot product to get the length of the velocity vector in the direction of the target.
			//This tells us how fast the actor is moving toward the target already
			float velocity = Vector2.Dot(-vTarget, posDifference);

			//Use the current velocity plus acceleration to calculate how long it will take to arrive using the formula for acceleration
			float eta = (-velocity / acceleration) + (float)Math.Sqrt((velocity * velocity / (acceleration * acceleration)) + 2 * distance / acceleration);

			//Use the velocity plus arrival time plus current target location to calculate the location the target will be at in the future
			Vector2 impactPos = target + vTarget * eta;

			//Generate a vector with length 'acceleration' pointing at that future location
			Vector2 distanceToTarget = impactPos - actor.Center;
			distanceToTarget.Normalize();
			Vector2 fixedAcceleration = distanceToTarget * acceleration;

			//If distance or acceleration is 0 it will have nans, this deals with that
			if (fixedAcceleration.HasNaNs())
			{
				fixedAcceleration = Vector2.Zero;
			}

			//Update its acceleration
			actor.velocity += fixedAcceleration;

			//Slow it down to the speed limit if it is above it
			if (actor.velocity.Length() > topSpeed)
			{
				actor.velocity.Normalize();
				actor.velocity *= topSpeed;
			}

			//If it needs to slow down when arriving then do so
			//A distance of 300 and the formula here are super fudged. Could use improvement.
			if (bufferZone && distance < 300)
			{
				actor.velocity *= (float)Math.Pow(distance / 300, bufferStrength);
			}
		}
	}
}
