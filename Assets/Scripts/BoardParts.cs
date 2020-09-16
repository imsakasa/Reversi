using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardParts : MonoBehaviour, IBoardParts
{
	[SerializeField] private GameObject m_focusEffectObj;

	private ColorType m_currentColor = ColorType.None;
	public ColorType CurrentColor => m_currentColor;

	public GameObject PieceObj { get; set; }
	private Address m_address;

	private Action<Address> m_onPressed;

	public void Setup(Address address, Action<Address> onPredded)
	{
		m_address = address;
		m_onPressed = onPredded;
	}

	public void PutPiece(GameObject pieceObj, ColorType colorType)
	{
		PieceObj = pieceObj;
		UpdateColorType(colorType);
	}

	public void Reverse()
	{
		UpdateColorType(m_currentColor == ColorType.White ? ColorType.Black : ColorType.White);
	}

	private void UpdateColorType(ColorType colorType)
	{
		m_currentColor = colorType;
		PieceObj.transform.localRotation =
			Quaternion.Euler(
				PieceObj.transform.localRotation.x,
				PieceObj.transform.localRotation.y,
				colorType == ColorType.Black ? 0f : 180f
			);
	}

	public void OnPressed()
	{
		m_onPressed.Invoke(m_address);
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
