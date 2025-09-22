using System;
using System.Collections.Generic;
using UnityEngine;
using FistVR;

namespace BagOfScripts
{
    [RequireComponent(typeof(Collider))]
    class OpenBoltProgressiveTrigger : MonoBehaviour
    {
        public static readonly Dictionary<OpenBoltReceiver, OpenBoltProgressiveTrigger> _existingOpenBoltProgressiveTriggers = new();

        public OpenBoltReceiver receiver;

        [Header("Must be set higher than TriggerFiringThreshold")]
        public float progressiveFullAutoThreshold;
        [SerializeField] int fireModeIndexToChange;
        private bool isTriggerSecondStageEngaged;

#if !DEBUG
        public void Awake()
        {
            if (receiver == null)
            {
                if (transform.root.GetComponentInChildren<OpenBoltReceiver>() != null)
                {
                    receiver = transform.root.GetComponentInChildren<OpenBoltReceiver>();
                }

                if (receiver != null) Hook();
                else Debug.LogError("No open bolt receiver assigned or found!");
            }
            else
            {
                Hook();
                receiver.FireSelector_Modes[fireModeIndexToChange].ModeType = OpenBoltReceiver.FireSelectorModeType.Single;
            }
        }

        private void OpenBoltReceiver_UpdateInteraction(On.FistVR.OpenBoltReceiver.orig_UpdateInteraction orig, OpenBoltReceiver self, FVRViveHand hand)
        {
            orig(self, hand);
            if (_existingOpenBoltProgressiveTriggers.TryGetValue(self, out OpenBoltProgressiveTrigger _))
            {
                if (receiver.m_triggerFloat >= progressiveFullAutoThreshold)
                {
                    if (!isTriggerSecondStageEngaged) SetFullAuto();
                }
                else if (isTriggerSecondStageEngaged) SetSingle();
            }
        }

        void SetFullAuto()
        {
            receiver.FireSelector_Modes[fireModeIndexToChange].ModeType = OpenBoltReceiver.FireSelectorModeType.FullAuto;
            receiver.m_hasTriggerCycled = false;
            isTriggerSecondStageEngaged = true;
        }

        void SetSingle()
        {
            receiver.FireSelector_Modes[fireModeIndexToChange].ModeType = OpenBoltReceiver.FireSelectorModeType.Single;
            isTriggerSecondStageEngaged = false;
        }

        void Hook()
        {
            _existingOpenBoltProgressiveTriggers.Add(receiver, this);
            On.FistVR.OpenBoltReceiver.UpdateInteraction += OpenBoltReceiver_UpdateInteraction;
        }

        void OnDestroy()
        {
            _existingOpenBoltProgressiveTriggers.Remove(receiver);
        }
#endif
    }
}
