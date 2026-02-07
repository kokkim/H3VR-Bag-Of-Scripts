using FistVR;
using OpenScripts2;
using UnityEngine;
using System.Reflection;

//Thanks to Cityrobo for assistance and the required hotfix to BreakOpenTrigger

namespace BagOfScripts
{
    public class BreakOpenTriggerGrabLatch : FVRInteractiveObject
    {
        public override void Awake()
        {
            base.Awake();
            latch_forward = Point_Fore.localPosition.z;
            latch_rear = Point_Rear.localPosition.z;
            m_currentHandleZ = transform.localPosition.z;

        }

        public override void UpdateInteraction(FVRViveHand hand)
        {
            base.UpdateInteraction(hand);
            Vector3 closestValidPoint = GetClosestValidPoint(Point_Fore.position, Point_Rear.position, m_hand.Input.Pos);
            transform.position = closestValidPoint;
            m_currentHandleZ = transform.localPosition.z;
            if (HasRotatingPart)
            {
                Vector3 normalized = (transform.position - m_hand.PalmTransform.position).normalized;
                if (Vector3.Dot(normalized, transform.right) > 0f)
                {
                    RotatingPart.localEulerAngles = RotatingPartLeftEulers;
                }
                else
                {
                    RotatingPart.localEulerAngles = RotatingPartRightEulers;
                }
            }
        }

        public override void EndInteraction(FVRViveHand hand)
        {
            if (HasRotatingPart)
            {
                RotatingPart.localEulerAngles = RotatingPartNeutralEulers;
            }
            base.EndInteraction(hand);
        }

        public override void FVRUpdate()
        {
            base.FVRUpdate();
            //reset pos when not held
            if (!IsHeld && Mathf.Abs(m_currentHandleZ - latch_forward) > 0.001f)
            {
                m_currentHandleZ = Mathf.MoveTowards(m_currentHandleZ, latch_forward, Time.deltaTime * ForwardSpeed);
                transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, m_currentHandleZ);
            }

            //handle position state
            if (Mathf.Abs(m_currentHandleZ - latch_forward) < 0.005f)
            {
                CurPos = LatchHandlePos.Forward;
            }
            else if (Mathf.Abs(m_currentHandleZ - latch_rear) < 0.005f)
            {
                CurPos = LatchHandlePos.Rear;
            }
            else
            {
                CurPos = LatchHandlePos.Middle;
            }

            if (CurPos == LatchHandlePos.Forward && LastPos != LatchHandlePos.Forward)
            {
                if (firearm != null)
                {
                    firearm.PlayAudioEvent(FirearmAudioEventType.StockClosed, 1f);
                }
            }
            else if (CurPos == LatchHandlePos.Rear && LastPos != LatchHandlePos.Rear)
            {
                if (firearm != null)
                {
                    firearm.PlayAudioEvent(FirearmAudioEventType.StockOpen, 1f);
                }
            }
            if (breakOpenTrigger != null)
            {
                if (m_currentHandleZ < Point_Unlatch.localPosition.z)
                {
                    breakOpenTrigger._latchHeldOpen = true;
                    typeof(BreakOpenTrigger).GetField("_latchRot", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(breakOpenTrigger, breakOpenTrigger.MaxLatchRot);
                }
                else
                {
                    breakOpenTrigger._latchHeldOpen = false;
                    typeof(BreakOpenTrigger).GetField("_latchRot", BindingFlags.Instance | BindingFlags.NonPublic).SetValue(breakOpenTrigger, 0f);
                }
            }
            LastPos = CurPos;
        }

        [Header("ChargingHandle")]
        public BreakOpenTrigger breakOpenTrigger;
        public FVRFireArm firearm;

        public Transform Point_Fore, Point_Rear, Point_Unlatch;

        public float ForwardSpeed = 1f;

        private float latch_forward, latch_rear, m_currentHandleZ;

        public LatchHandlePos CurPos, LastPos;

        [Header("Rotating Bit")]
        public bool HasRotatingPart;

        public Transform RotatingPart;

        public Vector3 RotatingPartNeutralEulers, RotatingPartLeftEulers, RotatingPartRightEulers;

        public enum LatchHandlePos
        {
            Forward,
            Middle,
            Rear
        }
    }
}
