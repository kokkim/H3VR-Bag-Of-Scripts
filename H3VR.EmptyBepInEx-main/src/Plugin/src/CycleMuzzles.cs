using System;
using System.Collections.Generic;
using FistVR;
using UnityEngine;

namespace BagOfScripts
{
    class CycleMuzzles : MonoBehaviour
    {
        public static readonly Dictionary<FVRFireArm, CycleMuzzles> _existingCycleMuzzles = new();

        public FVRFireArm firearm;
        public Transform[] muzzles;
        private int muzzleIndex;

        [Tooltip("Whether the first shot fired from a fresh mag always fires from muzzle 0 in the array")]
        public bool resetMuzzleIndexOnNewMag = true;

#if !DEBUG
        static CycleMuzzles()
        {
            On.FistVR.FVRFireArm.Fire += FVRFireArm_Fire;
            On.FistVR.FVRFireArm.LoadMag += FVRFireArm_LoadMag;
        }

        void Awake()
        {
            if (firearm == null && transform.GetComponentInParent<FVRFireArm>() != null) firearm = transform.GetComponentInParent<FVRFireArm>();
            _existingCycleMuzzles.Add(firearm, this);
        }

        void OnDestroy()
        {
            _existingCycleMuzzles.Remove(firearm);
        }

        private static void FVRFireArm_Fire(On.FistVR.FVRFireArm.orig_Fire orig, FVRFireArm self, FVRFireArmChamber chamber, Transform muzzle, bool doBuzz, float velMult, float rangeOverride)
        {
            //TODO add muzzle cycling
            if (_existingCycleMuzzles.TryGetValue(self, out CycleMuzzles cycler))
            {
                cycler.muzzleIndex = (cycler.muzzleIndex + 1) % cycler.muzzles.Length;
                orig(self, chamber, cycler.muzzles[cycler.muzzleIndex], doBuzz, velMult, rangeOverride);
                return;
            }
            orig(self, chamber, muzzle, doBuzz, velMult, rangeOverride);
        }

        private static void FVRFireArm_LoadMag(On.FistVR.FVRFireArm.orig_LoadMag orig, FVRFireArm self, FVRFireArmMagazine mag)
        {
            orig(self, mag);
            if (_existingCycleMuzzles.TryGetValue(self, out CycleMuzzles cycler))
            {
                if (cycler.resetMuzzleIndexOnNewMag) cycler.muzzleIndex = 0;
            }
        }
#endif
    }
}