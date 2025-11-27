using BepInEx;
using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.Mono;
using HarmonyLib;
using System;
using System.ComponentModel;
using System.Reflection;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;
using static UnityEngine.InputSystem.InputRemoting;
using System.Linq;

namespace AlwaysBeNaked
{
    [BepInPlugin("com.wolfitdm.AlwaysBeNaked", "AlwaysBeNaked Plugin", "1.0.0.0")]
    public class AlwaysBeNaked : BaseUnityPlugin
    {
        internal static new ManualLogSource Logger;
        public AlwaysBeNaked()
        {
        }

        public static Type MyGetType(string originalClassName)
        {
            return Type.GetType(originalClassName + ",Assembly-CSharp");
        }

        public static Type oldMyGetType(string originalClassName)
        {
            Type originalClass = null;

            try
            {
                switch (originalClassName)
                {
                    case "GlobalObjects_UW":
                        {
                            originalClass = typeof(GlobalObjects_UW);
                        }
                        break;

                    case "Character_Clothes_UW":
                        {
                            originalClass = typeof(Character_Clothes_UW);
                        }
                        break;

                    case "LevelManager_UW":
                        {
                            originalClass = typeof(LevelManager_UW);
                        }
                        break;

                    default:
                        {
                            originalClass = MyGetType(originalClassName);
                        }
                        break;
                }
            }
            catch (Exception e)
            {
                originalClass = null;
            }


            if (originalClass == null)
            {
                Logger.LogInfo($"GetType {originalClassName} == null");
                return null;
            }

            return originalClass;
        }

        public static void PatchHarmonyMethod(string originalClassName, string originalMethodName, string patchedMethodName, bool usePrefix, bool usePostfix)
        {
            // Create a new Harmony instance with a unique ID
            var harmony = new Harmony("com.wolfitdm.AlwaysBeNaked");

            Type originalClass = null;

            originalClass = MyGetType(originalClassName);

            if (originalClass == null)
            {
                Logger.LogInfo($"GetType {originalClassName} == null");
                return;
            }

            // Or apply patches manually
            MethodInfo original = AccessTools.Method(originalClass, originalMethodName);

            if (original == null)
            {
                Logger.LogInfo($"AccessTool.Method original {originalClassName} == null");
                return;
            }

            MethodInfo patched = AccessTools.Method(typeof(AlwaysBeNaked), patchedMethodName);

            if (patched == null)
            {
                Logger.LogInfo($"AccessTool.Method patched {patchedMethodName} == null");
                return;

            }

            HarmonyMethod patchedMethod = new HarmonyMethod(patched);
            var prefixMethod = usePrefix ? patchedMethod : null;
            var postfixMethod = usePostfix ? patchedMethod : null;

            harmony.Patch(original,
                prefix: prefixMethod,
                postfix: postfixMethod);
        }

        public static void PatchHarmonyMethodPostfix(string originalClassName, string originalMethodName)
        {
            PatchHarmonyMethod(originalClassName, originalMethodName, originalMethodName, false, true);
        }

        private void Awake()
        {
            // Plugin startup logic
            Logger = base.Logger;
            //Harmony.CreateAndPatchAll(typeof(AlwaysBeNaked));
            PatchHarmonyMethodPostfix("Character_Clothes_UW", "CheckAllClothes_Player");
            PatchHarmonyMethodPostfix("Character_Clothes_UW", "StartScripts");
            PatchHarmonyMethodPostfix("LevelManager_UW", "LoadLevel");
            PatchHarmonyMethodPostfix("GlobalObjects_UW", "CheckVersionEXCHBA");
            Logger.LogInfo($"Plugin AlwaysBeNaked is loaded!");
        }


        //[HarmonyPatch(typeof(Character_Clothes_UW), "CheckAllClothes_Player")] // Specify target method with HarmonyPatch attribute
        //[HarmonyPostfix]                              // There are different patch types. Prefix code runs before original code
        static void CheckAllClothes_Player(bool updateBreasts, bool updateCurs, object __instance)
        {
            SetPlayerHavePantsAndSHirt(__instance);
        }

        //[HarmonyPatch(typeof(Character_Clothes_UW), "StartScripts")] // Specify target method with HarmonyPatch attribute
        //[HarmonyPostfix]                                            // There are different patch types. Prefix code runs before original code
        static void StartScripts(object __instance)
        {
            SetPlayerHavePantsAndSHirt(__instance);
        }

