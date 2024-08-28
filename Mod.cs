using Kitchen;
using KitchenLib;
using KitchenLib.Event;
using KitchenLib.Preferences;
using System;
using System.Reflection; 
using UnityEngine;

namespace KitchenRandomCustomerColor
{
    public class Mod : BaseMod
    {
        public const string MOD_GUID = "freshpepperino.plateUp.randomCustomerColors";

        public const string MOD_NAME = "Random Customer Colors";

        public const string MOD_VERSION = "0.2.1";

        public const string MOD_AUTHOR = "freshpepperino";

        public const string MOD_GAMEVERSION = ">=1.1.1";

        public static PreferenceManager PreferenceManager;

        public static PreferenceBool CustomerPreference;

        public static PreferenceBool CatPreference;
        public static PreferenceBool RandomByGroupPreference;

        public Mod()
            : base(MOD_GUID, MOD_NAME, MOD_AUTHOR, MOD_VERSION, MOD_GAMEVERSION, Assembly.GetExecutingAssembly()) { }

        protected override void OnInitialise()
        { 
            PreferenceManager = new PreferenceManager(MOD_GUID);

            CustomerPreference = PreferenceManager.RegisterPreference(new PreferenceBool("EnableCustomerRandomColors", defaultValue: true));
            CatPreference = PreferenceManager.RegisterPreference(new PreferenceBool("EnableCatRandomColors", defaultValue: true));
            RandomByGroupPreference = PreferenceManager.RegisterPreference(new PreferenceBool("EnableRandomizeByGroup", defaultValue: false));
            PreferenceManager.Load(); 
            ModsPreferencesMenu<MainMenuAction>.RegisterMenu("Random Customer Colors", typeof(SettingsMenu<MainMenuAction>), typeof(MainMenuAction));
            ModsPreferencesMenu<PauseMenuAction>.RegisterMenu("Random Customer Colors", typeof(SettingsMenu<PauseMenuAction>), typeof(PauseMenuAction));

            Events.PreferenceMenu_MainMenu_CreateSubmenusEvent = (EventHandler<PreferenceMenu_CreateSubmenusArgs<MainMenuAction>>)Delegate.Combine(Events.PreferenceMenu_MainMenu_CreateSubmenusEvent, (EventHandler<PreferenceMenu_CreateSubmenusArgs<MainMenuAction>>)delegate (object s, PreferenceMenu_CreateSubmenusArgs<MainMenuAction> args)
            {
                args.Menus.Add(typeof(SettingsMenu<MainMenuAction>), new SettingsMenu<MainMenuAction>(args.Container, args.Module_list));
            });
            Events.PreferenceMenu_PauseMenu_CreateSubmenusEvent = (EventHandler<PreferenceMenu_CreateSubmenusArgs<PauseMenuAction>>)Delegate.Combine(Events.PreferenceMenu_PauseMenu_CreateSubmenusEvent, (EventHandler<PreferenceMenu_CreateSubmenusArgs<PauseMenuAction>>)delegate (object s, PreferenceMenu_CreateSubmenusArgs<PauseMenuAction> args)
            {
                args.Menus.Add(typeof(SettingsMenu<PauseMenuAction>), new SettingsMenu<PauseMenuAction>(args.Container, args.Module_list));
            });
        }

        public static void LogInfo(string _log)
        {
            Debug.Log("[Random Customer Colors] " + _log);
        }

        public static void LogError(string _log)
        {
            Debug.LogError("[Random Customer Colors] " + _log);
        }

        public static void LogWarning(string _log)
        {
            Debug.LogError("[Random Customer Colors] " + _log);
        } 
    }
}
