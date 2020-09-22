using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
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

		FinishTurn(PieceColorType.White);
	}

	private void FinishTurn(PieceColorType nextColorType)
	{
		m_CurrentTurnColor = nextColorType;
		BoardManager.I.UpdateBoardUIInfo(nextColorType);
	}

	private void TryPutPiece(Address putPos)
	{
		if (!m_BoardAnalyzer.CanPutPiece(m_BoardSquares, m_CurrentTurnColor, putPos))
		{
			return;
		}

		CreateAndPutPiece(putPos, m_CurrentTurnColor);
		ReverseSandwichedPieces(putPos);

		ChangeTurn();
	}

	private void CreateAndPutPiece(Address pos, PieceColorType pieceColorType)
	{
		var pieceObj = Instantiate(m_PieceObj, new Vector3(pos.X, 1f, -pos.Y), Quaternion.identity);
		GetSquare(pos).PutPiece(pieceObj, pieceColorType);
	}

	private void ReverseSandwichedPieces(Address putPos)
	{
		var reverseTargetSquares = m_BoardAnalyzer.GetReverseTargetSquares(m_BoardSquares, putPos, m_CurrentTurnColor);
		for (int i = 0; i < reverseTargetSquares.Count; i++)
		{
			reverseTargetSquares[i].Reverse();
		}
	}

	private void ChangeTurn()
	{
		PieceColorType changeColorType = (m_CurrentTurnColor == PieceColorType.Black) ?
			PieceColorType.White : PieceColorType.Black;

		FinishTurn(changeColorType);
	}

	public int GetTargetColorCount(PieceColorType targetColorType)
	{
		int resultCount = 0;
		for (int y = 0; y < Address.MAX_WIDTH; y++)
		{
			for (int x = 0; x < Address.MAX_WIDTH; x++)
			{
				if (m_BoardSquares[x, y].CurrentColor == targetColorType)
				{
					resultCount++;
				}
			}
		}

		return resultCount;
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
