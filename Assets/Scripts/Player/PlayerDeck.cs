using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

public class PlayerDeck : MonoBehaviour
{
    private int total1;
	private int total2;
	private int _final_value;
	protected List<Card> cards;
    private STATE state;

    [SerializeField] private GameObject scoreobject;

    public WIN_STATUS win_status { get; set; }

    public virtual void playerStayed() {
        state = STATE.STAYED;
    }

    public virtual void initializePack()
	{
		total1 = 0;
        total2 = 0;
        _final_value = 0;
        cards = new List<Card>();

        win_status = WIN_STATUS.NONE;



        prev_order_layer = 0;
        prev_pos.x = -0.2f;
        scoreobject.SetActive(false);


        RemoveTestCard();
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
        total1 = cards.Sum(card => { 
            no_of_A += card.value == 1 ? 1 : 0; 
            return card.value; 
        });
      
        total2 = total1 + ((no_of_A > 0) ? 10 : 0);
        checkWinStatus();
    }

    protected virtual void checkWinStatus()
    {
        state = STATE.STAYED;

        if (total1 > 21)
        {
            win_status = WIN_STATUS.BUST;
            _final_value = total1;
            return;
        }

        if (total1 == 21 || total2 == 21)
        {
            win_status = WIN_STATUS.BLACKJACK;
            _final_value = 21;
            return;
        }

        state = STATE.PLAYING;
        win_status = WIN_STATUS.NONE;

        if (total2 > 21)
        {
            _final_value = total1;
            return;
        }
        
        _final_value = total2;
    }

    public virtual void resetDeck(List<Card> finished_deck)
    {
		total1 = 0;
        total2 = 0;
        state = STATE.PLAYING;
        win_status = WIN_STATUS.NONE;


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
