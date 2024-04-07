using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using FistVR;

namespace BagOfScripts
{
    class PermanentlyAttachedWeapon : AttachableFirearmPhysicalObject
    {
        public override bool CanDetach()
        {
            return false;
        }
    }
}