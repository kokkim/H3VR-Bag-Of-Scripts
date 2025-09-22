using System;
using System.Collections.Generic;
using UnityEngine;
using FistVR;

namespace BagOfScripts
{
    class CopyLaserPropertiesToMaterial : MonoBehaviour
    {
        public static readonly Dictionary<LaserLightAttachment, CopyLaserPropertiesToMaterial> _existingLaserCopiers = new();

        [Header("Hover over variables for tooltips")]
        public LaserLightAttachment laserToCopyFrom;
        [Tooltip("Requires a mesh renderer with an assigned material.")]
        public GameObject affectedObject;
        [Tooltip("The order number of the material in the mesh renderer's list (first is 0).")]
        public int indexOfAffectedMaterial;

        Material affectedMaterial;

        enum ColorField
        {
            BaseColor,
            Emissive
        }
        [SerializeField] ColorField affectedTint = ColorField.Emissive;

        public bool copyOnOffState = true, copyColor = true;
        [Header("On Color is only used if copyColor is disabled.")]
        public Color OnColor = Color.red;
        [Tooltip("White on base color is no tint, black on emissive is no emission.")]
        public Color OffColor = Color.black;

        static int _Color, _EmissionColor;

#if !DEBUG
        static CopyLaserPropertiesToMaterial()  //Static hooks are enabled only once in constructor, and never disabled
        {
            On.FistVR.LaserLightAttachment.UpdateParams += LaserLightAttachment_UpdateParams;
        }

        void Awake()
        {
            affectedMaterial = affectedObject.GetComponent<MeshRenderer>().materials[indexOfAffectedMaterial];

            _Color = Shader.PropertyToID("_Color");
            _EmissionColor = Shader.PropertyToID("_EmissionColor");

            if (laserToCopyFrom == null && GetComponent<LaserLightAttachment>() != null)
            {
                laserToCopyFrom = GetComponentInChildren<LaserLightAttachment>(true);
            }
            _existingLaserCopiers.Add(laserToCopyFrom, this);
        }

        void OnDestroy()
        {
            _existingLaserCopiers.Remove(laserToCopyFrom);
        }

        private static void LaserLightAttachment_UpdateParams(On.FistVR.LaserLightAttachment.orig_UpdateParams orig, LaserLightAttachment self)
        {
            orig(self);
            if (_existingLaserCopiers.TryGetValue(self, out CopyLaserPropertiesToMaterial laserCopier))
            {
                LaserLightAttachment laser = laserCopier.laserToCopyFrom;

                //fuck it, why not
                laserCopier.affectedMaterial.SetColor(laserCopier.affectedTint ==
                ColorField.BaseColor
                    ? _Color
                    : _EmissionColor,
                    laserCopier.copyOnOffState
                        ? laser.Settings[laser.SettingsIndex].LaserMode == LaserAttachmentMode.Off
                            ? laserCopier.OffColor
                            : laserCopier.copyColor
                                ? laser.ColorSettings[laser.ColorSettingsIndex].color
                                : laserCopier.OnColor
                        : laserCopier.copyColor
                            ? laser.ColorSettings[laser.ColorSettingsIndex].color
                            : laserCopier.OnColor
                );
            }
        }
#endif
    }
}