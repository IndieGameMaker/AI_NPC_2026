using UnityEngine;

public class Connection : MonoBehaviour
{
    [Header("Ollama Server Info")]
    [SerializeField] private string _serverURL = "http://localhost:11434";
    [SerializeField] private string _modelName = "gemma4:e2b";
    

}
