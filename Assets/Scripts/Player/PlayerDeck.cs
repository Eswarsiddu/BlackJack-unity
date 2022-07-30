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
	private int _final_value;
	protected List<Card> cards;
    private STATE state;

    private GameObject scoreobject;

    [SerializeField] private GameObject twotoals;
    private TextMeshPro total1_text;
    private TextMeshPro total2_text;

    private GameObject finalscore;
    private TextMeshPro final_value_text;

    private Transform deck_parent;

    private Vector3 prev_pos;
    private int prev_order_layer;

    public WIN_STATUS win_status { get; set; }


	private void Awake()
	{
        deck_parent = transform.GetChild(DECK_PARENT_POSITION);
        scoreobject = transform.GetChild(UIELEMENTS_POSITION).gameObject;
        foreach (Transform uichild in scoreobject.transform)
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

        foreach(Transform totalchild in twotoals.transform)
		{
			switch (totalchild.tag)
			{
                case TAGS.TOTAL1:
                    total1_text = totalchild.GetComponent<TextMeshPro>(); break;
                case TAGS.TOTAL2:
                    total2_text = totalchild.GetComponent<TextMeshPro>();break;
            }
            if(total1_text != null && total2_text != null)
			{
                break;
			}
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

    private void EnableScore()
	{
        scoreobject.SetActive(true);
	}

    private void addcardgui(Card card)
	{
        EnableScore();
        card.parent = deck_parent;
        prev_pos.x += 0.2f;
        card.transform.localPosition = prev_pos;
        card.orderinlayer = prev_order_layer++;
	}

    private void updateDeck()
	{
        Vector3  pos = deck_parent.position;
        pos.x = -prev_pos.x / 4;
        deck_parent.position = pos;
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
