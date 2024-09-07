using Kitchen;
using KitchenLib;
using KitchenLib.Event;
using RandomCustomerColors.Preferences;
using RandomCustomerColors.Menus;
using System;
using System.Reflection; 
using UnityEngine;

namespace RandomCustomerColors
{
    public class Mod : BaseMod
    {
        public const string MOD_GUID = "freshpepperino.plateUp.randomCustomerColors";

        public const string MOD_NAME = "Random Customer Colors";

        public const string MOD_VERSION = "1.0.0";

        public const string MOD_AUTHOR = "freshpepperino";

        public const string MOD_GAMEVERSION = ">=1.2.0"; 

        public Mod()
            : base(MOD_GUID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, MOD_GAMEVERSION, Assembly.GetExecutingAssembly()) { }

        protected override void OnInitialise()
        { 
            CustomerColorPreferences.RegisterPreferences(MOD_GUID); 
            ModsPreferencesMenu<MenuAction>.RegisterMenu(MOD_NAME, typeof(SettingsMenu<MenuAction>), typeof(MenuAction));   
            Events.MainMenuView_SetupMenusEvent += (s, args) =>
            {
                args.addMenu.Invoke(args.instance, new object[] { typeof(SettingsMenu<MenuAction>), new SettingsMenu<MenuAction>(args.instance.ButtonContainer, args.module_list) });
            };

            Events.PlayerPauseView_SetupMenusEvent += (s, args) =>
            {
                args.addMenu.Invoke(args.instance, new object[] { typeof(SettingsMenu<MenuAction>), new SettingsMenu<MenuAction>(args.instance.ButtonContainer, args.module_list) });
            }; 

        }

        public static void LogInfo(string _log)
        {
            Debug.Log($"[Random Customer Colors] {DateTime.Now.ToString()} " + _log);
        }

        public static void LogError(string _log)
        {
            Debug.LogError($"[Random Customer Colors] {DateTime.Now.ToString()} " + _log);
        }

        public static void LogWarning(string _log)
        {
            Debug.LogError($"[Random Customer Colors] {DateTime.Now.ToString()} " + _log);
        } 
    }
}
