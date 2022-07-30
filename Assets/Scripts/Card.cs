using System.Collections;
using UnityEngine;

public class Card : MonoBehaviour
{
	private int _value;
	private bool face_down;

	private SpriteRenderer sprite_renderer;
	private Sprite card_sprite;

	public int value { get => face_down ? 0 : _value; }

	public int orderinlayer { set { sprite_renderer.sortingOrder = value; } }
	public Transform parent { set { transform.parent = value; } }

	public void TurnFaceUp()
	{
		face_down = false;
		updateCardImage();
	}

	public void TurnFaceDown()
	{
		face_down = true;
		updateCardImage();
	}

	public void GenerateCard(Sprite sprite, int number)
	{
		sprite_renderer = GetComponent<SpriteRenderer>();
		transform.parent = Constants.offsiteCardsParent;
		transform.position = Vector3.zero;
		this.card_sprite = sprite;
		if (number >= 10)
		{
			_value = 10;
		}
		else
		{
			_value = number;
		}
		TurnFaceDown();
	}

	private void updateCardImage()
	{
		sprite_renderer.sprite = face_down ? Constants.CARDBACKSPRITE : card_sprite;
	}

	private IEnumerator MoveCardOffScreen()
	{
		yield return null;
	}

	public void ResetCard()
	{
		TurnFaceDown();
		parent = Constants.offsiteCardsParent;
		transform.position = Vector3.zero;
		orderinlayer = 0;
		gameObject.SetActive(false);
	}

	#region Test

	public override string ToString()
	{
		return value.ToString();
	}

	#endregion
}
