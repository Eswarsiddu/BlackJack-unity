using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
	#region Value
	private int _value;
    private bool face_down;
    public int value { get => _value; }

	internal bool isFaceDown()
	{
		return face_down;
	}

	public void faceUp()
    {
        face_down = false;
        updateCardImage();
    }

	public void faceDown()
	{
		face_down = true;
		updateCardImage();
	}

	#endregion

	#region Graphics

	private SpriteRenderer renderer;
    private Sprite sprite;

	public void GenerateCard(Sprite sprite, int number)
	{
		renderer = GetComponent<SpriteRenderer>();
		transform.parent = Constants.offsiteCardsParent;
		transform.position = Vector3.zero;
		this.sprite = sprite;
        if(number >= 10)
		{
            _value = 10;
		}
		else
		{
            _value = number;
		}
		faceDown();
	}

	public int orderinlayer { set { renderer.sortingOrder = value; } }

	private void updateCardImage()
	{
		if(renderer == null)
		{
			Debug.Log("renderer is null");
		}
		if(sprite == null)
		{
			Debug.Log("sprite is null");
		}
		if (Constants.CARDBACKSPRITE)
		{
			Debug.Log("card back sprite is null"); ;
		}
		renderer.sprite = face_down ? Constants.CARDBACKSPRITE : sprite;
	}

	public Transform parent { set { transform.parent = value; } }

	private IEnumerator MoveCardOffScreen()
	{
		yield return null;
	}

	public void ResetCard()
	{
		faceDown();
		parent = Constants.offsiteCardsParent;
		transform.position = Vector3.zero;
		gameObject.SetActive(false);
		orderinlayer = 0;
	}

	#endregion
}
