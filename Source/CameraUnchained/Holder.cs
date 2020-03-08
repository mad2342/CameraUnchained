using System.Collections.Generic;
using UnityEngine;

namespace CameraUnchained
{
    public static class Fields
    {
        internal static Vector3 lastGroundPos;
        internal static Vector3 lastCameraPos;
        internal static Quaternion lastCameraRot;

        internal static List<string> EventList = new List<string>()
        {
            "BattleTech.ActiveProbeSequence.FireWave",
            "BattleTech.ActorMovementSequence.OnBlipAcquired",
            "BattleTech.ActorMovementSequence.OnPlayerVisChanged",
            "BattleTech.ActorMovementSequence.ShowCamera",
            "BattleTech.ArtilleryObjectiveSequence.SetState",
            "BattleTech.ArtillerySequence.SetState",
            "BattleTech.AttackStackSequence.OnActorDestroyed",
            "BattleTech.AttackStackSequence.OnAttackBegin",
            "BattleTech.AttackStackSequence.OnChildSequenceAdded",
            "BattleTech.MechDFASequence.BuildMeleeDirectorSequence",
            "BattleTech.MechDFASequence.BuildWeaponDirectorSequence",
            "BattleTech.MechDFASequence.OnAdded",
            "BattleTech.MechDisplacementSequence.ShowCamera",
            "BattleTech.MechJumpSequence.ShowCamera",
            "BattleTech.MechMeleeSequence.BuildMeleeDirectorSequence",
            "BattleTech.MechMeleeSequence.BuildWeaponDirectorSequence",
            "BattleTech.MechMeleeSequence.FireWeaponsFinal",
            "BattleTech.MechMeleeSequence.OnAdded",
            "BattleTech.MechMortarSequence.SetState",
            "BattleTech.MechStandSequence.OnAdded",
            "BattleTech.MechStartupSequence.OnAdded",
            "BattleTech.MissionEndSequence.ShowCamera",
            "BattleTech.MultiSequence.FocusCamera",
            "BattleTech.SensorLockSequence.OnAdded",
            "BattleTech.ShowActorInfoSequence.OnChildSequenceAdded",
            "BattleTech.ShowActorInfoSequence.SetState",
            "BattleTech.StrafeSequence.SetState"
        };
    }
}
