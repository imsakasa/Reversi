using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class BoardSquare : MonoBehaviour, IBoardSquare
{
	private static readonly float PUT_ANIM_MOVE_Y_AMOUNT = 1f;
	private static readonly float PUT_ANIM_TIME = 0.3f;
	private static readonly float BLACK_ROTATE = 0f;
	private static readonly float WHITE_ROTATE = 180f;

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

	public void Reset()
	{
		m_CurrentColor = PieceColorType.None;
		if (m_PieceObj != null)
		{
			Destroy(m_PieceObj);
		}
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
		seq.Append(m_PieceObj.transform.DOLocalMoveY(PUT_ANIM_MOVE_Y_AMOUNT, PUT_ANIM_TIME).SetRelative());
		seq.Join(m_PieceObj.transform.DOLocalRotateQuaternion(
			GetPieceQuaternion(colorType), PUT_ANIM_TIME));
		seq.Append(m_PieceObj.transform.DOLocalMoveY(-PUT_ANIM_MOVE_Y_AMOUNT, PUT_ANIM_TIME).SetRelative());
	}

	private Quaternion GetPieceQuaternion(PieceColorType colorType)
	{
		return Quaternion.Euler(
				m_PieceObj.transform.localRotation.x,
				m_PieceObj.transform.localRotation.y,
				colorType == PieceColorType.Black ? BLACK_ROTATE : WHITE_ROTATE);
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
