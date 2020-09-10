using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class BoardManager
{
	private static BoardManager m_BoardManager = new BoardManager();
	public static BoardManager I => m_BoardManager;

	private Board m_Board;

	private BoardManager()
	{
		m_Board = new Board();
	}

	public void PutPiece()
	{
		Debug.LogError("==PutPiece===");
	}
}
