using System;
using System.Collections.Generic;
using UnityEngine;
using FistVR;
using System.Linq;

//Updated for new, non-conflicting format
//Will be done for other scripts as well eventually

namespace BagOfScripts
{
    class DisableChamberOnLoad : MonoBehaviour
    {
        [Header("NOTE: Another script has to re-enable the chamber accessibility!")]
        public FVRFireArm firearm;
        public FVRFireArmChamber[] chambers;

        static readonly Dictionary<FVRFireArm, DisableChamberOnLoad> _existingDisableChamberOnLoad = new();

#if !DEBUG
        static DisableChamberOnLoad()
        {
            On.FistVR.FVRFireArmChamber.SetRound_FVRFireArmRound_bool += FVRFireArmChamber_SetRound_FVRFireArmRound_bool;
        }

        //NEEDS TESTING
        private static void FVRFireArmChamber_SetRound_FVRFireArmRound_bool(On.FistVR.FVRFireArmChamber.orig_SetRound_FVRFireArmRound_bool orig, FVRFireArmChamber self, FVRFireArmRound round, bool animate)
        {
            orig(self, round, animate);
            if (_existingDisableChamberOnLoad.TryGetValue(self.Firearm, out DisableChamberOnLoad disableChamberOnLoad))
            {
                if (disableChamberOnLoad.chambers.Contains(self))
                {
                    self.IsAccessible = false;
                }
            }
        }

        void Awake()
        {
            _existingDisableChamberOnLoad.Add(firearm, this);
        }

        private void OnDestroy()
        {
            _existingDisableChamberOnLoad.Remove(firearm);
        }
#endif
    }
}