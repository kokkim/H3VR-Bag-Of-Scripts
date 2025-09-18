using System;
using System.Collections.Generic;
using UnityEngine;
using FistVR;
using System.Collections;

namespace BagOfScripts
{
    class MagazineLoadedPhys : MonoBehaviour
    {
        public static readonly Dictionary<FVRFireArmMagazine, MagazineLoadedPhys> _existingMagLoadedPhys = new();

        [SerializeField] FVRFireArmMagazine magazine;
        [SerializeField] GameObject[] enableOnLoad, disableOnLoad;
        [Space(10)]
        [SerializeField] bool hasSecondarySlotObjects;
        [SerializeField] GameObject[] enableOnLoad_Secondary, disableOnLoad_Secondary;

#if !DEBUG
        public void Awake()
        {
            if (magazine == null)
            {
                if (transform.root.GetComponentInChildren<FVRFireArmMagazine>() != null)
                {
                    magazine = transform.root.GetComponentInChildren<FVRFireArmMagazine>();
                }

                if (magazine != null) Hook();
                else Debug.LogError("No magazine assigned or found!");
            }
            else
            {
                Hook();
            }
        }

        void Hook()
        {
            _existingMagLoadedPhys.Add(magazine, this);
            On.FistVR.FVRFireArmMagazine.Load_FVRFireArm += FVRFireArmMagazine_Load_FVRFireArm;
            On.FistVR.FVRFireArmMagazine.Release += FVRFireArmMagazine_Release;

            On.FistVR.FVRFireArmMagazine.Load_AttachableFirearm += FVRFireArmMagazine_Load_AttachableFirearm;
            On.FistVR.FVRFireArmMagazine.ReleaseFromAttachableFireArm += FVRFireArmMagazine_ReleaseFromAttachableFireArm;

            if (hasSecondarySlotObjects)    //Won't hook these if not necessary
            {
                On.FistVR.FVRFireArmMagazine.LoadIntoSecondary += FVRFireArmMagazine_LoadIntoSecondary;
                On.FistVR.FVRFireArmMagazine.ReleaseFromSecondarySlot += FVRFireArmMagazine_ReleaseFromSecondarySlot;
            }
        }


        //Firearm
        private void FVRFireArmMagazine_Load_FVRFireArm(On.FistVR.FVRFireArmMagazine.orig_Load_FVRFireArm orig, FVRFireArmMagazine self, FVRFireArm fireArm)
        {
            orig(self, fireArm);
            if (_existingMagLoadedPhys.TryGetValue(self, out MagazineLoadedPhys _))
            {
                OnLoad();
            }
        }
        private void FVRFireArmMagazine_Release(On.FistVR.FVRFireArmMagazine.orig_Release orig, FVRFireArmMagazine self, bool PhysicalRelease)
        {
            orig(self, PhysicalRelease);
            if (_existingMagLoadedPhys.TryGetValue(self, out MagazineLoadedPhys _))
            {
                OnRelease();
            }
        }

        //Attachable firearm
        private void FVRFireArmMagazine_Load_AttachableFirearm(On.FistVR.FVRFireArmMagazine.orig_Load_AttachableFirearm orig, FVRFireArmMagazine self, AttachableFirearm fireArm)
        {
            orig(self, fireArm);
            if (_existingMagLoadedPhys.TryGetValue(self, out MagazineLoadedPhys _))
            {
                OnLoad();
            }
        }
        private void FVRFireArmMagazine_ReleaseFromAttachableFireArm(On.FistVR.FVRFireArmMagazine.orig_ReleaseFromAttachableFireArm orig, FVRFireArmMagazine self, bool PhysicalRelease)
        {
            orig(self, PhysicalRelease);
            if (_existingMagLoadedPhys.TryGetValue(self, out MagazineLoadedPhys _))
            {
                OnRelease();
            }
        }

        //Secondary magazine slot
        private void FVRFireArmMagazine_LoadIntoSecondary(On.FistVR.FVRFireArmMagazine.orig_LoadIntoSecondary orig, FVRFireArmMagazine self, FVRFireArm fireArm, int slot)
        {
            orig(self, fireArm, slot);
            if (_existingMagLoadedPhys.TryGetValue(self, out MagazineLoadedPhys _))
            {
                OnLoadSecondary();
            }
        }
        private void FVRFireArmMagazine_ReleaseFromSecondarySlot(On.FistVR.FVRFireArmMagazine.orig_ReleaseFromSecondarySlot orig, FVRFireArmMagazine self, int slot, bool PhysicalRelease)
        {
            orig(self, slot, PhysicalRelease);
            if (_existingMagLoadedPhys.TryGetValue(self, out MagazineLoadedPhys _))
            {
                OnReleaseSecondary();
            }
        }

        void OnLoad()
        {
            foreach (GameObject GO in enableOnLoad)
            {
                GO.SetActive(true);
            }

            foreach (GameObject GO in disableOnLoad)
            {
                GO.SetActive(false);
            }
        }
        void OnRelease()
        {
            foreach (GameObject GO in enableOnLoad)
            {
                GO.SetActive(false);
            }

            foreach (GameObject GO in disableOnLoad)
            {
                GO.SetActive(true);
            }
        }

        void OnLoadSecondary()
        {
            foreach (GameObject GO in enableOnLoad_Secondary)
            {
                GO.SetActive(true);
            }

            foreach (GameObject GO in disableOnLoad_Secondary)
            {
                GO.SetActive(false);
            }
        }

        void OnReleaseSecondary()
        {
            foreach (GameObject GO in enableOnLoad_Secondary)
            {
                GO.SetActive(false);
            }

            foreach (GameObject GO in disableOnLoad_Secondary)
            {
                GO.SetActive(true);
            }
        }

        void OnDestroy()
        {
            _existingMagLoadedPhys.Remove(magazine);
        }
#endif
    }
}
