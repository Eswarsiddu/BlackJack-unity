using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class Pack : MonoBehaviour
{

    protected int total1;
	protected int total2;
	protected int final_value;
	protected List<Card> cards;
    protected STATE state;

	#region Value

	public abstract void playerStayed();

    protected abstract void checkWinStatus();

    public void initializePack()
	{
		#region value

		total1 = 0;
        total2 = 0;
        final_value = 0;
        cards = new List<Card>();

		#endregion


		#region graphics

		prev_order_layer = 0;
        prev_pos.x = -0.2f;

        #endregion
    }

    public void Stayed()
    {
        state = STATE.STAYED;
    }

    public int getFinal_value()
    {
        return final_value;
    }

    public void addCard(Card card)
    {
        cards.Add(card);
        addcardgui(card);
        calculateTotal();
    }

    protected void calculateTotal()
    {
        int no_of_A = 0;
        int total = 0;
        foreach (Card card in cards)
        {
            if (!card.isFaceDown())
            {
                total += card.value;
                if (card.value == 1) no_of_A += 1;
            }
        }
        total1 = total;
        total2 = total + ((no_of_A > 0) ? 10 : 0);
        checkWinStatus();
    }

    public void resetDeck(List<Card> finished_deck)
    {
		#region value

		total1 = 0;
        total2 = 0;
        state = STATE.PLAYING;
        finished_deck.AddRange(cards);
        cards.Clear();

        #endregion


        #region graphics

        prev_order_layer = 0;
        prev_pos.x = -0.2f;

		#endregion
	}

    #endregion

    #region Graphics

    [SerializeField] private GameObject twotoals;
	[SerializeField] private TextMeshProUGUI total1_text;
    [SerializeField] private TextMeshProUGUI total2_text;

    [SerializeField] private GameObject finalscore;
    [SerializeField] private TextMeshProUGUI final_value_text;

    [SerializeField] private Transform parent_deck;
    [SerializeField] private Vector3 prev_pos;
    private int prev_order_layer;

    private void addcardgui(Card card)
	{
        card.parent = parent_deck;
        prev_pos.x += 0.2f;
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
        final_value_text.text = final_value.ToString();
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

	#endregion

}
