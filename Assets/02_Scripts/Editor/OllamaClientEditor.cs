using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

[CustomEditor(typeof(OllamaClient))]
public class OllamaClientEditor : Editor
{
    // Ollama 설치 모델 목록 조회
    private const string TAGS_URL = "http://localhost:11434/api/tags";
    
    private string _systemMessage = "마법 왕국의 현자이자, 마법 도서관이 수호자이자 마법사입니다.";
    private string _userMessage = "안녕하세요. 저는 방랑 검객, 잭이라고 합니다.";

    private List<string> _modelNames = new();
    private int _selectedIndex;

    private void OnEnable() => FetchModels();

    private void FetchModels()
    {
        var request = UnityWebRequest.Get(TAGS_URL);
        request.SendWebRequest().completed += _ =>
        {
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(request.downloadHandler.text);
            }
        };
    }
    
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        var client = (OllamaClient)target;
        _systemMessage = EditorGUILayout.TextField("페르소나(system)", _systemMessage);
        _userMessage = EditorGUILayout.TextField("프롬프트(user)", _userMessage);

        if (GUILayout.Button("응답 요청 보내기"))
        {
            var messages = new List<OllamaMessage>
            {
                new OllamaMessage{role="system", content = _systemMessage},
                new OllamaMessage{role="user", content = _userMessage}
            };

            client.SendChat(messages);
        }
    }
}