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

	public void SetCurrentTurnText(PieceColorType colorType)
	{
		m_CurrentTurnText.text = colorType.ToString();
	}
}
