using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtension
{
	public static GameObject FindChild(this GameObject parent, string name)
	{
		foreach (Transform childTransform in parent.transform)
        {
			if (name == childTransform.gameObject.name)
			{
				return childTransform.gameObject;
			}
		}

		return null;
	}
}
