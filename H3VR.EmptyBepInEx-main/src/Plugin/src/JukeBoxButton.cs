///JukeBox.cs by Arpy
///Rewritten and expanded on by Okkim

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FistVR;

namespace BagOfScripts
{
    public class JukeBoxButton : FVRInteractiveObject
    {
        public JukeBox jukebox;
        public JBButtonTypes ButtonTypes;
        public GameObject thingPositiveObject;
        public GameObject thingNegativeObject;
        [SerializeField] private AudioEvent positiveButtonSounds, negativeButtonSounds;

        public override void SimpleInteraction(FVRViveHand hand)
        {
            base.SimpleInteraction(hand);
            switch (ButtonTypes)
            {
                case JBButtonTypes.Restart:
                    jukebox.restartMusic();
                    break;

                case JBButtonTypes.PausePlay:
                    if (jukebox.curDisc != null)
                    {
                        if (!jukebox.hasMusicStarted)
                        {
                            jukebox.startMusic();
                        }
                        else
                        {
                            jukebox.pausePlay();
                        }
                    }
                    else
                    {
                        swapActiveObjects();
                        if (thingPositiveObject.activeSelf)
                        {
                            jukebox.isPlaying = true;
                        }
                        else
                        {
                            jukebox.isPlaying = false;
                        }
                    }
                    break;

                case JBButtonTypes.Eject:
                    break;

                case JBButtonTypes.Repeat:
                    jukebox.repeatMusic();
                    swapActiveObjects();
                    break;

                case JBButtonTypes.Start:
                    jukebox.startMusic();
                    break;

                case JBButtonTypes.Mute:
                    jukebox.muteMusic();
                    break;

                default:
                    break;
            }
        }
        public void swapActiveObjects()
        {
            if (thingPositiveObject != null && thingPositiveObject != null)
            {
                if (thingNegativeObject.activeSelf)
                {
                    thingNegativeObject.SetActive(false);
                    thingPositiveObject.SetActive(true);

                    PlayToggleButtonSound(true);
                }
                else if (thingPositiveObject.activeSelf)
                {
                    thingNegativeObject.SetActive(true);
                    thingPositiveObject.SetActive(false);

                    PlayToggleButtonSound(false);
                }
            }
        }

        public void SetPositive()
        {
            if (thingPositiveObject != null && thingPositiveObject != null)
            {
                thingPositiveObject.SetActive(true);
                thingNegativeObject.SetActive(false);
            }
            PlayToggleButtonSound(true);
        }

        public void SetNegative()
        {
            if (thingPositiveObject != null && thingPositiveObject != null)
            {
                thingPositiveObject.SetActive(false);
                thingNegativeObject.SetActive(true);
            }
            PlayToggleButtonSound(false);
        }

        public void PlayToggleButtonSound(bool isPositive)
        {
            if (isPositive)
            {
                if (positiveButtonSounds.Clips.Count > 0)
                {
                    SM.PlayGenericSound(positiveButtonSounds, transform.position);
                }
            }
            else
            {
                if (negativeButtonSounds.Clips.Count > 0)
                {
                    SM.PlayGenericSound(negativeButtonSounds, transform.position);
                }
            }
        }

        public enum JBButtonTypes
        {
            Restart,
            PausePlay,
            Eject,
            Repeat,
            Start,
            Mute
        }
    }
}
