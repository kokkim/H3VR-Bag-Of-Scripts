using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FistVR;

//UNTESTED, MIGHT NOT WORK
namespace BagOfScripts
{
    public class ToggleLerp : FVRInteractiveObject
    {
		[Tooltip("The root FVRPhysicalObject this is parented to")]
		[SerializeField] FVRPhysicalObject mainObject;
		[SerializeField] GameObject objectToMove;
		[SerializeField] Vector2 endPointValues;

		[Tooltip("How long (in seconds) it takes to move the object between positions")]
		[SerializeField] float lerpDuration = 0.5f;
		[SerializeField] FVRPhysicalObject.Axis interpAxis;
		[Tooltip("Rotations will not work beyond 180 degrees because of quaternion fuckery")]
		[SerializeField] FVRPhysicalObject.InterpStyle interpStyle;

		[SerializeField] AudioEvent audioEvent_AToB, audioEvent_BToA;
		[Tooltip("Does the object need to reach the end point before it can be reactivated?")]
		[SerializeField] bool requireReachingEndToReactivate;

		[SerializeField] UnityEvent OnStartAToB, OnStartBToA, OnReachB, OnReachA;

		public enum LerpState
		{
			A,
			AtoB,
			B,
			BtoA
        }
		[HideInInspector]
		public LerpState lerpState;
		float lerpProgress;

		public override void Awake()
		{
			base.Awake();
			if (objectToMove == null) Debug.LogError("objectToMove is not assigned!");
		}

		public override void FVRUpdate()
		{
			base.FVRUpdate();

			if (lerpState == LerpState.AtoB || lerpState == LerpState.BtoA)
            {
				float newLerpAddition = Time.deltaTime / lerpDuration;

				if (lerpState == LerpState.BtoA) newLerpAddition *= -1;

				lerpProgress += newLerpAddition;
				if (lerpProgress >= 1f)
				{
					lerpProgress = 1f;
					lerpState = LerpState.B;
					OnReachB.Invoke();
				}
				else if (lerpProgress <= 0f)
				{
					lerpProgress = 0f;
					lerpState = LerpState.A;
					OnReachA.Invoke();
				}
				mainObject.SetAnimatedComponent(objectToMove.transform, Mathf.Lerp(endPointValues.x, endPointValues.y, lerpProgress), interpStyle, interpAxis);
			}
		}

		public void ToggleLerpState()
		{
			if (lerpState == LerpState.A || (!requireReachingEndToReactivate && lerpState == LerpState.BtoA)) LerpAtoB();
			else if (lerpState == LerpState.B || (!requireReachingEndToReactivate && lerpState == LerpState.AtoB)) LerpBtoA();
		}

		void LerpAtoB()
        {
			lerpState = LerpState.AtoB;
			if (audioEvent_AToB != null) SM.PlayCoreSound(FVRPooledAudioType.Generic, audioEvent_AToB, transform.position);
			OnStartAToB.Invoke();
        }

		void LerpBtoA()
        {
			lerpState = LerpState.BtoA;
			if (audioEvent_AToB != null) SM.PlayCoreSound(FVRPooledAudioType.Generic, audioEvent_BToA, transform.position);
			OnStartBToA.Invoke();
		}

        public override void SimpleInteraction(FVRViveHand hand)
        {
            base.SimpleInteraction(hand);
			ToggleLerpState();
        }
    }
}