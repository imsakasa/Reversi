using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoardSquare : MonoBehaviour, IBoardSquare
{
	[SerializeField] private GameObject m_FocusEffectObj;

	private PieceColorType m_CurrentColor = PieceColorType.None;
	public PieceColorType CurrentColor => m_CurrentColor;

	private GameObject m_PieceObj { get; set; }
	public Address Address { get; private set; }

	private Action<Address> m_onPressed;

	public void Setup(Address address, Action<Address> onPredded)
	{
		Address = address;
		m_onPressed = onPredded;
	}

	public void PutPiece(GameObject pieceObj, PieceColorType pieceColorType)
	{
		m_PieceObj = pieceObj;
		m_CurrentColor = pieceColorType;
		m_PieceObj.transform.localRotation = GetPieceQuaternion(pieceColorType);
	}

	public void Reverse()
	{
		PieceColorType reverseColor = GetReverseColor(m_CurrentColor);
		m_CurrentColor = reverseColor;
		ReverseAnimation(reverseColor);
	}

	private void ReverseAnimation(PieceColorType colorType)
	{
		Sequence seq = DOTween.Sequence();
		seq.Append(m_PieceObj.transform.DOLocalMoveY(1.5f, 0.5f).SetRelative());
		seq.Join(m_PieceObj.transform.DOLocalRotateQuaternion(
			GetPieceQuaternion(colorType), 0.5f)
		);
	}

	private Quaternion GetPieceQuaternion(PieceColorType colorType)
	{
		return Quaternion.Euler(
				m_PieceObj.transform.localRotation.x,
				m_PieceObj.transform.localRotation.y,
				colorType == PieceColorType.Black ? 0f : 180f);
	}

	public static PieceColorType GetReverseColor(PieceColorType colorType) => (colorType == PieceColorType.White) ? PieceColorType.Black : PieceColorType.White;

	public void OnPressed()
	{
		m_onPressed.Invoke(Address);
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
