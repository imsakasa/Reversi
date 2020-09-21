using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBoardSquare
{
	void PutPiece(GameObject pieceObj, PieceColorType colorType);
	void Focus();
	void UnFocus();
}
