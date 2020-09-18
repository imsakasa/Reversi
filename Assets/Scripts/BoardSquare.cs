using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSquare : MonoBehaviour, IBoardSquare
{
	[SerializeField] private GameObject m_FocusEffectObj;

	private ColorType m_CurrentColor = ColorType.None;
	public ColorType CurrentColor => m_CurrentColor;

	private GameObject m_PieceObj { get; set; }
	private Address m_Address;

	private Action<Address> m_onPressed;

	public void Setup(Address address, Action<Address> onPredded)
	{
		m_Address = address;
		m_onPressed = onPredded;
	}

	public void PutPiece(GameObject pieceObj, ColorType colorType)
	{
		m_PieceObj = pieceObj;
		UpdateColorType(colorType);
	}

	public void Reverse()
	{
		UpdateColorType(m_CurrentColor == ColorType.White ? ColorType.Black : ColorType.White);
	}

	private void UpdateColorType(ColorType colorType)
	{
		m_CurrentColor = colorType;
		m_PieceObj.transform.localRotation =
			Quaternion.Euler(
				m_PieceObj.transform.localRotation.x,
				m_PieceObj.transform.localRotation.y,
				colorType == ColorType.Black ? 0f : 180f
			);
	}

	public void OnPressed()
	{
		m_onPressed.Invoke(m_Address);
	}

	public void Focus() => m_FocusEffectObj.SetActive(true);
	public void UnFocus() => m_FocusEffectObj.SetActive(false);
	public bool IsFocus => m_FocusEffectObj.activeSelf;

	public bool IsEmpty() => m_CurrentColor == ColorType.None;
}

public enum ColorType
{
	None = 0,
	Black = 1,
	White = 2,
}
