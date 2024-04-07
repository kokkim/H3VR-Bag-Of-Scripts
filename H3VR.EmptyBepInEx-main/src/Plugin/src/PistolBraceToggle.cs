using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using FistVR;

namespace BagOfScripts
{
    class PistolBraceToggle : FVRInteractiveObject
    {
        public PistolBrace pistolBraceInterface;

#if !DEBUG
        public override void Awake()
        {
            base.Awake();
            if (pistolBraceInterface != null) pistolBraceInterface.isInBraceMode = false;
        }
        public override void SimpleInteraction(FVRViveHand hand)
        {
            base.SimpleInteraction(hand);
            if (pistolBraceInterface != null)
            {
                pistolBraceInterface.ToggleBrace();
            }
            else Debug.Log("pistolBraceInterface is missing!");
        }
#endif
    }
}