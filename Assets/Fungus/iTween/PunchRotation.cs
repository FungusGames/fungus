﻿using UnityEngine;
using System.Collections;

namespace Fungus
{
	[CommandInfo("iTween", 
	             "Punch Rotation", 
	             "Applies a jolt of force to a GameObject's rotation and wobbles it back to its initial rotation.")]
	public class PunchRotation : iTweenCommand 
	{
		public Vector3 amount;
		public Space space = Space.Self;

		public override void DoTween()
		{
			Hashtable tweenParams = new Hashtable();
			tweenParams.Add("amount", amount);
			tweenParams.Add("space", space);
			tweenParams.Add("time", duration);
			tweenParams.Add("easetype", easeType);
			tweenParams.Add("looptype", loopType);
			tweenParams.Add("oncomplete", "OnComplete");
			tweenParams.Add("oncompletetarget", gameObject);
			tweenParams.Add("oncompleteparams", this);
			iTween.PunchRotation(target, tweenParams);
		}
	}

}