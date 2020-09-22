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

	public void SetCurrentTurnText(PieceColorType colorType)
	{
		m_CurrentTurnText.text = colorType.ToString();
	}

	public void SetBlackCountText(int count)
	{
		m_BlackCountText.text = count.ToString();
	}

	public void SetWhiteCountText(int count)
	{
		m_WhiteCountText.text = count.ToString();
	}
}
