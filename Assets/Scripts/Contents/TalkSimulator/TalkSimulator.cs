using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class TalkSimulator : Singleton<TalkSimulator>
{
    [SerializeField]
    private ScenarioData scenarioData;

    [SerializeField]
    private int currentIndex = 0;

    public float textDisplayTimePerCharacter = 0.1f;

    [ShowInInspector]
    [ReadOnly]
    private TalkElementData currentTalkData;

    [ShowInInspector]
    [ReadOnly]
    private UITalkCharacter currentTalkCharacter;

    [SerializeField]
    private UITalkCharacter[] talkCharacterSlots;
    
    [ShowInInspector]
    [ReadOnly]
    private SerializableDictionary<string, UITalkCharacter> cachingCharacterSlotDic = new SerializableDictionary<string, UITalkCharacter>();

    [SerializeField]
    private UIBaseText characterNameText;
    [SerializeField]
    private UIBaseText talkText;

    public UnityEvent startScenarioEvnet;
    public UnityEvent endScenarioEvnet;

    public UnityEvent nextTalkEvent;
    public UnityEvent startTalkEvent;
    public UnityEvent endTalkEvent;

    private Coroutine textDisplayAnimation;

    [SerializeField]
    private bool isAllowNextTalk = false;

    [SerializeField]
    private bool isSkipHold = false;

    [SerializeField]
    private float skipHoldTime = 2f;
    private float currentSkipHoldTime = 0f;

    [SerializeField]
    private GameObject nextButton;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {

    }

    [Button("시나리오 테스트")]
    public void StartScenario(ScenarioData scenarioData)
    {
        this.scenarioData = scenarioData;

        currentIndex = -1;
        isAllowNextTalk = true;
        startScenarioEvnet?.Invoke();
        NextTalk();
    }

    public void EndScenario()
    {
        endScenarioEvnet?.Invoke();
    }

    public void NextTalk()
    {
        if (!isAllowNextTalk || Time.timeScale == 0f)
            return;

        nextTalkEvent?.Invoke();

        
        if (currentIndex < scenarioData.TalkElementDataList.Count - 1)
        {
            ++currentIndex;
            currentTalkData = scenarioData.GetTalkElementData(currentIndex);

            StartTalk();
        }
        else
        {
            EndScenario();
        }
    }

    public void StartTalk()
    {
        nextButton.gameObject.SetActive(false);
        isAllowNextTalk = false;
        startTalkEvent?.Invoke();

        if (textDisplayAnimation != null)
        {
            StopCoroutine(textDisplayAnimation);
        }

        AddCharacter(currentTalkData);
        characterNameText.SetText(currentTalkData.CharacterName);

        AutoHighlight(currentTalkData.CharacterName);

        textDisplayAnimation = StartCoroutine(CoTextDisplayAnimation());
    }

    public void EndTalk()
    {
        isAllowNextTalk = true;
        nextButton.gameObject.SetActive(true);
        endTalkEvent?.Invoke();
    }

    public void SkipTextDisplayAnimation()
    {
        if (textDisplayAnimation != null)
        {
            StopCoroutine(textDisplayAnimation);
        }

        talkText.SetText(currentTalkData.Talk);
    }

    public void SkipScenario()
    {
        EndScenario();
    }

    public bool ContainCharacter(string characterName)
    {
        return cachingCharacterSlotDic.ContainsKey(characterName);
    }

    private void AddCharacter(TalkElementData talkData)
    {
        UITalkCharacter slot = null;

        switch (talkData.CharacterStandAnchor)
        {
            case TalkElementData.CharacterStandAnchorType.Left:
                slot = talkCharacterSlots[0];
                break;
            case TalkElementData.CharacterStandAnchorType.Right:
                slot = talkCharacterSlots[1];
                break;
        }

        if (cachingCharacterSlotDic.ContainsKey(talkData.CharacterName))
        {
            cachingCharacterSlotDic[talkData.CharacterName] = slot;
        }
        else
        {
            cachingCharacterSlotDic.Add(talkData.CharacterName, slot);
        }

        slot.ChangeCharacter(talkData.CharacterName, talkData.Character);
        slot.gameObject.SetActive(true);

    }

    private void RemoveCharacter(string characterName)
    {
        if (cachingCharacterSlotDic.ContainsKey(characterName))
        {
            var slot = cachingCharacterSlotDic[characterName];
            slot.gameObject.SetActive(false);

            cachingCharacterSlotDic.Remove(characterName);
        }
    }

    public void AutoHighlight(string name)
    {
        for (var i = 0; i < talkCharacterSlots.Length; ++i)
        {
            if (talkCharacterSlots[i].GetCharacterName().Equals(name))
            {
                talkCharacterSlots[i].Show();
            }
            else
            {
                talkCharacterSlots[i].Hide();
            }
        }
    }


    IEnumerator CoTextDisplayAnimation()
    {
        var waitForTime = new WaitForSeconds(textDisplayTimePerCharacter);
        var currentTextIndex = 0;

        var reaplaceTalk = currentTalkData.Talk.Replace("\r","");
        StringBuilder stBuilder = new StringBuilder();

        var isSkipCharacter = false;

        while (reaplaceTalk.Length != currentTextIndex)
        {
            if (reaplaceTalk[currentTextIndex] == '<')
            {
                stBuilder.Append(reaplaceTalk[currentTextIndex]);
                isSkipCharacter = true;
                ++currentTextIndex;
                continue;
            }

            if (reaplaceTalk[currentTextIndex] == '>')
            {
                stBuilder.Append(reaplaceTalk[currentTextIndex]);
                isSkipCharacter = false;
                ++currentTextIndex;
                continue;
            }

            if (isSkipCharacter)
            {
                stBuilder.Append(reaplaceTalk[currentTextIndex]);
                ++currentTextIndex;
                continue;
            }

            stBuilder.Append(reaplaceTalk[currentTextIndex]);
            ++currentTextIndex;

            talkText.SetText(stBuilder.ToString());
            yield return waitForTime;
        }

        EndTalk();
    }

    private void StartSkipHold()
    {
        currentSkipHoldTime = 0f;
        isSkipHold = true;
    }

    private void Update()
    {
        if (!isSkipHold)
            return;

        currentSkipHoldTime += Time.deltaTime;
        if (currentSkipHoldTime >= skipHoldTime)
        {
            isSkipHold = false;
            currentSkipHoldTime = 0f;
            SkipScenario();
        }
    }

    private void OnDisable()
    {

    }

}
