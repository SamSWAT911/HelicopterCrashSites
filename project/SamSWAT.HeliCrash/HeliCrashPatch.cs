using Aki.Reflection.Patching;
using Comfort.Common;
using EFT;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace SamSWAT.HeliCrash
{
    public class HeliCrashPatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod()
        {
            return typeof(GameWorld).GetMethod("OnGameStarted", BindingFlags.Public | BindingFlags.Instance);
        }

        [PatchPostfix]
        public static void PatchPostFix()
        {
            var gameWorld = Singleton<GameWorld>.Instance;
            var points = LocationScene.GetAll<AirdropPoint>().Any();
            var location = gameWorld.RegisteredPlayers[0].Location;

            if (gameWorld != null && points && WillHeliCrash())
            {
                var heliCrash = gameWorld.gameObject.GetOrAddComponent<HeliCrash>();
                heliCrash.Init(location);
            }
        }

        public static bool WillHeliCrash()
        {
            return Random.Range(0, 100) <= Plugin.HeliCrashChance.Value;
        }
    }
}
