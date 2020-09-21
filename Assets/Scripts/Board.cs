using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour, IBoard
{
	/// <summary>
	/// 左上が(0,0)、右下が(7, 7)の 8 × 8 マスの盤
	/// </summary>
	readonly BoardSquare[,] m_BoardSquares = new BoardSquare[Address.MAX_WIDTH, Address.MAX_WIDTH];
	public BoardSquare GetSquare(Address pos) => m_BoardSquares[pos.X, pos.Y];

	private BoardAnalyzer m_BoardAnalyzer = new BoardAnalyzer();

	[SerializeField] private GameObject m_PieceObj;

	private PieceColorType m_CurrentTurnColor;

	void Start()
	{
		for (int y = 0; y < Address.MAX_WIDTH; y++)
		{
			GameObject rowObj = GameObject.Find($"Row_{y + 1}");
			for (int x = 0; x < Address.MAX_WIDTH; x++)
			{
				GameObject targetParts = rowObj.FindChild($"Column_{x + 1}");
				m_BoardSquares[x, y] = targetParts.GetComponent<BoardSquare>();
				m_BoardSquares[x, y].Setup(new Address(x, y), TryPutPiece);
			}
		}

		CreateAndPutPiece(new Address(4, 3), PieceColorType.Black);
		CreateAndPutPiece(new Address(3, 3), PieceColorType.White);
		CreateAndPutPiece(new Address(3, 4), PieceColorType.Black);
		CreateAndPutPiece(new Address(4, 4), PieceColorType.White);

		SetCurrentTurn(PieceColorType.White);
	}

	private void SetCurrentTurn(PieceColorType colorType)
	{
		m_CurrentTurnColor = colorType;
		BoardManager.I.SetCurrentTurnText(colorType);
	}

	public void TryPutPiece(Address pos)
	{
		if (!m_BoardAnalyzer.CanPutPiece(m_BoardSquares, m_CurrentTurnColor, pos))
		{
			return;
		}

		CreateAndPutPiece(pos, m_CurrentTurnColor);
		ChangeTurn();
	}

	private void CreateAndPutPiece(Address pos, PieceColorType pieceColorType)
	{
		var pieceObj = Instantiate(m_PieceObj, new Vector3(pos.X, 1f, -pos.Y), Quaternion.identity);
		GetSquare(pos).PutPiece(pieceObj, pieceColorType);
	}

	private void ChangeTurn()
	{
		PieceColorType changeColorType = (m_CurrentTurnColor == PieceColorType.Black) ?
			PieceColorType.White : PieceColorType.Black;

		SetCurrentTurn(changeColorType);
	}
}

public struct Address
{
	public static readonly int MAX_WIDTH = 8;

	public int X;
	public int Y;

	public Address(int x, int y)
	{
		X = x;
		Y = y;
	}

	public bool IsValid()
	{
		if (X >= MAX_WIDTH || 0 > X)
		{
			return false;
		}

		if (Y >= MAX_WIDTH || 0 > Y)
		{
			return false;
		}

		return true;
	}
}
