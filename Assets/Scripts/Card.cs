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

	private void Start()
	{
        renderer = GetComponent<SpriteRenderer>();
	}

	internal void GenerateCard(Sprite sprite, int number)
	{
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

	private void updateCardImage()
	{
		renderer.sprite = face_down ? Constants.CARDBACKSPRITE : sprite;
	}

	#endregion
}
