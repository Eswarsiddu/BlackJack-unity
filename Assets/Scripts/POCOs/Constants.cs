using System.Collections.Generic;
using UnityEngine;

public static class TAGS
{
    public const string BACK = "Back";
    public const string HAPTIC = "Haptic";
    public const string SOUND = "Sound";
    public const string HOME_SCREEN = "HomeScreen";
    public const string TOTAL1 = "Total1";
    public const string TOTAL2 = "Total2";
    public const string TOTAL = "Total";
    public const string FINAL_SCORE = "FinalScore";
    public const string PLAYER_DECK = "PlayerDeck";
    public const string DEALER_DECk = "DealerDeck";

    public const string COIN_OBJECT = "coins";
    public const string COIN_TEXT_OBJECT = "coinstextobject";
}

public static class Constants
{
    public const int DEFAULT_COINS = 500;
    public const float PRECENT = 0.2f;
    public const int DOUBLE = 2;

    public const float WAITING_TIME = 3f;

    public const string SETTINGS_PATH = "Scriptables/Settings";
    public const string PLAYER_DATA_PATH = "Scriptables/PlayerData";
    public const string VOICES_AUDIO_PATH = "Sounds/Voices";
    public const string CARD_FRONT_IMAGE_PATH = "PlayingCards";
    public const string CARD_BACK_BLACK_IMAGE_PATH = "PlayingCardsBack/Black";
    public const string CARD_BACK_BLUE_IMAGE_PATH = "PlayingCardsBack/Blue";
    public const string CARD_BACK_RED_IMAGE_PATH = "PlayingCardsBack/Red";
    public const string CARD_PREFAB_PATH = "Prefabs/card";

    private static System.Random random = new System.Random();

    public static Sprite CARDBACKSPRITE = Resources.Load<Sprite>(CARD_BACK_BLACK_IMAGE_PATH);

    public static Transform offsiteCardsParent;

    public static int GetRandomNumber(int n)
	{
        return random.Next(n);
    }
}
