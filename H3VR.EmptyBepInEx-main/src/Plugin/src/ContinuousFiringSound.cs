using System;
using System.Collections.Generic;
using UnityEngine;
using FistVR;
using System.Collections;

namespace BagOfScripts
{
    class ContinuousFiringSound : MonoBehaviour
    {
        [Header("Closed bolt, open bolt or pistol")]
        [SerializeField] private FVRFireArm firearm;
        [SerializeField] private AudioSource fireLoop;

        [Header("Optional")]
        [SerializeField] private bool hasSingleShot;
        [SerializeField] private AudioEvent? singleShot;
        [Space(5)]
        [SerializeField] private bool hasFiringStart;
        [SerializeField] private AudioEvent? firingStart;
        [SerializeField] private bool hasFiringStop;
        [SerializeField] private AudioEvent? firingStop;

        [Header("Fade Out")]
        [SerializeField] private bool fadesOut;
        [SerializeField] private AnimationCurve? fadeOutCurve;

        private float _fadeOutDuration, _origAudioVolume, timeSinceFadeOutStart;
        private IEnumerator _FadeOutCoroutine;

        private bool _isFiringSoundPlaying, _isFiring, hasFiredSingleShot;



#if !DEBUG
        void Awake()
        {
            GM.CurrentSceneSettings.ShotFiredEvent += OnShotFired;

            if (fadesOut && fadeOutCurve != null)
            {
                _fadeOutDuration = fadeOutCurve.keys[fadeOutCurve.length - 1].time;
                _origAudioVolume = fireLoop.volume;
                _FadeOutCoroutine = FiringSoundFadeOut();
                StartCoroutine(_FadeOutCoroutine);
            }

            if (firearm is ClosedBoltWeapon CBW)
            {
                On.FistVR.ClosedBoltWeapon.UpdateInputAndAnimate += ClosedBoltWeapon_UpdateInputAndAnimate;
            }
            else if (firearm is OpenBoltReceiver OBR)
            {
                On.FistVR.OpenBoltReceiver.UpdateInteraction += OpenBoltReceiver_UpdateInteraction;
            }/*
            else if (firearm is Handgun HG)
            {
                On.FistVR.Handgun.UpdateInputAndAnimate += Handgun_UpdateInputAndAnimate;
            }*/
            else
            {
                Debug.LogError("Unsupported firearm type!");
            }
        }

        void OnShotFired(FVRFireArm _firearm)
        {
            if (_firearm == firearm)
            {
                _isFiring = true;
                if (fadesOut)
                {
                    fireLoop.volume = _origAudioVolume;
                    timeSinceFadeOutStart = 0f;
                }
            }
        }

        void TryPlayFiringAudio()
        {
            if (!_isFiringSoundPlaying)
            {
                if (hasFiringStart)
                {
                    SM.PlayCoreSound(FVRPooledAudioType.GenericClose, firingStart, firearm.GetMuzzle().position);
                }
                fireLoop.Play();
                _isFiringSoundPlaying = true;
            }
        }

        void StopFiringAudio()
        {
            if (!fadesOut)
            {
                fireLoop.Stop();
                if (hasFiringStop)
                {
                    SM.PlayCoreSound(FVRPooledAudioType.GenericClose, firingStop, firearm.GetMuzzle().position);
                }
            }
            _isFiringSoundPlaying = false;
            _isFiring = false;
            hasFiredSingleShot = false;
        }

        IEnumerator FiringSoundFadeOut()
        {
            while (true)
            {
                if (!_isFiring)
                {
                    if (fireLoop.isPlaying)
                    {
                        fireLoop.volume = _origAudioVolume * fadeOutCurve.Evaluate(timeSinceFadeOutStart);
                        timeSinceFadeOutStart += Time.deltaTime;

                        if (timeSinceFadeOutStart > _fadeOutDuration)
                        {
                            fireLoop.volume = 0f;
                            fireLoop.Stop();
                        }
                    }
                }
                yield return null;
            }
        }

