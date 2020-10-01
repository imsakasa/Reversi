using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

public class BoardUI : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI m_CurrentTurnText;

	[SerializeField]
	private TextMeshProUGUI m_BlackCountText;
	[SerializeField]
	private TextMeshProUGUI m_WhiteCountText;

	[SerializeField]
	private GameObject m_ResultDialog;
	[SerializeField]
	private TextMeshProUGUI m_ResultColorText;

	void Start()
	{
		HideResultDialog();
	}

	public void SetCurrentTurnText(PieceColorType colorType)
	{
		m_CurrentTurnText.text = colorType.ToString();
		m_CurrentTurnText.color = (colorType == PieceColorType.White) ? Color.white : Color.black;
	}

	public void SetBlackCountText(int count)
	{
		m_BlackCountText.text = count.ToString();
	}

	public void SetWhiteCountText(int count)
	{
		m_WhiteCountText.text = count.ToString();
	}

	public void ShowResultDialog(PieceColorType winColor)
	{
		m_ResultColorText.text = (winColor == PieceColorType.White) ? "WHITE" : "BLACK";
		m_CurrentTurnText.color = (winColor == PieceColorType.White) ? Color.white : Color.black;
		m_ResultDialog.SetActive(true);
	}
	public void HideResultDialog() => m_ResultDialog.SetActive(false);
}
