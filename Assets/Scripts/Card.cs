using UnityEngine;

public class Card : MonoBehaviour
{
	private int _value;
	private bool face_down;

	private SpriteRenderer sprite_renderer;
	private Sprite card_sprite;

	public int value { get => face_down ? 0 : _value; }


	public void UpdateDetails(Transform parent, Vector3 position, int orderinlayer)
	{
		transform.parent = parent;
		transform.localPosition = position;
		sprite_renderer.sortingOrder = orderinlayer;
	}

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

	private void updateCardImage()
	{
		sprite_renderer.sprite = face_down ? Constants.CARDBACKSPRITE : card_sprite;
	}

	public void GenerateCard(Sprite sprite, int number)
	{
		sprite_renderer = GetComponent<SpriteRenderer>();
		card_sprite = sprite;
		_value = (number >= 10) ? 10 : number;
		ResetCard();
	}

	public void ResetCard()
	{
		TurnFaceDown();
		UpdateDetails(Constants.offsiteCardsParent, Vector3.zero, 0);
		gameObject.SetActive(false);
	}

}
