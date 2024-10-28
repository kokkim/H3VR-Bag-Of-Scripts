using System;
using System.Collections.Generic;
using UnityEngine;
using FistVR;

namespace BagOfScripts
{
    [RequireComponent(typeof(Collider))]
    class SecondaryAttachmentDetachPoint : FVRInteractiveObject
    {
        public FVRFireArmAttachmentInterface interfaceToDetach;
        public Collider grabTrigger;

        public override void Awake()
        {
            base.Awake();
            if (grabTrigger == null)
            {
                if (GetComponent<Collider>() != null) grabTrigger = GetComponent<Collider>();
                else Debug.LogError("No collider for SecondaryAttachmentDetachPoint!");
            }
            if (grabTrigger != null && !grabTrigger.isTrigger) Debug.LogError("SecondaryAttachmentDetachPoint's collider is not a trigger!");
        }

        public override void FVRUpdate()
        {
            base.FVRUpdate();
            if (interfaceToDetach.Attachment.curMount != null)
            {
                grabTrigger.enabled = true;
            }
            else grabTrigger.enabled = false;
        }

        public override void UpdateInteraction(FVRViveHand hand)
        {
            base.UpdateInteraction(hand);

            bool doDetach = false;
            if (hand.IsInStreamlinedMode)
            {
                if (hand.Input.AXButtonDown)
                {
                    doDetach = true;
                }
            }
            else if (hand.Input.TouchpadDown && hand.Input.TouchpadAxes.magnitude > 0.25f && Vector2.Angle(hand.Input.TouchpadAxes, Vector2.down) <= 45f)
            {
                doDetach = true;
            }
            if (doDetach && !interfaceToDetach.IsLocked && interfaceToDetach.Attachment != null && interfaceToDetach.Attachment.curMount != null && !interfaceToDetach.HasAttachmentsOnIt() && interfaceToDetach.Attachment.CanDetach())
            {
                interfaceToDetach.DetachRoutine(hand);
                grabTrigger.enabled = false;
            }
        }
    }
}
