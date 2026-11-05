using System;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class NDJsonDownloadHandler : DownloadHandlerScript
{
    // 토큰 하나가 파싱될 때 마다 호출할 콜백
    private readonly Action<string> _onToken;
    
    // \n 끝나지 않은 불완전 JSON 청크(Chunk)을 임시로 저장할 버퍼
    private readonly StringBuilder _lineBuffer = new();
    
    // 스트리밍 처음부터 마지막까지 모든 토큰을 순서대로 이어 붙이는 버퍼
    private readonly StringBuilder _fullBuffer = new();
    
    public string FullBuffer => _fullBuffer.ToString();
    
    // 생성자 : 토큰 수신 콜백(호출)할 이벤트를 매개변수로 전달 받아서 연결
    public NDJsonDownloadHandler(Action<string> onToken)
    {
        _onToken = onToken;
    }

    // UnityWebRequest 내부에서 새로운 데이터 청크를 수신할 때 마다 자동으로 호출되는 콜백
    protected override bool ReceiveData(byte[] data, int dataLength)
    {
        string buffer = Encoding.UTF8.GetString(data, 0, dataLength);
        // 임시 버퍼에 신규 청크를 추가
        _lineBuffer.Append(buffer);
        string[] lines = _lineBuffer.ToString().Split('\n');
        
        _lineBuffer.Clear();
        // 불완전한 마지막 청크(\n X)를 
        _lineBuffer.Append(lines[lines.Length - 1]);

        foreach (var line in lines[..^1]) // 마지막 한 원소를 제외한 모든 라인을 순회
        {
            
        }

        return true;
    }
    
}
