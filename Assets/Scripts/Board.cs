using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour, IBoard
{
	static readonly int MAX_WIDTH = 8;

	public BoardParts GetPiece(Address pos) => m_BoardParts[pos.Row, pos.Column];

	readonly BoardParts[,] m_BoardParts = new BoardParts[MAX_WIDTH, MAX_WIDTH];

	[SerializeField] private GameObject m_pieceObj;

	private ColorType m_currentColorType = ColorType.White;

	void Start()
	{
		for (int row = 0; row < MAX_WIDTH; row++)
		{
			GameObject rowObj = GameObject.Find($"Row_{row + 1}");
			for (int column = 0; column < MAX_WIDTH; column++)
			{
				GameObject targetParts = rowObj.FindChild($"Column_{column + 1}");
				m_BoardParts[row, column] = targetParts.GetComponent<BoardParts>();
				m_BoardParts[row, column].Setup(new Address(row, column), PutPiece);
			}
		}

		PutPiece(new Address(3, 3));
		PutPiece(new Address(4, 3));
		PutPiece(new Address(4, 4));
		PutPiece(new Address(3, 4));
	}

	void Update()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			GetPiece(new Address(3, 1)).Focus();
		}
	}

	public void PutPiece(Address pos)
	{
		if (GetPiece(pos).CurrentColor != ColorType.None)
		{
			return;
		}

		var pieceObj = Instantiate(m_pieceObj, new Vector3(pos.Column, 1f, -pos.Row), Quaternion.identity);
		GetPiece(pos).PutPiece(pieceObj, m_currentColorType);

		ChangeTurn();
	}

	private void ChangeTurn()
	{
		m_currentColorType = (m_currentColorType == ColorType.Black) ?
			ColorType.White : ColorType.Black;
	}
}

public struct Address
{
	public int Row;
	public int Column;

	public Address(int row, int column)
	{
		Row = row;
		Column = column;
	}
}
