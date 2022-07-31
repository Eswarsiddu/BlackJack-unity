using TMPro;
using UnityEngine;

public class PlayerDeck : DealerDeck
{
    private GameObject twotoals;

    private TextMeshPro total1_text;
    private TextMeshPro total2_text;

    private STATE state;
    public WIN_STATUS win_status { get; set; }


    public virtual void Stayed()
    {
        state = STATE.STAYED;
        UpdateScore();
    }

    protected override void ChangeCardState(Card card)
    {
        card.TurnFaceUp();
    }

	protected override void CalculateTotal()
	{
		base.CalculateTotal();
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

    protected override void UpdateScore()
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

	protected override void SetFinalScore()
	{
        twotoals.SetActive(false);
        base.SetFinalScore();
	}

	private void SetTwoScores()
	{
        finalscore.SetActive(false);
        twotoals.SetActive(true);
        total1_text.text = total1.ToString();
        total2_text.text = total2.ToString();
    }

	protected override void VirtualAwake(Transform score_object)
	{
        foreach (Transform uichild in score_object)
        {
            switch (uichild.tag)
            {
                case TAGS.TOTAL:
                    twotoals = uichild.gameObject; break;
                case TAGS.FINAL_SCORE:
                    finalscore = uichild.gameObject; break;
            }
        }

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

	protected override void ResetDeck()
	{
		base.ResetDeck();
        state = STATE.PLAYING;
        win_status = WIN_STATUS.NONE;
    }

}
