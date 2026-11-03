using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Connection : MonoBehaviour
{
    [Header("Ollama Server Info")]
    [SerializeField] private string _serverURL = "http://localhost:11434";
    [SerializeField] private string _modelName = "gemma4:e2b";
    [SerializeField] private string _context = "안녕하세요. 당신을 소개해주세요.";

    // RESTApi
    public IEnumerator OllamaConnect()
    {
        string url = $"{_serverURL}/api/chat";

        string json =
            $@"{{
                  ""model"": ""{_modelName}"",
                  ""messages"": [
                    {{
                      ""role"": ""user"",
                      ""content"": ""{_context}""
                    }}
                  ],
                  ""stream"": true
                }}";
        
        // HTTP 통신 규약
        // POST 방식 : 보안성 있고, 파라메터의 길의 제약이 없다.
        // GET 방식 : 보안 X   http://www.naver.com?cafe_id=1234&user_id=zack
        UnityWebRequest request = UnityWebRequest.Post(url, json, "application/json");
        Debug.Log($"[접속] Ollama 서버에 요청 전송 중 ...");
        
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log($"[응답] 연결 성공!\n{request.downloadHandler.text}");
        }
        else
        {
            Debug.Log($"[연결 실패] {request.error}");
        }

    }
}
