using System;
using System.Collections.Generic;
using UnityEngine;
using FistVR;

namespace BagOfScripts
{
    class TriggerActivatedLaser : MonoBehaviour
    {
        public static readonly Dictionary<LaserLightAttachment, TriggerActivatedLaser> _existingTriggerActivatedLasers = new();

        public LaserLightAttachment laser;

        public bool playSoundOnLaserToggle;
        public float laserTriggerDeadzone = 0.01f;

        FVRFireArm? curFireArm;
        bool triggerPulled;

        void Awake()
        {
            if (laser == null && GetComponent<LaserLightAttachment>() != null)
            {
                laser = GetComponent<LaserLightAttachment>();
            }
            _existingTriggerActivatedLasers.Add(laser, this);

#if !DEBUG
            On.FistVR.LaserLightAttachment.OnAttach += LaserLightAttachment_OnAttach;
            On.FistVR.LaserLightAttachment.OnDetach += LaserLightAttachment_OnDetach;
#endif
        }

#if !DEBUG
        void OnDestroy()
        {
            _existingTriggerActivatedLasers.Remove(laser);

            On.FistVR.LaserLightAttachment.OnAttach -= LaserLightAttachment_OnAttach;
            On.FistVR.LaserLightAttachment.OnDetach -= LaserLightAttachment_OnDetach;
        }

        void Update()
        {
            if (laser.Attachment.curMount == null)
            {
                DisableLaser();
                return;
            }

            if (curFireArm != null && curFireArm.m_hand != null)
            {
                float triggerFloat = curFireArm.m_hand.Input.TriggerFloat;

                if (!triggerPulled)
                {
                    if (triggerFloat >= laserTriggerDeadzone)
                    {
                        triggerPulled = true;
                        EnableLaser();
                    }
                }
                else if (triggerFloat < laserTriggerDeadzone)
                {
                    triggerPulled = false;
                    DisableLaser();
                }
            }
        }

        void EnableLaser()
        {
            laser.SettingsIndex = laser.m_savedSetting;
            if (playSoundOnLaserToggle && laser.UI != null) SM.PlayCoreSound(FVRPooledAudioType.GenericClose, laser.UI.AudEvent_Click, transform.position);
            laser.UpdateParams();
        }

        void DisableLaser()
        {
            laser.m_savedSetting = laser.SettingsIndex;
            laser.SettingsIndex = 0;
            if (playSoundOnLaserToggle && laser.UI != null) SM.PlayCoreSound(FVRPooledAudioType.GenericClose, laser.UI.AudEvent_Clack, transform.position);
            laser.UpdateParams();
        }

        private void LaserLightAttachment_OnAttach(On.FistVR.LaserLightAttachment.orig_OnAttach orig, LaserLightAttachment self)    //Assign CurFireArm
        {
            orig(self);
            if (_existingTriggerActivatedLasers.TryGetValue(self, out TriggerActivatedLaser TAL))
            {
                if (self.Attachment.curMount.GetRootMount().MyObject is FVRFireArm)
                {
                    TAL.curFireArm = laser.Attachment.curMount.GetRootMount().MyObject as FVRFireArm;
                }
            }
        }

        private void LaserLightAttachment_OnDetach(On.FistVR.LaserLightAttachment.orig_OnDetach orig, LaserLightAttachment self)    //Un-assign CurFireArm
        {
            orig(self);
            if (_existingTriggerActivatedLasers.TryGetValue(self, out TriggerActivatedLaser TAL))
            {
                TAL.curFireArm = null;
            }
        }
#endif
    }
}