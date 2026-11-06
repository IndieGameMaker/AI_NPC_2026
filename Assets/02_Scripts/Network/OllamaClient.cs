using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

// REST API 사용해서 Ollama Server 와 통신하는 클래스
// 앤드포인트(End-Point) => POST http://localhost:11434/api/chat
// API - Key
public class OllamaClient : MonoBehaviour
{
    // 앤드 포인트
    private const string API_URL = "http://localhost:11434/api/chat";
    // LLM 모델
    private const string MODEL = "gemma4:e2b";
    
    // 이벤트 선언
    public static event Action<string> OnResponseReceived;
    public static event Action<bool> OnLoadingChanged;
    
    // 요청 레퍼 메서드
    public void SendChat(List<OllamaMessage> messages)
    {
        // Request 호출
        StartCoroutine(PostRequest(messages));
    }
    
    // REST API 메서드
    public IEnumerator PostRequest(List<OllamaMessage> messages)
    {
        OnLoadingChanged?.Invoke(true); // UI 변경 "생각 중 ..."
        
        // 직렬화 : 객체 => JSON
        string json = JsonUtility.ToJson(new OllamaRequest
        {
            model = MODEL,
            messages = messages,
            stream = false
        });
        
        // UnityWebRequest
        // using
        using UnityWebRequest request = new UnityWebRequest(API_URL, "POST");
        
        request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(json));
        request.downloadHandler = new DownloadHandlerBuffer();
        request.timeout = 60;
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Debug.Log(request.downloadHandler.text);
            
            // JSON 파싱 실제 message.content 부분을 추출
            var response = JsonUtility.FromJson<OllamaResponse>(request.downloadHandler.text);
            
            // UI에 출력하기위한 이벤트 Raise
            OnResponseReceived?.Invoke(response.message.content);
        }
        else
        {
            Debug.Log(request.error);
        }
        
        OnLoadingChanged?.Invoke(false); // UI 변경
    }
}

/* NDJson (Newline Delimiter JSON)
 * - stream = true
 * - 
 * JSON\n
 * JSON\n
 * JSON\n
 *
 * 안녕하\n
 * 세요. 저는
 *  잭이라고\n
 */
