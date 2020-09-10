using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPiece
{
	public ColorType ColorType = ColorType.None;
	public GameObject PieceObj;

	public void Setup(GameObject obj, ColorType colorType)
	{
		PieceObj = obj;
		UpdateColorType(colorType);
	}

	public void UpdateColorType(ColorType colorType)
	{
		ColorType = colorType;
		PieceObj.transform.localRotation =
			Quaternion.Euler(
				PieceObj.transform.localRotation.x,
				PieceObj.transform.localRotation.y,
				colorType == ColorType.Black ? 0f : 180f
			);
	}
}

public enum ColorType
{
	None = 0,
	Black = 1,
	White = 2,
}
