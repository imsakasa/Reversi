using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BoardManager : MonoBehaviour
{
	private static BoardManager m_Instance;
	public static BoardManager I => m_Instance;

	[SerializeField] private Board m_Board;
	[SerializeField] private BoardUI m_BoardUI;

	void Awake()
	{
		if (m_Instance == null)
		{
			m_Instance = this;
		}
	}

	public void SetCurrentTurnText(PieceColorType colorType)
	{
		m_BoardUI.SetCurrentTurnText(colorType);
	}
}
