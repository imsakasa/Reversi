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

	private void Finish()
	{
		var winnerColorType = CalcMoreCountColor();
		m_BoardUI.ShowResultDialog(winnerColorType);
	}

	public int GetTargetColorCount(PieceColorType targetColorType)
	{
		return m_Board.GetTargetColorCount(targetColorType);
	}

	public void UpdateBoardUIInfo(PieceColorType nextColorType)
	{
		int blackCount = GetTargetColorCount(PieceColorType.Black);
		int whiteCount = GetTargetColorCount(PieceColorType.White);
		m_BoardUI.SetBlackCountText(blackCount);
		m_BoardUI.SetWhiteCountText(whiteCount);

		m_BoardUI.SetCurrentTurnText(nextColorType);

		int emptyCount = GetTargetColorCount(PieceColorType.None);
		if (emptyCount == 0)
		{
			Finish();
		}
	}

	private PieceColorType CalcMoreCountColor()
	{
		int blackCount = GetTargetColorCount(PieceColorType.Black);
		int whiteCount = GetTargetColorCount(PieceColorType.White);

		return (blackCount > whiteCount) ? PieceColorType.Black : PieceColorType.White;
	}
}
