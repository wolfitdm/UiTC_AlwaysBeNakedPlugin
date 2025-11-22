using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.Mono;
using HarmonyLib;
using BepInEx;
using System.Reflection;
using System;

namespace AlwaysBeNaked
{

    [BepInPlugin("com.wolfitdm.AlwaysBeNaked", "AlwaysBeNaked Plugin", "1.0.0.0")]
    public class AlwaysBeNaked : BaseUnityPlugin
    {
        internal static new ManualLogSource Logger;

        public AlwaysBeNaked()
        {
        }

        private void Awake()
        {
            // Plugin startup logic
            Logger = base.Logger;
            Harmony.CreateAndPatchAll(typeof(AlwaysBeNaked));
            // Create a new Harmony instance with a unique ID
            var harmony = new Harmony("com.wolfitdm.AlwaysBeNaked");
            Logger.LogInfo($"Plugin AlwaysBeNaked is loaded!");
        }


        [HarmonyPatch(typeof(Character_Clothes_UW), "CheckAllClothes_Player")] // Specify target method with HarmonyPatch attribute
        [HarmonyPostfix]                              // There are different patch types. Prefix code runs before original code
        static void CheckAllClothes_Player(bool updateBreasts, bool updateCurs, object __instance)
        {
            Character_Clothes_UW _this = (Character_Clothes_UW)__instance;
            if (_this.isPlayer)
            {
                _this.hasPantsNow = true;
                _this.hasShirtNow = true;
            }
        }

        [HarmonyPatch(typeof(Character_Clothes_UW), "StartScripts")] // Specify target method with HarmonyPatch attribute
        [HarmonyPostfix]                                            // There are different patch types. Prefix code runs before original code
        static void StartScripts(object __instance)
        {
            Character_Clothes_UW _this = (Character_Clothes_UW)__instance;
            if (_this.isPlayer)
            {
                _this.hasPantsNow = true;
                _this.hasShirtNow = true;
            }
        }

        [HarmonyPatch(typeof(GlobalObjects_UW), "CheckVersionEXCHBA")]
        [HarmonyPostfix]
        static void GlobalObjects_UW_cctor_run()
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
            }*/

            try
            {
                // your settings
                GlobalObjects_UW.isDevBuild = true;
                GlobalObjects_UW.isVersionCheats = true;
                GlobalObjects_UW.isVersionExtended = true;
                GlobalObjects_UW.isSuperDebugging = true;
                GlobalObjects_UW.isDebugMode = false;
                GlobalObjects_UW.showInternalDebug = false;
                GlobalObjects_UW.cheatsOn = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("Load cctor from GlobalObjects_UW");
            Logger.LogInfo($"Load cctor from GlobalObjects_UW");
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object other)
        {
            return base.Equals(other);
        }
    }
}