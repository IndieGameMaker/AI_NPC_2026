using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(OllamaClient))]
public class OllamaClientEditor : Editor
{
    private string _systemMessage = "마법 왕국의 현자이자, 마법 도서관이 수호자이자 마법사입니다.";
    private string _userMessage = "안녕하세요. 저는 방랑 검객, 잭이라고 합니다.";

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