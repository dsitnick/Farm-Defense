using UnityEngine;

public class PlayerTools : MonoBehaviour
{
    [Header("Toolbar Settings")]
    public Tool[] tools = new Tool[9];
    public HandTool handTool;

    [Header("Camera Settings")]
    public Transform cameraRoot;

    public int currentToolIndex { get; private set; } = 0;
    public Tool currentTool { get; private set; }

    void Start()
    {
        // Activate the first tool by default
        if (tools.Length > 0)
        {
            SetActiveTool(0);
        }
    }

    void Update()
    {
        HandleToolbarInput();
        currentTool?.UpdateTool();
    }

    private void HandleToolbarInput()
    {
        // Check for number keys 1-9
        for (int i = 1; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                int toolIndex = i - 1; // Convert to 0-based index
                if (toolIndex < tools.Length)
                {
                    SetActiveTool(toolIndex);
                    break;
                }
            }
        }
    }

    private void SetActiveTool(int toolIndex)
    {
        // Deactivate current tool
        currentTool?.SetActive(false);

        // Set new tool
        currentToolIndex = toolIndex;
        currentTool = tools[toolIndex];

        // If tool is null, use hand tool
        if (currentTool == null)
        {
            currentTool = handTool;
        }

        // Activate new tool
        currentTool?.SetActive(true);
    }
}
