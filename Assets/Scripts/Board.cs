using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour, IBoard
{
	static readonly int MAX_WIDTH = 8;

	BoardPiece[,] m_BoardPieces = new BoardPiece[MAX_WIDTH, MAX_WIDTH];

	[SerializeField] private GameObject m_pieceObj;

	public Board()
	{
		for (int row = 0; row < MAX_WIDTH; row++)
		{
			for (int column = 0; column < MAX_WIDTH; column++)
			{
				m_BoardPieces[row, column] = new BoardPiece();
			}
		}
	}

	void Start()
	{
		PutPiece(new Address(3, 3), ColorType.White);
		PutPiece(new Address(4, 3), ColorType.Black);
		PutPiece(new Address(3, 4), ColorType.Black);
		PutPiece(new Address(4, 4), ColorType.White);
	}

	void Update()
	{
		if (Input.GetKey(KeyCode.Space))
		{
			PutPiece(new Address(3, 1), ColorType.White);
		}
	}

	public void PutPiece(Address pos, ColorType colorType)
	{
		if (m_BoardPieces[pos.Row, pos.Column].ColorType != ColorType.None)
		{
			return;
		}

		var pieceObj = Instantiate(m_pieceObj, new Vector3(pos.Row, 1f, -pos.Column), Quaternion.identity);
		m_BoardPieces[pos.Row, pos.Column].Setup(pieceObj, colorType); 
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
