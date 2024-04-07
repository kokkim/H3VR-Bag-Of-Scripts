using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;

namespace BagOfScripts
{
    class DropSlideOnMagRelease : MonoBehaviour
    {
        [Header("Weapon type must have an external magazine and a bolt or slide!")]
        public FVRFireArm firearm;

#if !DEBUG

        void Awake()
        {
            Hook();
        }

        private void OnDestroy()
        {
            Unhook();
        }

        private void FVRFireArm_EjectMag(On.FistVR.FVRFireArm.orig_EjectMag orig, FVRFireArm self, bool PhysicalRelease)
        {
            orig(self, PhysicalRelease);
            if (self is Handgun handgun && handgun == firearm && handgun.Slide != null) handgun.DropSlideRelease();
            else if (self is ClosedBoltWeapon closedBoltWeapon && closedBoltWeapon == firearm && closedBoltWeapon.Bolt != null) closedBoltWeapon.Bolt.ReleaseBolt();
            else if (firearm is OpenBoltReceiver openBoltReceiver && openBoltReceiver == firearm && openBoltReceiver.Bolt != null) openBoltReceiver.ReleaseSeer();
        }

        void Hook()
        {
            On.FistVR.FVRFireArm.EjectMag += FVRFireArm_EjectMag;
        }

        void Unhook()
        {
            On.FistVR.FVRFireArm.EjectMag -= FVRFireArm_EjectMag;
        }
#endif
    }
}