///PlaySoundOnLoadState.cs by Okkim
///Plays a sound for either when the gun runs out of ammo or when the specified magazine is loaded

using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using FistVR;
using Valve.VR.InteractionSystem;

namespace BagOfScripts
{
    class PlaySoundOnLoadState : MonoBehaviour
    {
		[Header("Only fill firearm or external magazine, not both!")]
		[Header("If script is attached to firearm")]
		[SerializeField] private FVRFireArm? firearm;
		[Tooltip("Do you want the sound to play on the final shot, even if the gun doesn't have a magazine loaded in it?")]
		public bool finalShotRequiresMagazine = false;

		[Header("If script is attached to external magazine")]
		[SerializeField] private FVRFireArmMagazine? magazine;

		[Header("Clips")]
		public bool playSoundOnMagazineFull = false;
		[Tooltip("The sound the gun plays when the magazine is fully loaded.")]
		[SerializeField] private AudioEvent? onMagazineFull;
		public bool playSoundOnFirearmEmpty = false;
		[Tooltip("The sound the gun plays when it fires its final shot.")]
		[SerializeField] private AudioEvent? onFirearmEmpty;

#if !DEBUG
		private void Awake()
		{
			if (firearm != null && magazine != null)
            {
				Debug.LogError("Warning: Both firearm and external magazine fields are filled out!");
            }
			if (firearm == null && magazine == null)
            {
				Debug.LogError("Warning: no FVRFireArm or FVRFireArmMagazine assigned!");
            }

			if (playSoundOnFirearmEmpty)
            {
				//Debug.Log("PlaySoundOnFirearmEmpty");
				GM.CurrentSceneSettings.ShotFiredEvent += OnShotFired;
			}

			if (playSoundOnMagazineFull)
            {
				//Debug.Log("PlaySoundOnMagazineFull");
				Hook();
			}
		}

		void Hook()
        {
			On.FistVR.FVRFireArmMagazine.AddRound_FVRFireArmRound_bool_bool_bool += FVRFireArmMagazine_AddRound_FVRFireArmRound_bool_bool_bool;
        }

		void Unhook()
        {
			On.FistVR.FVRFireArmMagazine.AddRound_FVRFireArmRound_bool_bool_bool -= FVRFireArmMagazine_AddRound_FVRFireArmRound_bool_bool_bool;
		}

		//if magazine is fully loaded, plays a sound
		private void FVRFireArmMagazine_AddRound_FVRFireArmRound_bool_bool_bool(On.FistVR.FVRFireArmMagazine.orig_AddRound_FVRFireArmRound_bool_bool_bool orig, FVRFireArmMagazine self, FVRFireArmRound round, bool makeSound, bool updateDisplay, bool animate)
		{
			orig(self, round, makeSound, updateDisplay, animate);

			//Runs if firearm assigned
			if (firearm != null && magazine == null)
            {
				if (self != null && self == firearm.Magazine && self.m_numRounds == self.m_capacity)
                {
					SM.PlayCoreSound(FVRPooledAudioType.Generic, onMagazineFull, firearm.Magazine.transform.position);
				}
			}
			//Runs if magazine assigned
			if (firearm == null && magazine != null)
			{
				if (self != null && self == magazine && self.m_numRounds == self.m_capacity)
				{
					SM.PlayCoreSound(FVRPooledAudioType.Generic, onMagazineFull, magazine.transform.position);
				}
			}
		}

		private void OnShotFired(FVRFireArm _firearm) //checks if gun is empty, and if it is, plays a sound
		{
			//Runs if firearm assigned
			if (firearm != null && magazine == null)
			{
				if (_firearm == firearm)
				{
					//if round fired and the magazine is null, it must be empty.
					if (!finalShotRequiresMagazine)
                    {
						if (firearm.Magazine == null)
						{
							SM.PlayCoreSound(FVRPooledAudioType.Generic, onFirearmEmpty, firearm.transform.position);
						}
					}
					//if round fired and magazine has no bullets, it must be empty.
					if (firearm.Magazine != null && firearm.Magazine.m_numRounds < 1)
					{
						SM.PlayCoreSound(FVRPooledAudioType.Generic, onFirearmEmpty, firearm.transform.position);
					}


				}
			}
			//Runs if magazine assigned
			if (firearm == null && magazine != null)
			{
				if (magazine.FireArm == _firearm)
				{
					//if round fired and magazine has no bullets, it must be empty.
					if (magazine.m_numRounds < 1)
					{
						SM.PlayCoreSound(FVRPooledAudioType.Generic, onFirearmEmpty, magazine.FireArm.transform.position);
					}
				}
			}
		}

		private void OnDestroy()
		{
			GM.CurrentSceneSettings.ShotFiredEvent -= OnShotFired;
			Unhook();
		}
#endif
	}
}