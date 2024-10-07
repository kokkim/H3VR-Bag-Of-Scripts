///JukeBox.cs by Arpy
///Rewritten and expanded on by Okkim

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FistVR;

namespace BagOfScripts
{
	public class JukeBox : MonoBehaviour
	{
		[SerializeField] private AudioClip recordStartSound;
		[HideInInspector] public RecordDisc curDisc;
		private RecordDisc prevDisc;
		[SerializeField] private AudioSource music;
		[SerializeField] private AudioSource startSoundSource;
		[SerializeField] private Transform discPosition;
		[SerializeField] private float discRotateSpeed = 1;

		public bool isPlaying, hasMusicStarted;
		private float songProgress, songLength;

		[SerializeField] private JukeBoxButton pausePlayInteractable, muteInteractable;
		private GameObject rotatingArm;
		[SerializeField] private Vector2 minMaxYArmRot = new Vector2();

		void Start()
		{
			rotatingArm = muteInteractable.thingNegativeObject;
			startSoundSource.clip = recordStartSound;
		}

		private void OnTriggerEnter(Collider other)
		{
			if (curDisc == null)
			{
				RecordDisc TD = other.GetComponentInParent<RecordDisc>();
				if (TD != null && TD != prevDisc)
				{
					DiscInsert(TD);
				}
			}
        }

		void OnTriggerExit(Collider other)
		{
			if (prevDisc != null)
            {
				if (other.gameObject == prevDisc.gameObject)
				{
					prevDisc = null;
				}
			}
		}

		void Update()
		{
			if (hasMusicStarted && isPlaying)
            {
				discPosition.Rotate(0, Time.deltaTime * discRotateSpeed, 0);

				if (songLength != 0)
				{
					songProgress = music.time / songLength;
					rotatingArm.transform.localRotation = Quaternion.Euler(0f, Mathf.Lerp(minMaxYArmRot.x, minMaxYArmRot.y, songProgress), 0f);
				}
                else
                {
					rotatingArm.transform.localRotation = Quaternion.Euler(0f, minMaxYArmRot.x, 0f);
				}
            }
		}

		private void DiscInsert(RecordDisc D)
        {
			D.ForceBreakInteraction();
			D.CurBox = this;
			curDisc = D;
			prevDisc = null;

			music.clip = D.RecordSong;
			songLength = music.clip.length;
			songProgress = 0f;
			music.mute = true;

			D.SetParentage(discPosition);
			D.Transform.localPosition = new Vector3(0, 0, 0);
			D.Transform.localRotation = Quaternion.identity;
			D.StoreAndDestroyRigidbody();

			if (pausePlayInteractable.thingPositiveObject.activeSelf)
			{
				startMusic();
			}
		}

		public void DiscExit()
		{
			songProgress = 0f;
			stopMusic();
			music.clip = null;

			curDisc.RecoverRigidbody();
			prevDisc = curDisc;
			curDisc = null;

			music.mute = true;
			muteInteractable.SetPositive();
		}

		public void startMusic()
        {
			if (music.clip != null)
			{
				hasMusicStarted = true;
				if (muteInteractable.thingNegativeObject.activeSelf)
				{
					startSoundSource.Play();
					music.PlayDelayed(recordStartSound.length);
				}
                else
                {
					music.Play();
                }
				isPlaying = true;
				pausePlayInteractable.SetPositive();
			}
        }

		public void stopMusic()
        {
			startSoundSource.Stop();
			music.UnPause();
			music.Stop();
			hasMusicStarted = false;
        }
		public void pausePlay()
        {
			if (isPlaying)
			{
				music.Pause();
				isPlaying = false;
				if (pausePlayInteractable != null)
				{
					pausePlayInteractable.SetNegative();
				}
			}
            else
			{
				music.UnPause();
				isPlaying = true;
				if (pausePlayInteractable != null)
				{
					pausePlayInteractable.SetPositive();
				}
			}
        }

		public void restartMusic()
        {
			hasMusicStarted = true;
			startSoundSource.Stop();
			music.Stop();
			startSoundSource.Play();
			music.PlayDelayed(recordStartSound.length);
			isPlaying = false;
		}

		public void repeatMusic()
        {
			music.loop = !music.loop;
        }

		public void muteMusic()
        {
			music.mute = !music.mute;

			if (curDisc != null)
			{
				muteInteractable.swapActiveObjects();
			}
		}
	}
}