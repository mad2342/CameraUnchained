using System;
using System.Diagnostics;
using System.Reflection;
using BattleTech;
using BattleTech.UI;
using Harmony;
using UnityEngine;

namespace CameraUnchained.Patches
{
    class Camera
    {
        // Main suppression
        [HarmonyPatch(typeof(MultiSequence), "SetCamera")]
        public static class MultiSequence_SetCamera_Patch
        {
            public static bool Prefix(MultiSequence __instance)
            {
                try
                {
                    //Logger.Debug($"[MultiSequence_SetCamera_PREFIX] Suppressing camera focus...");

                    StackTrace stackTrace = new StackTrace();
                    for (var i = 0; i < stackTrace.FrameCount; i++)
                    {
                        MethodBase methodBase = stackTrace.GetFrame(i).GetMethod();
                        Type declaringType = methodBase.DeclaringType;
                        string caller = $"{declaringType}.{methodBase.Name}";

                        if (CameraUnchained.Settings.EventWhitelist.Contains(caller))
                        {
                            Logger.Debug($"[MultiSequence_SetCamera_PREFIX] WHITELISTED: {caller}");
                            return true;
                        }

                        if (Fields.EventList.Contains(caller))
                        {
                            Logger.Info($"[MultiSequence_SetCamera_PREFIX] SUPPRESSING: {caller}");
                            return false;
                        }
                    }

                    return false;
                }
                catch (Exception e)
                {
                    Logger.Error(e);

                    return true;
                }
            }
        }



        // Focus on enemies standing up
        [HarmonyPatch(typeof(MechStandSequence), "OnAdded")]
        public static class MechStandSequence_OnAdded_Patch
        {
            public static bool Prepare()
            {
                return CameraUnchained.Settings.FocusOnStandingUp;
            }

