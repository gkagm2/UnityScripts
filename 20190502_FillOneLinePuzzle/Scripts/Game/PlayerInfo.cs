﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerInfo : MonoBehaviour {

    [Header("Player")]
    [Tooltip("현재 가지고 있는 코인")]
    public int coin = 200;

    [Space]

    [Tooltip("얻을 수 있는 최대 별의 개수")]
    public int maxStars = 0;

    [Space]

    [Tooltip("현재 가지고있는 별의 총 개수")]
    public int currentHaveStarsTotalNumber = 0;

    [Space]

    [Tooltip("레벨마다의 최대 별의 개수")]
    public int[] maxStarsPerLevel;

    [Space]

    [Tooltip("현재 가지고 있는 각 레벨에 해당하는 별의 개수")]
    public int[] currentHaveStarsPerLevel;
    

    public LevelStageInfo[] levelStageInfo; // 각 레벨과 스테이지의 정보를 담고있다.
    


    // 현재 하고있는 레벨과 스테이지
    public short currentLevel = 1;
    public short currentStage = 1;

    // 현재까지 진행된 레벨과 스테이지
    public short myProgressLevel = 1;
    public short myProgressStage = 1;


    // Use this for initialization
    void Start () {
        CurrentProgressLevelAndStageSetting();  // 현재 진도 정보 세팅
        LevelStageInfoSetting();                // 레벨과 스테이지의 정보를 세팅한다.
        InitStarSetting();                      //별의 개수 초기화.
    }

   
    
	
	// Update is called once per frame
	void Update () {
        Debug.Log("현재 레벨 : " + currentLevel + ", 현재 스테이지 : " + currentStage);
        Debug.Log("진행 레벨 : " + myProgressLevel + ", 진행 스테이지 : " + myProgressStage);
        UpdateStars(); //별들의 개수를 업데이트.
    }




    // 현재 진도 레벨과 스테이지 세팅
    public void CurrentProgressLevelAndStageSetting()
    {
        
        if (myProgressLevel == 1 && myProgressStage == 1)// 처음(Level : 1, Stage : 1)이면 PlayerPrefabs에  저장
        {
            PlayerPrefs.SetInt("myProgressLevel", (int)myProgressLevel);
            PlayerPrefs.SetInt("myProgressStage", (int)myProgressStage);
        }
        else // 처음이 아니면
        {
            myProgressLevel = (short)PlayerPrefs.GetInt("myProgressLevel");
            myProgressStage = (short)PlayerPrefs.GetInt("myProgressStage");
        }
    }


    // 해당 레벨과 스테이지 값에 일치하는 레벨과 스테이지의 정보 오브젝트를 반환한다.
    public LevelStageInfo GetLevelStageInfoObj(short level, short stage)
    {
        for(int i=0; i< levelStageInfo.Length; i++)
        {
            if(level == levelStageInfo[i].level && stage == levelStageInfo[i].stage)
            {
                LevelStageInfo tempLSInfo = new LevelStageInfo();
                tempLSInfo = levelStageInfo[i];
                return tempLSInfo;
            }
        }
        return null;
    }

    // 해당 레벨과 스테이지 값에 일치하는 레벨과 스테이지의 성공 플래그를 설정한다.
    public void SetLeveStageInfoObjSuccessValue(short level, short stage,bool success)
    {
        for (int i = 0; i < levelStageInfo.Length; i++)
        {
            if (level == levelStageInfo[i].level && stage == levelStageInfo[i].stage)
            {
                levelStageInfo[i].isSuccess = success;
                Debug.Log("LevelStageInfo " + levelStageInfo[i].obj.name + "의 플레그를 : " + levelStageInfo[i].isSuccess + "로 바꿈");
            }
        }
    }

    // 해당 레벨과 스테이지가 성공했는지 안했는지의 플래그를 반환한다.
    public bool GetLevelStageInfoObjSuccessValue(short level, short stage)
    {

        for (int i = 0; i < levelStageInfo.Length; i++)
        {
            if (level == levelStageInfo[i].level && stage == levelStageInfo[i].stage)
            {
                Debug.Log("LevelStageInfo " + levelStageInfo[i].obj.name + "의 플레그를 : " + levelStageInfo[i].isSuccess + "로 바꿈");
                return levelStageInfo[i].isSuccess;
            }
        }

        
        Debug.Log("Level의 개수");
        
        // 0보다 같거나 작고 가지고있는 레벨의 개수보다 크면
        if(level <= 0 || level > GetLevelCount())
        {
            Debug.Log("레벨의 개수 입력이 잘 못 되었습니다. Error");
        }
        // 0보다 같거나 작고 가지고있는 스테이지의 개수보다 크면
        if(stage <= 0 || stage > maxStars)
        {
            Debug.Log("스테이지의 개수 입력이 잘 못 되었습니다. Error");
        }
        return false;
    }

    // 존재하고 있는 레벨의 개수를 반환한다.
    public int GetLevelCount()
    {
        int levelCount = transform.Find("LevelsScreen/Scroll View/Grid/").childCount;
        return levelCount;
    }

    // 레벨과 스테이지의 정보를 세팅한다.
    public void LevelStageInfoSetting()
    {
        // 스테이지의 개수를 가져온다. (자식 오브젝트들의 개수를 구함)
        int childCount = GameObject.Find(Path.gameScreen_Maps).gameObject.transform.childCount; 

        GameObject[] maps = new GameObject[childCount]; 
        maps = GameObject.FindGameObjectsWithTag("Map"); // 존재 하고 있는 스테이지의 GameObject를 가져옴.
        for(int i = 0; i < maps.Length; i++)
        {
            //Debug.Log("name : " + maps[i].name + "level : " + maps[i].GetComponent<MapInfo>().myLevel + " stage : " + maps[i].GetComponent<MapInfo>().myStage);

        }
        levelStageInfo = new LevelStageInfo[maps.Length]; //존재하고 있는 스테이지의 개수만큼 동적으로 할당받는다.
        //Debug.Log("levelStageInfo 할당 개수 : " + levelStageInfo.Length);

        // TODO : PlayPrefabs에서 Player의 currentLevel과 currentStage를 불러와야 함.

        //Test i는 0으로 바꾸셈
        for (int i=0; i < levelStageInfo.Length; i++)
        {
            levelStageInfo[i] = new LevelStageInfo(); //인스턴스 할당

            // 레벨, 스테이지, 오브젝트를 초기화
            levelStageInfo[i].level = maps[i].GetComponent<MapInfo>().myLevel;
            levelStageInfo[i].stage = maps[i].GetComponent<MapInfo>().myStage;
            levelStageInfo[i].obj = maps[i];

            if (levelStageInfo[i].level <= currentLevel && levelStageInfo[i].stage <= currentStage) //현재 레벨보다 작거나 같고 현재 스테이지보다 작거나 같으면
            {
                levelStageInfo[i].isSuccess = true; //성공으로 바꾼다.

            }
            
        }
    }

    //별의 개수를 업데이트한다.
    public void UpdateStars()
    {

        currentHaveStarsTotalNumber = 0; // 현재 모든 레벨에서 깬 별의 개수를 초기화한다.

        // 현재 가지고 있는 각 레벨마다 별의 개수를 0으로 초기화한다.
        for (int i=0; i < GetLevelCount(); i++)
        {
            currentHaveStarsPerLevel[i] = 0; //현재 가지고있는 각 레벨마다 별의 개수를 0으로 초기화한다.
            
        }

        // 각 레벨에 얻은 별의 개수를 가져옴
        for(int i=0; i < GetLevelCount(); i++)
        {
            currentHaveStarsPerLevel[i] = GetLevelSuccessStageNumber((short)(i+1)); // 해당 레벨의 성공한 스테이지의 개수를 반환받음.
        }

        // 현재 모든 레벨에서 깬 별의 개수를 가져옴
        for(int i=0; i < GetLevelCount(); i++)
        {
            currentHaveStarsTotalNumber += currentHaveStarsPerLevel[i];
        }
    }

    // 모든 별의 개수를 가져옴.
    public void InitStarSetting()
    {
        // 각 레벨에 깰 수 있는 별의 개수를 가져옴
        for (int i = 0; i < GetLevelCount(); i++)
        {
            maxStarsPerLevel[i] = GetLevelMaxStageNumber((short)(i + 1)); // 해당 레벨의 모든 스테이지의 개수를 반환받음.
        }

        // 모든 별의 개수를 가져옴
        for (int i = 0; i < GetLevelCount(); i++)
        {
            maxStars += maxStarsPerLevel[i];
        }
        for(int i=0; i< GetLevelCount(); i++)
        {
            currentHaveStarsPerLevel[i] = GetLevelSuccessStageNumber((short)(i + 1)); // 해당 레벨의 성공한 스테이지의 개수를 반환받음.
        }
    }


    // 해당 레벨의 성공한 스테이지의 개수를 반환한다.
    public short GetLevelSuccessStageNumber(short level)
    {
        short count = 0;
        for(int i=0; i < levelStageInfo.Length; i++)
        {
            if(levelStageInfo[i].level == level)
            {
                if(levelStageInfo[i].isSuccess == true)
                {
                    ++count;
                }
            }
        }
        return count;
    }

    // 해당 레벨의 모든 스테이지의 개수를 반환한다.
    public short GetLevelMaxStageNumber(short level)
    {
        short count = 0;
        for(int i=0; i< levelStageInfo.Length; i++)
        {
            if(levelStageInfo[i].level == level)
            {
                ++count;
            }
        }
        return count;
    }

    // 레벨이 언락 될 경우 
    public void LevelUnLockSetting()
    {
        //  현재까지 진행된 레벨과 스테이지를 바꿈
        if(GetLevelCount() >= myProgressLevel+1)
            ++myProgressLevel;
        myProgressStage = 1;
        Debug.Log("myProgressSage가 1로 바뀜 :  " + myProgressStage);
    }
}
