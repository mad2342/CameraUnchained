using System.Collections.Generic;

namespace CameraUnchained
{
    internal class Settings
    {
        public List<string> EventWhitelist = new List<string>()
        {
            "BattleTech.ArtilleryObjectiveSequence.SetState",
            "BattleTech.ArtillerySequence.SetState",
            "BattleTech.MechMortarSequence.SetState",
            "BattleTech.MissionEndSequence.ShowCamera",
            "BattleTech.MultiSequence.FocusCamera",
            "BattleTech.StrafeSequence.SetState"
        };
        public bool FocusOnStandingUp = true;
        public bool FocusOnPoweringUp = true;
        public bool FocusOnMovement = true;
        public bool FocusOnSensorLock = true;
        public bool FocusOnMeleeTarget = true;
        public bool FocusOnRangedTarget = true;

        public bool SaveAndRestoreCamPos = false;
        public bool CenterOnTarget = false;
    }
}
