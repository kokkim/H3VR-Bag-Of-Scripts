using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BagOfScripts
{
    public class ToggleAnimation : MonoBehaviour
    {
		float animLength, animProgress;
		bool isForward = false;
		enum ToggleType
		{
			Animation,
			Visibility
		}
		[SerializeField] ToggleType toggleType = ToggleType.Animation;

		[Header("Only used with ToggleType of Animation")]
		public GameObject objectToAnimate;
		public string? forwardAnimationName, rewindAnimationName;
		private Animator? animator;

		[Header("Only used with ToggleType of Visibility")]
		public GameObject? forwardObject;
		public GameObject? rewindObject;


		void Awake()
		{
			switch (toggleType)
            {
				case ToggleType.Animation:
                    {
						if (objectToAnimate != null)
						{
							animator = objectToAnimate.GetComponent<Animator>();
							if (animator != null && animator.runtimeAnimatorController.animationClips.Length == 2)
							{
								animLength = animator.runtimeAnimatorController.animationClips[0].length;
								if (animator.runtimeAnimatorController.animationClips[0].length != animator.runtimeAnimatorController.animationClips[0].length)
								{
									Debug.LogWarning("The two animations in " + objectToAnimate + "are of differing lengths. This will cause jittery movement.");
								}
							}
							else Debug.LogError("animator is missing or it doesn't have only two animations!");
						}
						else Debug.LogError("objectToAnimate is missing!");
						break;
                    }
				case ToggleType.Visibility:
                    {
						if (forwardObject == null && rewindObject == null)
                        {
							Debug.LogError("forward and rewind objects are both missing, or ToggleType is accidentally on Visibility!");
                        }
						break;
                    }
            }
		}

		void Update()
		{
			if (isForward)
			{
				animProgress += Time.deltaTime;
			}
			else if (!isForward)
			{
				animProgress -= Time.deltaTime;
			}
			animProgress = Mathf.Clamp(animProgress, 0, animLength);
		}

		public void Toggle(bool _isForward)
		{
			//The animation starts at a percentage calculated from the animation progress 
			if (_isForward)
			{
				if (toggleType == ToggleType.Animation)
                {
					animator.Play(forwardAnimationName, 0, animProgress / animLength);
				}
                else
                {
					if (forwardObject != null) forwardObject.SetActive(true);
					if (rewindObject != null) rewindObject.SetActive(false);
                }
			}
			else
			{
				if (toggleType == ToggleType.Animation)
				{
					animator.Play(rewindAnimationName, 0, 1 - animProgress / animLength);
				}
				else
				{
					if (forwardObject != null) forwardObject.SetActive(false);
					if (rewindObject != null) rewindObject.SetActive(true);
				}
			}
		}
	}
}