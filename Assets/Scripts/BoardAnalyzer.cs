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

	public bool CanPutPiece(BoardSquare[,] boardSquares, PieceColorType putPieceColor, Address pos)
	{
		var square = GetSquare(boardSquares, pos);
		if (!square.IsEmpty())
		{
			return false;
		}

		var reverseTargetSquareList = GetReverseTargetSquares(boardSquares, pos, putPieceColor);
		if (reverseTargetSquareList.Count == 0)
		{
			return false;
		}

		return true;
	}

	public bool CanPutPieceSomeWhere(BoardSquare[,] boardSquares, PieceColorType putPieceColor)
	{
		// 盤上にある置こうとしてる色とは反対の色のマスを取得
		var targetColorSquares = GetTargetColorSquares(boardSquares, BoardSquare.GetReverseColor(putPieceColor));
		foreach (var square in targetColorSquares)
		{
			// 隣接するマスを取得(最大8マス)
			var adjacentSquareDic = GetAdjacentSquares(square.Address);
			foreach (var dic in adjacentSquareDic)
			{
				var adjacentSquare = GetSquare(boardSquares, dic.Value);
				if (CanPutPiece(boardSquares, putPieceColor, adjacentSquare.Address))
				{
					return true;
				}
			}
		}

		return false;
	}

	private List<BoardSquare> GetTargetColorSquares(BoardSquare[,] boardSquares, PieceColorType targetColor)
	{
		var targetColorSquares = new List<BoardSquare>();
		for (int x = 0; x < Address.MAX_WIDTH; x++)
		{
			for (int y = 0; y < Address.MAX_WIDTH; y++)
			{
				var square = boardSquares[x, y];
				if (square.CurrentColor == targetColor)
				{
					targetColorSquares.Add(square);
				}
			}
		}

		return targetColorSquares;
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

	private Address GetUpPos(Address pos) => new Address(pos.X, pos.Y - 1);
	private Address GetDownPos(Address pos) => new Address(pos.X, pos.Y + 1);
	private Address GetRightPos(Address pos) => new Address(pos.X + 1, pos.Y);
	private Address GetLeftPos(Address pos) => new Address(pos.X - 1, pos.Y);
	private Address GetUpperRightPos(Address pos) => new Address(pos.X + 1, pos.Y - 1);
	private Address GetUpperLeftPos(Address pos) => new Address(pos.X - 1, pos.Y - 1);
	private Address GetLowerRightPos(Address pos) => new Address(pos.X + 1, pos.Y + 1);
	private Address GetLowerLeftPos(Address pos) => new Address(pos.X - 1, pos.Y + 1);

	private Dictionary<Direction, Address> GetTargetColorSquare(BoardSquare[,] boardSquares, Dictionary<Direction, Address> squareDic, PieceColorType pieceColorType)
	{
		var resultSquareDic = new Dictionary<Direction, Address>();
		if (squareDic.Count == 0)
		{
			return resultSquareDic;
		}

		return squareDic.Where(square => GetSquare(boardSquares, square.Value).CurrentColor == pieceColorType).ToDictionary(s => s.Key, s => s.Value);
	}

	public List<BoardSquare> GetReverseTargetSquares(BoardSquare[,] boardSquares, Address putPos, PieceColorType currentColor)
	{
		var reverseTargetSquareList = new List<BoardSquare>();

		// 隣接するマスを取得(最大8マス)
		var adjacentSquareDic = GetAdjacentSquares( putPos);
		// 隣接するマスから現在の色と反対色のマスを取得
		var reverseColorSquareDic = GetTargetColorSquare(boardSquares, adjacentSquareDic, BoardSquare.GetReverseColor(currentColor));
		if (reverseColorSquareDic.Count == 0)
		{
			return reverseTargetSquareList;
		}

		foreach (var item in reverseColorSquareDic)
		{
			Address currentSearchPos = item.Value;
			var reservationSquareList = new List<BoardSquare>();
			reservationSquareList.Add(GetSquare(boardSquares, currentSearchPos));
			while (true)
			{
				Address searchPos = GetDirectionPos(item.Key, currentSearchPos);
				if (!searchPos.IsValid())
				{
					break;
				}

				var square = GetSquare(boardSquares, searchPos);
				if (square.CurrentColor == currentColor)
				{
					reverseTargetSquareList.AddRange(reservationSquareList);
					break;
				}
				else if (square.CurrentColor == BoardSquare.GetReverseColor(currentColor))
				{
					reservationSquareList.Add(square);
				}
				else if(square.IsEmpty())
				{
					break;
				}
				currentSearchPos = searchPos;
			}
		}

		return reverseTargetSquareList;
	}

	private Dictionary<Direction, Address> GetAdjacentSquares(Address pos)
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
}
