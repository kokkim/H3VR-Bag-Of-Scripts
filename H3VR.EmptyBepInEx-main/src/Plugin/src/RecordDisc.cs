///JukeBox.cs by Arpy

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FistVR;

namespace BagOfScripts
{
	public class RecordDisc : FVRPhysicalObject
	{
		public AudioClip RecordSong;
		[HideInInspector] public JukeBox CurBox;

        public override void BeginInteraction(FVRViveHand hand)
        {
            base.BeginInteraction(hand);
            if (CurBox != null)
            {
                CurBox.DiscExit();
                transform.parent = null;
                CurBox = null;
            }
        }
    }
}
