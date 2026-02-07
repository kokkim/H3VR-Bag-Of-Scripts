using System;
using System.Collections.Generic;
using FistVR;
using UnityEngine;

//Thanks to Potatoes for allowing me to rework their script

namespace BagOfScripts
{
    class ImprovedAutoRackOnMagLoad : MonoBehaviour
    {
        public static readonly Dictionary<FVRFireArm, ImprovedAutoRackOnMagLoad> _existingAutoRacks = new();

        [Header("Only handguns, closed bolt and open bolt weapons are supported")]
        public FVRFireArm weapon;
        public bool onlyRackOnEmptyChamber = true;
        private Handgun hg;
        private ClosedBoltWeapon cbw;
        private OpenBoltReceiver obr;

#if !DEBUG

        static ImprovedAutoRackOnMagLoad()
        {
            On.FistVR.FVRFireArm.LoadMag += FVRFireArm_LoadMag;
        }

        void Awake()
        {
            _existingAutoRacks.Add(weapon, this);

            if (weapon is Handgun)
            {
                hg = weapon as Handgun;
            }
            else if (weapon is ClosedBoltWeapon)
            {
                cbw = weapon as ClosedBoltWeapon;
            }
            else if (weapon is Handgun)
            {
                obr = weapon as OpenBoltReceiver;
            }
        }

        void OnDestroy()
        {
            _existingAutoRacks.Remove(weapon);
        }

        private static void FVRFireArm_LoadMag(On.FistVR.FVRFireArm.orig_LoadMag orig, FVRFireArm self, FVRFireArmMagazine mag)
        {
            orig(self, mag);
            if (_existingAutoRacks.TryGetValue(self, out ImprovedAutoRackOnMagLoad autoRack))
            {
                if (!mag.HasARound()) return;   //skips if mag is empty
                if (autoRack.hg != null)
                {
                    if (autoRack.onlyRackOnEmptyChamber && autoRack.hg.Chamber.IsFull) return;  //skips chambering a round if it the chamber is full
                    autoRack.hg.Slide.ImpartFiringImpulse();
                }
                if (autoRack.cbw != null)
                {
                    if (autoRack.onlyRackOnEmptyChamber && autoRack.cbw.Chamber.IsFull) return;
                    autoRack.cbw.Bolt.ImpartFiringImpulse();
                }
                if (autoRack.obr != null)
                {
                    if (autoRack.onlyRackOnEmptyChamber && autoRack.obr.Chamber.IsFull) return;
                    autoRack.obr.Bolt.ImpartFiringImpulse();
                }
            }
        }
#endif
    }
}