        private void ClosedBoltWeapon_UpdateInputAndAnimate(On.FistVR.ClosedBoltWeapon.orig_UpdateInputAndAnimate orig, ClosedBoltWeapon self, FVRViveHand hand)
        {
            orig(self, hand);
            if (self == firearm)
            {
                if (_isFiring)
                {
                    ClosedBoltWeapon.FireSelectorModeType modeType = self.FireSelector_Modes[self.m_fireSelectorMode].ModeType;

                    if (self.m_triggerFloat < self.TriggerFiringThreshold ||                                                            //trigger is up
                        self.Magazine == null ||                                                                                        //no magazine
                        (self.Magazine != null && self.Magazine.m_numRounds < 1 && !self.Chamber.IsFull && !self.m_proxy.IsFull) ||     //magazine, chamber and proxy are empty
                        (modeType == ClosedBoltWeapon.FireSelectorModeType.Burst && self.m_CamBurst < 1))                               //firing burst is finished
                    {
                        StopFiringAudio();
                    }
                    else
                    {
                        if (modeType == ClosedBoltWeapon.FireSelectorModeType.Single)
                        {
                            if (!hasFiredSingleShot)
                            {
                                if (hasSingleShot)
                                {
                                    hasFiredSingleShot = true;
                                    SM.PlayCoreSound(FVRPooledAudioType.GenericClose, singleShot, firearm.GetMuzzle().position);
                                }
                                else
                                {
                                    TryPlayFiringAudio();
                                    StopFiringAudio();
                                }
                            }
                        }
                        else if (modeType != ClosedBoltWeapon.FireSelectorModeType.Safe)
                        {
                            TryPlayFiringAudio();
                        }
                    }
                }
            }
        }

        private void OpenBoltReceiver_UpdateInteraction(On.FistVR.OpenBoltReceiver.orig_UpdateInteraction orig, OpenBoltReceiver self, FVRViveHand hand)
        {
            orig(self, hand);
            if (self == firearm)
            {
                if (_isFiring)
                {
                    OpenBoltReceiver.FireSelectorModeType modeType = self.FireSelector_Modes[self.m_fireSelectorMode].ModeType;

                    if (self.m_triggerFloat < self.TriggerFiringThreshold ||                                                            //trigger is up
                        self.Magazine == null ||                                                                                        //no magazine
                        (self.Magazine != null && self.Magazine.m_numRounds < 1 && !self.Chamber.IsFull && !self.m_proxy.IsFull) ||     //magazine, chamber and proxy are empty
                        (modeType == OpenBoltReceiver.FireSelectorModeType.SuperFastBurst && self.m_CamBurst < 1))                      //firing burst is finished
                    {
                        StopFiringAudio();
                    }
                    else
                    {
                        if (modeType == OpenBoltReceiver.FireSelectorModeType.Single)
                        {
                            if (!hasFiredSingleShot)
                            {
                                if (hasSingleShot)
                                {
                                    hasFiredSingleShot = true;
                                    SM.PlayCoreSound(FVRPooledAudioType.GenericClose, singleShot, firearm.GetMuzzle().position);
                                }
                                else
                                {
                                    TryPlayFiringAudio();
                                    StopFiringAudio();
                                }
                            }
                        }
                        else if (modeType != OpenBoltReceiver.FireSelectorModeType.Safe)
                        {
                            //Bolt does not reciprocate, so just play one firing sound
                            if (self.Bolt.IsHeld)
                            {
                                if (hasSingleShot)
                                {
                                    SM.PlayCoreSound(FVRPooledAudioType.GenericClose, singleShot, firearm.GetMuzzle().position);
                                }
                                else
                                {
                                    TryPlayFiringAudio();
                                    StopFiringAudio();
                                }
                            }
                            TryPlayFiringAudio();
                        }
                    }
                }
            }
        }

        /*private void Handgun_UpdateInputAndAnimate(On.FistVR.Handgun.orig_UpdateInputAndAnimate orig, Handgun self, FVRViveHand hand)
        {
            orig(self, hand);
            if (self == firearm)
            {

            }
        }*/

        void OnDestroy()
        {
            GM.CurrentSceneSettings.ShotFiredEvent -= OnShotFired;

            On.FistVR.ClosedBoltWeapon.UpdateInputAndAnimate -= ClosedBoltWeapon_UpdateInputAndAnimate;

            On.FistVR.OpenBoltReceiver.UpdateInteraction -= OpenBoltReceiver_UpdateInteraction;

            //On.FistVR.Handgun.UpdateInputAndAnimate -= Handgun_UpdateInputAndAnimate;

            StopCoroutine(_FadeOutCoroutine);
        }
#endif
    }
}