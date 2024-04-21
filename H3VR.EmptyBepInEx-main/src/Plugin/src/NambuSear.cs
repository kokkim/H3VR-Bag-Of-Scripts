using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;

namespace BagOfScripts
{
    [RequireComponent(typeof(Collider))]
    class NambuSear : FVRInteractiveObject, IFVRDamageable
    {
        [SerializeField] private Handgun handgun;

        private enum SearType
        {
            trigger,
            collision
        }

        [Tooltip("Remember to tick the \"is trigger\" checkbox if using trigger mode.")]
        [SerializeField] private SearType searType;

        public override void Awake()
        {
            if (handgun == null)
            {
                Debug.LogError("handgun is not assigned!");
            }
        }

        public override void SimpleInteraction(FVRViveHand hand)
        {
            base.SimpleInteraction(hand);
            if (searType == SearType.trigger)
            {
                handgun.ReleaseSeer();
            }
        }

        public void Damage(Damage d)
        {
            if (searType == SearType.collision)
            {
                handgun.ReleaseSeer();
            }
        }

        void Update()
        {
            if (!handgun.m_isSeerReady)
            {
                if (!handgun.IsHeld)
                {
                    if (handgun.Slide.CurPos == HandgunSlide.SlidePos.Forward)
                    {
                        handgun.m_isSeerReady = true;
                    }
                }
            }
        }
    }
}
