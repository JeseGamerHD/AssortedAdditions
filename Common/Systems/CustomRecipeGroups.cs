﻿using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace AssortedAdditions.Common.Systems
{
    internal class CustomRecipeGroups : ModSystem
    {
        public override void AddRecipeGroups()
        {
            RecipeGroup drinks = new RecipeGroup(() => "Any Drink or Beverage",
                ItemID.CreamSoda,
                ItemID.Milkshake,
                ItemID.AppleJuice,
                ItemID.GrapeJuice,
                ItemID.Lemonade,
                ItemID.BananaDaiquiri,
                ItemID.PeachSangria,
                ItemID.PinaColada,
                ItemID.TropicalSmoothie,
                ItemID.BloodyMoscato,
                ItemID.SmoothieofDarkness,
                ItemID.PrismaticPunch,
                ItemID.FruitJuice,
                ItemID.Teacup,
                ItemID.MilkCarton,
                ItemID.CoffeeCup,
                ItemID.JojaCola
                );

            RecipeGroup.RegisterGroup("AssortedAdditions:Drinks", drinks);


            RecipeGroup dungeonVases = new RecipeGroup(() => "Any dungeon vase",
                ItemID.BlueDungeonVase,
                ItemID.GreenDungeonVase,
                ItemID.PinkDungeonVase
                );
            RecipeGroup.RegisterGroup("AssortedAdditions:DungeonVases", dungeonVases);
        }
    }
}
