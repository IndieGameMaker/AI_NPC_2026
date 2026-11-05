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
    }

    private void OnDisable()
    {
        _sendButton.onClick.RemoveListener(OnSendButtonClick);
        OllamaClient.OnResponseReceived -= ResponseHandler;
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
                      "반드시 캐릭터 성격을 유지하고, 한글로 답하세요."
        });
        
        _npcNameText.text = _npcPersona.name;
        _npcDialogueText.text = $"{_npcPersona.greeting}";
    }
}
