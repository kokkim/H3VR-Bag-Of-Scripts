using System;
using UnityEngine;
using FistVR;

namespace BagOfScripts
{
    [RequireComponent(typeof(Collider))]
    class RPMSecondarySwitch : FVRInteractiveObject
    {
        [Header("RPM Secondary Switch Params")]
        [SerializeField] private FVRFireArm? firearm;
        [SerializeField] private GameObject? openOrClosedBolt;
        [SerializeField] private BoltSettings[] boltRPMSettings;
        private int index;

        enum BoltType
        {
            Null,
            Open,
            Closed
        }
        BoltType boltType = BoltType.Null;

        [Header("Visual Params")]
        public Transform secondarySwitch;
        public FVRPhysicalObject.Axis axis;
        public FVRPhysicalObject.InterpStyle interpStyle;

        [Serializable]
        public class BoltSettings
        {
            public float selectorPosition;

            public float forwardSpeed;
            public float rearwardSpeed;
            public float springStiffness;
        }

        void OnValidate()   //Checks for correct components in editor
        {
            if (firearm != null)
            {
                if (firearm.GetComponent<ClosedBoltWeapon>() == null && firearm.GetComponent<OpenBoltReceiver>() == null)
                {
                    firearm = null;
                    Debug.LogError("Assigned firearm is not an open or closed bolt weapon!");
                }
            }

            if (openOrClosedBolt != null)
            {
                if (openOrClosedBolt.GetComponent<OpenBoltReceiverBolt>() == null && openOrClosedBolt.GetComponent<ClosedBolt>() == null)
                {
                    openOrClosedBolt = null;
                    Debug.LogError("Object assigned as bolt has no open or closed bolt component!");
                }
            }
        }

        public override void Awake()
        {
            if (openOrClosedBolt != null)
            {
                if (openOrClosedBolt.GetComponent<OpenBoltReceiverBolt>() != null) boltType = BoltType.Open;
                else if (openOrClosedBolt.GetComponent<ClosedBolt>() != null) boltType = BoltType.Closed;
            }
            else Debug.LogError("No open or closed bolt assigned!");
        }

        public override void SimpleInteraction(FVRViveHand hand)
        {
            base.SimpleInteraction(hand);
            AdvanceSecondarySwitch();
        }

        void AdvanceSecondarySwitch()
        {
            index++;
            if (index == boltRPMSettings.Length) index = 0;

            firearm.SetAnimatedComponent(secondarySwitch, boltRPMSettings[index].selectorPosition, interpStyle, axis);

            switch (boltType)
            {
                case BoltType.Open:
                    OpenBoltReceiverBolt openBolt = openOrClosedBolt.GetComponent<OpenBoltReceiverBolt>();

                    openBolt.BoltSpeed_Forward = boltRPMSettings[index].forwardSpeed;
                    openBolt.BoltSpeed_Rearward = boltRPMSettings[index].rearwardSpeed;
                    openBolt.BoltSpringStiffness = boltRPMSettings[index].springStiffness;
                    break;
                case BoltType.Closed:
                    ClosedBolt closedBolt = openOrClosedBolt.GetComponent<ClosedBolt>();

                    closedBolt.Speed_Forward = boltRPMSettings[index].forwardSpeed;
                    closedBolt.Speed_Rearward = boltRPMSettings[index].rearwardSpeed;
                    closedBolt.SpringStiffness = boltRPMSettings[index].springStiffness;
                    break;
            }
        }
    }
}
