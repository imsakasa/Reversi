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

	public ColorCountInfo GetColorCountInfo() => m_Board.GetColorCountInfo();

	public void UpdateBoardUIInfo(PieceColorType nextColorType)
	{
		m_BoardUI.SetCurrentTurnText(nextColorType);

		var colorCountInfo = GetColorCountInfo();
		m_BoardUI.SetBlackCountText(colorCountInfo.Black);
		m_BoardUI.SetWhiteCountText(colorCountInfo.White);
		if (colorCountInfo.None == 0)
		{
			Finish();
		}
	}

	private PieceColorType CalcMoreCountColor()
	{
		var colorCountInfo = GetColorCountInfo();
		if (colorCountInfo.Black == colorCountInfo.White)
		{
			return PieceColorType.None;
		}

		return (colorCountInfo.Black > colorCountInfo.White) ? PieceColorType.Black : PieceColorType.White;
	}
}

public struct ColorCountInfo
{
	public int None;
	public int Black;
	public int White;
}
