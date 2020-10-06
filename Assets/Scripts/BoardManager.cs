using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BoardManager : SingletonMonoBehaviour<BoardManager>
{
	[SerializeField] private Board m_Board;
	[SerializeField] private BoardUI m_BoardUI;

	new void Awake()
	{
		base.Awake();

		m_BoardUI.Setup(InitBoard);
	}

	private void InitBoard()
	{
		m_Board.InitBoard();
	}

	private void Finish()
	{
		var winnerColorType = CalcMoreCountColor();
		m_BoardUI.ShowResultDialog(winnerColorType);
	}

	private ColorCountInfo GetColorCountInfo() => m_Board.GetColorCountInfo();

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
