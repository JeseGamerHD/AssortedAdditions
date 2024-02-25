/*using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace AssortedAdditions.Helpers
{
	internal class Temporary : ModProjectile
	{
		private void AI_026()
		{
			if (!Main.player[this.owner].active)
			{
				base.active = false;
				return;
			}

			bool flag = false;
			bool flag9 = false;
			bool flag10 = false;
			bool flag11 = false;
			bool flag12 = false;
			int num = 85;
			bool flag13 = false;

			if (this.type == 266)
			{
				num = 60 + 30 * this.minionPos;
			}

			bool flag14 = this.ai[0] == -1f || this.ai[0] == -2f;
			bool num201 = this.ai[0] == -1f;
			bool flag15 = this.ai[0] == -2f;
			

			if (this.type == 266)
			{
				if (Main.player[this.owner].dead)
				{
					Main.player[this.owner].slime = false;
				}
				if (Main.player[this.owner].slime)
				{
					this.timeLeft = 2;
				}
			}

			if (flag14)
			{
				this.timeLeft = 2;
			}

			if (flag13 || this.type == 266 || (this.type >= 390 && this.type <= 392))
			{
				num = 10;
				int num127 = 40 * (this.minionPos + 1) * Main.player[this.owner].direction;
				if (Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2) < base.position.X + (float)(base.width / 2) - (float)num + (float)num127)
				{
					flag9 = true;
				}
				else if (Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2) > base.position.X + (float)(base.width / 2) + (float)num + (float)num127)
				{
					flag10 = true;
				}
			}
			else if (Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2) < base.position.X + (float)(base.width / 2) - (float)num)
			{
				flag9 = true;
			}
			else if (Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2) > base.position.X + (float)(base.width / 2) + (float)num)
			{
				flag10 = true;
			}

			if (num201)
			{
				flag9 = false;
				flag10 = true;
				num = 30;
			}

			if (flag15)
			{
				flag9 = false;
				flag10 = false;
			}

			bool flag3 = this.ai[1] == 0f;

			if (flag)
			{
				flag3 = true;
			}

			if (flag3)
			{
				int num178 = 500;

				if (flag13 || this.type == 266 || (this.type >= 390 && this.type <= 392))
				{
					num178 += 40 * this.minionPos;
					if (this.localAI[0] > 0f)
					{
						num178 += 500;
					}
					if (this.type == 266 && this.localAI[0] > 0f)
					{
						num178 += 100;
					}
				}

				if (Main.player[this.owner].rocketDelay2 > 0)
				{
					this.ai[0] = 1f;
				}

				Vector2 vector12 = new Vector2(base.position.X + (float)base.width * 0.5f, base.position.Y + (float)base.height * 0.5f);
				float num206 = Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2) - vector12.X;
				float num180 = Main.player[this.owner].position.Y + (float)(Main.player[this.owner].height / 2) - vector12.Y;
				float num181 = (float)Math.Sqrt(num206 * num206 + num180 * num180);
				
				if (!flag14)
				{
					if (num181 > 2000f)
					{
						base.position.X = Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2) - (float)(base.width / 2);
						base.position.Y = Main.player[this.owner].position.Y + (float)(Main.player[this.owner].height / 2) - (float)(base.height / 2);
					}
				}
			}

			if (this.ai[0] != 0f && !flag14)
			{
				float num183 = 0.2f;
				int num184 = 200;
				
				if (flag13)
				{
					num183 = 0.5f;
					num184 = 100;
				}
				this.tileCollide = false;
				Vector2 vector13 = new Vector2(base.position.X + (float)base.width * 0.5f, base.position.Y + (float)base.height * 0.5f);
				float num185 = Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2) - vector13.X;
				
				if (flag13)
				{
					num185 -= (float)(40 * Main.player[this.owner].direction);
					float num186 = 700f;
					if (flag13)
					{
						num186 += 100f;
					}
					bool flag4 = false;
					int num187 = -1;
					for (int num188 = 0; num188 < 200; num188++)
					{
						if (!Main.npc[num188].CanBeChasedBy(this))
						{
							continue;
						}
						float num189 = Main.npc[num188].position.X + (float)(Main.npc[num188].width / 2);
						float num191 = Main.npc[num188].position.Y + (float)(Main.npc[num188].height / 2);
						if (Math.Abs(Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2) - num189) + Math.Abs(Main.player[this.owner].position.Y + (float)(Main.player[this.owner].height / 2) - num191) < num186)
						{
							if (Collision.CanHit(base.position, base.width, base.height, Main.npc[num188].position, Main.npc[num188].width, Main.npc[num188].height))
							{
								num187 = num188;
							}
							flag4 = true;
							break;
						}
					}
					if (!flag4)
					{
						num185 -= (float)(40 * this.minionPos * Main.player[this.owner].direction);
					}
					if (flag4 && num187 >= 0)
					{
						this.ai[0] = 0f;
					}
				}

				float num192 = Main.player[this.owner].position.Y + (float)(Main.player[this.owner].height / 2) - vector13.Y;
				float num193 = (float)Math.Sqrt(num185 * num185 + num192 * num192);
				float num194 = num193;
				float num195 = 10f;
				float num196 = num193;

				if (flag13)
				{
					num183 = 0.4f;
					num195 = 12f;
					if (flag13)
					{
						num183 = 0.8f;
					}
					if (num195 < Math.Abs(Main.player[this.owner].velocity.X) + Math.Abs(Main.player[this.owner].velocity.Y))
					{
						num195 = Math.Abs(Main.player[this.owner].velocity.X) + Math.Abs(Main.player[this.owner].velocity.Y);
					}
				}

				if (num193 < (float)num184 && Main.player[this.owner].velocity.Y == 0f && base.position.Y + (float)base.height <= Main.player[this.owner].position.Y + (float)Main.player[this.owner].height && !Collision.SolidCollision(base.position, base.width, base.height))
				{
					this.ai[0] = 0f;
					if (base.velocity.Y < -6f)
					{
						base.velocity.Y = -6f;
					}
				}
				if (num193 < 60f)
				{
					num185 = base.velocity.X;
					num192 = base.velocity.Y;
				}
				else
				{
					num193 = num195 / num193;
					num185 *= num193;
					num192 *= num193;
				}


				if (base.velocity.X < num185)
				{
					base.velocity.X += num183;
					if (base.velocity.X < 0f)
					{
						base.velocity.X += num183 * 1.5f;
					}
				}
				if (base.velocity.X > num185)
				{
					base.velocity.X -= num183;
					if (base.velocity.X > 0f)
					{
						base.velocity.X -= num183 * 1.5f;
					}
				}
				if (base.velocity.Y < num192)
				{
					base.velocity.Y += num183;
					if (base.velocity.Y < 0f)
					{
						base.velocity.Y += num183 * 1.5f;
					}
				}
				if (base.velocity.Y > num192)
				{
					base.velocity.Y -= num183;
					if (base.velocity.Y > 0f)
					{
						base.velocity.Y -= num183 * 1.5f;
					}
				}

				if (this.type != 313)
				{
					if ((double)base.velocity.X > 0.5)
					{
						this.spriteDirection = -1;
					}
					else if ((double)base.velocity.X < -0.5)
					{
						this.spriteDirection = 1;
					}
				}

				if (this.type == 266)
				{
					this.frameCounter++;
					if (this.frameCounter > 6)
					{
						this.frame++;
						this.frameCounter = 0;
					}
					if (this.frame < 2 || this.frame > 5)
					{
						this.frame = 2;
					}
					this.rotation = base.velocity.X * 0.1f;
				}
				else if (this.spriteDirection == -1)
				{
					this.rotation = (float)Math.Atan2(base.velocity.Y, base.velocity.X);
				}
				else
				{
					this.rotation = (float)Math.Atan2(base.velocity.Y, base.velocity.X) + 3.14f;
				}

				if (!flag13)
				{
					int num18 = Dust.NewDust(new Vector2(base.position.X + (float)(base.width / 2) - 4f, base.position.Y + (float)(base.height / 2) - 4f) - base.velocity, 8, 8, 16, (0f - base.velocity.X) * 0.5f, base.velocity.Y * 0.5f, 50, default(Color), 1.7f);
					Main.dust[num18].velocity.X = Main.dust[num18].velocity.X * 0.2f;
					Main.dust[num18].velocity.Y = Main.dust[num18].velocity.Y * 0.2f;
					Main.dust[num18].noGravity = true;

				}
			}
			else
			{
				if (flag13)
				{
					float num19 = 40 * this.minionPos;
					int num20 = 30;
					int num21 = 60;
					this.localAI[0] -= 1f;
					if (this.localAI[0] < 0f)
					{
						this.localAI[0] = 0f;
					}
					if (this.ai[1] > 0f)
					{
						this.ai[1] -= 1f;
					}
					else
					{
						float num22 = base.position.X;
						float num23 = base.position.Y;
						float num25 = 100000f;
						float num26 = num25;
						int num27 = -1;
						float num28 = 20f;
						NPC ownerMinionAttackTargetNPC = this.OwnerMinionAttackTargetNPC;
						if (ownerMinionAttackTargetNPC != null && ownerMinionAttackTargetNPC.CanBeChasedBy(this))
						{
							float num29 = ownerMinionAttackTargetNPC.position.X + (float)(ownerMinionAttackTargetNPC.width / 2);
							float num30 = ownerMinionAttackTargetNPC.position.Y + (float)(ownerMinionAttackTargetNPC.height / 2);
							float num31 = Math.Abs(base.position.X + (float)(base.width / 2) - num29) + Math.Abs(base.position.Y + (float)(base.height / 2) - num30);
							if (num31 < num25)
							{
								if (num27 == -1 && num31 <= num26)
								{
									num26 = num31;
									num22 = num29;
									num23 = num30;
								}
								if (Collision.CanHit(base.position, base.width, base.height, ownerMinionAttackTargetNPC.position, ownerMinionAttackTargetNPC.width, ownerMinionAttackTargetNPC.height))
								{
									num25 = num31;
									num22 = num29;
									num23 = num30;
									num27 = ownerMinionAttackTargetNPC.whoAmI;
								}
							}
						}
						if (num27 == -1)
						{
							for (int num32 = 0; num32 < 200; num32++)
							{
								if (!Main.npc[num32].CanBeChasedBy(this))
								{
									continue;
								}
								float num33 = Main.npc[num32].position.X + (float)(Main.npc[num32].width / 2);
								float num34 = Main.npc[num32].position.Y + (float)(Main.npc[num32].height / 2);
								float num36 = Math.Abs(base.position.X + (float)(base.width / 2) - num33) + Math.Abs(base.position.Y + (float)(base.height / 2) - num34);
								if (num36 < num25)
								{
									if (num27 == -1 && num36 <= num26)
									{
										num26 = num36;
										num22 = num33 + Main.npc[num32].velocity.X * num28;
										num23 = num34 + Main.npc[num32].velocity.Y * num28;
									}
									if (Collision.CanHit(base.position, base.width, base.height, Main.npc[num32].position, Main.npc[num32].width, Main.npc[num32].height))
									{
										num25 = num36;
										num22 = num33 + Main.npc[num32].velocity.X * num28;
										num23 = num34 + Main.npc[num32].velocity.Y * num28;
										num27 = num32;
									}
								}
							}
						}
						if (num27 == -1 && num26 < num25)
						{
							num25 = num26;
						}
						float num37 = 400f;
						if ((double)base.position.Y > Main.worldSurface * 16.0)
						{
							num37 = 200f;
						}
						if (num25 < num37 + num19 && num27 == -1)
						{
							float num38 = num22 - (base.position.X + (float)(base.width / 2));
							if (num38 < -5f)
							{
								flag9 = true;
								flag10 = false;
							}
							else if (num38 > 5f)
							{
								flag10 = true;
								flag9 = false;
							}
						}
						else if (num27 >= 0 && num25 < 800f + num19)
						{
							this.localAI[0] = num21;
							float num39 = num22 - (base.position.X + (float)(base.width / 2));
							if (num39 > 450f || num39 < -450f)
							{
								if (num39 < -50f)
								{
									flag9 = true;
									flag10 = false;
								}
								else if (num39 > 50f)
								{
									flag10 = true;
									flag9 = false;
								}
							}
							else if (this.owner == Main.myPlayer)
							{
								this.ai[1] = num20;
								Vector2 vector2 = new Vector2(base.position.X + (float)base.width * 0.5f, base.position.Y + (float)(base.height / 2) - 8f);
								float num40 = num22 - vector2.X + (float)Main.rand.Next(-20, 21);
								float num41 = Math.Abs(num40) * 0.1f;
								num41 = num41 * (float)Main.rand.Next(0, 100) * 0.001f;
								float num42 = num23 - vector2.Y + (float)Main.rand.Next(-20, 21) - num41;
								float num43 = (float)Math.Sqrt(num40 * num40 + num42 * num42);
								num43 = 11f / num43;
								num40 *= num43;
								num42 *= num43;
								int num44 = this.damage;
								int num45 = 195;
								int num47 = Projectile.NewProjectile(this.GetProjectileSource_FromThis(), vector2.X, vector2.Y, num40, num42, num45, num44, this.knockBack, Main.myPlayer);
								Main.projectile[num47].timeLeft = 300;
								if (num40 < 0f)
								{
									base.direction = -1;
								}
								if (num40 > 0f)
								{
									base.direction = 1;
								}
								this.netUpdate = true;
							}
						}
					}
				}
				bool flag5 = false;
				Vector2 vector3 = Vector2.Zero;
				bool flag6 = false;
				if (this.type == 266 || (this.type >= 390 && this.type <= 392))
				{
					float num48 = 40 * this.minionPos;
					int num49 = 60;
					this.localAI[0] -= 1f;
					if (this.localAI[0] < 0f)
					{
						this.localAI[0] = 0f;
					}
					if (this.ai[1] > 0f)
					{
						this.ai[1] -= 1f;
					}
					else
					{
						float num50 = base.position.X;
						float num51 = base.position.Y;
						float num52 = 100000f;
						float num53 = num52;
						int num54 = -1;
						NPC ownerMinionAttackTargetNPC2 = this.OwnerMinionAttackTargetNPC;
						if (ownerMinionAttackTargetNPC2 != null && ownerMinionAttackTargetNPC2.CanBeChasedBy(this))
						{
							float x = ownerMinionAttackTargetNPC2.Center.X;
							float y = ownerMinionAttackTargetNPC2.Center.Y;
							float num55 = Math.Abs(base.position.X + (float)(base.width / 2) - x) + Math.Abs(base.position.Y + (float)(base.height / 2) - y);
							if (num55 < num52)
							{
								if (num54 == -1 && num55 <= num53)
								{
									num53 = num55;
									num50 = x;
									num51 = y;
								}
								if (Collision.CanHit(base.position, base.width, base.height, ownerMinionAttackTargetNPC2.position, ownerMinionAttackTargetNPC2.width, ownerMinionAttackTargetNPC2.height))
								{
									num52 = num55;
									num50 = x;
									num51 = y;
									num54 = ownerMinionAttackTargetNPC2.whoAmI;
								}
							}
						}
						if (num54 == -1)
						{
							for (int num56 = 0; num56 < 200; num56++)
							{
								if (!Main.npc[num56].CanBeChasedBy(this))
								{
									continue;
								}
								float num58 = Main.npc[num56].position.X + (float)(Main.npc[num56].width / 2);
								float num59 = Main.npc[num56].position.Y + (float)(Main.npc[num56].height / 2);
								float num60 = Math.Abs(base.position.X + (float)(base.width / 2) - num58) + Math.Abs(base.position.Y + (float)(base.height / 2) - num59);
								if (num60 < num52)
								{
									if (num54 == -1 && num60 <= num53)
									{
										num53 = num60;
										num50 = num58;
										num51 = num59;
									}
									if (Collision.CanHit(base.position, base.width, base.height, Main.npc[num56].position, Main.npc[num56].width, Main.npc[num56].height))
									{
										num52 = num60;
										num50 = num58;
										num51 = num59;
										num54 = num56;
									}
								}
							}
						}
						if (this.type >= 390 && this.type <= 392 && !Collision.SolidCollision(base.position, base.width, base.height))
						{
							this.tileCollide = true;
						}
						if (num54 == -1 && num53 < num52)
						{
							num52 = num53;
						}
						else if (num54 >= 0)
						{
							flag5 = true;
							vector3 = new Vector2(num50, num51) - base.Center;
							if (this.type >= 390 && this.type <= 392)
							{
								if (Main.npc[num54].position.Y > base.position.Y + (float)base.height)
								{
									int num61 = (int)(base.Center.X / 16f);
									int num62 = (int)((base.position.Y + (float)base.height + 1f) / 16f);
									if (Main.tile[num61, num62] != null && Main.tile[num61, num62].active() && TileID.Sets.Platforms[Main.tile[num61, num62].type])
									{
										this.tileCollide = false;
									}
								}
								Rectangle rectangle = new Rectangle((int)base.position.X, (int)base.position.Y, base.width, base.height);
								Rectangle value = new Rectangle((int)Main.npc[num54].position.X, (int)Main.npc[num54].position.Y, Main.npc[num54].width, Main.npc[num54].height);
								int num63 = 10;
								value.X -= num63;
								value.Y -= num63;
								value.Width += num63 * 2;
								value.Height += num63 * 2;
								if (rectangle.Intersects(value))
								{
									flag6 = true;
									Vector2 vector4 = Main.npc[num54].Center - base.Center;
									if (base.velocity.Y > 0f && vector4.Y < 0f)
									{
										base.velocity.Y *= 0.5f;
									}
									if (base.velocity.Y < 0f && vector4.Y > 0f)
									{
										base.velocity.Y *= 0.5f;
									}
									if (base.velocity.X > 0f && vector4.X < 0f)
									{
										base.velocity.X *= 0.5f;
									}
									if (base.velocity.X < 0f && vector4.X > 0f)
									{
										base.velocity.X *= 0.5f;
									}
									if (vector4.Length() > 14f)
									{
										vector4.Normalize();
										vector4 *= 14f;
									}
									this.rotation = (this.rotation * 5f + vector4.ToRotation() + (float)Math.PI / 2f) / 6f;
									base.velocity = (base.velocity * 9f + vector4) / 10f;
									for (int num64 = 0; num64 < 1000; num64++)
									{
										if (base.whoAmI != num64 && this.owner == Main.projectile[num64].owner && Main.projectile[num64].type >= 390 && Main.projectile[num64].type <= 392 && (Main.projectile[num64].Center - base.Center).Length() < 15f)
										{
											float num65 = 0.5f;
											if (base.Center.Y > Main.projectile[num64].Center.Y)
											{
												Main.projectile[num64].velocity.Y -= num65;
												base.velocity.Y += num65;
											}
											else
											{
												Main.projectile[num64].velocity.Y += num65;
												base.velocity.Y -= num65;
											}
											if (base.Center.X > Main.projectile[num64].Center.X)
											{
												base.velocity.X += num65;
												Main.projectile[num64].velocity.X -= num65;
											}
											else
											{
												base.velocity.X -= num65;
												Main.projectile[num64].velocity.Y += num65;
											}
										}
									}
								}
							}
						}
						float num66 = 300f;
						if ((double)base.position.Y > Main.worldSurface * 16.0)
						{
							num66 = 150f;
						}
						if (this.type >= 390 && this.type <= 392)
						{
							num66 = 500f;
							if ((double)base.position.Y > Main.worldSurface * 16.0)
							{
								num66 = 250f;
							}
						}
						if (num52 < num66 + num48 && num54 == -1)
						{
							float num67 = num50 - (base.position.X + (float)(base.width / 2));
							if (num67 < -5f)
							{
								flag9 = true;
								flag10 = false;
							}
							else if (num67 > 5f)
							{
								flag10 = true;
								flag9 = false;
							}
						}
						bool flag7 = false;
						if (this.type >= 390 && this.type <= 392 && this.localAI[1] > 0f)
						{
							flag7 = true;
							this.localAI[1] -= 1f;
						}
						if (num54 >= 0 && num52 < 800f + num48)
						{
							this.friendly = true;
							this.localAI[0] = num49;
							float num68 = num50 - (base.position.X + (float)(base.width / 2));
							if (num68 < -10f)
							{
								flag9 = true;
								flag10 = false;
							}
							else if (num68 > 10f)
							{
								flag10 = true;
								flag9 = false;
							}
							if (num51 < base.Center.Y - 100f && num68 > -50f && num68 < 50f && base.velocity.Y == 0f)
							{
								float num69 = Math.Abs(num51 - base.Center.Y);
								if (num69 < 120f)
								{
									base.velocity.Y = -10f;
								}
								else if (num69 < 210f)
								{
									base.velocity.Y = -13f;
								}
								else if (num69 < 270f)
								{
									base.velocity.Y = -15f;
								}
								else if (num69 < 310f)
								{
									base.velocity.Y = -17f;
								}
								else if (num69 < 380f)
								{
									base.velocity.Y = -18f;
								}
							}
							if (flag7)
							{
								this.friendly = false;
								if (base.velocity.X < 0f)
								{
									flag9 = true;
								}
								else if (base.velocity.X > 0f)
								{
									flag10 = true;
								}
							}
						}
						else
						{
							this.friendly = false;
						}
					}
				}
				if (this.ai[1] != 0f)
				{
					flag9 = false;
					flag10 = false;
				}
				else if (flag13 && this.localAI[0] == 0f)
				{
					base.direction = Main.player[this.owner].direction;
				}
				else if (this.type >= 390 && this.type <= 392)
				{
					int num70 = (int)(base.Center.X / 16f);
					int num71 = (int)(base.Center.Y / 16f);
					if (Main.tile[num70, num71] != null && Main.tile[num70, num71].wall > 0)
					{
						flag9 = (flag10 = false);
					}
				}
				if (this.type == 127)
				{
					if ((double)this.rotation > -0.1 && (double)this.rotation < 0.1)
					{
						this.rotation = 0f;
					}
					else if (this.rotation < 0f)
					{
						this.rotation += 0.1f;
					}
					else
					{
						this.rotation -= 0.1f;
					}
				}
				else if (this.type != 313 && !flag6)
				{
					this.rotation = 0f;
				}
				if (this.type < 390 || this.type > 392)
				{
					this.tileCollide = true;
				}
				float num72 = 0.08f;
				float num73 = 6.5f;
				if (this.type == 127)
				{
					num73 = 2f;
					num72 = 0.04f;
				}
				if (this.type == 112)
				{
					num73 = 6f;
					num72 = 0.06f;
				}
				if (this.type == 334)
				{
					num73 = 8f;
					num72 = 0.08f;
				}
				if (this.type == 268)
				{
					num73 = 8f;
					num72 = 0.4f;
				}
				if (this.type == 324)
				{
					num72 = 0.1f;
					num73 = 3f;
				}
				if (this.type == 858)
				{
					num72 = 0.3f;
					num73 = 7f;
				}
				if (flag13 || this.type == 266 || (this.type >= 390 && this.type <= 392) || this.type == 816 || this.type == 821 || this.type == 825 || this.type == 859 || this.type == 860 || this.type == 881 || this.type == 884 || this.type == 890 || this.type == 891 || this.type == 897 || this.type == 899 || this.type == 900 || this.type == 934 || this.type == 956 || this.type == 958 || this.type == 959 || this.type == 960 || this.type == 994 || this.type == 998 || this.type == 1003 || this.type == 1004)
				{
					num73 = 6f;
					num72 = 0.2f;
					if (num73 < Math.Abs(Main.player[this.owner].velocity.X) + Math.Abs(Main.player[this.owner].velocity.Y))
					{
						num73 = Math.Abs(Main.player[this.owner].velocity.X) + Math.Abs(Main.player[this.owner].velocity.Y);
						num72 = 0.3f;
					}
					if (flag13)
					{
						num72 *= 2f;
					}
				}
				if (this.type == 875)
				{
					num73 = 7f;
					num72 = 0.25f;
					if (num73 < Math.Abs(Main.player[this.owner].velocity.X) + Math.Abs(Main.player[this.owner].velocity.Y))
					{
						num73 = Math.Abs(Main.player[this.owner].velocity.X) + Math.Abs(Main.player[this.owner].velocity.Y);
						num72 = 0.35f;
					}
				}
				if (this.type >= 390 && this.type <= 392)
				{
					num72 *= 2f;
				}
				if (flag14)
				{
					num73 = 6f;
				}
				if (flag9)
				{
					if ((double)base.velocity.X > -3.5)
					{
						base.velocity.X -= num72;
					}
					else
					{
						base.velocity.X -= num72 * 0.25f;
					}
				}
				else if (flag10)
				{
					if ((double)base.velocity.X < 3.5)
					{
						base.velocity.X += num72;
					}
					else
					{
						base.velocity.X += num72 * 0.25f;
					}
				}
				else
				{
					base.velocity.X *= 0.9f;
					if (base.velocity.X >= 0f - num72 && base.velocity.X <= num72)
					{
						base.velocity.X = 0f;
					}
				}
				if (this.type == 208)
				{
					base.velocity.X *= 0.95f;
					if ((double)base.velocity.X > -0.1 && (double)base.velocity.X < 0.1)
					{
						base.velocity.X = 0f;
					}
					flag9 = false;
					flag10 = false;
				}
				if (flag9 || flag10)
				{
					int num74 = (int)(base.position.X + (float)(base.width / 2)) / 16;
					int j2 = (int)(base.position.Y + (float)(base.height / 2)) / 16;
					if (this.type == 236)
					{
						num74 += base.direction;
					}
					if (flag9)
					{
						num74--;
					}
					if (flag10)
					{
						num74++;
					}
					num74 += (int)base.velocity.X;
					if (WorldGen.SolidTile(num74, j2))
					{
						flag12 = true;
					}
				}
				if (Main.player[this.owner].position.Y + (float)Main.player[this.owner].height - 8f > base.position.Y + (float)base.height)
				{
					flag11 = true;
				}
				if (this.type == 268 && this.frameCounter < 10)
				{
					flag12 = false;
				}
				if (this.type == 860 && base.velocity.X != 0f)
				{
					flag12 = true;
				}
				if ((this.type == 881 || this.type == 934) && base.velocity.X != 0f)
				{
					flag12 = true;
				}
				Collision.StepUp(ref base.position, ref base.velocity, base.width, base.height, ref this.stepSpeed, ref this.gfxOffY);
				if (base.velocity.Y == 0f || this.type == 200)
				{
					if (!flag11 && (base.velocity.X < 0f || base.velocity.X > 0f))
					{
						int num75 = (int)(base.position.X + (float)(base.width / 2)) / 16;
						int j3 = (int)(base.position.Y + (float)(base.height / 2)) / 16 + 1;
						if (flag9)
						{
							num75--;
						}
						if (flag10)
						{
							num75++;
						}
						WorldGen.SolidTile(num75, j3);
					}
					if (flag12)
					{
						int num76 = (int)(base.position.X + (float)(base.width / 2)) / 16;
						int num77 = (int)(base.position.Y + (float)base.height) / 16;
						if (WorldGen.SolidTileAllowBottomSlope(num76, num77) || Main.tile[num76, num77].halfBrick() || Main.tile[num76, num77].slope() > 0 || this.type == 200)
						{
							if (this.type == 200)
							{
								base.velocity.Y = -3.1f;
							}
							else
							{
								try
								{
									num76 = (int)(base.position.X + (float)(base.width / 2)) / 16;
									num77 = (int)(base.position.Y + (float)(base.height / 2)) / 16;
									if (flag9)
									{
										num76--;
									}
									if (flag10)
									{
										num76++;
									}
									num76 += (int)base.velocity.X;
									if (!WorldGen.SolidTile(num76, num77 - 1) && !WorldGen.SolidTile(num76, num77 - 2))
									{
										base.velocity.Y = -5.1f;
									}
									else if (!WorldGen.SolidTile(num76, num77 - 2))
									{
										base.velocity.Y = -7.1f;
									}
									else if (WorldGen.SolidTile(num76, num77 - 5))
									{
										base.velocity.Y = -11.1f;
									}
									else if (WorldGen.SolidTile(num76, num77 - 4))
									{
										base.velocity.Y = -10.1f;
									}
									else
									{
										base.velocity.Y = -9.1f;
									}
								}
								catch
								{
									base.velocity.Y = -9.1f;
								}
							}
							if (this.type == 127)
							{
								this.ai[0] = 1f;
							}
						}
					}
					else if (this.type == 266 && (flag9 || flag10))
					{
						base.velocity.Y -= 6f;
					}
				}
				if (base.velocity.X > num73)
				{
					base.velocity.X = num73;
				}
				if (base.velocity.X < 0f - num73)
				{
					base.velocity.X = 0f - num73;
				}
				if (base.velocity.X < 0f)
				{
					base.direction = -1;
				}
				if (base.velocity.X > 0f)
				{
					base.direction = 1;
				}
				if (base.velocity.X > num72 && flag10)
				{
					base.direction = 1;
				}
				if (base.velocity.X < 0f - num72 && flag9)
				{
					base.direction = -1;
				}
				if (this.type != 313)
				{
					if (base.direction == -1)
					{
						this.spriteDirection = 1;
					}
					if (base.direction == 1)
					{
						this.spriteDirection = -1;
					}
				}
				if (this.type == 398 || this.type == 958 || this.type == 960 || this.type == 956 || this.type == 959 || this.type == 994)
				{
					this.spriteDirection = base.direction;
				}
				bool flag8 = base.position.X - base.oldPosition.X == 0f;
				if (this.type == 956)
				{
					if (this.alpha > 0)
					{
						int num79 = Dust.NewDust(base.position, base.width, base.height, 6, base.velocity.X, base.velocity.Y, 0, default(Color), 1.2f);
						Main.dust[num79].velocity.X += Main.rand.NextFloat() - 0.5f;
						Main.dust[num79].velocity.Y += (Main.rand.NextFloat() + 0.5f) * -1f;
						if (Main.rand.Next(3) != 0)
						{
							Main.dust[num79].noGravity = true;
						}
						this.alpha -= 5;
						if (this.alpha < 0)
						{
							this.alpha = 0;
						}
					}
					if (base.velocity.Y != 0f)
					{
						this.frame = 10;
					}
					else if (flag8)
					{
						this.spriteDirection = 1;
						if (Main.player[this.owner].Center.X < base.Center.X)
						{
							this.spriteDirection = -1;
						}
						this.frame = 0;
					}
					else
					{
						float num80 = base.velocity.Length();
						this.frameCounter += (int)num80;
						if (this.frameCounter > 7)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame < 1 || this.frame > 9)
						{
							this.frame = 1;
						}
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
				else if (this.type == 958)
				{
					if (base.velocity.Y != 0f)
					{
						this.localAI[0] = 0f;
						this.frame = 4;
					}
					else if (flag8)
					{
						this.spriteDirection = 1;
						if (Main.player[this.owner].Center.X < base.Center.X)
						{
							this.spriteDirection = -1;
						}
						this.localAI[0] += 1f;
						if (this.localAI[0] > 200f)
						{
							this.frame = 1 + (int)(this.localAI[0] - 200f) / 6;
							if (this.localAI[0] >= 218f)
							{
								this.frame = 0;
								this.localAI[0] = Main.rand.Next(100);
							}
						}
						else
						{
							this.frame = 0;
						}
					}
					else
					{
						this.localAI[0] = 0f;
						float num81 = base.velocity.Length();
						this.frameCounter += (int)num81;
						if (this.frameCounter > 6)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame < 5 || this.frame > 12)
						{
							this.frame = 5;
						}
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
				else if (this.type == 959)
				{
					if (base.velocity.Y != 0f)
					{
						this.frame = ((base.velocity.Y > 0f) ? 10 : 9);
					}
					else if (flag8)
					{
						this.spriteDirection = 1;
						if (Main.player[this.owner].Center.X < base.Center.X)
						{
							this.spriteDirection = -1;
						}
						this.frame = 0;
					}
					else
					{
						float num82 = base.velocity.Length();
						this.frameCounter += (int)num82;
						if (this.frameCounter > 6)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame < 1 || this.frame > 8)
						{
							this.frame = 1;
						}
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
				else if (this.type == 998)
				{
					if (base.velocity.Y != 0f)
					{
						this.frame = 1;
					}
					else if (flag8)
					{
						this.spriteDirection = -1;
						if (Main.player[this.owner].Center.X < base.Center.X)
						{
							this.spriteDirection = 1;
						}
						this.frame = 0;
					}
					else
					{
						float num83 = base.velocity.Length();
						this.frameCounter += (int)num83;
						if (this.frameCounter > 6)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame < 0 || this.frame > 5)
						{
							this.frame = 0;
						}
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
				else if (this.type == 1003)
				{
					if (base.velocity.Y != 0f)
					{
						this.frame = 1;
					}
					else if (flag8)
					{
						this.spriteDirection = -1;
						if (Main.player[this.owner].Center.X < base.Center.X)
						{
							this.spriteDirection = 1;
						}
						this.frame = 0;
					}
					else
					{
						float num84 = base.velocity.Length();
						this.frameCounter += (int)num84;
						if (this.frameCounter > 6)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame < 2 || this.frame > 11)
						{
							this.frame = 2;
						}
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
				else if (this.type == 1004)
				{
					if (base.velocity.Y != 0f)
					{
						this.frame = 1;
					}
					else if (flag8)
					{
						this.spriteDirection = -1;
						if (Main.player[this.owner].Center.X < base.Center.X)
						{
							this.spriteDirection = 1;
						}
						this.frame = 0;
					}
					else
					{
						float num85 = base.velocity.Length();
						this.frameCounter += (int)num85;
						if (this.frameCounter > 6)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame < 2 || this.frame > 9)
						{
							this.frame = 2;
						}
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
				else if (this.type == 994)
				{
					if (base.velocity.Y != 0f)
					{
						this.frame = 4;
					}
					else if (flag8)
					{
						this.spriteDirection = 1;
						if (Main.player[this.owner].Center.X < base.Center.X)
						{
							this.spriteDirection = -1;
						}
						this.frameCounter++;
						if (this.frameCounter > 5)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame > 3)
						{
							this.frame = 0;
						}
					}
					else
					{
						float num86 = base.velocity.Length();
						this.frameCounter += (int)num86;
						if (this.frameCounter > 6)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame < 5 || this.frame > 12)
						{
							this.frame = 5;
						}
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
				else if (this.type == 960)
				{
					_ = Main.player[this.owner];
					if (base.velocity.Y != 0f)
					{
						this.localAI[0] = 0f;
						this.localAI[1] = 0f;
						this.frameCounter = 0;
						this.frame = 4;
					}
					else if (flag8)
					{
						if (!flag14)
						{
							this.spriteDirection = 1;
							if (Main.player[this.owner].Center.X < base.Center.X)
							{
								this.spriteDirection = -1;
							}
						}
						if (this.frame >= 5 && this.frame < 12)
						{
							this.frameCounter++;
							if (this.frameCounter > 3)
							{
								this.frame++;
								this.frameCounter = 0;
							}
							if (this.frame >= 12)
							{
								this.frame = 0;
							}
						}
						else if (this.Chester_IsAnyPlayerTrackingThisProjectile())
						{
							if (this.localAI[0] == 0f)
							{
								if (this.localAI[1] == 0f)
								{
									this.localAI[1] = 1f;
									this.frameCounter = 0;
								}
								this.frame = 13;
								this.frameCounter++;
								if (this.frameCounter > 6)
								{
									this.localAI[0] = 1f;
									this.frame = 14;
									this.frameCounter = 0;
								}
							}
							else
							{
								this.frameCounter++;
								if (this.frameCounter > 6)
								{
									this.frame++;
									if (this.frame > 18)
									{
										this.frame = 14;
									}
									this.frameCounter = 0;
								}
							}
						}
						else
						{
							this.localAI[0] = 0f;
							if (this.localAI[1] == 1f)
							{
								this.localAI[1] = 0f;
								this.frameCounter = 0;
							}
							if (this.frame >= 12 && this.frame <= 19)
							{
								this.frame = 19;
								this.frameCounter++;
								if (this.frameCounter > 6)
								{
									this.frame = 0;
									this.frameCounter = 0;
								}
							}
							else
							{
								this.frameCounter++;
								if (this.frameCounter >= 24)
								{
									this.frameCounter = 0;
								}
								this.frame = this.frameCounter / 6;
							}
						}
					}
					else
					{
						this.localAI[0] = 0f;
						this.localAI[1] = 0f;
						float val = base.velocity.Length();
						this.frameCounter += (int)Math.Max(2f, val);
						if (this.frameCounter > 6)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame < 5 || this.frame > 12)
						{
							this.frame = 5;
						}
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
				else if (this.type == 816)
				{
					if (base.velocity.Y != 0f)
					{
						this.frame = 4;
					}
					else if (flag8)
					{
						this.spriteDirection = -1;
						if (Main.player[this.owner].Center.X < base.Center.X)
						{
							this.spriteDirection = 1;
						}
						if (++this.frameCounter > 5)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame < 0 || this.frame > 3)
						{
							this.frame = 0;
						}
					}
					else
					{
						int num87 = 5;
						float num88 = base.velocity.Length();
						if (num88 > 4f)
						{
							num87 = 3;
						}
						else if (num88 > 2f)
						{
							num87 = 4;
						}
						if (++this.frameCounter > num87)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame < 4 || this.frame > 10)
						{
							this.frame = 4;
						}
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
				if (this.type == 860)
				{
					if (base.velocity.Y != 0f)
					{
						this.localAI[0] = 0f;
						if (this.frame >= 5)
						{
							this.frame = 5;
							this.frameCounter = 0;
						}
						else if (++this.frameCounter > 5)
						{
							this.frame++;
							this.frameCounter = 0;
						}
					}
					else if (Math.Abs(base.velocity.X) < 1f)
					{
						if (this.localAI[0] > 800f)
						{
							this.frameCounter++;
							if (this.frameCounter > 3)
							{
								this.frameCounter = 0;
								this.frame++;
								if (this.frame > 3)
								{
									this.frame = 3;
								}
							}
							this.localAI[0] += 1f;
							if (this.localAI[0] > 850f)
							{
								this.localAI[0] = 0f;
							}
							if (this.frame == 3 && this.localAI[0] == 820f)
							{
								for (int num90 = 0; num90 < 3 + Main.rand.Next(3); num90++)
								{
									int num91 = Gore.NewGore(new Vector2(base.position.X, base.Center.Y - 10f), Vector2.Zero, 1218);
									Main.gore[num91].velocity = new Vector2((float)Main.rand.Next(1, 10) * 0.3f * (float)(-this.spriteDirection), 0f - (2f + (float)Main.rand.Next(4) * 0.3f));
								}
							}
						}
						else if (this.frame == 0)
						{
							this.localAI[0] += 1f;
							this.frame = 0;
							this.frameCounter = 0;
						}
						else
						{
							this.localAI[0] = 0f;
							if (this.frame > 5)
							{
								this.frame = 5;
								this.frameCounter = 0;
							}
							if (++this.frameCounter > 4)
							{
								this.frame--;
								this.frameCounter = 0;
							}
						}
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
				if (this.type == 859)
				{
					if (base.velocity.Y != 0f)
					{
						this.frame = 4;
					}
					else if (flag8)
					{
						this.spriteDirection = -1;
						if (Main.player[this.owner].Center.X < base.Center.X)
						{
							this.spriteDirection = 1;
						}
						if (this.frame == 6)
						{
							if (++this.frameCounter > 5)
							{
								this.frame = 0;
								this.frameCounter = 0;
							}
						}
						else if (this.frame > 3)
						{
							this.frame = 6;
							this.frameCounter = 0;
						}
						else
						{
							if (++this.frameCounter > 5)
							{
								this.frame++;
								this.frameCounter = 0;
							}
							if (this.frame < 0 || this.frame > 3)
							{
								this.frame = 0;
							}
						}
					}
					else
					{
						float num92 = base.velocity.Length();
						int num93 = 8;
						if (num92 < 3f)
						{
							num93 = 4;
						}
						if (num92 < 1f)
						{
							num93 = 2;
						}
						this.frameCounter += (int)num92;
						if (this.frameCounter > num93)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame < 5 || this.frame > 17)
						{
							this.frame = 5;
						}
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
				else if (this.type == 858)
				{
					if (base.velocity.Y != 0f)
					{
						this.frame = 1;
					}
					else if (flag8)
					{
						this.spriteDirection = -1;
						if (Main.player[this.owner].Center.X < base.Center.X)
						{
							this.spriteDirection = 1;
						}
						this.frame = 0;
					}
					else
					{
						float num94 = base.velocity.Length();
						this.frameCounter += (int)num94;
						if (this.frameCounter > 3)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame < 2 || this.frame > 9)
						{
							this.frame = 2;
						}
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
				else if (this.type == 900)
				{
					this.spriteDirection = base.direction;
					if (base.velocity.Y != 0f)
					{
						this.frame = 1;
						this.frameCounter = 0;
					}
					else if (flag8)
					{
						this.spriteDirection = 1;
						if (Main.player[this.owner].Center.X < base.Center.X)
						{
							this.spriteDirection = -1;
						}
						this.frame = 0;
						this.frameCounter = 0;
					}
					else
					{
						this.frameCounter += 1 + (int)Math.Abs(base.velocity.X * 0.3f);
						if (this.frame < 2)
						{
							this.frame = 2;
							this.frameCounter = 0;
						}
						if (this.frameCounter > 4)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame > 9)
						{
							this.frame = 2;
						}
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
				else if (this.type == 899)
				{
					this.spriteDirection = base.direction;
					if (base.velocity.Y != 0f)
					{
						this.frame = 1;
						this.frameCounter = 0;
					}
					else if (flag8)
					{
						this.spriteDirection = 1;
						if (Main.player[this.owner].Center.X < base.Center.X)
						{
							this.spriteDirection = -1;
						}
						this.frame = 0;
						this.frameCounter = 0;
					}
					else
					{
						this.frameCounter += 1 + (int)Math.Abs(base.velocity.X * 0.3f);
						if (this.frame < 2)
						{
							this.frame = 2;
							this.frameCounter = 0;
						}
						if (this.frameCounter > 4)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame > 9)
						{
							this.frame = 2;
						}
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
				else if (this.type == 897)
				{
					this.spriteDirection = base.direction;
					if (base.velocity.Y != 0f)
					{
						this.frame = 1;
						this.frameCounter = 0;
					}
					else if (flag8)
					{
						this.spriteDirection = 1;
						if (Main.player[this.owner].Center.X < base.Center.X)
						{
							this.spriteDirection = -1;
						}
						this.frame = 0;
						this.frameCounter = 0;
					}
					else
					{
						this.frameCounter += 1 + (int)Math.Abs(base.velocity.X * 0.3f);
						if (this.frame < 2)
						{
							this.frame = 2;
							this.frameCounter = 0;
						}
						if (this.frameCounter > 4)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame > 7)
						{
							this.frame = 2;
						}
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
				else if (this.type == 891)
				{
					this.spriteDirection = base.direction;
					if (base.velocity.Y != 0f)
					{
						this.frame = 1;
						this.frameCounter = 0;
					}
					else if (flag8)
					{
						this.spriteDirection = Main.player[this.owner].direction;
						this.frame = 0;
						this.frameCounter = 0;
					}
					else
					{
						this.frameCounter += 1 + (int)Math.Abs(base.velocity.X * 0.3f);
						if (this.frame < 2)
						{
							this.frame = 2;
							this.frameCounter = 0;
						}
						if (this.frameCounter > 4)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame > 8)
						{
							this.frame = 2;
						}
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
				else if (this.type == 890)
				{
					this.spriteDirection = base.direction;
					if (base.velocity.Y != 0f)
					{
						this.frame = 1;
						this.frameCounter = 0;
					}
					else if (flag8)
					{
						this.spriteDirection = 1;
						if (Main.player[this.owner].Center.X < base.Center.X)
						{
							this.spriteDirection = -1;
						}
						this.frame = 0;
						this.frameCounter = 0;
					}
					else
					{
						this.frameCounter += 1 + (int)Math.Abs(base.velocity.X * 0.3f);
						if (this.frame < 2)
						{
							this.frame = 2;
							this.frameCounter = 0;
						}
						if (this.frameCounter > 4)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame > 7)
						{
							this.frame = 2;
						}
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
				else if (this.type == 884)
				{
					this.spriteDirection = base.direction;
					if (base.velocity.Y != 0f)
					{
						if (base.velocity.Y < 0f)
						{
							this.frame = 9;
						}
						else
						{
							this.frame = 1;
						}
						this.frameCounter = 0;
					}
					else if (flag8)
					{
						this.spriteDirection = 1;
						if (Main.player[this.owner].Center.X < base.Center.X)
						{
							this.spriteDirection = -1;
						}
						this.frame = 0;
						this.frameCounter = 0;
					}
					else
					{
						this.frameCounter += 1 + (int)Math.Abs(base.velocity.X * 0.5f);
						if (this.frameCounter > 6)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame > 8)
						{
							this.frame = 2;
						}
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
				else if (this.type == 881 || this.type == 934)
				{
					this.spriteDirection = 1;
					if (Main.player[this.owner].Center.X < base.Center.X)
					{
						this.spriteDirection = -1;
					}
					if (base.velocity.Y > 0f)
					{
						this.frameCounter++;
						if (this.frameCounter > 2)
						{
							this.frame++;
							if (this.frame >= 2)
							{
								this.frame = 2;
							}
							this.frameCounter = 0;
						}
					}
					else if (base.velocity.Y < 0f)
					{
						this.frameCounter++;
						if (this.frameCounter > 2)
						{
							this.frame++;
							if (this.frame >= 5)
							{
								this.frame = 0;
							}
							this.frameCounter = 0;
						}
					}
					else if (this.frame == 0)
					{
						this.frame = 0;
					}
					else if (++this.frameCounter > 3)
					{
						this.frame++;
						if (this.frame >= 6)
						{
							this.frame = 0;
						}
						this.frameCounter = 0;
					}
					if (base.wet && Main.player[this.owner].position.Y + (float)Main.player[this.owner].height < base.position.Y + (float)base.height && this.localAI[0] == 0f)
					{
						if (base.velocity.Y > -4f)
						{
							base.velocity.Y -= 0.2f;
						}
						if (base.velocity.Y > 0f)
						{
							base.velocity.Y *= 0.95f;
						}
					}
					else
					{
						base.velocity.Y += 0.4f;
					}
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
				else if (this.type == 875)
				{
					if (base.velocity.Y != 0f)
					{
						if (base.velocity.Y < 0f)
						{
							this.frame = 3;
						}
						else
						{
							this.frame = 6;
						}
						this.frameCounter = 0;
					}
					else if (flag8)
					{
						this.spriteDirection = -1;
						if (Main.player[this.owner].Center.X < base.Center.X)
						{
							this.spriteDirection = 1;
						}
						this.frame = 0;
						this.frameCounter = 0;
					}
					else
					{
						this.frameCounter += 1 + (int)Math.Abs(base.velocity.X * 0.75f);
						if (this.frameCounter > 6)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame > 6)
						{
							this.frame = 0;
						}
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
				else if (this.type == 854)
				{
					if (base.velocity.Y != 0f)
					{
						this.frame = 7;
					}
					else if (flag8)
					{
						this.spriteDirection = -1;
						if (Main.player[this.owner].Center.X < base.Center.X)
						{
							this.spriteDirection = 1;
						}
						if (++this.frameCounter > 5)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame < 0 || this.frame > 3)
						{
							this.frame = 0;
						}
					}
					else
					{
						int num95 = 3;
						float num96 = base.velocity.Length();
						if (num96 > 4f)
						{
							num95 = 1;
						}
						else if (num96 > 2f)
						{
							num95 = 2;
						}
						if (++this.frameCounter > num95)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame < 4 || this.frame > 12)
						{
							this.frame = 4;
						}
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
				else if (this.type == 825)
				{
					if (base.velocity.Y != 0f)
					{
						this.localAI[0] = 0f;
						this.frame = 12;
					}
					else if (flag8)
					{
						this.spriteDirection = -1;
						if (Main.player[this.owner].Center.X < base.Center.X)
						{
							this.spriteDirection = 1;
						}
						if (this.frame >= 1 && this.frame <= 2)
						{
							this.localAI[0] = 0f;
							if (++this.frameCounter > 5)
							{
								this.frame++;
								this.frameCounter = 0;
							}
							if (this.frame > 2)
							{
								this.frame = 0;
							}
						}
						else if (this.frame >= 3 && this.frame <= 11)
						{
							this.localAI[0] = 0f;
							if (++this.frameCounter > 5)
							{
								this.frame++;
								this.frameCounter = 0;
							}
							if (this.frame > 11)
							{
								this.frame = 0;
							}
						}
						else
						{
							if (this.frame == 13)
							{
								if (++this.frameCounter > 8)
								{
									this.frame++;
									this.frameCounter = 0;
								}
								if (this.frame == 14)
								{
									this.frame = 0;
								}
							}
							if (this.frame != 0)
							{
								this.frame = 13;
							}
							else
							{
								this.frame = 0;
							}
							if (this.frame == 0)
							{
								this.localAI[0] += 1f;
								if (this.localAI[0] > 300f && Main.rand.Next(50) == 0)
								{
									switch (Main.rand.Next(2))
									{
										case 0:
											this.frame = 1;
											break;
										case 1:
											this.frame = 3;
											break;
									}
								}
							}
						}
					}
					else
					{
						this.localAI[0] = 0f;
						int num97 = 3;
						float num98 = base.velocity.Length();
						if (num98 > 4f)
						{
							num97 = 2;
						}
						else if (num98 > 2f)
						{
							num97 = 1;
						}
						if (++this.frameCounter > num97)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame < 13)
						{
							this.frame = 13;
						}
						if (this.frame > 19)
						{
							this.frame = 14;
						}
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
				else if (this.type == 821)
				{
					if (base.velocity.Y != 0f)
					{
						this.localAI[0] = 0f;
						this.frame = 12;
					}
					else if (flag8)
					{
						this.spriteDirection = -1;
						if (Main.player[this.owner].Center.X < base.Center.X)
						{
							this.spriteDirection = 1;
						}
						this.localAI[0] += 1f;
						if (this.localAI[0] > 400f)
						{
							int num99 = 7;
							if (this.frame == 9)
							{
								num99 = 25;
							}
							if (++this.frameCounter > num99)
							{
								this.frame++;
								this.frameCounter = 0;
							}
							if (this.frame < 5)
							{
								this.frame = 5;
							}
							if (this.frame > 11)
							{
								this.localAI[0] = 0f;
								this.frame = 0;
							}
						}
						else
						{
							if (++this.frameCounter > 6)
							{
								this.frame++;
								this.frameCounter = 0;
							}
							if (this.frame < 0 || this.frame > 4)
							{
								this.frame = 0;
							}
						}
					}
					else
					{
						this.localAI[0] = 0f;
						int num101 = 4;
						float num207 = base.velocity.Length();
						if (num207 > 3f)
						{
							num101 = 3;
						}
						if (num207 > 5f)
						{
							num101 = 2;
						}
						if (++this.frameCounter > num101)
						{
							this.frame++;
							if (num101 == 0)
							{
								this.frame++;
							}
							this.frameCounter = 0;
						}
						if (this.frame < 13 || this.frame > 18)
						{
							this.frame = 13;
						}
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
				else if (flag13)
				{
					if (this.ai[1] > 0f)
					{
						if (this.localAI[1] == 0f)
						{
							this.localAI[1] = 1f;
							this.frame = 1;
						}
						if (this.frame != 0)
						{
							this.frameCounter++;
							if (this.frameCounter > 4)
							{
								this.frame++;
								this.frameCounter = 0;
							}
							if (this.frame >= 4)
							{
								this.frame = 0;
							}
						}
					}
					else if (base.velocity.Y == 0f)
					{
						this.localAI[1] = 0f;
						if (flag8)
						{
							this.frame = 0;
							this.frameCounter = 0;
						}
						else if ((double)base.velocity.X < -0.8 || (double)base.velocity.X > 0.8)
						{
							this.frameCounter += (int)Math.Abs(base.velocity.X);
							this.frameCounter++;
							if (this.frameCounter > 6)
							{
								this.frame++;
								this.frameCounter = 0;
							}
							if (this.frame < 5)
							{
								this.frame = 5;
							}
							if (this.frame >= 11)
							{
								this.frame = 5;
							}
						}
						else
						{
							this.frame = 0;
							this.frameCounter = 0;
						}
					}
					else if (base.velocity.Y < 0f)
					{
						this.frameCounter = 0;
						this.frame = 4;
					}
					else if (base.velocity.Y > 0f)
					{
						this.frameCounter = 0;
						this.frame = 4;
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
					_ = base.velocity;
				}
				else if (this.type == 268)
				{
					if (base.velocity.Y == 0f)
					{
						if (this.frame > 5)
						{
							this.frameCounter = 0;
						}
						if (flag8)
						{
							int num102 = 3;
							this.frameCounter++;
							if (this.frameCounter < num102)
							{
								this.frame = 0;
							}
							else if (this.frameCounter < num102 * 2)
							{
								this.frame = 1;
							}
							else if (this.frameCounter < num102 * 3)
							{
								this.frame = 2;
							}
							else if (this.frameCounter < num102 * 4)
							{
								this.frame = 3;
							}
							else
							{
								this.frameCounter = num102 * 4;
							}
						}
						else
						{
							base.velocity.X *= 0.8f;
							this.frameCounter++;
							int num103 = 3;
							if (this.frameCounter < num103)
							{
								this.frame = 0;
							}
							else if (this.frameCounter < num103 * 2)
							{
								this.frame = 1;
							}
							else if (this.frameCounter < num103 * 3)
							{
								this.frame = 2;
							}
							else if (this.frameCounter < num103 * 4)
							{
								this.frame = 3;
							}
							else if (flag9 || flag10)
							{
								base.velocity.X *= 2f;
								this.frame = 4;
								base.velocity.Y = -6.1f;
								this.frameCounter = 0;
								for (int num104 = 0; num104 < 4; num104++)
								{
									int num105 = Dust.NewDust(new Vector2(base.position.X, base.position.Y + (float)base.height - 2f), base.width, 4, 5);
									Main.dust[num105].velocity += base.velocity;
									Main.dust[num105].velocity *= 0.4f;
								}
							}
							else
							{
								this.frameCounter = num103 * 4;
							}
						}
					}
					else if (base.velocity.Y < 0f)
					{
						this.frameCounter = 0;
						this.frame = 5;
					}
					else
					{
						this.frame = 4;
						this.frameCounter = 3;
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
				else if (this.type == 269)
				{
					if (base.velocity.Y >= 0f && (double)base.velocity.Y <= 0.8)
					{
						if (flag8)
						{
							this.frame = 0;
							this.frameCounter = 0;
						}
						else if ((double)base.velocity.X < -0.8 || (double)base.velocity.X > 0.8)
						{
							int num106 = Dust.NewDust(new Vector2(base.position.X, base.position.Y + (float)base.height - 2f), base.width, 6, 76);
							Main.dust[num106].noGravity = true;
							Main.dust[num106].velocity *= 0.3f;
							Main.dust[num106].noLight = true;
							this.frameCounter += (int)Math.Abs(base.velocity.X);
							this.frameCounter++;
							if (this.frameCounter > 6)
							{
								this.frame++;
								this.frameCounter = 0;
							}
							if (this.frame > 3)
							{
								this.frame = 0;
							}
						}
						else
						{
							this.frame = 0;
							this.frameCounter = 0;
						}
					}
					else
					{
						this.frameCounter = 0;
						this.frame = 2;
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
				else if (this.type == 313)
				{
					int i3 = (int)(base.Center.X / 16f);
					int num107 = (int)(base.Center.Y / 16f);
					int num108 = 0;
					Tile tileSafely2 = Framing.GetTileSafely(i3, num107);
					Tile tileSafely3 = Framing.GetTileSafely(i3, num107 - 1);
					Tile tileSafely4 = Framing.GetTileSafely(i3, num107 + 1);
					if (tileSafely2.wall > 0)
					{
						num108++;
					}
					if (tileSafely3.wall > 0)
					{
						num108++;
					}
					if (tileSafely4.wall > 0)
					{
						num108++;
					}
					if (num108 > 1)
					{
						base.position.Y += base.height;
						base.height = 34;
						base.position.Y -= base.height;
						base.position.X += base.width / 2;
						base.width = 34;
						base.position.X -= base.width / 2;
						Vector2 vector5 = new Vector2(base.position.X + (float)base.width * 0.5f, base.position.Y + (float)base.height * 0.5f);
						float num109 = Main.player[this.owner].Center.X - vector5.X;
						float num111 = Main.player[this.owner].Center.Y - vector5.Y;
						float num112 = (float)Math.Sqrt(num109 * num109 + num111 * num111);
						float num113 = 4f / num112;
						num109 *= num113;
						num111 *= num113;
						if (num112 < 120f)
						{
							base.velocity.X *= 0.9f;
							base.velocity.Y *= 0.9f;
							if ((double)(Math.Abs(base.velocity.X) + Math.Abs(base.velocity.Y)) < 0.1)
							{
								base.velocity *= 0f;
							}
						}
						else
						{
							base.velocity.X = (base.velocity.X * 9f + num109) / 10f;
							base.velocity.Y = (base.velocity.Y * 9f + num111) / 10f;
						}
						if (num112 >= 120f)
						{
							this.spriteDirection = base.direction;
							this.rotation = (float)Math.Atan2(base.velocity.Y * (float)(-base.direction), base.velocity.X * (float)(-base.direction));
						}
						this.frameCounter += (int)(Math.Abs(base.velocity.X) + Math.Abs(base.velocity.Y));
						if (this.frameCounter > 6)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame > 10)
						{
							this.frame = 5;
						}
						if (this.frame < 5)
						{
							this.frame = 10;
						}
					}
					else
					{
						this.rotation = 0f;
						if (base.direction == -1)
						{
							this.spriteDirection = 1;
						}
						if (base.direction == 1)
						{
							this.spriteDirection = -1;
						}
						base.position.Y += base.height;
						base.height = 30;
						base.position.Y -= base.height;
						base.position.X += base.width / 2;
						base.width = 30;
						base.position.X -= base.width / 2;
						if (base.velocity.Y >= 0f && (double)base.velocity.Y <= 0.8)
						{
							if (flag8)
							{
								this.frame = 0;
								this.frameCounter = 0;
							}
							else if ((double)base.velocity.X < -0.8 || (double)base.velocity.X > 0.8)
							{
								this.frameCounter += (int)Math.Abs(base.velocity.X);
								this.frameCounter++;
								if (this.frameCounter > 6)
								{
									this.frame++;
									this.frameCounter = 0;
								}
								if (this.frame > 3)
								{
									this.frame = 0;
								}
							}
							else
							{
								this.frame = 0;
								this.frameCounter = 0;
							}
						}
						else
						{
							this.frameCounter = 0;
							this.frame = 4;
						}
						base.velocity.Y += 0.4f;
						if (base.velocity.Y > 10f)
						{
							base.velocity.Y = 10f;
						}
					}
				}
				else if (this.type >= 390 && this.type <= 392)
				{
					int i4 = (int)(base.Center.X / 16f);
					int num114 = (int)(base.Center.Y / 16f);
					int num115 = 0;
					Tile tileSafely5 = Framing.GetTileSafely(i4, num114);
					Tile tileSafely6 = Framing.GetTileSafely(i4, num114 - 1);
					Tile tileSafely7 = Framing.GetTileSafely(i4, num114 + 1);
					if (tileSafely5.wall > 0)
					{
						num115++;
					}
					if (tileSafely6.wall > 0)
					{
						num115++;
					}
					if (tileSafely7.wall > 0)
					{
						num115++;
					}
					if (num115 > 1)
					{
						base.position.Y += base.height;
						base.height = 34;
						base.position.Y -= base.height;
						base.position.X += base.width / 2;
						base.width = 34;
						base.position.X -= base.width / 2;
						float num116 = 9f;
						float num117 = 40 * (this.minionPos + 1);
						Vector2 v3 = Main.player[this.owner].Center - base.Center;
						if (flag5)
						{
							v3 = vector3;
							num117 = 10f;
						}
						else if (!Collision.CanHitLine(base.Center, 1, 1, Main.player[this.owner].Center, 1, 1))
						{
							this.ai[0] = 1f;
						}
						if (v3.Length() < num117)
						{
							base.velocity *= 0.9f;
							if ((double)(Math.Abs(base.velocity.X) + Math.Abs(base.velocity.Y)) < 0.1)
							{
								base.velocity *= 0f;
							}
						}
						else if (v3.Length() < 800f || !flag5)
						{
							base.velocity = (base.velocity * 9f + v3.SafeNormalize(Vector2.Zero) * num116) / 10f;
						}
						if (v3.Length() >= num117)
						{
							this.spriteDirection = base.direction;
							this.rotation = base.velocity.ToRotation() + (float)Math.PI / 2f;
						}
						else
						{
							this.rotation = v3.ToRotation() + (float)Math.PI / 2f;
						}
						this.frameCounter += (int)(Math.Abs(base.velocity.X) + Math.Abs(base.velocity.Y));
						if (this.frameCounter > 5)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame > 7)
						{
							this.frame = 4;
						}
						if (this.frame < 4)
						{
							this.frame = 7;
						}
					}
					else
					{
						if (!flag6)
						{
							this.rotation = 0f;
						}
						if (base.direction == -1)
						{
							this.spriteDirection = 1;
						}
						if (base.direction == 1)
						{
							this.spriteDirection = -1;
						}
						base.position.Y += base.height;
						base.height = 30;
						base.position.Y -= base.height;
						base.position.X += base.width / 2;
						base.width = 30;
						base.position.X -= base.width / 2;
						if (!flag5 && !Collision.CanHitLine(base.Center, 1, 1, Main.player[this.owner].Center, 1, 1))
						{
							this.ai[0] = 1f;
						}
						if (!flag6 && this.frame >= 4 && this.frame <= 7)
						{
							Vector2 vector6 = Main.player[this.owner].Center - base.Center;
							if (flag5)
							{
								vector6 = vector3;
							}
							float num118 = 0f - vector6.Y;
							if (!(vector6.Y > 0f))
							{
								if (num118 < 120f)
								{
									base.velocity.Y = -10f;
								}
								else if (num118 < 210f)
								{
									base.velocity.Y = -13f;
								}
								else if (num118 < 270f)
								{
									base.velocity.Y = -15f;
								}
								else if (num118 < 310f)
								{
									base.velocity.Y = -17f;
								}
								else if (num118 < 380f)
								{
									base.velocity.Y = -18f;
								}
							}
						}
						if (flag6)
						{
							this.frameCounter++;
							if (this.frameCounter > 3)
							{
								this.frame++;
								this.frameCounter = 0;
							}
							if (this.frame >= 8)
							{
								this.frame = 4;
							}
							if (this.frame <= 3)
							{
								this.frame = 7;
							}
						}
						else if (base.velocity.Y >= 0f && (double)base.velocity.Y <= 0.8)
						{
							if (flag8)
							{
								this.frame = 0;
								this.frameCounter = 0;
							}
							else if ((double)base.velocity.X < -0.8 || (double)base.velocity.X > 0.8)
							{
								this.frameCounter += (int)Math.Abs(base.velocity.X);
								this.frameCounter++;
								if (this.frameCounter > 5)
								{
									this.frame++;
									this.frameCounter = 0;
								}
								if (this.frame > 2)
								{
									this.frame = 0;
								}
							}
							else
							{
								this.frame = 0;
								this.frameCounter = 0;
							}
						}
						else
						{
							this.frameCounter = 0;
							this.frame = 3;
						}
						base.velocity.Y += 0.4f;
						if (base.velocity.Y > 10f)
						{
							base.velocity.Y = 10f;
						}
					}
				}
				else if (this.type == 314)
				{
					if (base.velocity.Y >= 0f && (double)base.velocity.Y <= 0.8)
					{
						if (flag8)
						{
							this.frame = 0;
							this.frameCounter = 0;
						}
						else if ((double)base.velocity.X < -0.8 || (double)base.velocity.X > 0.8)
						{
							this.frameCounter += (int)Math.Abs(base.velocity.X);
							this.frameCounter++;
							if (this.frameCounter > 6)
							{
								this.frame++;
								this.frameCounter = 0;
							}
							if (this.frame > 6)
							{
								this.frame = 1;
							}
						}
						else
						{
							this.frame = 0;
							this.frameCounter = 0;
						}
					}
					else
					{
						this.frameCounter = 0;
						this.frame = 7;
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
				else if (this.type == 319)
				{
					if (base.velocity.Y >= 0f && (double)base.velocity.Y <= 0.8)
					{
						if (flag8)
						{
							this.frame = 0;
							this.frameCounter = 0;
						}
						else if ((double)base.velocity.X < -0.8 || (double)base.velocity.X > 0.8)
						{
							this.frameCounter += (int)Math.Abs(base.velocity.X);
							this.frameCounter++;
							if (this.frameCounter > 8)
							{
								this.frame++;
								this.frameCounter = 0;
							}
							if (this.frame > 5)
							{
								this.frame = 2;
							}
						}
						else
						{
							this.frame = 0;
							this.frameCounter = 0;
						}
					}
					else
					{
						this.frameCounter = 0;
						this.frame = 1;
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
				else if (this.type == 236)
				{
					if (base.velocity.Y >= 0f && (double)base.velocity.Y <= 0.8)
					{
						if (flag8)
						{
							this.frame = 0;
							this.frameCounter = 0;
						}
						else if ((double)base.velocity.X < -0.8 || (double)base.velocity.X > 0.8)
						{
							if (this.frame < 2)
							{
								this.frame = 2;
							}
							this.frameCounter += (int)Math.Abs(base.velocity.X);
							this.frameCounter++;
							if (this.frameCounter > 6)
							{
								this.frame++;
								this.frameCounter = 0;
							}
							if (this.frame > 8)
							{
								this.frame = 2;
							}
						}
						else
						{
							this.frame = 0;
							this.frameCounter = 0;
						}
					}
					else
					{
						this.frameCounter = 0;
						this.frame = 1;
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
				else if (this.type == 499)
				{
					if (base.velocity.Y >= 0f && (double)base.velocity.Y <= 0.8)
					{
						if (flag8)
						{
							this.frame = 0;
							this.frameCounter = 0;
						}
						else if ((double)base.velocity.X < -0.8 || (double)base.velocity.X > 0.8)
						{
							if (this.frame < 2)
							{
								this.frame = 2;
							}
							this.frameCounter += (int)Math.Abs(base.velocity.X);
							this.frameCounter++;
							if (this.frameCounter > 6)
							{
								this.frame++;
								this.frameCounter = 0;
							}
							if (this.frame >= 8)
							{
								this.frame = 2;
							}
						}
						else
						{
							this.frame = 0;
							this.frameCounter = 0;
						}
					}
					else
					{
						this.frameCounter = 0;
						this.frame = 1;
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
				else if (this.type == 765)
				{
					if (base.velocity.Y >= 0f && (double)base.velocity.Y <= 0.8)
					{
						if (flag8)
						{
							this.frame = 0;
							this.frameCounter = 0;
						}
						else if ((double)base.velocity.X < -0.8 || (double)base.velocity.X > 0.8)
						{
							if (this.frame < 1)
							{
								this.frame = 1;
							}
							this.frameCounter += (int)Math.Abs(base.velocity.X);
							this.frameCounter++;
							if (this.frameCounter > 6)
							{
								this.frame++;
								this.frameCounter = 0;
							}
							if (this.frame >= 6)
							{
								this.frame = 1;
							}
						}
						else
						{
							this.frame = 0;
							this.frameCounter = 0;
						}
					}
					else
					{
						this.frame = 0;
						this.frameCounter = 0;
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
				else if (this.type == 266)
				{
					if (base.velocity.Y >= 0f && (double)base.velocity.Y <= 0.8)
					{
						if (flag8)
						{
							this.frameCounter++;
						}
						else
						{
							this.frameCounter += 3;
						}
					}
					else
					{
						this.frameCounter += 5;
					}
					if (this.frameCounter >= 20)
					{
						this.frameCounter -= 20;
						this.frame++;
					}
					if (this.frame > 1)
					{
						this.frame = 0;
					}
					if (base.wet && Main.player[this.owner].position.Y + (float)Main.player[this.owner].height < base.position.Y + (float)base.height && this.localAI[0] == 0f)
					{
						if (base.velocity.Y > -4f)
						{
							base.velocity.Y -= 0.2f;
						}
						if (base.velocity.Y > 0f)
						{
							base.velocity.Y *= 0.95f;
						}
					}
					else
					{
						base.velocity.Y += 0.4f;
					}
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
				else if (this.type == 334)
				{
					if (base.velocity.Y == 0f)
					{
						if (flag8)
						{
							if (this.frame > 0)
							{
								this.frameCounter += 2;
								if (this.frameCounter > 6)
								{
									this.frame++;
									this.frameCounter = 0;
								}
								if (this.frame >= 7)
								{
									this.frame = 0;
								}
							}
							else
							{
								this.frame = 0;
								this.frameCounter = 0;
							}
						}
						else if ((double)base.velocity.X < -0.8 || (double)base.velocity.X > 0.8)
						{
							this.frameCounter += (int)Math.Abs((double)base.velocity.X * 0.75);
							this.frameCounter++;
							if (this.frameCounter > 6)
							{
								this.frame++;
								this.frameCounter = 0;
							}
							if (this.frame >= 7 || this.frame < 1)
							{
								this.frame = 1;
							}
						}
						else if (this.frame > 0)
						{
							this.frameCounter += 2;
							if (this.frameCounter > 6)
							{
								this.frame++;
								this.frameCounter = 0;
							}
							if (this.frame >= 7)
							{
								this.frame = 0;
							}
						}
						else
						{
							this.frame = 0;
							this.frameCounter = 0;
						}
					}
					else if (base.velocity.Y < 0f)
					{
						this.frameCounter = 0;
						this.frame = 2;
					}
					else if (base.velocity.Y > 0f)
					{
						this.frameCounter = 0;
						this.frame = 4;
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
				else if (this.type == 353)
				{
					if (base.velocity.Y == 0f)
					{
						if (flag8)
						{
							this.frame = 0;
							this.frameCounter = 0;
						}
						else if ((double)base.velocity.X < -0.8 || (double)base.velocity.X > 0.8)
						{
							this.frameCounter += (int)Math.Abs(base.velocity.X);
							this.frameCounter++;
							if (this.frameCounter > 6)
							{
								this.frame++;
								this.frameCounter = 0;
							}
							if (this.frame > 9)
							{
								this.frame = 2;
							}
						}
						else
						{
							this.frame = 0;
							this.frameCounter = 0;
						}
					}
					else if (base.velocity.Y < 0f)
					{
						this.frameCounter = 0;
						this.frame = 1;
					}
					else if (base.velocity.Y > 0f)
					{
						this.frameCounter = 0;
						this.frame = 1;
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
				else if (this.type == 111)
				{
					if (base.velocity.Y == 0f)
					{
						if (flag8)
						{
							this.frame = 0;
							this.frameCounter = 0;
						}
						else if ((double)base.velocity.X < -0.8 || (double)base.velocity.X > 0.8)
						{
							this.frameCounter += (int)Math.Abs(base.velocity.X);
							this.frameCounter++;
							if (this.frameCounter > 6)
							{
								this.frame++;
								this.frameCounter = 0;
							}
							if (this.frame >= 7)
							{
								this.frame = 0;
							}
						}
						else
						{
							this.frame = 0;
							this.frameCounter = 0;
						}
					}
					else if (base.velocity.Y < 0f)
					{
						this.frameCounter = 0;
						this.frame = 4;
					}
					else if (base.velocity.Y > 0f)
					{
						this.frameCounter = 0;
						this.frame = 6;
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
				else if (this.type == 112)
				{
					if (base.velocity.Y == 0f)
					{
						if (flag8)
						{
							this.frame = 0;
							this.frameCounter = 0;
						}
						else if ((double)base.velocity.X < -0.8 || (double)base.velocity.X > 0.8)
						{
							this.frameCounter += (int)Math.Abs(base.velocity.X);
							this.frameCounter++;
							if (this.frameCounter > 6)
							{
								this.frame++;
								this.frameCounter = 0;
							}
							if (this.frame >= 3)
							{
								this.frame = 0;
							}
						}
						else
						{
							this.frame = 0;
							this.frameCounter = 0;
						}
					}
					else if (base.velocity.Y != 0f)
					{
						this.frameCounter = 0;
						this.frame = 1;
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
				else if (this.type == 127)
				{
					if (base.velocity.Y == 0f)
					{
						if (flag8)
						{
							this.frame = 0;
							this.frameCounter = 0;
						}
						else if ((double)base.velocity.X < -0.1 || (double)base.velocity.X > 0.1)
						{
							this.frameCounter += (int)Math.Abs(base.velocity.X);
							this.frameCounter++;
							if (this.frameCounter > 6)
							{
								this.frame++;
								this.frameCounter = 0;
							}
							if (this.frame > 5)
							{
								this.frame = 0;
							}
						}
						else
						{
							this.frame = 0;
							this.frameCounter = 0;
						}
					}
					else
					{
						this.frame = 0;
						this.frameCounter = 0;
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
				else if (this.type == 200)
				{
					if (base.velocity.Y == 0f)
					{
						if (flag8)
						{
							this.frame = 0;
							this.frameCounter = 0;
						}
						else if ((double)base.velocity.X < -0.1 || (double)base.velocity.X > 0.1)
						{
							this.frameCounter += (int)Math.Abs(base.velocity.X);
							this.frameCounter++;
							if (this.frameCounter > 6)
							{
								this.frame++;
								this.frameCounter = 0;
							}
							if (this.frame > 5)
							{
								this.frame = 0;
							}
						}
						else
						{
							this.frame = 0;
							this.frameCounter = 0;
						}
					}
					else
					{
						this.rotation = base.velocity.X * 0.1f;
						this.frameCounter++;
						if (base.velocity.Y < 0f)
						{
							this.frameCounter += 2;
						}
						if (this.frameCounter > 6)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame > 9)
						{
							this.frame = 6;
						}
						if (this.frame < 6)
						{
							this.frame = 6;
						}
					}
					base.velocity.Y += 0.1f;
					if (base.velocity.Y > 4f)
					{
						base.velocity.Y = 4f;
					}
				}
				else if (this.type == 208)
				{
					if (base.velocity.Y == 0f && flag8)
					{
						if (Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2) < base.position.X + (float)(base.width / 2))
						{
							base.direction = -1;
						}
						else if (Main.player[this.owner].position.X + (float)(Main.player[this.owner].width / 2) > base.position.X + (float)(base.width / 2))
						{
							base.direction = 1;
						}
						this.rotation = 0f;
						this.frame = 0;
					}
					else
					{
						this.rotation = base.velocity.X * 0.075f;
						this.frameCounter++;
						if (this.frameCounter > 6)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame > 4)
						{
							this.frame = 1;
						}
						if (this.frame < 1)
						{
							this.frame = 1;
						}
					}
					base.velocity.Y += 0.1f;
					if (base.velocity.Y > 4f)
					{
						base.velocity.Y = 4f;
					}
				}
				else if (this.type == 209)
				{
					if (this.alpha > 0)
					{
						this.alpha -= 5;
						if (this.alpha < 0)
						{
							this.alpha = 0;
						}
					}
					if (base.velocity.Y == 0f)
					{
						if (flag8)
						{
							this.frame = 0;
							this.frameCounter = 0;
						}
						else if ((double)base.velocity.X < -0.1 || (double)base.velocity.X > 0.1)
						{
							this.frameCounter += (int)Math.Abs(base.velocity.X);
							this.frameCounter++;
							if (this.frameCounter > 6)
							{
								this.frame++;
								this.frameCounter = 0;
							}
							if (this.frame > 11)
							{
								this.frame = 2;
							}
							if (this.frame < 2)
							{
								this.frame = 2;
							}
						}
						else
						{
							this.frame = 0;
							this.frameCounter = 0;
						}
					}
					else
					{
						this.frame = 1;
						this.frameCounter = 0;
						this.rotation = 0f;
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
				else if (this.type == 324)
				{
					if (base.velocity.Y == 0f)
					{
						if ((double)base.velocity.X < -0.1 || (double)base.velocity.X > 0.1)
						{
							this.frameCounter += (int)Math.Abs(base.velocity.X);
							this.frameCounter++;
							if (this.frameCounter > 6)
							{
								this.frame++;
								this.frameCounter = 0;
							}
							if (this.frame > 5)
							{
								this.frame = 2;
							}
							if (this.frame < 2)
							{
								this.frame = 2;
							}
						}
						else
						{
							this.frame = 0;
							this.frameCounter = 0;
						}
					}
					else
					{
						this.frameCounter = 0;
						this.frame = 1;
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 14f)
					{
						base.velocity.Y = 14f;
					}
				}
				else if (this.type == 210)
				{
					if (base.velocity.Y == 0f)
					{
						if ((double)base.velocity.X < -0.1 || (double)base.velocity.X > 0.1)
						{
							this.frameCounter += (int)Math.Abs(base.velocity.X);
							this.frameCounter++;
							if (this.frameCounter > 6)
							{
								this.frame++;
								this.frameCounter = 0;
							}
							if (this.frame > 6)
							{
								this.frame = 0;
							}
						}
						else
						{
							this.frame = 0;
							this.frameCounter = 0;
						}
					}
					else
					{
						this.rotation = base.velocity.X * 0.05f;
						this.frameCounter++;
						if (this.frameCounter > 6)
						{
							this.frame++;
							this.frameCounter = 0;
						}
						if (this.frame > 11)
						{
							this.frame = 7;
						}
						if (this.frame < 7)
						{
							this.frame = 7;
						}
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
				else if (this.type == 398)
				{
					if (base.velocity.Y == 0f)
					{
						if (flag8)
						{
							this.frame = 0;
							this.frameCounter = 0;
						}
						else if ((double)base.velocity.X < -0.8 || (double)base.velocity.X > 0.8)
						{
							this.frameCounter += (int)Math.Abs(base.velocity.X);
							this.frameCounter++;
							if (this.frameCounter > 6)
							{
								this.frame++;
								this.frameCounter = 0;
							}
							if (this.frame >= 5)
							{
								this.frame = 0;
							}
						}
						else
						{
							this.frame = 0;
							this.frameCounter = 0;
						}
					}
					else if (base.velocity.Y != 0f)
					{
						this.frameCounter = 0;
						this.frame = 5;
					}
					base.velocity.Y += 0.4f;
					if (base.velocity.Y > 10f)
					{
						base.velocity.Y = 10f;
					}
				}
			}
		}
	}
}
*/