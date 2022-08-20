using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class DealerDeck : MonoBehaviour
{
	private const string ADD_CARD_TRIGGER = "AddCard";
	private const int DECK_PARENT_POSITION = 1;
	private const int UIELEMENTS_POSITION = 3;

	protected int total1;
	protected int total2;
	public int final_value { get; protected set; }
	private int prev_order_layer;

	private Vector3 prev_pos;

	protected List<Card> cards;

	private GameObject score_object;

	protected GameObject finalscore;

	private Transform deck_parent;

	private TextMeshPro final_value_text;
	
	private Animator animator;

	private AnimationStateReference animation_completed;
	public void AddCard(Card card,AnimationStateReference animation_completed)
	{
		this.animation_completed = animation_completed;
		this.animation_completed.running = true;
		ChangeCardState(card);
		cards.Add(card);
		animator.SetTrigger(ADD_CARD_TRIGGER);
	}

	protected virtual void ChangeCardState(Card card)
	{
		if (cards.Count == 0)
		{
			card.TurnFaceDown();
			return;
		}

		card.TurnFaceUp();
	}

	public void AddCardGUI()
	{
		prev_pos.x += 0.2f;
		Card card = cards[cards.Count - 1];
		card.gameObject.SetActive(true);
		card.UpdateDetails(deck_parent, prev_pos, prev_order_layer++);

		if (!score_object.activeInHierarchy)
			score_object.SetActive(true);
		CalculateTotal();
		RearrangeDeckPosition();
		UpdateScore();
		animation_completed.value = true;
		animation_completed.running = false;
	}

	protected virtual void CalculateTotal()
	{
		int no_of_A = 0;
		total1 = cards.Sum(card => {
			no_of_A += card.value == 1 ? 1 : 0;
			return card.value;
		});

		total2 = total1 + ((no_of_A > 0) ? 10 : 0);
		CalculateFinalValue();
	}

	private void CalculateFinalValue()
	{
		if (total1 > 21)
		{
			final_value = total1;
			return;
		}

		if (total1 == 21 || total2 == 21)
		{
			final_value = 21;
			return;
		}

		if (total2 > 21)
		{
			final_value = total1;
			return;
		}

		final_value = total2;
	}

	private void RearrangeDeckPosition()
	{
		Vector3 pos = deck_parent.position;
		pos.x = -prev_pos.x / 4;
		deck_parent.position = pos;
	}

	protected virtual void UpdateScore()
	{
		SetFinalScore();
	}

	protected virtual void SetFinalScore()
	{
		finalscore.SetActive(true);
		final_value_text.text = final_value.ToString();
	}

	public void PlayerStayed()
	{
		cards[0].TurnFaceUp();
	}

	private void Awake()
	{
		animator = GetComponent<Animator>();
		deck_parent = transform.GetChild(DECK_PARENT_POSITION);
		score_object = transform.GetChild(UIELEMENTS_POSITION).gameObject;

		VirtualAwake(score_object.transform);

		final_value_text = finalscore.GetComponent<TextMeshPro>();
	}

	protected virtual void VirtualAwake(Transform score_object)
	{
		foreach(Transform uichild in score_object)
		{
			if (uichild.CompareTag(TAGS.FINAL_SCORE))
			{
				finalscore = uichild.gameObject;
				break;
			}
		}
	}

	public virtual void InitializeDeckk()
	{
		cards = new List<Card>();
		ResetDeck();
		RemoveTestCard();
	}

	public virtual void ResetDeck(List<Card> finished_deck)
	{
		ResetDeck();
		if (finished_deck == null) return; // null only at initial
		foreach (Card card in cards)
		{
			card.ResetCard();
			finished_deck.Add(card);
		}
		cards.Clear();
	}

	protected virtual void ResetDeck()
	{
		total1 = 0;
		total2 = 0;
		prev_order_layer = 0;
		prev_pos.x = -0.2f;
		score_object.SetActive(false);
	}


	#region Testing

	[Header("\n\nTesing")]
	public GameObject testingcard;


	public void RemoveTestCard()
	{
		Destroy(testingcard);
	}

	#endregion

}
