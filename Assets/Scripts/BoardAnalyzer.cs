using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoardAnalyzer
{
	private enum Direction
	{
		Invalid,
		Up,
		Down,
		Left,
		Right,
		UpperLeft,
		UpperRight,
		LowerLeft,
		LowerRight,
	}

	private BoardSquare GetSquare(BoardSquare[,] squares, Address pos) => squares[pos.X, pos.Y];

	public bool CanPutPiece(BoardSquare[,] boardSquares, PieceColorType currentTurnColor, Address pos)
	{
		var square = GetSquare(boardSquares, pos);
		if (!square.IsEmpty())
		{
			Debug.LogError("===このマスはすでに置かれています==");
			return false;
		}

		var adjacentSquareDic = GetAdjacentSquares(boardSquares, pos);
		var targetColorSquareDic = GetTargetColorSquare(boardSquares, adjacentSquareDic, BoardSquare.GetReverseColor(currentTurnColor));
		if (targetColorSquareDic.Count == 0)
		{
			Debug.LogError("===このマスには置けません==");
			return false;
		}

		var enablePutDirection = new List<Direction>();
		foreach (var item in targetColorSquareDic)
		{
			Address currentSearchPos = item.Value;
			while (true)
			{
				Address searchPos = GetDirectionPos(item.Key, currentSearchPos);
				if (!searchPos.IsValid())
				{
					break;
				}
				if (GetSquare(boardSquares, searchPos).CurrentColor == currentTurnColor)
				{
					enablePutDirection.Add(item.Key);
					break;
				}
				currentSearchPos = searchPos;
			}
		}

		if (enablePutDirection.Count == 0)
		{
			Debug.LogError("===このマスには置けません==");
			return false;
		}

		return true;
	}

	// private bool IsExistTargetColorLine(KeyValuePair<Direction, Address> keyValue, PieceColorType targetColor)
	// {

	// }

	private Dictionary<Direction, Address> GetAdjacentSquares(BoardSquare[,] boardSquares, Address pos)
	{
		var adjacentSquareDic = new Dictionary<Direction, Address>();

		// 隣接するマスを取得(最大8マス)
		TryRegisterSquare(Direction.Up,	pos, adjacentSquareDic);
		TryRegisterSquare(Direction.Down, pos, adjacentSquareDic);
		TryRegisterSquare(Direction.Left, pos, adjacentSquareDic);
		TryRegisterSquare(Direction.Right, pos, adjacentSquareDic);
		TryRegisterSquare(Direction.UpperLeft, pos, adjacentSquareDic);
		TryRegisterSquare(Direction.UpperRight, pos, adjacentSquareDic);
		TryRegisterSquare(Direction.LowerLeft, pos, adjacentSquareDic);
		TryRegisterSquare(Direction.LowerRight, pos, adjacentSquareDic);

		return adjacentSquareDic;
	}

	private void TryRegisterSquare(Direction direction, Address pos, Dictionary<Direction, Address> squareDic)
	{
		Address registerPos = GetDirectionPos(direction, pos);
		if (registerPos.IsValid())
		{
			squareDic.Add(direction, registerPos);
		}
	}

	private Address GetDirectionPos(Direction direction, Address pos)
	{
		switch (direction)
		{
			case Direction.Up: return GetUpPos(pos);
			case Direction.Down: return GetDownPos(pos);
			case Direction.Left: return GetLeftPos(pos);
			case Direction.Right: return GetRightPos(pos);
			case Direction.UpperLeft: return GetUpperLeftPos(pos);
			case Direction.UpperRight: return GetUpperRightPos(pos);
			case Direction.LowerLeft: return GetLowerLeftPos(pos);
			case Direction.LowerRight: return GetLowerRightPos(pos);
			default: return new Address(-1, -1);
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

	private Dictionary<Direction, Address> GetTargetColorSquare(BoardSquare[,] boardSquares, Dictionary<Direction, Address> squareDic, PieceColorType pieceColorType)
	{
		var resultSquareDic = new Dictionary<Direction, Address>();
		if (squareDic.Count == 0)
		{
			return resultSquareDic;
		}

		return squareDic.Where(square => GetSquare(boardSquares, square.Value).CurrentColor == pieceColorType).ToDictionary(s => s.Key, s => s.Value);
	}
}
