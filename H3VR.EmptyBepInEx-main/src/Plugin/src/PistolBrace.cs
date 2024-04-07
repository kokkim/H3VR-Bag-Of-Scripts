using FistVR;
using System.Collections.Generic;
using UnityEngine;


namespace BagOfScripts
{
    public class PistolBrace : AttachableStock
    {
        public static FVRFireArm? bracedFirearm = null;
        [HideInInspector] public bool isInBraceMode;

        public AudioEvent? audClipBraceMode, audClipStockMode;

        public ToggleAnimation braceToggleAnimation;

#if !DEBUG
        //Patched method; detects when Recoil() is run, changes the bool if requirements are met and runs the original method with the changed bool
        private bool FVRFireArm_IsTwoHandStabilized(On.FistVR.FVRFireArm.orig_IsTwoHandStabilized orig, FVRFireArm self)
        {
            if (bracedFirearm == self && isInBraceMode)
            {
                return true;
            }
            return orig(self);
        }

        ///These hooks listen for the original method being called, then override them
        ///To call the original method, use orig(self)
        void Hook()
        {
            On.FistVR.FVRFireArm.IsTwoHandStabilized += FVRFireArm_IsTwoHandStabilized;
        }

        void Unhook()
        {
            On.FistVR.FVRFireArm.IsTwoHandStabilized -= FVRFireArm_IsTwoHandStabilized;
        }

        public override void OnAttach()
        {
            base.OnAttach();
            if (Attachment.curMount.GetRootMount().Parent is FVRFireArm firearmToAttachTo)    //If the object that the brace is being attached to is a firearm, assign it to firearmToAttachTo
            {
                bracedFirearm = firearmToAttachTo;
                if (isInBraceMode)  //If in brace mode, disable shoulder stock and stocked recoil profile
                {
                    firearmToAttachTo.HasActiveShoulderStock = false;
                }
                firearmToAttachTo.UsesStockedRecoilProfile = false;

                /*Debug.Log("OnAttach(): isInBraceMode == " + isInBraceMode +
                          ", UsesStockedRecoilProfile == " + bracedFirearm.UsesStockedRecoilProfile +
                          ", HasActiveShoulderStock == " + bracedFirearm.HasActiveShoulderStock);*/
            }
            Hook();
        }

        public override void OnDetach()
        {
            base.OnDetach();
            if (Attachment.curMount.GetRootMount().Parent is FVRFireArm firearmToDetachFrom && firearmToDetachFrom == bracedFirearm)
            {
                if (firearmToDetachFrom.RecoilProfileStocked != null)   //if has stocked recoil profile, re-enable it
                {
                    firearmToDetachFrom.UsesStockedRecoilProfile = true;
                }

                /*Debug.Log("OnDetach(): isInBraceMode == " + isInBraceMode +
                          ", UsesStockedRecoilProfile == " + bracedFirearm.UsesStockedRecoilProfile +
                          ", HasActiveShoulderStock == " + bracedFirearm.HasActiveShoulderStock);*/
                bracedFirearm = null;
            }
            Unhook();
        }

        public override void OnDestroy()
        {
            Unhook();
        }

        public void ToggleBrace()
        {
            isInBraceMode = !isInBraceMode;
            if (bracedFirearm != null)
            {
                if (isInBraceMode)  //disable stock, play sound, show brace model, hide stock model
                {
                    bracedFirearm.HasActiveShoulderStock = false;
                    SM.PlayCoreSound(FVRPooledAudioType.Generic, audClipBraceMode, transform.position);
                    braceToggleAnimation.Toggle(isInBraceMode);
                }
                else                //enable stock, play sound, hide brace model, show stock model
                {
                    bracedFirearm.HasActiveShoulderStock = true;
                    SM.PlayCoreSound(FVRPooledAudioType.Generic, audClipStockMode, transform.position);
                    braceToggleAnimation.Toggle(isInBraceMode);
                }

                /*Debug.Log("ToggleBrace(): isInBraceMode == " + isInBraceMode +
                          ", UsesStockedRecoilProfile == " + bracedFirearm.UsesStockedRecoilProfile +
                          ", HasActiveShoulderStock == " + bracedFirearm.HasActiveShoulderStock);*/
            }
        }
#endif
    }
}