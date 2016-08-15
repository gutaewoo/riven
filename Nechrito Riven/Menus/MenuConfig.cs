#region

using LeagueSharp.Common;

#endregion

namespace NechritoRiven.Menus
{
    internal class MenuConfig : Core.Core
    {
        public static Menu Config;
        public static Menu TargetSelectorMenu;
        public static string menuName = "Nechrito Riven";

        public static void LoadMenu()
        {
            Config = new Menu(menuName, menuName, true);

            TargetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(TargetSelectorMenu);
            Config.AddSubMenu(TargetSelectorMenu);

            var orbwalker = new Menu("Orbwalker", "rorb");
            _orbwalker = new Orbwalking.Orbwalker(orbwalker);
            Config.AddSubMenu(orbwalker);

            var animation = new Menu("Animation", "Animation");
            var emoteMenu = new Menu("Emote", "Emote");
            animation.AddItem(new MenuItem("qReset", "Fast & Legit Q").SetValue(true)).SetTooltip("Uses dance in animation, off will not dance & look more legit");
            emoteMenu.AddItem(new MenuItem("Qstrange", "Enable").SetValue(false)).SetTooltip("Enables Emote");
            emoteMenu.AddItem(new MenuItem("animLaugh", "Laugh").SetValue(false));
            emoteMenu.AddItem(new MenuItem("animTaunt", "Taunt").SetValue(false));
            emoteMenu.AddItem(new MenuItem("animTalk", "Joke").SetValue(false));
            emoteMenu.AddItem(new MenuItem("animDance", "Dance").SetValue(false));
            animation.AddSubMenu(emoteMenu);
            Config.AddSubMenu(animation);

            var combo = new Menu("Combo", "Combo");
            combo.AddItem(new MenuItem("ignite", "Auto Ignite").SetValue(true)).SetTooltip("Auto Ignite When target is killable");
            combo.AddItem(new MenuItem("AlwaysR", "Force R").SetValue(new KeyBind('G', KeyBindType.Toggle))).SetTooltip("Off will only use R when target is killable");
            combo.AddItem(new MenuItem("AlwaysF", "Force Flash").SetValue(new KeyBind('L', KeyBindType.Toggle))).SetTooltip("Off Will only use Flash when target is killable");
            combo.AddItem(new MenuItem("ComboE", "Use E").SetValue(true));
            Config.AddSubMenu(combo);

            var lane = new Menu("Lane", "Lane");
            lane.AddItem(new MenuItem("LaneQ", "Use Q").SetValue(true));
            lane.AddItem(new MenuItem("LaneW", "Use W").SetValue(true));
            lane.AddItem(new MenuItem("LaneE", "Use E").SetValue(true));
            Config.AddSubMenu(lane);

            var jngl = new Menu("Jungle", "Jungle");
            jngl.AddItem(new MenuItem("JungleQ", "Use Q").SetValue(true));
            jngl.AddItem(new MenuItem("JungleW", "Use W").SetValue(true));
            jngl.AddItem(new MenuItem("JungleE", "Use E").SetValue(true));
            Config.AddSubMenu(jngl);

            var misc = new Menu("Misc", "Misc");
            misc.AddItem(new MenuItem("GapcloserMenu", "Anti-Gapcloser").SetValue(true));
            misc.AddItem(new MenuItem("InterruptMenu", "Interrupter").SetValue(true));
            misc.AddItem(new MenuItem("KeepQ", "Keep Q Alive").SetValue(true));
            misc.AddItem(new MenuItem("QD", "Q1, Q2 Delay").SetValue(new Slider(29, 23, 43)));
            misc.AddItem(new MenuItem("QLD", "Q3 Delay").SetValue(new Slider(39, 36, 53)));
            Config.AddSubMenu(misc);

            var trinket = new Menu("Trinket", "Trinket");
            trinket.AddItem(new MenuItem("Buytrinket", "Buy Trinket").SetValue(true).SetTooltip("Will buy trinket"));
            trinket.AddItem(new MenuItem("Trinketlist", "Choose Trinket").SetValue(new StringList(new[] { "Oracle Alternation", "Farsight Alternation"  })));
            Config.AddSubMenu(trinket);

            var draw = new Menu("Draw", "Draw");
            draw.AddItem(new MenuItem("FleeSpot", "Draw Flee Spots").SetValue(true));
            draw.AddItem(new MenuItem("Dind", "Damage Indicator").SetValue(true));
            draw.AddItem(new MenuItem("DrawForceFlash", "Flash Status").SetValue(true));
            draw.AddItem(new MenuItem("DrawAlwaysR", "R Status").SetValue(true));
            draw.AddItem(new MenuItem("DrawCB", "Combo Engage").SetValue(false));
            draw.AddItem(new MenuItem("DrawBT", "Burst Engage").SetValue(false));
            draw.AddItem(new MenuItem("DrawFH", "FastHarass Engage").SetValue(false));
            draw.AddItem(new MenuItem("DrawHS", "Harass Engage").SetValue(false));
            Config.AddSubMenu(draw);

            var flee = new Menu("Flee", "Flee");
            flee.AddItem(new MenuItem("WallFlee", "WallJump in Flee").SetValue(true).SetTooltip("Jumps over walls in flee mode"));
            Config.AddSubMenu(flee);

            var skin = new Menu("SkinChanger", "SkinChanger");
            skin.AddItem(new MenuItem("UseSkin", "Use SkinChanger").SetValue(false)).SetTooltip("Toggles Skinchanger");
            skin.AddItem(new MenuItem("Skin", "Skin").SetValue(new StringList(new[] { "Default", "Redeemed", "Crimson Elite", "Battle Bunny", "Championship", "Dragonblade", "Arcade" })));
            Config.AddSubMenu(skin);

            var qmove = new Menu("Q Move", "Q Move");
            qmove.AddItem(new MenuItem("QMove", "Q Move").SetValue(new KeyBind('K', KeyBindType.Press))).SetTooltip("Will Q Move to mouse");
            Config.AddSubMenu(qmove);

            var credit = new Menu("Credit", "Credit");
            credit.AddItem(new MenuItem("nechrito", "Originally made by Hoola, completely re-written By Nechrito"));
            Config.AddSubMenu(credit);

            Config.AddToMainMenu();
        }

