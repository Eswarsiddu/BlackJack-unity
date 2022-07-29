using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerDeck : MonoBehaviour
{

    protected int total1;
	protected int total2;
	protected int _final_value;
	protected List<Card> cards;
    protected STATE state;

    [SerializeField] private GameObject scoreobject;

    private WIN_STATUS _win_status;

    public WIN_STATUS win_status
    {
        get => _win_status;

        set
        {
            _win_status = value;
        }
    }

    public virtual void playerStayed() {
        state = STATE.STAYED;
    }

    public virtual void initializePack()
	{

		total1 = 0;
        total2 = 0;
        _final_value = 0;
        cards = new List<Card>();

        _win_status = WIN_STATUS.NONE;



        prev_order_layer = 0;
        prev_pos.x = -0.2f;
        scoreobject.SetActive(false);


        RemoveTestCard();
    }

    public void Stayed()
    {
        state = STATE.STAYED;
    }

    public int final_value { get => _final_value; }

    protected virtual void ChangeCardState(Card card)
	{
        card.TurnFaceUp();
	}


    public void addCard(Card card)
    {
        ChangeCardState(card);
        card.gameObject.SetActive(true);
        cards.Add(card);
        addcardgui(card);
        calculateTotal();
        updateGraphics();
    }

    protected void calculateTotal()
    {
        int no_of_A = 0;
        int total = 0;
        foreach (Card card in cards)
        {
            if (card.isFaceUp)
            {
                total += card.value;
                if (card.value == 1) no_of_A += 1;
            }
        }
        total1 = total;
        total2 = total + ((no_of_A > 0) ? 10 : 0);
        checkWinStatus();
    }

    protected virtual void checkWinStatus()
    {
        if (total1 > 21)
        {
            win_status = WIN_STATUS.BUST;
            state = STATE.STAYED;
            _final_value = total1;
            return;
        }

        if (total1 == 21 || total2 == 21)
        {
            win_status = WIN_STATUS.BLACKJACK;
            state = STATE.STAYED;
            _final_value = 21;
            return;
        }

        if (total2 > 21)
        {
            _final_value = total1;
        }
        else
        {
            _final_value = total2;
        }

        win_status = WIN_STATUS.NONE;
    }

    public virtual void resetDeck(List<Card> finished_deck)
    {

		total1 = 0;
        total2 = 0;
        state = STATE.PLAYING;
        _win_status = WIN_STATUS.NONE;


        prev_order_layer = 0;
        prev_pos.x = -0.2f;
        scoreobject.SetActive(false);

        foreach (Card card in cards)
		{
            card.ResetCard();
            finished_deck.Add(card);
		}

        cards.Clear();
    }

    [SerializeField] private GameObject twotoals;
	[SerializeField] private TextMeshPro total1_text;
    [SerializeField] private TextMeshPro total2_text;

    [SerializeField] private GameObject finalscore;
    [SerializeField] private TextMeshPro final_value_text;

    [SerializeField] private Transform parent_deck;
    private Vector3 prev_pos;
    private int prev_order_layer;

    private void EnableScore()
	{
        scoreobject.SetActive(true);
	}

    private void addcardgui(Card card)
	{
        EnableScore();
        card.parent = parent_deck;
        prev_pos.x += 0.2f;
        card.transform.localPosition = prev_pos;
        card.orderinlayer = prev_order_layer++;
	}

    private void updateDeck()
	{
        Vector3  pos = parent_deck.position;
        pos.x = -prev_pos.x / 4;
        parent_deck.position = pos;
	}

    private void setfinalscore()
	{
        finalscore.SetActive(true);
        twotoals.SetActive(false);
        final_value_text.text = _final_value.ToString();
    }

    private void settwoScores()
	{
        finalscore.SetActive(false);
        twotoals.SetActive(true);
        total1_text.text = total1.ToString();
        total2_text.text = total2.ToString();
    }

    private void updateScore()
	{
        if(state == STATE.STAYED)
		{
            setfinalscore();
		}
		else
		{
            if(total1 != total2)
			{
                if(total2 > 21)
				{
                    setfinalscore();
				}
				else
				{
                    settwoScores();
				}
			}
			else
			{
                setfinalscore();
			}
		}
	}

    private void updateGraphics()
	{
        updateDeck();
        updateScore();
        virtualupdateGraphics();
	}
    protected virtual void virtualupdateGraphics(){}



    #region Testing

    [Header("\n\nTesing")]
    public GameObject testingcard;


    public void RemoveTestCard()
	{
        Destroy(testingcard);
	}

	#endregion

}