        static void SetPlayerHavePantsAndSHirt(object __instance)
        {
            try
            {
                Character_Clothes_UW _this = (Character_Clothes_UW)__instance;
                if (_this.isPlayer)
                {
                    _this.hasPantsNow = true;
                    _this.hasShirtNow = true;
                    _this.hasBraNow = true;
                    _this.hasPantiesNow = true;
                    _this.hasShoesNow = true;
                    _this.hasHatNow = true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Logger.LogInfo(e.ToString());
            }

            try
            {
                Character_Clothes_UW _this = (Character_Clothes_UW)__instance;
                bool isPlayer = (bool)Character_Clothes_UW_getVar(_this, "isPlayer");
                if (isPlayer)
                {
                    Character_Clothes_UW_setProperty(_this, "hasPantsNow", true);
                    Character_Clothes_UW_setProperty(_this, "hasShirtNow", true);
                    Character_Clothes_UW_setProperty(_this, "hasBraNow", true);
                    Character_Clothes_UW_setProperty(_this, "hasPantiesNow", true);
                    Character_Clothes_UW_setProperty(_this, "hasShoesNow", true);
                    Character_Clothes_UW_setProperty(_this, "hasHatNow", true);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Logger.LogInfo(e.ToString());
            }
        }

        //[HarmonyPatch(typeof(LevelManager_UW), "LevelManager_LoadLevel")]
        //[HarmonyPostfix]
        static void LoadLevel(ELevels newLevel, int spawnNum, bool checkEvents, object __instance)
        {
            GlobalObjects_UW_cctor_run();
        }

        //[HarmonyPatch(typeof(GlobalObjects_UW), "CheckVersionEXCHBA")]
        //[HarmonyPostfix]
        static void CheckVersionEXCHBA()
        {
            GlobalObjects_UW_cctor_run();
        }

        static void setOldSettings()
        {
            //init settings
            /*try
            {
                GlobalObjects_UW.isDevBuild = true;
                GlobalObjects_UW.isDebugMode = true;
                GlobalObjects_UW.showMovePoints = true;
                GlobalObjects_UW.testAnimVals = false;
                GlobalObjects_UW.forceNudity = 0;
                GlobalObjects_UW.forceDickType = EDickType.None;
                GlobalObjects_UW.allowCumShake = true;
                GlobalObjects_UW.disableScreenFade = true;
                GlobalObjects_UW.forceSeasonType = ESeasonType.NONE;
                GlobalObjects_UW.forceHolidayType = EHolidayType.NONE;
                GlobalObjects_UW.disablePregnancy = true;
                GlobalObjects_UW.pregnancyInfoCheat = true;
                GlobalObjects_UW.pregnantBelly = true;
                GlobalObjects_UW.hidePregnantBelly = true;
                GlobalObjects_UW.isRandomNPCSize = true;
                GlobalObjects_UW.boobBouncingPower = 0;
                GlobalObjects_UW.eyesOverHairs = true;
                GlobalObjects_UW.dontCleanSperm = true;
                GlobalObjects_UW.stopTimeFlow = true;
                GlobalObjects_UW.lockLW = EThresholdsLewdness.Off;
                GlobalObjects_UW.npcDickSize = 1f;
                GlobalObjects_UW.isSteam = false;
                GlobalObjects_UW.cheatsOn = true;
                GlobalObjects_UW.isVersionCheats = true;
                GlobalObjects_UW.isVersionExtended = true;
                GlobalObjects_UW.enc = 78;
                GlobalObjects_UW.xrayDickSharesNPCSkin = true;
                GlobalObjects_UW.moveOnInteraction = true;
                GlobalObjects_UW.curDifficulty = EDifficultyTypes.NORMAL;
                GlobalObjects_UW.allowIdleInDialogue = true;
                GlobalObjects_UW.backgroundSkipsDialogue = true;
                GlobalObjects_UW.allowedBodyTypes = 0;
                GlobalObjects_UW.disableRandomNPCs = false;
                GlobalObjects_UW.isRunToggled = true;
                GlobalObjects_UW.showInternalDebug = true;
                GlobalObjects_UW.isAndroid = false;
                GlobalObjects_UW.isSuperDebugging = true;
                GlobalObjects_UW.preloadDialogues = true;
                GlobalObjects_UW.biggerAndroidUI = false;
                GlobalObjects_UW.tooltipFontSize = 30;
                GlobalObjects_UW.seasonType = ESeasonType.NONE;
                GlobalObjects_UW.holidayType = EHolidayType.NONE;
                GlobalObjects_UW.moodFilter = true;
                GlobalObjects_UW.useUnityFont = false;
                GlobalObjects_UW.enableHolidays = true;
                GlobalObjects_UW.useKeyboardOnly = false;
                GlobalObjects_UW.allowMaleMoans = true;
                GlobalObjects_UW.showUseIcons = true;
                GlobalObjects_UW.showXRay = true;
                GlobalObjects_UW.allowXRay = true;
                GlobalObjects_UW.showCutIns = true;
                GlobalObjects_UW.showCutInsXRay = true;
                GlobalObjects_UW.showMoans = true;
                GlobalObjects_UW.allowHats = true;
                GlobalObjects_UW.allowGlasses = true;
                GlobalObjects_UW.isClock12 = false;
                GlobalObjects_UW.isDialTransparent = true;
                GlobalObjects_UW.isAutoAnimState = true;
                GlobalObjects_UW.gameScreenModeStr = "Windowed";
                GlobalObjects_UW.gameResolutionWidth = 1920;
                GlobalObjects_UW.gameResolutionHeight = 1080;
                GlobalObjects_UW.is60FPS = true;
                GlobalObjects_UW.isVSync = true;
                GlobalObjects_UW.autoSaveType = 2;
                GlobalObjects_UW.allowAhegao = true;
                GlobalObjects_UW.expressionSoundChance = 50;
                GlobalObjects_UW.idleChance = 80;
                GlobalObjects_UW.hatChance = 20;
                GlobalObjects_UW.glassesChance = 20;
                GlobalObjects_UW.frecklesChance = 30;
                GlobalObjects_UW.beardChance = 30;
                GlobalObjects_UW.mustacheChance = 30;
                GlobalObjects_UW.cheeksChance = 30;
                GlobalObjects_UW.bodyNormalChance = 100;
                GlobalObjects_UW.bodyMuscledChance = 100;
                GlobalObjects_UW.bodyFatChance = 0;
                GlobalObjects_UW.bodySkinnyChance = 100;
                GlobalObjects_UW.dickThinShortChance = 100;
                GlobalObjects_UW.dickThinNormalChance = 100;
                GlobalObjects_UW.dickThinLongChance = 100;
                GlobalObjects_UW.dickNormalShortChance = 100;
                GlobalObjects_UW.dickNormalNormalChance = 100;
                GlobalObjects_UW.dickNormalLongChance = 100;
                GlobalObjects_UW.dickFatShortChance = 100;
                GlobalObjects_UW.dickFatNormalChance = 100;
                GlobalObjects_UW.dickFatLongChance = 100;
                GlobalObjects_UW.femaleBreastSizeMin = 10;
                GlobalObjects_UW.femaleBreastSizeMax = 90;
                GlobalObjects_UW.qualityLevel = 0;
                GlobalObjects_UW.waitForNPCsToLoad = true;
                GlobalObjects_UW.zoomInOnSex = false;
                GlobalObjects_UW.zoomOutOnDialogueExit = false;
                GlobalObjects_UW.curMenuTheme = 0;
                GlobalObjects_UW.canAnimateBoobs = true;
                GlobalObjects_UW.canAnimateHairsSkirts = true;
                GlobalObjects_UW.npcLoaderAmount = 0;
                GlobalObjects_UW.isSpecial = false;
                GlobalObjects_UW.isSaveLoaded = false;
                GlobalObjects_UW.isMoaning = false;
                GlobalObjects_UW.isLewd = false;
                GlobalObjects_UW.isEro = true;
                GlobalObjects_UW.isRaining = false;
                GlobalObjects_UW.isInAnimState = false;
                GlobalObjects_UW.showAnimState = false;
                GlobalObjects_UW.isSceneLoaded = false;
                GlobalObjects_UW.isSleeping = false;
                GlobalObjects_UW.disNPCs = false;
                GlobalObjects_UW.IsLoadedPosition = false;
                GlobalObjects_UW.isVirginityLocked = false;
                GlobalObjects_UW.isHotkeysBlocked = false;
                GlobalObjects_UW.camReadyToZoom = false;
                GlobalObjects_UW.fontSizePC = 40;
                GlobalObjects_UW.fontSizeAndroid = 60;
                GlobalObjects_UW.lastMovePoint = 0;
                GlobalObjects_UW.lastAutoSave = 0;
                GlobalObjects_UW.lastQuickSave = 0;
                GlobalObjects_UW.spawnOnLoadCar = false;
                GlobalObjects_UW.spawnOnLoadTaxi = false;
                GlobalObjects_UW.doOncePopUp = false;
                GlobalObjects_UW.blockLeaveRoom = false;
                GlobalObjects_UW.isInDialogue = false;
                GlobalObjects_UW.isDialMini = false;
                GlobalObjects_UW.isInCharacterCreation = false;
                GlobalObjects_UW.isInTraitsShop = false;
                GlobalObjects_UW.isInitialized = false;
                GlobalObjects_UW.isInClothesShop = false;
                GlobalObjects_UW.isChangingScene = false;
                GlobalObjects_UW.changeSceneCheckEvents = false;
                GlobalObjects_UW.spawnInt = 1;
                GlobalObjects_UW.volume_Master = 1f;
                GlobalObjects_UW.volume_Music = 0.4f;
                GlobalObjects_UW.volume_MoansFemale = 1f;
                GlobalObjects_UW.volume_MoansMale = 0.3f;
                GlobalObjects_UW.volume_Effects = 0.8f;
                GlobalObjects_UW.volume_Environment = 0.5f;
                GlobalObjects_UW.dialTransparentVal = 1f;
                GlobalObjects_UW.loadedSlot = 0;
                GlobalObjects_UW.loadedSaveType = ESaveType.Normal;
                GlobalObjects_UW.isTimeStopped = false;
                GlobalObjects_UW.curAnimSpeed = 1f;
                GlobalObjects_UW.curAnimSpeedState = 0;
                GlobalObjects_UW.curAnimSpeedStateStr = "";
                GlobalObjects_UW.useEcho = false;
                GlobalObjects_UW.errID = 0;
                GlobalObjects_UW.isErrorLoaded = false;
                GlobalObjects_UW.lastRand = 0;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Logger.LogInfo(e.ToString());
                GlobalObjects_UW_setVar("isDevBuild", true);
                GlobalObjects_UW_setVar("isDebugMode", true);
                GlobalObjects_UW_setVar("showMovePoints", true);
                GlobalObjects_UW_setVar("testAnimVals", false);
                GlobalObjects_UW_setVar("forceNudity", 0);
                GlobalObjects_UW_setVarEnum("forceDickType", "EDickType.None");
                GlobalObjects_UW_setVar("allowCumShake", true);
                GlobalObjects_UW_setVar("disableScreenFade", true);
                GlobalObjects_UW_setVarEnum("forceSeasonType", "ESeasonType.NONE");
                GlobalObjects_UW_setVarEnum("forceHolidayType", EHolidayType.NONE");
                GlobalObjects_UW_setVar("disablePregnancy", true);
                GlobalObjects_UW_setVar("pregnancyInfoCheat", true);
                GlobalObjects_UW_setVar("pregnantBelly", true);
                GlobalObjects_UW_setVar("hidePregnantBelly", true);
                GlobalObjects_UW_setVar("isRandomNPCSize", true);
                GlobalObjects_UW_setVar("boobBouncingPower", 0);
                GlobalObjects_UW_setVar("eyesOverHairs", true);
                GlobalObjects_UW_setVar("dontCleanSperm", true);
                GlobalObjects_UW_setVar("stopTimeFlow", true);
                GlobalObjects_UW_setVarEnum("lockLW", "EThresholdsLewdness.Off");
                GlobalObjects_UW_setVar("npcDickSize", 1f);
                GlobalObjects_UW_setVar("isSteam", false);
                GlobalObjects_UW_setVar("cheatsOn", true);
                GlobalObjects_UW_setVar("isVersionCheats", true);
                GlobalObjects_UW_setVar("isVersionExtended", true);
                GlobalObjects_UW_setVar("enc", 78);
                GlobalObjects_UW_setVar("xrayDickSharesNPCSkin", true);
                GlobalObjects_UW_setVar("moveOnInteraction", true);
                GlobalObjects_UW_setVarEnum("curDifficulty", "EDifficultyTypes.NORMAL");
                GlobalObjects_UW_setVar("allowIdleInDialogue", true);
                GlobalObjects_UW_setVar("backgroundSkipsDialogue", true);
                GlobalObjects_UW_setVar("allowedBodyTypes", 0);
                GlobalObjects_UW_setVar("disableRandomNPCs", false);
                GlobalObjects_UW_setVar("isRunToggled", true);
                GlobalObjects_UW_setVar("showInternalDebug", true);
                GlobalObjects_UW_setVar("isAndroid", false);
                GlobalObjects_UW_setVar("isSuperDebugging", true);
                GlobalObjects_UW_setVar("preloadDialogues", true);
                GlobalObjects_UW_setVar("biggerAndroidUI", false);
                GlobalObjects_UW_setVar("tooltipFontSize", 30);
                GlobalObjects_UW_setVarEnum("seasonType", "ESeasonType.NONE");
                GlobalObjects_UW_setVarEnum("holidayType", "EHolidayType.NONE");
                GlobalObjects_UW_setVar("moodFilter", true);
                GlobalObjects_UW_setVar("useUnityFont", false);
                GlobalObjects_UW_setVar("enableHolidays", true);
                GlobalObjects_UW_setVar("useKeyboardOnly", false);
                GlobalObjects_UW_setVar("allowMaleMoans", true);
                GlobalObjects_UW_setVar("showUseIcons", true);
                GlobalObjects_UW_setVar("showXRay", true);
                GlobalObjects_UW_setVar("allowXRay", true);
                GlobalObjects_UW_setVar("showCutIns", true);
                GlobalObjects_UW_setVar("showCutInsXRay", true);
                GlobalObjects_UW_setVar("showMoans", true);
                GlobalObjects_UW_setVar("allowHats", true);
                GlobalObjects_UW_setVar("allowGlasses", true);
                GlobalObjects_UW_setVar("isClock12", false);
                GlobalObjects_UW_setVar("isDialTransparent", true);
                GlobalObjects_UW_setVar("isAutoAnimState", true);
                GlobalObjects_UW_setVar("gameScreenModeStr", "Windowed");
                GlobalObjects_UW_setVar("gameResolutionWidth", 1920);
                GlobalObjects_UW_setVar("gameResolutionHeight", 1080);
                GlobalObjects_UW_setVar("is60FPS", true);
                GlobalObjects_UW_setVar("isVSync", true);
                GlobalObjects_UW_setVar("autoSaveType", 2);
                GlobalObjects_UW_setVar("allowAhegao", true);
                GlobalObjects_UW_setVar("expressionSoundChance", 50);
                GlobalObjects_UW_setVar("idleChance", 80);
                GlobalObjects_UW_setVar("hatChance", 20);
                GlobalObjects_UW_setVar("glassesChance", 20);
                GlobalObjects_UW_setVar("frecklesChance", 30);
                GlobalObjects_UW_setVar("beardChance", 30);
                GlobalObjects_UW_setVar("mustacheChance", 30);
                GlobalObjects_UW_setVar("cheeksChance", 30);
                GlobalObjects_UW_setVar("bodyNormalChance", 100);
                GlobalObjects_UW_setVar("bodyMuscledChance", 100);
                GlobalObjects_UW_setVar("bodyFatChance", 0);
                GlobalObjects_UW_setVar("bodySkinnyChance", 100);
                GlobalObjects_UW_setVar("dickThinShortChance", 100);
                GlobalObjects_UW_setVar("dickThinNormalChance", 100);
                GlobalObjects_UW_setVar("dickThinLongChance", 100);
                GlobalObjects_UW_setVar("dickNormalShortChance", 100);
                GlobalObjects_UW_setVar("dickNormalNormalChance", 100);
                GlobalObjects_UW_setVar("dickNormalLongChance", 100);
                GlobalObjects_UW_setVar("dickFatShortChance", 100);
                GlobalObjects_UW_setVar("dickFatNormalChance", 100);
                GlobalObjects_UW_setVar("dickFatLongChance", 100);
                GlobalObjects_UW_setVar("femaleBreastSizeMin", 10);
                GlobalObjects_UW_setVar("femaleBreastSizeMax", 90);
                GlobalObjects_UW_setVar("qualityLevel", 0);
                GlobalObjects_UW_setVar("waitForNPCsToLoad", true);
                GlobalObjects_UW_setVar("zoomInOnSex", false);
                GlobalObjects_UW_setVar("zoomOutOnDialogueExit", false);
                GlobalObjects_UW_setVar("curMenuTheme", 0);
                GlobalObjects_UW_setVar("canAnimateBoobs", true);
                GlobalObjects_UW_setVar("canAnimateHairsSkirts", true);
                GlobalObjects_UW_setVar("npcLoaderAmount", 0);
                GlobalObjects_UW_setVar("isSpecial", false);
                GlobalObjects_UW_setVar("isSaveLoaded", false);
                GlobalObjects_UW_setVar("isMoaning", false);
                GlobalObjects_UW_setVar("isLewd", false);
                GlobalObjects_UW_setVar("isEro", true);
                GlobalObjects_UW_setVar("isRaining", false);
                GlobalObjects_UW_setVar("isInAnimState", false);
                GlobalObjects_UW_setVar("showAnimState", false);
                GlobalObjects_UW_setVar("isSceneLoaded", false);
                GlobalObjects_UW_setVar("isSleeping", false);
                GlobalObjects_UW_setVar("disNPCs", false);
                GlobalObjects_UW_setVar("IsLoadedPosition", false);
                GlobalObjects_UW_setVar("isVirginityLocked", false);
                GlobalObjects_UW_setVar("isHotkeysBlocked", false);
                GlobalObjects_UW_setVar("camReadyToZoom", false);
                GlobalObjects_UW_setVar("fontSizePC", 40);
                GlobalObjects_UW_setVar("fontSizeAndroid", 60);
                GlobalObjects_UW_setVar("lastMovePoint", 0);
                GlobalObjects_UW_setVar("lastAutoSave", 0);
                GlobalObjects_UW_setVar("lastQuickSave", 0);
                GlobalObjects_UW_setVar("spawnOnLoadCar", false);
                GlobalObjects_UW_setVar("spawnOnLoadTaxi", false);
                GlobalObjects_UW_setVar("doOncePopUp", false);
                GlobalObjects_UW_setVar("blockLeaveRoom", false);
                GlobalObjects_UW_setVar("isInDialogue", false);
                GlobalObjects_UW_setVar("isDialMini", false);
                GlobalObjects_UW_setVar("isInCharacterCreation", false);
                GlobalObjects_UW_setVar("isInTraitsShop", false);
                GlobalObjects_UW_setVar("isInitialized", false);
                GlobalObjects_UW_setVar("isInClothesShop", false);
                GlobalObjects_UW_setVar("isChangingScene", false);
                GlobalObjects_UW_setVar("changeSceneCheckEvents", false);
                GlobalObjects_UW_setVar("spawnInt", 1);
                GlobalObjects_UW_setVar("volume_Master", 1f);
                GlobalObjects_UW_setVar("volume_Music", 0.4f);
                GlobalObjects_UW_setVar("volume_MoansFemale", 1f);
                GlobalObjects_UW_setVar("volume_MoansMale", 0.3f);
                GlobalObjects_UW_setVar("volume_Effects", 0.8f);
                GlobalObjects_UW_setVar("volume_Environment", 0.5f);
                GlobalObjects_UW_setVar("dialTransparentVal", 1f);
                GlobalObjects_UW_setVar("loadedSlot", 0);
                GlobalObjects_UW_setVarEnum("loadedSaveType", "ESaveType.Normal");
                GlobalObjects_UW_setVar("isTimeStopped", false);
                GlobalObjects_UW_setVar("curAnimSpeed", 1f);
                GlobalObjects_UW_setVar("curAnimSpeedState", 0);
                GlobalObjects_UW_setVar("curAnimSpeedStateStr", "");
                GlobalObjects_UW_setVar("useEcho", false);
                GlobalObjects_UW_setVar("errID", 0);
                GlobalObjects_UW_setVar("isErrorLoaded", false);
                GlobalObjects_UW_setVar("lastRand", 0);
            }*/

            try
            {
                GlobalObjects_UW.isDevBuild = false;
            }
            catch (Exception e)
            {
            }

            try
            {
                GlobalObjects_UW.isVersionCheats = true;
            }
            catch (Exception e)
            {
            }

            try
            {
                GlobalObjects_UW.isVersionExtended = true;
            }
            catch (Exception e)
            {
            }

            try
            {
                GlobalObjects_UW.isDebugMode = false;
            }
            catch (Exception e)
            {
            }

            try
            {
                GlobalObjects_UW.showInternalDebug = false;
            }
            catch (Exception e)
            {
            }

            try
            {
                GlobalObjects_UW.forceDickType = EDickType.None;
            }
            catch (Exception e)
            {
            }
        }

        static void GlobalObjects_UW_cctor_run()
        {
            setOldSettings();
            GlobalObjects_UW_setVar("showDevDebug", true);
            GlobalObjects_UW_setVar("isDevBuild", true);
            GlobalObjects_UW_setVar("isDebugMode", false);
            GlobalObjects_UW_setVar("showMovePoints", false);
            GlobalObjects_UW_setVar("ignoreChoiceReqs", true);
            GlobalObjects_UW_setVar("overpowerStars", true);
            GlobalObjects_UW_setVar("accessAllClothes", true);
            GlobalObjects_UW_setVar("maxLuck", true);
            GlobalObjects_UW_setVar("pregnancyInfoCheat", true);
            GlobalObjects_UW_setVar("cheatsOn", true);
            GlobalObjects_UW_setVar("isVersionCheats", true);
            GlobalObjects_UW_setVar("isVersionExtended", true);
            GlobalObjects_UW_setVar("showInternalDebug", false);
            GlobalObjects_UW_setVar("isFurries", true);
            GlobalObjects_UW_setVar("isSuperDebugging", true);
            GlobalObjects_UW_setVar("allowCumShake", true);
            GlobalObjects_UW_setVar("stopTimeFlow", true);
            GlobalObjects_UW_setVar("dontCleanSperm", true);
            GlobalObjects_UW_setVar("isRandomNPCSize", true);
            GlobalObjects_UW_setVar("disableScreenFade", true);
            GlobalObjects_UW_setVar("eyesOverHairs", true);
            GlobalObjects_UW_setVar("pregnantBelly", true);
            GlobalObjects_UW_setVar("disablePregnancy", true);
            GlobalObjects_UW_setVar("hidePregnantBelly", true); ;
            GlobalObjects_UW_setVarEnum("forceDickType", "EDickType.None");

            Console.WriteLine("Load cctor from GlobalObjects_UW");
            Logger.LogInfo($"Load cctor from GlobalObjects_UW");
        }

        static object getEnum(string enumtype, string enumkey)
        {
            string message = null;

            Type type = null;

            try
            {
                // Get the type of the object
                type = MyGetType(enumtype);
            }
            catch (Exception e)
            {
                message = e.ToString();
                Console.WriteLine(message);
                Logger.LogInfo(message);
                return null;
            }

            string className = enumtype;
            if (type == null)
            {
                message = "enum " + className + " not found, the dev have removed it!";
                Console.WriteLine(message);
                Logger.LogInfo(message);
                return null;
            }

            // Validate that the type is actually an enum
            if (!type.IsEnum)
            {
                Console.WriteLine("Provided type is not an enum.");
                message = "Provided type is not an enum.";
                Console.WriteLine(message);
                Logger.LogInfo(message);
                return null;
            }

            // Validate that the enum value exists
            if (!Enum.IsDefined(type, enumkey))
            {
                message = $"'{enumkey}' is not a valid value for {type.Name}.";
                Logger.LogInfo(message);
                return null;
            }

            try
            {

                // Get the enum value object
                object enumValue = Enum.Parse(type, enumkey);

                // Get the underlying numeric value using reflection
                object numericValue = Convert.ChangeType(enumValue, Enum.GetUnderlyingType(type));

                return numericValue;
            }
            catch (Exception e)
            {
                message = e.ToString();
                Console.WriteLine(message);
                Logger.LogInfo(message);
                return null;
            }

            return null;
        }

        static string[] getEnumString(string typeValue)
        {
            char separator = '.';

            if (typeValue == null)
            {
                return null;
            }

            if (typeValue.Length == 0)
            {
                return null;
            }

            if (!typeValue.Contains("" + separator))
            {
                return null;
            }

            // Split the string
            string[] parts = typeValue.Split(separator);
            if (parts.Length < 2)
            {
                return null;
            }

            string lastpath = parts[1];

            for (int i = 2; i < parts.Length; i++)
            {
                lastpath = lastpath + "." + parts[i];
            }

            parts[1] = lastpath;

            return new string[]
            {
                parts[0],
                parts[1]
            };
        }

        static void UW_setVarEnum(object obj, string className, string field, BindingFlags flags, string enumvalue)
        {
            string[] enumparts = getEnumString(enumvalue);
            string message = null;
            if (enumparts == null)
            {
                message = enumvalue + " is not a valid enum name";
                Console.WriteLine(message);
                Logger.LogInfo(message);
                return;
            }

            string enumtype = enumparts[0];
            string enumkey = enumparts[1];

            object value = getEnum(enumtype, enumkey);
            if (value == null)
            {
                message = enumtype + " " + enumkey + " is not a valid enum";
                Console.WriteLine(message);
                Logger.LogInfo(message);
                return;
            }

            UW_setVar(obj, className, field, flags, value);
        }

        static void UW_setVar(object obj, string className, string field, BindingFlags flags, object value)
        {
            Type myType1 = MyGetType(className);
            if (myType1 == null)
            {
                throw new Exception("class " + className + " not found, the dev have removed it!");
            }

            try
            {
                // Get the FieldInfo for the public static field
                FieldInfo fieldInfo = myType1.GetField(
                    field,
                    flags
                );

                if (fieldInfo == null)
                {
                    throw new Exception("field " + field + " not found, the dev have removed it!");
                }

                fieldInfo.SetValue(obj, value);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Logger.LogInfo(e.ToString());
            }
        }

        static void UW_setProperty(object obj, string className, string field, BindingFlags flags, object value)
        {
            Type myType1 = MyGetType(className);
            if (myType1 == null)
            {
                throw new Exception("class " + className + " not found, the dev have removed it!");
            }

            try
            {
                // Get the FieldInfo for the public static field
                PropertyInfo fieldInfo = myType1.GetProperty(
                    field,
                    flags
                );

                if (fieldInfo == null)
                {
                    throw new Exception("field " + field + " not found, the dev have removed it!");
                }

                fieldInfo.SetValue(obj, value, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Logger.LogInfo(e.ToString());
            }
        }

        static object UW_getVar(object obj, string className, string field, BindingFlags flags)
        {
            Type myType1 = MyGetType(className);
            if (myType1 == null)
            {
                throw new Exception("class " + className + " not found, the dev have removed it!");
            }

            try
            {
                // Get the FieldInfo for the public static field
                FieldInfo fieldInfo = myType1.GetField(
                    field,
                    flags
                );

                if (fieldInfo == null)
                {
                    throw new Exception("field " + field + " not found, the dev have removed it!");
                }

                return fieldInfo.GetValue(obj);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Logger.LogInfo(e.ToString());
                return null;
            }
        }

        static object UW_getProperty(object obj, string className, string field, BindingFlags flags)
        {
            Type myType1 = MyGetType(className);
            if (myType1 == null)
            {
                throw new Exception("class " + className + " not found, the dev have removed it!");
            }

            try
            {
                // Get the FieldInfo for the public static field
                PropertyInfo fieldInfo = myType1.GetProperty(
                    field,
                    flags
                );

                if (fieldInfo == null)
                {
                    throw new Exception("field " + field + " not found, the dev have removed it!");
                }

                return fieldInfo.GetValue(obj, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Logger.LogInfo(e.ToString());
                return null;
            }
        }

        static void GlobalObjects_UW_setVar(string field, object value)
        {
            string className = "GlobalObjects_UW";
            BindingFlags flags = BindingFlags.Public | BindingFlags.Static;
            object obj = null;
            UW_setVar(obj, className, field, flags, value);
        }

        static object GlobalObjects_UW_getVar(string field)
        {
            string className = "GlobalObjects_UW";
            BindingFlags flags = BindingFlags.Public | BindingFlags.Static;
            object obj = null;
            return UW_getVar(obj, className, field, flags);
        }

        static void GlobalObjects_UW_setVarEnum(string field, string enumvalue)
        {
            string className = "GlobalObjects_UW";
            BindingFlags flags = BindingFlags.Public | BindingFlags.Static;
            object obj = null;
            UW_setVarEnum(obj, className, field, flags, enumvalue);
        }

        static void Character_Clothes_UW_setVar(object obj, string field, object value)
        {
            string className = "Character_Clothes_UW";
            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            UW_setVar(obj, className, field, flags, value);
        }

        static object Character_Clothes_UW_getVar(object obj, string field)
        {
            string className = "Character_Clothes_UW";
            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            return UW_getVar(obj, className, field, flags);
        }

        static void Character_Clothes_UW_setProperty(object obj, string field, object value)
        {
            string className = "Character_Clothes_UW";
            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            UW_setProperty(obj, className, field, flags, value);
        }

        static object Character_Clothes_UW_getProperty(object obj, string field)
        {
            string className = "Character_Clothes_UW";
            BindingFlags flags = BindingFlags.Public | BindingFlags.Instance;
            return UW_getProperty(obj, className, field, flags);
        }
    }
}