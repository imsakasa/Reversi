using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardAnalyzer
{
	private BoardSquare GetSquare(BoardSquare[,] squares, Address pos) => squares[pos.X, pos.Y];

	public bool CanPutPiece(BoardSquare[,] boardSquares, Address pos)
	{
		var square = GetSquare(boardSquares, pos);
		if (!square.IsEmpty())
		{
			return false;
		}

		var list = GetAdjacentSquares(boardSquares, pos);

		return true;
	}

	private List<BoardSquare> GetAdjacentSquares(BoardSquare[,] boardSquares, Address pos)
	{
		var adjacentSquares = new List<BoardSquare>();

		// 隣接するマスを取得
		TryRegisterSquare(GetUpPos(pos), 			boardSquares, adjacentSquares);
		TryRegisterSquare(GetDownPos(pos), 			boardSquares, adjacentSquares);
		TryRegisterSquare(GetRightPos(pos), 		boardSquares, adjacentSquares);
		TryRegisterSquare(GetLeftPos(pos), 			boardSquares, adjacentSquares);
		TryRegisterSquare(GetUpperRightPos(pos), 	boardSquares, adjacentSquares);
		TryRegisterSquare(GetUpperLeftPos(pos), 	boardSquares, adjacentSquares);
		TryRegisterSquare(GetLowerRightPos(pos), 	boardSquares, adjacentSquares);
		TryRegisterSquare(GetLowerLeftPos(pos), 	boardSquares, adjacentSquares);

		return adjacentSquares;
	}

	private void TryRegisterSquare(Address registerPos, BoardSquare[,] boardSquares, List<BoardSquare> squareList)
	{
		if (registerPos.IsValid())
		{
			squareList.Add(GetSquare(boardSquares, registerPos));
		}
	}

	private Address GetUpPos(Address pos)
	{
		if (pos.Y == 0)
		{
			return new Address(-1, -1);
		}

		return new Address(pos.X, pos.Y - 1);
	}

	private Address GetDownPos(Address pos)
	{
		if (pos.Y == Address.MAX_WIDTH - 1)
		{
			return new Address(-1, -1);
		}

		return new Address(pos.X, pos.Y + 1);
	}

	private Address GetRightPos(Address pos)
	{
		if (pos.X == Address.MAX_WIDTH - 1)
		{
			return new Address(-1, -1);
		}

		return new Address(pos.X + 1, pos.Y);
	}

	private Address GetLeftPos(Address pos)
	{
		if (pos.X == 0)
		{
			return new Address(-1, -1);
		}

		return new Address(pos.X - 1, pos.Y);
	}

	private Address GetUpperRightPos(Address pos)
	{
		if (pos.X == Address.MAX_WIDTH - 1 && pos.Y == 0)
		{
			return new Address(-1, -1);
		}

		return new Address(pos.X + 1, pos.Y - 1);
	}

	private Address GetUpperLeftPos(Address pos)
	{
		if (pos.X == 0 && pos.Y == 0)
		{
			return new Address(-1, -1);
		}

		return new Address(pos.X - 1, pos.Y - 1);
	}

	private Address GetLowerRightPos(Address pos)
	{
		if (pos.X == Address.MAX_WIDTH - 1 && pos.Y == Address.MAX_WIDTH - 1)
		{
			return new Address(-1, -1);
		}

		return new Address(pos.X + 1, pos.Y + 1);
	}

	private Address GetLowerLeftPos(Address pos)
	{
		if (pos.X == 0 && pos.Y == Address.MAX_WIDTH - 1)
		{
			return new Address(-1, -1);
		}

		return new Address(pos.X - 1, pos.Y + 1);
	}
}
