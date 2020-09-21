using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSquare : MonoBehaviour, IBoardSquare
{
	[SerializeField] private GameObject m_FocusEffectObj;

	private PieceColorType m_CurrentColor = PieceColorType.None;
	public PieceColorType CurrentColor => m_CurrentColor;

	private GameObject m_PieceObj { get; set; }
	private Address m_Address;

	private Action<Address> m_onPressed;

	public void Setup(Address address, Action<Address> onPredded)
	{
		m_Address = address;
		m_onPressed = onPredded;
	}

	public void PutPiece(GameObject pieceObj, PieceColorType pieceColorType)
	{
		m_PieceObj = pieceObj;
		UpdateColorType(pieceColorType);
	}

	public void Reverse()
	{
		UpdateColorType(GetReverseColor(m_CurrentColor));
	}

	private void UpdateColorType(PieceColorType colorType)
	{
		m_CurrentColor = colorType;
		m_PieceObj.transform.localRotation =
			Quaternion.Euler(
				m_PieceObj.transform.localRotation.x,
				m_PieceObj.transform.localRotation.y,
				colorType == PieceColorType.Black ? 0f : 180f
			);
	}

	public static PieceColorType GetReverseColor(PieceColorType colorType) => (colorType == PieceColorType.White) ? PieceColorType.Black : PieceColorType.White;

	public void OnPressed()
	{
		m_onPressed.Invoke(m_Address);
	}

	public void Focus() => m_FocusEffectObj.SetActive(true);
	public void UnFocus() => m_FocusEffectObj.SetActive(false);
	public bool IsFocus => m_FocusEffectObj.activeSelf;

	public bool IsEmpty() => m_CurrentColor == PieceColorType.None;
}

public enum PieceColorType
{
	None = 0,
	Black = 1,
	White = 2,
}
