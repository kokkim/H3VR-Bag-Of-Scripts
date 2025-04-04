using System;
using System.Collections.Generic;
using UnityEngine;
using FistVR;
using System.Collections;

namespace BagOfScripts
{
    class EjectHandgunMagOnEmpty : MonoBehaviour
    {
        public static readonly Dictionary<HandgunSlide, EjectHandgunMagOnEmpty> _existingAutoMagEjectingHandguns = new();

        [SerializeField] HandgunSlide handgunSlide;
        [SerializeField] bool requiresEmptyChamberForEject = true;

        [Header("Optional")]
        [SerializeField] AudioEvent audEvent_AutoEjectMag;

#if !DEBUG
        public void Awake()
        {
            if (handgunSlide == null)
            {
                if (transform.root.GetComponentInChildren<HandgunSlide>() != null)
                {
                    handgunSlide = transform.root.GetComponentInChildren<HandgunSlide>();
                }

                if (handgunSlide == null) Debug.LogError("No handgun slide assigned or found!");
            }
            else
            {
                _existingAutoMagEjectingHandguns.Add(handgunSlide, this);

                On.FistVR.HandgunSlide.SlideEvent_SmackRear += HandgunSlide_SlideEvent_SmackRear;
            }
        }

        static void HandgunSlide_SlideEvent_SmackRear(On.FistVR.HandgunSlide.orig_SlideEvent_SmackRear orig, HandgunSlide self)
        {
            orig(self);
            if (_existingAutoMagEjectingHandguns.TryGetValue(self, out EjectHandgunMagOnEmpty ejectHG))
            {
                FVRFireArmMagazine mag = self.Handgun.Magazine;
                if (mag != null && !mag.HasARound() && (!ejectHG.requiresEmptyChamberForEject || (ejectHG.requiresEmptyChamberForEject && !self.Handgun.Chamber.IsFull)))
                {
                    self.Handgun.EjectMag();
                    SM.PlayCoreSound(FVRPooledAudioType.GunMech, ejectHG.audEvent_AutoEjectMag, self.transform.position);
                }
            }
        }

        void OnDestroy()
        {
            _existingAutoMagEjectingHandguns.Remove(handgunSlide);
        }
#endif
    }
}
