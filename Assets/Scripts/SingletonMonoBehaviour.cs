using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T instance;
	public static T I
	{
		get
		{
			if (instance == null)
			{
				instance = (T)FindObjectOfType(typeof(T));
				if (instance == null)
				{
					Debug.LogError(typeof(T) + " is nothing");
				}
			}

			return instance;
		}
	}

	protected void Awake()
	{
		CheckInstance();
	}

	protected bool CheckInstance()
	{
		if (this == I)
		{
			return true;
		}

		Destroy(this);
		return true;
	}
}
