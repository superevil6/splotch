using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Enums;
using System;

public static class Constants {





	public static Vector2 FindOffset(GameObject GO){
		float XOffset = GO.GetComponent<RectTransform>().rect.width;
		float YOffset = GO.GetComponent<RectTransform>().rect.height;
		return new Vector2(XOffset, YOffset);
	}

	
}
