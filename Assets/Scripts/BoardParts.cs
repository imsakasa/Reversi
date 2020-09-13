using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardParts : MonoBehaviour, IBoardParts
{
	[SerializeField] private GameObject m_focusEffectObj;

	public ColorType currentColor = ColorType.None;

	public GameObject PieceObj { get; set; }

	public void PutPiece(GameObject pieceObj, ColorType colorType)
	{
		PieceObj = pieceObj;
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

	public void Focus() => m_focusEffectObj.SetActive(true);
	public void UnFocus() => m_focusEffectObj.SetActive(false);
	public bool IsFocus => m_focusEffectObj.activeSelf;
}

public enum ColorType
{
	None = 0,
	Black = 1,
	White = 2,
}
