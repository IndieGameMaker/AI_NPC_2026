using System;
using UnityEngine;

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
}
