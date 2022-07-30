using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class PlayerDeck : MonoBehaviour
{
    private const int DECK_PARENT_POSITION = 0;
    private const int UIELEMENTS_POSITION = 2;

    private int total1;
    private int total2;
    public int final_value { get; private set; }
    private int prev_order_layer;

    private Vector3 prev_pos;

    protected List<Card> cards;

    private GameObject score_object;
    private GameObject twotoals;
    private GameObject finalscore;

    private Transform deck_parent;

    private TextMeshPro total1_text;
    private TextMeshPro total2_text;
    private TextMeshPro final_value_text;

    private STATE state;
    public WIN_STATUS win_status { get; set; }


    public virtual void Stayed()
    {
        state = STATE.STAYED;
        UpdateScore();
    }

    public void AddCard(Card card)
    {
        ChangeCardState(card);
        cards.Add(card);
        CalculateTotal();
        AddCardGUI(card);
    }

    protected virtual void ChangeCardState(Card card)
    {
        card.TurnFaceUp();
    }

    private void CalculateTotal()
    {
        int no_of_A = 0;
        total1 = cards.Sum(card => { 
            no_of_A += card.value == 1 ? 1 : 0; 
            return card.value; 
        });
      
        total2 = total1 + ((no_of_A > 0) ? 10 : 0);
        CheckWinStatus();
    }

    private void CheckWinStatus()
    {
        state = STATE.STAYED;

        if (total1 > 21)
        {
            win_status = WIN_STATUS.BUST;
            final_value = total1;
            return;
        }

        if (total1 == 21 || total2 == 21)
        {
            win_status = WIN_STATUS.BLACKJACK;
            final_value = 21;
            return;
        }

        state = STATE.PLAYING;
        win_status = WIN_STATUS.NONE;

        if (total2 > 21)
        {
            final_value = total1;
            return;
        }
        
        final_value = total2;
    }

    private void AddCardGUI(Card card)
	{
        prev_pos.x += 0.2f;

        card.gameObject.SetActive(true);
        card.UpdateDetails(deck_parent, prev_pos, prev_order_layer++);

        if (!score_object.activeInHierarchy)
            score_object.SetActive(true);

        RearrangeDeckPosition();
        UpdateScore();
    }

    private void RearrangeDeckPosition()
	{
        Vector3  pos = deck_parent.position;
        pos.x = -prev_pos.x / 4;
        deck_parent.position = pos;
	}

    protected virtual void UpdateScore()
    {
        if (state == STATE.STAYED)
        {
            SetFinalScore();
            return;
        }

        if (total1 <= total2)
        {
            SetFinalScore();
            return;
        }

        if (total2 > 21)
        {
            SetFinalScore();
            return;
        }

        SetTwoScores();
    }

    protected void SetFinalScore()
	{
        finalscore.SetActive(true);
        twotoals.SetActive(false);
        final_value_text.text = final_value.ToString();
    }

    private void SetTwoScores()
	{
        finalscore.SetActive(false);
        twotoals.SetActive(true);
        total1_text.text = total1.ToString();
        total2_text.text = total2.ToString();
    }

    private void Awake()
    {
        deck_parent = transform.GetChild(DECK_PARENT_POSITION);
        score_object = transform.GetChild(UIELEMENTS_POSITION).gameObject;
        foreach (Transform uichild in score_object.transform)
        {
            switch (uichild.tag)
            {
                case TAGS.TOTAL:
                    twotoals = uichild.gameObject; break;
                case TAGS.FINAL_SCORE:
                    finalscore = uichild.gameObject; break;
            }
        }

        final_value_text = finalscore.GetComponent<TextMeshPro>();

        foreach (Transform totalchild in twotoals.transform)
        {
            switch (totalchild.tag)
            {
                case TAGS.TOTAL1:
                    total1_text = totalchild.GetComponent<TextMeshPro>(); break;
                case TAGS.TOTAL2:
                    total2_text = totalchild.GetComponent<TextMeshPro>(); break;
            }
            if (total1_text != null && total2_text != null)
            {
                break;
            }
        }
    }

    public virtual void InitializeDeckk()
    {
        ResetDeck();
        cards = new List<Card>();
        RemoveTestCard();
    }

    public virtual void ResetDeck(List<Card> finished_deck)
    {
        ResetDeck();
        foreach (Card card in cards)
        {
            card.ResetCard();
            finished_deck.Add(card);
        }
        cards.Clear();
    }

    private void ResetDeck()
    {
        total1 = 0;
        total2 = 0;
        state = STATE.PLAYING;
        win_status = WIN_STATUS.NONE;
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
