using UnityEngine;
public class Constants
{
    public static string getCardNumber(int value)
    {
		switch (value)
		{
            case 1: return "A";
            case 11: return "J";
            case 12:return "Q";
            case 13:return "K";
		}
        return value.ToString();
    }

    public static string[] CARD_NUMBERS = { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };

    public static Sprite CARDBACKSPRITE;

    public static Transform finisheddeckparent;
}
