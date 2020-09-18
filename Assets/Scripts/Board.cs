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

	private ColorType m_CurrentColorType = ColorType.White;

	void Start()
	{
		for (int y = 0; y < Address.MAX_WIDTH; y++)
		{
			GameObject rowObj = GameObject.Find($"Row_{y + 1}");
			for (int x = 0; x < Address.MAX_WIDTH; x++)
			{
				GameObject targetParts = rowObj.FindChild($"Column_{x + 1}");
				m_BoardSquares[x, y] = targetParts.GetComponent<BoardSquare>();
				m_BoardSquares[x, y].Setup(new Address(x, y), PutPiece);
			}
		}

		PutPiece(new Address(3, 3));
		PutPiece(new Address(4, 3));
		PutPiece(new Address(4, 4));
		PutPiece(new Address(3, 4));
	}

	public void PutPiece(Address pos)
	{
		// そこには置けない
		if (!m_BoardAnalyzer.CanPutPiece(m_BoardSquares, pos))
		{
			return;
		}

		var pieceObj = Instantiate(m_PieceObj, new Vector3(pos.X, 1f, -pos.Y), Quaternion.identity);
		GetSquare(pos).PutPiece(pieceObj, m_CurrentColorType);

		ChangeTurn();
	}

	private void ChangeTurn()
	{
		m_CurrentColorType = (m_CurrentColorType == ColorType.Black) ?
			ColorType.White : ColorType.Black;
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