        public static bool GapcloserMenu => Config.Item("GapcloserMenu").GetValue<KeyBind>().Active;
        public static bool InterruptMenu => Config.Item("InterruptMenu").GetValue<KeyBind>().Active;

        public static bool QMove => Config.Item("QMove").GetValue<KeyBind>().Active;

        public static StringList ItemList => Config.Item("ItemList").GetValue<StringList>();
        public static StringList Trinketlist => Config.Item("Trinketlist").GetValue<StringList>();

        public static bool Buytrinket => Config.Item("Buytrinket").GetValue<bool>();
        public static bool FleeSpot => Config.Item("FleeSpot").GetValue<bool>();
        public static bool WallFlee => Config.Item("WallFlee").GetValue<bool>();
        public static bool jnglQ => Config.Item("JungleQ").GetValue<bool>();
        public static bool jnglW => Config.Item("JungleW").GetValue<bool>();
        public static bool jnglE => Config.Item("JungleE").GetValue<bool>();
        public static bool UseSkin => Config.Item("UseSkin").GetValue<bool>();
        public static bool AlwaysF => Config.Item("AlwaysF").GetValue<KeyBind>().Active;
        public static bool ignite => Config.Item("ignite").GetValue<bool>();
        public static bool ForceFlash => Config.Item("DrawForceFlash").GetValue<bool>();
        public static bool IreliaLogic => Config.Item("IreliaLogic").GetValue<bool>();
        public static bool QReset => Config.Item("qReset").GetValue<bool>();
        public static bool Dind => Config.Item("Dind").GetValue<bool>();
        public static bool DrawCb => Config.Item("DrawCB").GetValue<bool>();
        public static bool AnimLaugh => Config.Item("animLaugh").GetValue<bool>();
        public static bool AnimTaunt => Config.Item("animTaunt").GetValue<bool>();
        public static bool AnimDance => Config.Item("animDance").GetValue<bool>();
        public static bool AnimTalk => Config.Item("animTalk").GetValue<bool>();
        public static bool DrawAlwaysR => Config.Item("DrawAlwaysR").GetValue<bool>();
        public static bool KeepQ => Config.Item("KeepQ").GetValue<bool>();
        public static bool DrawFh => Config.Item("DrawFH").GetValue<bool>();
        public static bool DrawHs => Config.Item("DrawHS").GetValue<bool>();
        public static bool DrawBt => Config.Item("DrawBT").GetValue<bool>();
        public static bool AlwaysR => Config.Item("AlwaysR").GetValue<KeyBind>().Active;
        public static int Qd => Config.Item("QD").GetValue<Slider>().Value;
        public static int Qld => Config.Item("QLD").GetValue<Slider>().Value;
        public static bool LaneW => Config.Item("LaneW").GetValue<bool>();
        public static bool LaneE => Config.Item("LaneE").GetValue<bool>();
        public static bool Qstrange => Config.Item("Qstrange").GetValue<bool>();
        public static bool LaneQ => Config.Item("LaneQ").GetValue<bool>();
        public static bool ComboE => Config.Item("ComboE").GetValue<bool>();
    }
}
