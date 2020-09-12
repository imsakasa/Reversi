using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPiece
{
	public ColorType currentColor = ColorType.None;
	public GameObject PieceObj;

	public void Setup(GameObject obj, ColorType colorType)
	{
		PieceObj = obj;
		UpdateColorType(colorType);
	}

	public void Reverse()
	{
		UpdateColorType(currentColor == ColorType.White ? ColorType.Black : ColorType.White);
	}

	private void UpdateColorType(ColorType colorType)
	{
		currentColor = colorType;
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
