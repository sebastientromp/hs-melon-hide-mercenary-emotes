using MelonLoader;
using UnityEngine;
using HarmonyLib;
using System.Reflection;
using System;

namespace NoMercsDiamondVoiceLines
{
    public class NoMercsDiamondVoiceLinesMod : MelonMod
    {
        public static MelonLogger.Instance SharedLogger;

        public override void OnInitializeMelon()
        {
            NoMercsDiamondVoiceLinesMod.SharedLogger = LoggerInstance;
            var harmony = this.HarmonyInstance;
            harmony.PatchAll(typeof(PlayEmotePatcher));
        }
    }

    public static class PlayEmotePatcher
    {
        [HarmonyPatch(typeof(Card), "PlayEmote", new Type[] { typeof(EmoteType), typeof(Notification.SpeechBubbleDirection) })]
        [HarmonyPrefix]
        public static bool Prefix(EmoteType emoteType, Notification.SpeechBubbleDirection overrideDirection, ref CardSoundSpell __result, Card __instance)
        {
            if (__instance.GetEntity().IsLettuceMercenary())
            {
                NoMercsDiamondVoiceLinesMod.SharedLogger.Msg($"Blocking Mercenary Emote, emoteType={emoteType}, direction={overrideDirection}");
                __result = null;
                return false;
            }
            return true;
        }
    }
}
