using System;
using System.Collections.Generic;
using UnityEngine;
using FistVR;

namespace BagOfScripts
{
    class CustomCenterOfMass : MonoBehaviour
    {
        [Header("Note: Will probably cause stuttering with break-action weapons!")]

        [SerializeField] private FVRPhysicalObject mainObject;
        [SerializeField] private Transform centerOfMassOverride;

        [Tooltip("Set to true if you want attachments to change the weapon's center of mass. This will ignore the center of mass override, nothing I can do about that unfortunately.")]
        public bool attachmentsChangeCenterOfMass;

        void Awake()
        {
            if (mainObject == null)
            {
                Debug.LogError("mainObject is not assigned!");
                return;
            }

            if (centerOfMassOverride == null)
            {
                Debug.LogError("centerOfMassOverride is not assigned!");
                return;
            }
#if !DEBUG
            On.FistVR.FVRPhysicalObject.ResetClampCOM += FVRPhysicalObject_ResetClampCOM;
#endif
        }

#if !DEBUG
        void OnDestroy()
        {
            On.FistVR.FVRPhysicalObject.ResetClampCOM -= FVRPhysicalObject_ResetClampCOM;
        }

        void Start()
        {
            mainObject.RootRigidbody.centerOfMass = centerOfMassOverride.localPosition;
            mainObject.m_storedCOMLocal = centerOfMassOverride.localPosition;
        }

        private void FVRPhysicalObject_ResetClampCOM(On.FistVR.FVRPhysicalObject.orig_ResetClampCOM orig, FVRPhysicalObject self)
        {
            if (attachmentsChangeCenterOfMass)
            {
                orig(self);
                if (mainObject.AttachmentsList.Count == 0)
                {
                    mainObject.RootRigidbody.centerOfMass = centerOfMassOverride.localPosition;
                }
            }
        }
#endif
    }
}