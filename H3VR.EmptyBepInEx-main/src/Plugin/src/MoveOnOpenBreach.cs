using System;
using System.Collections.Generic;
using UnityEngine;
using FistVR;
using System.Collections;

namespace BagOfScripts.src
{
    class TogglePosBreakActionWeapon : BreakActionWeapon
    {
        private GameObject movingBreachObject;

        private Vector3 oldPos, oldRot, oldScale;
        [SerializeField] private Vector3 newPos, newRot, newScale;

        private bool isLatchedPrevFrame;

        public override void Awake()
        {
            base.Awake();
            if (movingBreachObject != null)
            {
                StartCoroutine(CheckBreachState());

                oldPos = movingBreachObject.transform.localPosition;
                oldRot = movingBreachObject.transform.localRotation.eulerAngles;
                oldPos = movingBreachObject.transform.localScale;
            }
            else
            {
                Debug.LogWarning("movingBreachObject is not assigned");
            }
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            StopCoroutine(CheckBreachState());
        }

        IEnumerator CheckBreachState()
        {
            while (true)
            {
                if (isLatchedPrevFrame != m_isLatched)
                {
                    if (!m_isLatched)
                    {
                        SetNewTransforms();
                    }
                    else
                    {
                        SetOldTransforms();
                    }
                    isLatchedPrevFrame = m_isLatched;
                }
                yield return null;
            }
        }

        void SetNewTransforms()
        {
            movingBreachObject.transform.localPosition = newPos;
            movingBreachObject.transform.localRotation = Quaternion.Euler(newRot);
            movingBreachObject.transform.localScale = newScale;
        }

        void SetOldTransforms()
        {
            movingBreachObject.transform.localPosition = oldPos;
            movingBreachObject.transform.localRotation = Quaternion.Euler(oldRot);
            movingBreachObject.transform.localScale = oldScale;
        }
    }
}
