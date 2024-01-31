// system
using System.Collections;
// bepinex
using BepInEx;
using BepInEx.Logging;
using GameNetcodeStuff;
// harmony
using HarmonyLib;
// unity
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
// reedybee
using LCRevolverMod.Scripts;

namespace LCRevolverMod
{
    [BepInPlugin(modGUID, modName, modVersion)]
    [BepInDependency(LethalLib.Plugin.ModGUID)]
    public class RevolverModBase : BaseUnityPlugin
    {
        private const string modGUID = "reedybee.LCRevolverMod";
        private const string modName = "LC Revolver Mod";
        private const string modVersion = "0.0.1";

        private readonly Harmony harmony = new Harmony(modGUID);

        public static RevolverModBase instance;

        public static ManualLogSource log;
        // called once upon game startup
        public void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            log = BepInEx.Logging.Logger.CreateLogSource(modGUID);

            log.LogInfo($"{modName} has awoken.");

            SceneManager.sceneLoaded += OnSceneLoaded;

            harmony.PatchAll();
;        }
        // callback when new scene loads
        public void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
        {
            log.LogMessage("Scene Loaded " + scene.name + " with " + mode + " mode.");
            log.LogMessage(scene.name);
            if (scene.name == "SampleSceneRelay")
            {

            }
        }
    }
} 
