using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBoardParts
{
	void PutPiece(GameObject pieceObj, ColorType colorType);
	void Focus();
	void UnFocus();
}
