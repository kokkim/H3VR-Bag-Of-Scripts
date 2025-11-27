using FistVR;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RetractBipodOnCollapse : MonoBehaviour
{
    public static Dictionary<FVRFireArmBipod, RetractBipodOnCollapse> _existingRetractingBipods = new Dictionary<FVRFireArmBipod, RetractBipodOnCollapse>();
    public FVRFireArmBipod bipod;
    [Tooltip("The Nth element in the list of lengths (and heights)")]
    public int bipodDefaultMLIndex = 0;

#if !DEBUG
    void Awake()
    {
        On.FistVR.FVRFireArmBipod.Contract += FVRFireArmBipod_Contract;

        if (bipod == null) bipod = GetComponent<FVRFireArmBipod>();
        _existingRetractingBipods.Add(bipod, this);
    }

    private void FVRFireArmBipod_Contract(On.FistVR.FVRFireArmBipod.orig_Contract orig, FVRFireArmBipod self, bool playSound)
    {
        RetractBipodOnCollapse retractor;
        if (_existingRetractingBipods.TryGetValue(self, out retractor))
        {
            self.m_mlIndex = retractor.bipodDefaultMLIndex;
            self.UpdateML();
        }
        orig(self, playSound);
    }

    void OnDestroy()
    {
        _existingRetractingBipods.Remove(bipod);
    }
#endif
}