            public static void Postfix(MechStandSequence __instance)
            {
                try
                {
                    //if (__instance.owningActor.team.IsLocalPlayer)
                    if (__instance.owningActor.team.LocalPlayerControlsTeam)
                    {
                        return;
                    }

                    Logger.Debug($"[MechStandSequence_OnAdded_POSTFIX] Focus on enemy standing up...");

                    CombatGameState ___combatGameState = (CombatGameState)AccessTools.Property(typeof(MechStandSequence), "Combat").GetValue(__instance, null);

                    if (___combatGameState.LocalPlayerTeam.VisibilityToTarget(__instance.owningActor) == VisibilityLevel.LOSFull)
                    {
                        CameraControl.Instance.SetMovingToGroundPos(__instance.owningActor.CurrentPosition, 0.95f);
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }



        // Focus on enemies powering up
        [HarmonyPatch(typeof(MechStartupSequence), "OnAdded")]
        public static class MechStartupSequence_OnAdded_Patch
        {
            public static bool Prepare()
            {
                return CameraUnchained.Settings.FocusOnPoweringUp;
            }

            public static void Postfix(MechStartupSequence __instance)
            {
                try
                {
                    //if (__instance.owningActor.team.IsLocalPlayer)
                    if (__instance.owningActor.team.LocalPlayerControlsTeam)
                    {
                        return;
                    }

                    Logger.Debug($"[MechStartupSequence_OnAdded_POSTFIX] Focus on enemy powering up...");

                    CombatGameState ___combatGameState = (CombatGameState)AccessTools.Property(typeof(MechStartupSequence), "Combat").GetValue(__instance, null);

                    if (___combatGameState.LocalPlayerTeam.VisibilityToTarget(__instance.owningActor) == VisibilityLevel.LOSFull)
                    {
                        CameraControl.Instance.SetMovingToGroundPos(__instance.owningActor.CurrentPosition, 0.95f);
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }



        // Focus on enemy jumping
        [HarmonyPatch(typeof(MechJumpSequence), "OnAdded")]
        public static class MechJumpSequence_OnAdded_Patch
        {
            public static bool Prepare()
            {
                return CameraUnchained.Settings.FocusOnMovement;
            }

            public static void Postfix(MechJumpSequence __instance)
            {
                try
                {
                    //if (__instance.owningActor.team.IsLocalPlayer)
                    if (__instance.owningActor.team.LocalPlayerControlsTeam)
                    {
                        return;
                    }

                    Logger.Debug($"[MechJumpSequence_OnAdded_POSTFIX] Focus on enemy jumping...");

                    CombatGameState ___combatGameState = (CombatGameState)AccessTools.Property(typeof(MechJumpSequence), "Combat").GetValue(__instance, null);

                    if (___combatGameState.LocalPlayerTeam.CanDetectPosition(__instance.OwningMech.CurrentPosition, __instance.OwningMech))
                    {
                        CameraControl.Instance.SetMovingToGroundPos(__instance.OwningMech.CurrentPosition, 0.95f);
                    }
                    else if (___combatGameState.LocalPlayerTeam.CanDetectPosition(__instance.FinalPos, __instance.OwningMech))
                    {
                        CameraControl.Instance.SetMovingToGroundPos(__instance.FinalPos, 0.95f);
                    }
                    else
                    {
                        // Nothing?
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }



        // Focus on enemy moving
        [HarmonyPatch(typeof(ActorMovementSequence), "OnAdded")]
        public static class ActorMovementSequence_OnAdded_Patch
        {
            public static bool Prepare()
            {
                return CameraUnchained.Settings.FocusOnMovement;
            }

            public static void Postfix(ActorMovementSequence __instance)
            {
                try
                {
                    //if (__instance.owningActor.team.IsLocalPlayer)
                    if (__instance.owningActor.team.LocalPlayerControlsTeam)
                    {
                        return;
                    }

                    Logger.Debug($"[ActorMovementSequence_OnAdded_POSTFIX] Focus on enemy moving...");

                    CombatGameState ___combatGameState = (CombatGameState)AccessTools.Property(typeof(ActorMovementSequence), "Combat").GetValue(__instance, null);

                    if (___combatGameState.LocalPlayerTeam.CanDetectPosition(__instance.OwningActor.CurrentPosition, __instance.OwningActor))
                    {
                        //CameraSequence cameraSequence = CameraControl.Instance.ShowMovementCam(__instance.OwningActor.CurrentPosition, __instance.FinalPos, __instance);
                        //MultiSequence multiSequence = __instance;
                        //multiSequence.SetCamera(cameraSequence, __instance.MessageIndex);

                        //Quaternion rotation = CameraControl.Instance.CameraRot;
                        //Traverse FrameTwoPoints = Traverse.Create(CameraControl.Instance).Method("FrameTwoPoints", rotation * Vector3.forward, __instance.OwningActor.CurrentPosition, __instance.FinalPos, 0.9f, ___combatGameState.Constants.CameraConstants.MaxHeightAboveTerrain * 0.9f);
                        //Vector3 poi = (Vector3)FrameTwoPoints.GetValue();

                        //Vector3 poi = Vector3.Lerp(__instance.OwningActor.CurrentPosition, __instance.FinalPos, 0.5f);
                        //CameraControl.Instance.SetMovingToGroundPos(poi, 0.95f);

                        // KISS
                        CameraControl.Instance.SetMovingToGroundPos(__instance.OwningActor.CurrentPosition, 0.95f);
                    }
                    else if (___combatGameState.LocalPlayerTeam.CanDetectPosition(__instance.FinalPos, __instance.OwningActor))
                    {
                        CameraControl.Instance.SetMovingToGroundPos(__instance.FinalPos, 0.95f);
                    }
                    else
                    {
                        // Nothing?
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }



        // Focus on sensor lock target
        [HarmonyPatch(typeof(SensorLockSequence), "OnAdded")]
        public static class SensorLockSequence_OnAdded_Patch
        {
            public static bool Prepare()
            {
                return CameraUnchained.Settings.FocusOnSensorLock;
            }

            public static void Postfix(SensorLockSequence __instance)
            {
                try
                {
                    //if (__instance.owningActor.team.IsLocalPlayer)
                    if (__instance.owningActor.team.LocalPlayerControlsTeam)
                    {
                        return;
                    }

                    Logger.Debug($"[SensorLockSequence_OnAdded_POSTFIX] Focus on sensor lock target...");

                    CameraControl.Instance.SetMovingToGroundPos(__instance.Target.CurrentPosition, 0.95f);
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }



        // Focus on melee target
        [HarmonyPatch(typeof(MechMeleeSequence), "OnAdded")]
        public static class MechMeleeSequence_OnAdded_Patch
        {
            public static bool Prepare()
            {
                return CameraUnchained.Settings.FocusOnMelee;
            }

            public static void Postfix(MechMeleeSequence __instance)
            {
                try
                {
                    Logger.Debug($"[MechMeleeSequence_OnAdded_POSTFIX] Focus on melee target...");

                    CombatGameState ___combatGameState = (CombatGameState)AccessTools.Property(typeof(MechMeleeSequence), "Combat").GetValue(__instance, null);

                    if (
                        __instance.owningActor.TeamId == ___combatGameState.LocalPlayerTeamGuid ||
                        __instance.MeleeTarget.team.GUID == ___combatGameState.LocalPlayerTeamGuid ||
                        (___combatGameState.HostilityMatrix.IsLocalPlayerFriendly(__instance.owningActor.TeamId) || ___combatGameState.HostilityMatrix.IsLocalPlayerFriendly(__instance.MeleeTarget.team.GUID)) ||
                        ___combatGameState.LocalPlayerTeam.CanDetectPosition(__instance.MeleeTarget.CurrentPosition, __instance.owningActor)
                    )
                    {
                        CameraControl.Instance.SetMovingToGroundPos(__instance.MeleeTarget.CurrentPosition, 0.95f);
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }



        // Focus on attacked target
        [HarmonyPatch(typeof(AttackStackSequence), "OnAttackBegin")]
        public static class AttackStackSequence_OnAttackBegin_Patch
        {
            public static bool Prepare()
            {
                return CameraUnchained.Settings.FocusOnTargetedFriendly || CameraUnchained.Settings.FocusOnTargetedEnemy;
            }

            public static void Postfix(AttackStackSequence __instance, MessageCenterMessage message)
            {
                try
                {
                    Logger.Debug($"[AttackStackSequence_OnAttackBegin_POSTFIX] Focus on attacked target...");

                    CombatGameState ___combatGameState = (CombatGameState)AccessTools.Property(typeof(AttackStackSequence), "Combat").GetValue(__instance, null);
                    AttackSequenceBeginMessage attackSequenceBeginMessage = message as AttackSequenceBeginMessage;
                    AttackDirector.AttackSequence attackSequence = ___combatGameState.AttackDirector.GetAttackSequence(attackSequenceBeginMessage.sequenceId);
                    
                    bool isChosenTargetFriendly = attackSequence.chosenTarget.team.GUID == ___combatGameState.LocalPlayerTeamGuid || ___combatGameState.HostilityMatrix.IsLocalPlayerFriendly(attackSequence.chosenTarget.team.GUID);
                    bool shouldFocus = (CameraUnchained.Settings.FocusOnTargetedFriendly && isChosenTargetFriendly) || (CameraUnchained.Settings.FocusOnTargetedEnemy && !isChosenTargetFriendly);
                    Logger.Info($"[AttackStackSequence_OnAttackBegin_POSTFIX] isChosenTargetFriendly: {isChosenTargetFriendly}, shouldFocus: {shouldFocus}");

                    if (attackSequence == null || !shouldFocus)
                    {
                        return;
                    }

                    if (attackSequence.stackItemUID == __instance.SequenceGUID && !__instance.hasOwningSequence)
                    {
                        CameraControl.Instance.SetMovingToGroundPos(attackSequence.chosenTarget.CurrentPosition, 0.95f);
                    }
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }



        // Save current camera position
        [HarmonyPatch(typeof(OrderSequence), "OnAdded")]
        public static class OrderSequence_OnAdded_Patch
        {
            public static bool Prepare()
            {
                return CameraUnchained.Settings.SaveAndRestoreCamPos;
            }

            public static void Prefix(OrderSequence __instance)
            {
                try
                {
                    //if (__instance.owningActor.team.IsLocalPlayer)
                    if (__instance.owningActor.team.LocalPlayerControlsTeam)
                    {
                        return;
                    }

                    Logger.Debug($"[OrderSequence_OnAdded_PREFIX] Saving current camera position");

                    Fields.lastCameraPos = CameraControl.Instance.CameraPos;
                    Fields.lastCameraRot = CameraControl.Instance.CameraRot;
                    Fields.lastGroundPos = CameraControl.Instance.ScreenCenterToGroundPosition;
                    Logger.Debug($"[OrderSequence_OnAdded_PREFIX] Fields.lastCameraPos: {Fields.lastCameraPos}");
                    Logger.Debug($"[OrderSequence_OnAdded_PREFIX] Fields.lastCameraRot: {Fields.lastCameraRot}");
                    Logger.Debug($"[OrderSequence_OnAdded_PREFIX] Fields.lastGroundPos: {Fields.lastGroundPos}");
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }



        // Restore current camera position
        [HarmonyPatch(typeof(OrderSequence), "OnComplete")]
        public static class OrderSequence_OnComplete_Patch
        {
            public static bool Prepare()
            {
                return CameraUnchained.Settings.SaveAndRestoreCamPos;
            }

            public static void Postfix(OrderSequence __instance)
            {
                try
                {
                    //if (__instance.owningActor.team.IsLocalPlayer)
                    if (__instance.owningActor.team.LocalPlayerControlsTeam)
                    {
                        return;
                    }

                    Logger.Debug($"[OrderSequence_OnComplete_POSTFIX] Restoring camera position");

                    CameraControl.Instance.SetMovingToGroundPos(Fields.lastGroundPos, 0.95f);
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }



        // Center on selected target
        [HarmonyPatch(typeof(CombatSelectionHandler), "TrySelectTarget")]
        public static class CombatSelectionHandler_TrySelectTarget_Patch
        {
            public static bool Prepare()
            {
                return CameraUnchained.Settings.CenterOnTarget;
            }

            public static void Postfix(CombatSelectionHandler __instance, ICombatant target)
            {
                try
                {
                    Logger.Debug($"[CombatSelectionHandler_TrySelectTarget_POSTFIX] Called");

                    CombatGameState ___combatGameState = (CombatGameState)AccessTools.Property(typeof(CombatSelectionHandler), "Combat").GetValue(__instance, null);
                    
                    if (target != null && target.team != ___combatGameState.LocalPlayerTeam && !target.IsDead && target != __instance.SelectedTarget)
                    {
                        CameraControl.Instance.SetMovingToGroundPos(target.CurrentPosition, 0.95f);
                    }
                    
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }



        // Don't touch players camera height
        [HarmonyPatch(typeof(CameraControl), "ForceMovingToGroundPos")]
        public static class CameraControl_ForceMovingToGroundPos_Patch
        {
            public static bool Prepare()
            {
                return CameraUnchained.Settings.MaintainHeight;
            }

            public static bool Prefix(CameraControl __instance, Transform ___cTrans, float ___zoomTarget, ref Vector3 ___smoothToGroundPosDest, ref Vector3 ___smoothToGroundPosCamDest, ref bool ___isMovingToGroundPos, ref float ___smoothToGroundRatio, CameraControl.CameraState ___state, Vector3 i_dest, float screenRatio = 0.95f)
            {
                try
                {
                    Logger.Debug($"[CameraControl_ForceMovingToGroundPos_PREFIX] Don't touch players camera height...");

                    if (___state == CameraControl.CameraState.PlayerControlled)
                    {
                        CombatGameState ___CGS = (CombatGameState)AccessTools.Property(typeof(CameraControl), "Combat").GetValue(__instance, null);

                        ___smoothToGroundRatio = screenRatio;
                        i_dest.y = ___CGS.MapMetaData.GetCellAt(i_dest).cachedHeight;
                        ___isMovingToGroundPos = true;
                        ___smoothToGroundPosDest = i_dest;
                        
                        // Original
                        //___smoothToGroundPosCamDest = i_dest - ___cTrans.forward * ((___CGS.Constants.CameraConstants.MinHeightAboveTerrain + ___CGS.Constants.CameraConstants.MaxHeightAboveTerrain) * 0.8f);

                        // Max Height (Works)
                        ___smoothToGroundPosCamDest = i_dest - ___cTrans.forward * ((___CGS.Constants.CameraConstants.MinHeightAboveTerrain + ___CGS.Constants.CameraConstants.MaxHeightAboveTerrain) * 1.0f);

                        // Current Height(Testing)
                        //___smoothToGroundPosCamDest = i_dest - ___cTrans.forward * ___cTrans.position.y;

                        return false;
                    }

                    return true;
                }
                catch (Exception e)
                {
                    Logger.Error(e);
                    return true;
                }
            }
        }


        /*
        // Enable complete control (debug cam)
        [HarmonyPatch(typeof(CameraControl), "Init")]
        public static class CameraControl_Init_Patch
        {
            public static void Postfix(CameraControl __instance)
            {
                try
                {
                    Logger.Debug($"[CameraControl_Init_POSTFIX] Called");

                    if (__instance.IsInTutorialMode)
                    {
                        return;
                    }

                    __instance.DEBUG_TakeCompleteControl = true;

                }
                catch (Exception e)
                {
                    Logger.Error(e);
                }
            }
        }
        */
    }
}
