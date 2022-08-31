using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameScreen : MonoBehaviour
{
	private class Movement
	{
		public Vector3 original_position;
		public Vector3 target_position;
		public Vector3 off_screen_position;
		private RectTransform trans;
		public Vector3 current_position { 
			get => trans.localPosition; 
			set { trans.localPosition = value; }
		}
		public Movement(RectTransform trans)
		{
			this.trans = trans;
			original_position = current_position;
		}

		public void SetOffScreenPosiiton(bool isheight = true,int direction = 1, float offscreenwidth = 20)
		{
			off_screen_position = original_position;
			if (isheight)
			{
				off_screen_position.y -= (direction * (trans.rect.height + offscreenwidth));
			}
			else
			{
				off_screen_position.x -= (direction * (trans.rect.width + offscreenwidth));
			}
		}

		public Movement(GameObject obj)
		{
			trans = obj.GetComponent<RectTransform>();
			original_position = current_position;
		}

		public float DifferenceX()
		{
			return Mathf.Abs(current_position.x - target_position.x);
		}

		public float DifferenceY()
		{
			return Mathf.Abs(current_position.y - target_position.y);
		}

		public void Move()
		{
			current_position = Vector3.Lerp(current_position,target_position,InnerConstants.ANIMATION_SPEED);
		}

		internal void ChangeTagetPosition(bool reverse= false)
		{
			if (reverse)
			{
				current_position = off_screen_position;
				target_position = original_position;
			}
			else
			{
				target_position = off_screen_position;
			}
		}
	}

	private static class InnerConstants
	{
		public const string BET_SELECT_AREA = "betselect";
		public const string DEAL_AREA = "dealarea";
		public const string DEALING = "dealing";
		public const string BET_AMOUNT = "betamount";
		public const string WIN_TEXT = "wintext";
		public const string SCROOLBAR = "scrollbar";
		public const string MIN_TEXT = "mintext";
		public const string MAX_TEXT = "maxtext";
		public const string START_DEAL_OBJECT = "startdeal";
		public const string HIT_OBJECT = "hit";
		public const string STAY_OBJECT = "stay";

		public const string WIN_TEXT_TRIGER = "BetArea";

		public const float ANIMATION_SPEED = 0.075f;
	};

	[SerializeField] private GameObject home_Screen_object;
	private HomeScreen home_screen_sript;

	[SerializeField] private GameObject wintext_object;

	[SerializeField] private DeckManager deckmanager;
	[SerializeField] private TextMeshProUGUI coins_text;

	[SerializeField] private Scrollbar scrollbar;

	[SerializeField] private Button backbutton;

	[SerializeField] private PlayerData playerdata;

	private int minvalue;
	private int maxvalue; // TODO: Set min and values of table based on coins
	private int betamount;

	[SerializeField] private TextMeshProUGUI min_text;
	[SerializeField] private TextMeshProUGUI max_text;
	[SerializeField] private TextMeshProUGUI bet_text;

	[SerializeField] private GameObject deal_area_obj;
	[SerializeField] private GameObject bet_select_object;
	[SerializeField] private GameObject deal_object;
	[SerializeField] private GameObject hit_object;
	[SerializeField] private GameObject stay_object;

	private Movement bet_select_movment;
	private Movement hit_movement;
	private Movement stay_movement;
	private Movement bet_amount_movement;

	private List<Movement> movements;
	private Vector3 bet_text_target_pos;
	private Animator animator;

	private void Awake()
	{
		playerdata = Resources.Load<PlayerData>(Constants.PLAYER_DATA_PATH);
		bet_text_target_pos = new Vector3(0, bet_text.GetComponent<RectTransform>().rect.height, 0);

		bet_select_movment = new Movement(bet_select_object);
		bet_select_movment.SetOffScreenPosiiton();

		hit_movement = new Movement(hit_object);
		hit_movement.SetOffScreenPosiiton(isheight: false);

		stay_movement = new Movement(stay_object);
		stay_movement.SetOffScreenPosiiton(isheight: false, direction: -1,offscreenwidth:40);

		bet_amount_movement = new Movement(bet_text.gameObject);
		bet_amount_movement.target_position = bet_amount_movement.current_position;

		home_screen_sript = home_Screen_object.GetComponent<HomeScreen>();

		movements = new List<Movement>();
		movements.Add(bet_amount_movement);
		movements.Add(bet_select_movment);
		movements.Add(stay_movement);
		movements.Add(hit_movement);
	}

	void Start()
	{

		/*foreach(Transform child in transform)
		{
			switch(child.tag)
			{
				case InnerTags.WIN_TEXT:
					win_text = child.GetComponent<TextMeshProUGUI>();
					break;
				case TAGS.COIN_OBJECT:
					foreach(Transform ch in child)
					{
						if (ch.CompareTag(TAGS.COIN_TEXT_OBJECT))
						{
							coins_text = ch.GetComponent<TextMeshProUGUI>();
							break;
						}
					}
					break;
				case InnerTags.DEAL_AREA:
					foreach(Transform child1 in child)
					{
						switch(child1.tag)
						{
							case InnerTags.BET_SELECT_AREA:

						}
					}
					break;
				case TAGS.BACK:
					backbutton = child.GetComponent<Button>();
					break;
			}
		}*/


		/*hit_transform = hit_object.GetComponent<RectTransform>();
		stay_transform = stay_object.GetComponent<RectTransform>();*/

		playerdata.RefreshCoinsText();
		deckmanager.nextDeal = nextDeal;
		deckmanager.DisableDealOptions = DisableDealOptions;
		deckmanager.GameScreenDealEnd = dealEnd;
		deckmanager.EnableDealOptions = EnableDealOptions;
		backbutton.onClick.AddListener(SoundManager.PlayUIElementClickSound);
		scrollbar.onValueChanged.AddListener(CalculateBetAmount);

		animator = GetComponent<Animator>();
	}

	public void CalculateBetAmount(float value)
	{
		betamount = minvalue + (int)((maxvalue - minvalue) * value);
		if(minvalue == 0)
		{
			home_Screen_object.SetActive(true);
			home_screen_sript.insufficentbalance();
			gameObject.SetActive(false);
		}
		bet_text.text = betamount.ToString();
	}

	private void OnEnable()
	{
		playerdata.UpdateCoins += UpdateCoinsText;
		bet_select_movment.current_position = bet_select_movment.original_position;
		nextDeal();
	}

	private void OnDisable()
	{
		playerdata.UpdateCoins -= UpdateCoinsText;
		deckmanager.resetDeck();
	}

	public void StartDeal()
	{
		SoundManager.PlayUIElementClickSound();
		deckmanager.SetBetAmount(betamount);
		deckmanager.startDeal();
		bet_amount_movement.target_position = bet_text_target_pos;
		bet_select_movment.ChangeTagetPosition();

		IEnumerator StartDealEnumerator(){
			yield return new WaitForSeconds(0.5f);
			bet_select_object.SetActive(false);
		};

		StartCoroutine(StartDealEnumerator());
	}

	public void EnableDealOptions()
	{
		deal_object.SetActive(true);
		hit_movement.ChangeTagetPosition(reverse: true);
		/*Vector3 pos = stay_movement.current_position;
		pos.x = -hit_movement.current_position.x;
		stay_movement.current_position = pos;*/
		stay_movement.ChangeTagetPosition(reverse:true);
	}

	private void dealEnd()
	{
		wintext_object.SetActive(true);
		animator.SetTrigger(InnerConstants.WIN_TEXT_TRIGER);
		StartCoroutine(dealEndEnnumerator());
	}

	private void DisableDealOptions()
	{
		hit_movement.ChangeTagetPosition();
		stay_movement.ChangeTagetPosition();
	}

	private IEnumerator dealEndEnnumerator()
	{
		yield return new WaitForSeconds(0.25f);
		deal_area_obj.SetActive(false);
	}

	public GameObject bet_text_object;
	private void nextDeal()
	{
		minvalue = Constants.GetMinimumBet(playerdata.coins);
		maxvalue = playerdata.coins - minvalue;
		min_text.text = minvalue.ToString();
		max_text.text = maxvalue.ToString();
		CalculateBetAmount(scrollbar.value);
		bet_amount_movement.current_position = bet_amount_movement.original_position;
		bet_amount_movement.target_position = bet_amount_movement.original_position;
		deal_area_obj.SetActive(true);
		wintext_object.SetActive(false);
		bet_select_object.SetActive(true);
		bet_text.gameObject.SetActive(false);
		bet_select_movment.ChangeTagetPosition(reverse: true);
		deal_object.SetActive(false);
		StartCoroutine(nextDealenummerator());
	}

	private IEnumerator nextDealenummerator()
	{
		yield return new WaitForSeconds(0.7f);
			bet_text.gameObject.SetActive(true);
	}

	private void UpdateCoinsText(object sender, EventArgs e)
	{
		coins_text.text = playerdata.coins.ToString();
		// TODO : Include Graphics
	}

	private void Update()
	{
		/*foreach(Movement movement in movements)
			if (movement.DifferenceX() > 0.1f || movement.DifferenceX() > 0.1f)
				movement.Move();*/

		if(bet_select_movment.DifferenceY()>0.1f)
		{
			bet_select_movment.Move();
		}

		if (bet_amount_movement.DifferenceX() > 0.1f)
		{
			bet_amount_movement.Move();
		}

		if (hit_movement.DifferenceX() > 0.01f)
		{
			hit_movement.Move();
		}

		if (stay_movement.DifferenceX() > 0.01f)
		{
			stay_movement.Move();
		}

		if (Input.GetKeyDown(KeyCode.Escape))
			backbutton.onClick.Invoke();


		#region Testimg

		if(startdeal_bool)
		{
			startdeal_bool = false;
			StartDeal();
		}

		if (hit_bool)
		{
			hit_bool = false;
			deckmanager.playerHit();
		}

		if (stay_bool)
		{
			stay_bool = false;
			deckmanager.playerStay();
		}

		#endregion

	}

	#region Testimg
	
	public bool startdeal_bool = false;
	public bool hit_bool = false;
	public bool stay_bool =false;
	
	#endregion

}
