﻿using LeagueSharp.Common;

namespace Nechrito_Gragas
{
    class MenuConfig
    {
        public static Menu Config;
        public static Menu TargetSelectorMenu;
        public static Orbwalking.Orbwalker _orbwalker;

        public static string menuName = "Nechrito Gragas";
        public static void LoadMenu()
        {
            Config = new Menu(menuName, menuName, true);

            TargetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(TargetSelectorMenu);
            Config.AddSubMenu(TargetSelectorMenu);
            var orbwalker = new Menu("Orbwalker", "rorb");
            _orbwalker = new Orbwalking.Orbwalker(orbwalker);
            Config.AddSubMenu(orbwalker);

            //COMBOS ETC HERE
            var combo = new Menu("Combo", "Combo");
            combo.AddItem(new MenuItem("OnlyR", "Only R Selected Target")).SetValue(true);
            combo.AddItem(new MenuItem("ComboR", "Use R")).SetValue(true);
            Config.AddSubMenu(combo);

            var Lane = new Menu("Lane", "Lane");
            Lane.AddItem(new MenuItem("LaneQ", "Use Q")).SetValue(true);
            Lane.AddItem(new MenuItem("LaneW", "Use W")).SetValue(true);
            Lane.AddItem(new MenuItem("LaneE", "Use E")).SetValue(true);
            Config.AddSubMenu(Lane);

            var misc = new Menu("Misc", "Misc");
            misc.AddItem(new MenuItem("SmiteJngl", "Auto Smite")).SetValue(true).SetTooltip("Smites Dragon & Baron when killable");
            misc.AddItem(new MenuItem("UseSkin", "Use Skinchanger")).SetValue(true).SetTooltip("Toggles Skinchanger");
            misc.AddItem(new MenuItem("Skin", "Skin").SetValue(new StringList(new[] { "Default", "Scuba Gragas", "Hillbilly Gragas", "Santa Gragas", "Gragas, Esq", "Vandal Gragas", "Oktoberfest Gragas", "Superfan Gragas", "Fnatic Gragas", "Gragas Caskbreaker" })));
            Config.AddSubMenu(misc);

            var draw = new Menu("Draw", "Draw");
            draw.AddItem(new MenuItem("dind", "Damage Indicator")).SetValue(true);
            Config.AddSubMenu(draw);

            SPrediction.Prediction.Initialize(Config);
            Config.AddToMainMenu();
        }
        public static bool UseSkin => Config.Item("UseSkin").GetValue<bool>();
        public static bool OnlyR => Config.Item("OnlyR").GetValue<bool>();
        public static bool ComboR => Config.Item("ComboR").GetValue<bool>();
        public static bool LaneQ => Config.Item("LaneQ").GetValue<bool>();
        public static bool LaneW => Config.Item("LaneW").GetValue<bool>();
        public static bool LaneE => Config.Item("LaneE").GetValue<bool>();
        public static bool dind => Config.Item("dind").GetValue<bool>();
    }
}
