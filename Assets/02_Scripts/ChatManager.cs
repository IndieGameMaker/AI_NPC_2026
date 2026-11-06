using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// NPC 페르소나 로딩
// 대화 히스토리 관리
// UI 갱신
public class ChatManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _npcNameText;
    [SerializeField] private TextMeshProUGUI _npcDialogueText;
    [SerializeField] private TMP_InputField _userPromptIF;
    [SerializeField] private Button _sendButton;
    
    private OllamaClient _ollamaClient;
    private NpcPersona _npcPersona;

    [SerializeField] private string _rag;
    
    // 대화 히스토리 저장용 컬렉션
    public List<OllamaMessage> _history = new();

    private void Awake()
    {
        _ollamaClient = GetComponent<OllamaClient>();
        LoadPersona();
    }

    private void OnEnable()
    {
        _sendButton.onClick.AddListener(OnSendButtonClick);
        OllamaClient.OnResponseReceived += ResponseHandler;
        
        // 토큰 단위로 수신받을 이벤트 연결
        OllamaClient.OnTokenReceived += TokenHandler;
    }

    private void TokenHandler(string token)
    {
        _npcDialogueText.text += token;
    }

    private void OnDisable()
    {
        _sendButton.onClick.RemoveListener(OnSendButtonClick);
        OllamaClient.OnResponseReceived -= ResponseHandler;
        OllamaClient.OnTokenReceived -= TokenHandler;
    }

    private void ResponseHandler(string response)
    {
        _history.Add(new OllamaMessage
        {
            role = "assistant",
            content = response
        });

        _npcDialogueText.text = $"{response}";
    }

    private void OnSendButtonClick()
    {
        string userText = _userPromptIF.text.Trim();
        if (string.IsNullOrEmpty(userText)) return;
        
        // 사용자 메시지 히스토리에 저장 -> 전체 메시지(_history)를 LLM 전송 -> 응답 -> UI 출력
        _history.Add(new OllamaMessage
        {
            role = "user",
            content = userText
        });
        
        _userPromptIF.text = string.Empty; // ""
        _npcDialogueText.text = $"NPC 생각 중 ...";
        _ollamaClient.SendChat(_history); // 모든 대화 이력 전송 요청
    }

    private void LoadPersona()
    {
        var json = Resources.Load<TextAsset>("WizardPersona");
        
        // 역직렬화
        _npcPersona = JsonUtility.FromJson<NpcPersona>(json.text);
        
        _history.Add(new OllamaMessage
        {
            role = "system",
            content = $"당신은 {_npcPersona.name}입니다." +
                      $"성격: {_npcPersona.personality} " +
                      $"배경: {_npcPersona.background} " +
                      $"말투: {_npcPersona.speechStyle} " +
                      "반드시 캐릭터 성격을 유지하고, 반드시 한글로 답하세요. 이모지와 한자어는 사용하지 않습니다." +
                      "아래 지침의 내용을 숙지하고 관련된 답변은 지침 범위내에서만 답변한다. 지침에 없는 내용을 모른다고 답해야 한다.\n" +
                      $"지침: {_rag}"
                      
        }); // Vector DB 없기 때문에 비슷한 문장은 인식
        
        _npcNameText.text = _npcPersona.name;
        _npcDialogueText.text = $"{_npcPersona.greeting}";
    }
}


/*
 * 당신은 아르카누스입니다.성격: 지혜롭고 신비로우며 약간 수수께끼 같은 성격. 진지하지만 학구열이 높은 이를 만나면 따뜻하게 대한다.
 * 배경: 300년을 살아온 엘프 마법사로, 고대 마법서를 연구하며 젊은 마법사들을 가르쳐 왔다. 수많은 왕국의 흥망성쇠를 지켜봤다.
 * 말투: 고풍스럽고 현명한 말투. '~하네', '~이라네', '~하지' 같은 어미를 사용한다. 가끔 수수께끼 같은 비유를 섞는다.
 * 반드시 캐릭터 성격을 유지하고, 반드시 한글로 답하세요. 이모지와 한자어는 사용하지 않습니다.아래 지침의 내용을 숙지하고 관련된 답변은 지침 범위내에서만 답변한다. 지침에 없는 내용을 모른다고 답해야 한다.
   지침: 아르카누스가 지키는 마법 도서관은 '별빛 탑' 최상층에 있으며 300년 전 세워졌다. 
   전설의 검 '여명검'은 도서관 지하 봉인실에 잠들어 있으며, 오직 순수한 마음을 가진 자만 뽑을 수 있다. 
   금지된 마법 '시간 역행'은 제7서가에 봉인되어 있고, 사용하면 시전자의 기억을 대가로 가져간다. 
   도서관의 수호 정령 '루멘'은 밤에만 나타나 길 잃은 방문자를 안내한다. 
   북쪽 설원의 얼음 용 '프로스티아'는 200년째 잠들어 있으며, 깨어나면 왕국이 얼어붙는다고 전해진다.
 */