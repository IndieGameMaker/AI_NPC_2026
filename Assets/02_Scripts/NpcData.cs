using System;
using System.Collections.Generic;

[Serializable]
public class NpcPersona
{
   public string name;
   public string role;
   public string personality;
   public string background;
   public string speechStyle;
   public string greeting;
}

[Serializable]
public class OllamaMessage
{
   public string role;
   public string content;
}

[Serializable]
public class OllamaRequest
{
   public string model; // 사용할 모델
   public List<OllamaMessage> messages; // 이전 대화 목록을 리스트에서 관리/ 모두 전송
   public bool stream; 
}

[Serializable]
public class OllamaResponse
{
   public OllamaMessage message;
}

/* Local LLM , OpenAI, Claud API
 [
    {"role": "system", "content":"너는 마법사야."},
    {"role": "user", "content": "혹시, 전설의 검에 대해서 알고있나요?"},
    {"role": "assistant", "content": "아무한테 알려줄수 없다네."},
 ]
 */