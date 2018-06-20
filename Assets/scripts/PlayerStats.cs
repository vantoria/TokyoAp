using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour 
{
	public GameObject GOStatPanel;
	public const int maxLevel = 10;

	int mAddStatPoints = 0, mPointsTemp = 0;
	float mExpMaxTimer = 1.0f;
	bool mIsShowExpPanel = false;

	GameObject mGOLevelUpPanel;
	SaveManager mSaveManager;

	// -----------------------------ExpUI-------------------------------------
	[System.Serializable] 
	public class ExpUI
	{
		public Transform statusValueTrans;
		public Transform statusAddTrans;
		public Transform expPointsTrans;
		public Transform expPointsNextTrans;
		public Transform levelUpTrans;
		public Transform levelUpStayedTrans;
		public Transform levelUpGetTrans;
		public Transform levelUpPointTrans;
		public Transform upButtonTrans;
		public Transform confirmBtnTrans;
		public Transform resetBtnTrans;

	}
	public ExpUI expUI = new ExpUI();

	// -----------------------------Status UI------------------------------------
	[System.Serializable] 
	public class StatUI
	{
		public Transform statusValueTrans;
		public Transform expPointsNextTrans;
	}
	public StatUI statUI = new StatUI();

	// -----------------------------Status------------------------------------
	[System.Serializable]
	public class Status
	{
		public int currLevel = 0;
		public int strength = 0;
		public float speed = 0.0f;
		public int knowledge = 0;
		public int defense = 0;
		public int expToNextLevel = 0;

		public Status()
		{
			currLevel = 0;
			strength = 0;
			speed = 0.0f;
			knowledge = 0;
			defense = 0;
			expToNextLevel = 0;
		}

		public Status(int level, int str, float spd, int knowledge, int def, int expToNextLevel)
		{
			this.currLevel = level;
			this.strength = str;
			this.speed = spd;
			this.knowledge = knowledge;
			this.defense = def;
			this.expToNextLevel = expToNextLevel;
		}

		public bool IsMaxLevel()
		{ return currLevel == PlayerStats.maxLevel; }

		public void Clear()
		{
			this.currLevel = 0;
			this.strength = 0;
			this.speed = 0;
			this.knowledge = 0;
			this.defense = 0;
			this.expToNextLevel = 0;
		}
	}
	public List<Status> mStatusProgressionList = new List<Status>();
	[HideInInspector] public Status mStatus, mAddedStatus, mTempAddedStatus, mPreDefUpStatus;

	void Awake()
	{
		mSaveManager = GameObject.FindGameObjectWithTag ("SaveManager").GetComponent<SaveManager>();
	}

	void Start () 
	{
		mGOLevelUpPanel = GameObject.FindGameObjectWithTag("LevelUpPanel").gameObject;
		StatusInitialization();

		mGOLevelUpPanel.SetActive(false);
	}
	
	void Update () 
	{
		// TO BE DELETED.
		if(Input.GetKeyDown(KeyCode.Space) && !mGOLevelUpPanel.activeSelf) ShowLevelUpStatsUI(50);
		else if (Input.GetKeyDown(KeyCode.Space) && mGOLevelUpPanel.activeSelf && !mIsShowExpPanel)
		{
			if (!mStatus.IsMaxLevel ()) 
			{
				StartCoroutine (ExpCalculate (50));
				mIsShowExpPanel = true;
			}
			else StartCoroutine (WaitForKeyDown (KeyCode.Space));
		}
	}

	// Hard-coded progression stats.
	void StatusInitialization()
	{
		mStatusProgressionList.Add(new Status(1, 2, 1.0f, 2, 5, 2));
		mStatusProgressionList.Add(new Status(2, 8, 1.5f, 2, 10, 5));
		mStatusProgressionList.Add(new Status(3, 12, 2.0f, 2, 15, 8));
		mStatusProgressionList.Add(new Status(4, 16, 2.5f, 3, 20, 12));
		mStatusProgressionList.Add(new Status(5, 20, 3.0f, 3, 25, 17));
		mStatusProgressionList.Add(new Status(6, 24, 3.5f, 3, 30, 24));
		mStatusProgressionList.Add(new Status(7, 28, 4.0f, 4, 35, 32));
		mStatusProgressionList.Add(new Status(8, 32, 4.5f, 4, 40, 42));
		mStatusProgressionList.Add(new Status(9, 36, 5.0f, 4, 45, 53));
		mStatusProgressionList.Add(new Status(10, 40, 5.5f, 5, 50, 65));

		mStatus = mSaveManager.LoadCurrStatus ();
		mAddedStatus = mSaveManager.LoadAddedStatus ();

		if(mStatus.currLevel == 0) mStatus = mStatusProgressionList[0];
	}

	public void ShowStats()
	{
		GOStatPanel.SetActive(true);
//		Debug.Log ("Curr Level +       " + mStatus.currLevel);
		int count = expUI.statusValueTrans.childCount;
		for(int i = 0; i < count; i++)
		{
			string name = statUI.statusValueTrans.GetChild(i).name;

			Text text = statUI.statusValueTrans.GetChild(i).GetComponent<Text>();
			if (name == "Level") text.text = mStatus.currLevel.ToString();
			else if (name == "Strength") text.text = (mStatus.strength + mAddedStatus.strength).ToString();
			else if (name == "Speed") text.text = (mStatus.speed + mAddedStatus.speed).ToString();
			else if (name == "Knowledge") text.text = (mStatus.knowledge + mAddedStatus.knowledge).ToString();
			else if (name == "Defense") text.text = (mStatus.defense + mAddedStatus.defense).ToString();
		}

		Text expNext = statUI.expPointsNextTrans.GetComponent<Text>();

		if (mStatus.currLevel == maxLevel) expNext.text = "---";
		else expNext.text = mStatusProgressionList[mStatus.currLevel - 1].expToNextLevel.ToString();
	}

	public void AddStatPoints(Transform selfBox)
	{
		if (mPointsTemp <= 0) return;

		int  count = expUI.statusAddTrans.childCount;
		string name = selfBox.name;

		mPointsTemp -= 1;
		expUI.levelUpPointTrans.GetComponent<Text>().text = mPointsTemp.ToString();

		for(int i = 1; i < count; i++)
		{
			string goName = expUI.statusAddTrans.GetChild (i).name;
			Text text = expUI.statusAddTrans.GetChild(i).GetComponent<Text>();

			if (goName == "Strength" && name == "Strength") 
			{ 
				mTempAddedStatus.strength += 1;
				text.text = (mPreDefUpStatus.strength + mTempAddedStatus.strength).ToString(); 
				break; 
			}
			else if (goName == "Speed" && name == "Speed") 
			{ 
				mTempAddedStatus.speed += 1;
				text.text = (mPreDefUpStatus.speed + mTempAddedStatus.speed).ToString();  
				break; 
			}
			else if (goName == "Knowledge" && name == "Knowledge") 
			{ 
				expUI.statusAddTrans.GetChild (i).gameObject.SetActive (true);
				mTempAddedStatus.knowledge += 1;
				text.text = (mPreDefUpStatus.knowledge + mTempAddedStatus.knowledge).ToString(); 
				break; 
			}
			else if (goName == "Defense" && name == "Defense") 
			{ 
				mTempAddedStatus.defense += 1;
				text.text = (mPreDefUpStatus.defense + mTempAddedStatus.defense).ToString(); 
				break; 
			}
		}

		if (mPointsTemp == 0) 
		{
			expUI.confirmBtnTrans.gameObject.SetActive (true);
			expUI.upButtonTrans.gameObject.SetActive (false);
		}
	}

	public void Confirm()
	{
		mAddedStatus.strength += mTempAddedStatus.strength;
		mAddedStatus.speed += mTempAddedStatus.speed;
		mAddedStatus.knowledge += mTempAddedStatus.knowledge;
		mAddedStatus.defense += mTempAddedStatus.defense;

		mAddStatPoints = 0;
		mTempAddedStatus.Clear ();

		mPreDefUpStatus.Clear();
		expUI.levelUpGetTrans.gameObject.SetActive (false);
		expUI.statusAddTrans.gameObject.SetActive (false);
		expUI.upButtonTrans.gameObject.SetActive (false);
		mGOLevelUpPanel.SetActive(false);
		mIsShowExpPanel = false;

		mSaveManager.SaveStatus (mStatus, mAddedStatus);
		SceneManager.LoadScene("mainMenu");
	}

	public void Reset()
	{
		mPointsTemp = mAddStatPoints;
		expUI.levelUpPointTrans.GetComponent<Text>().text = mAddStatPoints.ToString();
		mTempAddedStatus.Clear ();

		expUI.upButtonTrans.gameObject.SetActive (true);
		expUI.confirmBtnTrans.gameObject.SetActive (false);

		int count = expUI.statusAddTrans.childCount;
		for (int i = 1; i < count; i++) 
		{
			Transform child = expUI.statusAddTrans.GetChild (i);
			string name = child.name;
			Text text = child.GetComponent<Text>();

			if (name == "Strength") text.text = (mPreDefUpStatus.strength + mTempAddedStatus.strength).ToString();
			else if (name == "Speed") text.text = (mPreDefUpStatus.speed + mTempAddedStatus.speed).ToString();
			else if (name == "Knowledge") 
			{
				if (mPreDefUpStatus.knowledge == 0) expUI.statusAddTrans.GetChild (i).gameObject.SetActive (false);
				text.text = (mPreDefUpStatus.knowledge + mTempAddedStatus.knowledge).ToString();
			}
			else if (name == "Defense") text.text = (mPreDefUpStatus.defense + mTempAddedStatus.defense).ToString();
		}
	}

	void ShowLevelUpStatsUI(int getExpPoint)
	{
		mGOLevelUpPanel.SetActive(true);
		expUI.resetBtnTrans.gameObject.SetActive (false);
		expUI.confirmBtnTrans.gameObject.SetActive (false);
		expUI.statusAddTrans.gameObject.SetActive (false);
		expUI.levelUpPointTrans.gameObject.SetActive (false);
		expUI.upButtonTrans.gameObject.SetActive (false);

		int count = expUI.statusValueTrans.childCount;
		for(int i = 0; i < count; i++)
		{
			string name = expUI.statusValueTrans.GetChild(i).name;

			Text text = expUI.statusValueTrans.GetChild(i).GetComponent<Text>();
			if (name == "Level") text.text = mStatus.currLevel.ToString();
			else if (name == "Strength") text.text = (mStatus.strength + mAddedStatus.strength).ToString();
			else if (name == "Speed") text.text = (mStatus.speed + mAddedStatus.speed).ToString();
			else if (name == "Knowledge") text.text = (mStatus.knowledge + mAddedStatus.knowledge).ToString();
			else if (name == "Defense") text.text = (mStatus.defense + mAddedStatus.defense).ToString();
		}

		Text exp = expUI.expPointsTrans.GetComponent<Text>();
		Text expNext = expUI.expPointsNextTrans.GetComponent<Text>();

		if (mStatus.currLevel == maxLevel) 
		{
			exp.text = "MAX";
			expNext.text = "---";
		}
		else 
		{
			exp.text = getExpPoint.ToString();
			expNext.text = mStatusProgressionList[mStatus.currLevel - 1].expToNextLevel.ToString();
		}
	}

	IEnumerator ExpCalculate(int getExp) 
	{
		float maxTimer = mExpMaxTimer;
		float currTimer = 0.0f;

		Text expText = expUI.expPointsTrans.GetComponent<Text>();
		Text expNextText = expUI.expPointsNextTrans.GetComponent<Text>();

		int usedExp = 0, levelUpNo = 0;
		int prevLvl = mStatus.currLevel;

		while(currTimer < maxTimer)
		{
			int expToMinus = (int) Mathf.Ceil((currTimer / maxTimer) * getExp);
			int expVal = getExp - expToMinus;
			expText.text = expVal.ToString();

			int expNext = mStatus.expToNextLevel - (expToMinus - usedExp);
			expNextText.text = expNext.ToString();

			if (mStatus.currLevel == maxLevel) 
			{
				expText.text = "MAX";
				expNextText.text = "---";
			}
			else if (expNext <= 0)  // Leveled up.
			{
				Debug.Log ("Leveled up" + mStatus.currLevel);

				levelUpNo += 1;
				StartCoroutine(LevelUpIcon(1.0f));

				expUI.levelUpStayedTrans.gameObject.SetActive (true);
				GameObject go = expUI.statusAddTrans.gameObject;
				if(!go.activeSelf) expUI.statusAddTrans.gameObject.SetActive (true);

				usedExp += expToMinus;
				mStatus = mStatusProgressionList[mStatus.currLevel]; // Move on to next level.
				mAddStatPoints += 1;
				mPointsTemp = mAddStatPoints;

				GameObject getGO = expUI.levelUpGetTrans.gameObject;
				if (!getGO.activeSelf) 
				{
					getGO.SetActive (true);
					expUI.statusAddTrans.gameObject.SetActive (true);
				}
				expUI.levelUpPointTrans.GetComponent<Text>().text = mAddStatPoints.ToString();

				// Pre-defined updated stats.
				int  count = expUI.statusAddTrans.childCount;
				for(int i = 0; i < count; i++)
				{
					string name = expUI.statusAddTrans.GetChild(i).name;
					Text text = expUI.statusAddTrans.GetChild(i).GetComponent<Text>();
					Status prevStats = mStatusProgressionList[prevLvl - 1];

					if (name == "Level") 
					{
						mPreDefUpStatus.currLevel = levelUpNo;
						text.text = mPreDefUpStatus.currLevel.ToString (); 
					} 
					else if (name == "Strength") 
					{
						mPreDefUpStatus.strength = mStatus.strength - prevStats.strength;
						text.text = mPreDefUpStatus.strength.ToString ();
					}
					else if (name == "Speed") 
					{
						mPreDefUpStatus.speed = mStatus.speed - prevStats.speed;
						text.text = mPreDefUpStatus.speed.ToString();
					}
					else if (name == "Knowledge")
					{
						mPreDefUpStatus.knowledge = mStatus.knowledge - prevStats.knowledge;
						if (mPreDefUpStatus.knowledge == 0) expUI.statusAddTrans.GetChild (i).gameObject.SetActive (false);
						else expUI.statusAddTrans.GetChild (i).gameObject.SetActive (true);
						text.text = mPreDefUpStatus.knowledge.ToString(); 
					}
					else if (name == "Defense") 
					{
						mPreDefUpStatus.defense = mStatus.defense - prevStats.defense;
						text.text = mPreDefUpStatus.defense.ToString(); 
					}
				}
			}

			currTimer += Time.deltaTime;
			yield return null;
		}

		int temp;
		bool isConverted = int.TryParse(expNextText.text.ToString(), out temp);
		if (isConverted && temp != 0) mStatus.expToNextLevel = temp;

		// After calculating exp, if leveled up, make buttons visi	ble.
		if (expUI.statusAddTrans.gameObject.activeSelf) 
		{
			expUI.upButtonTrans.gameObject.SetActive (true);
			expUI.resetBtnTrans.gameObject.SetActive (true);
			expUI.levelUpPointTrans.gameObject.SetActive (true);
		} 
		else yield return StartCoroutine (WaitForKeyDown (KeyCode.Space));
	}

	// TO BE DELETED. When user press `keycode` at the end of expCalculate, disable level up panel.
	IEnumerator WaitForKeyDown(KeyCode keyCode)
	{
		while (!Input.GetKeyDown(keyCode))
			yield return null;

		mIsShowExpPanel = false;
		mGOLevelUpPanel.gameObject.SetActive (false);
	}

	IEnumerator LevelUpIcon(float maxTime) 
	{
		GameObject levelUpGO = Instantiate (expUI.levelUpTrans.gameObject, expUI.levelUpTrans.position, expUI.levelUpTrans.rotation);
		levelUpGO.SetActive (true);
		levelUpGO.transform.SetParent(expUI.levelUpTrans.parent);
		float currTimer = 0.0f;

		while (currTimer < maxTime) 
		{

			Vector3 temp = levelUpGO.transform.position;
			temp.x += 1.0f;
			temp.y += 1.0f;
			levelUpGO.transform.position = temp;

			Image image = levelUpGO.GetComponent<Image>();
			float percent = 1 - ((currTimer / maxTime) * 1.0f);
			Color c = image.color;
			c.a = percent;
			image.color = c;

			currTimer += Time.deltaTime;
			yield return null;
		}

		Destroy (levelUpGO);
	}
}
