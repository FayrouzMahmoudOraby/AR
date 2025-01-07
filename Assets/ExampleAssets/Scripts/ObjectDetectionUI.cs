using UnityEngine;
using UnityEngine.UI;
using Vuforia;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class ObjectDetectionUI : MonoBehaviour
{
    private TcpClient tcpClient;
    private NetworkStream stream;
    private byte[] receivedData = new byte[1024];
    private Thread receiveThread;
    private bool isRunning = true;

    public GameObject menuPanel;
    public Text objectNameText;
    public Button[] taskButtons;

    private readonly string[] cellPhoneTasks = { "Call Contact", "Open Messaging App", "Set Alarm", "Check Notifications" };
    private readonly string[] remoteTasks = { "Change Channel", "Increase Volume", "Mute TV", "Turn Off TV" };
    private readonly string[] keyboardTasks = { "Open Word Processor", "Start Coding", "Search on Browser", "Play Music" };

    void Start()
    {
        try
        {
            tcpClient = new TcpClient("localhost", 8765);
            stream = tcpClient.GetStream();

            receiveThread = new Thread(ReceiveData);
            receiveThread.IsBackground = true;
            receiveThread.Start();

            menuPanel.SetActive(false);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error connecting to server: " + ex.Message);
        }
    }

    void ReceiveData()
    {
        try
        {
            while (isRunning && tcpClient != null && tcpClient.Connected)
            {
                if (stream != null && stream.CanRead)
                {
                    int bytesRead = stream.Read(receivedData, 0, receivedData.Length);
                    if (bytesRead > 0)
                    {
                        string detectedObject = Encoding.ASCII.GetString(receivedData, 0, bytesRead).Trim();
                        MainThreadInvoke(() => UpdateMenu(detectedObject));
                    }
                }
            }
        }
        catch (System.ObjectDisposedException)
        {
            Debug.LogWarning("Stream was disposed. Stopping receive loop.");
        }
        catch (System.Exception ex)
        {
            if (isRunning)
            {
                Debug.LogError("Error receiving data: " + ex.Message);
            }
        }
    }

    void MainThreadInvoke(System.Action action)
    {
        UnityEngine.Debug.Log("Update on main thread");
        action();
    }

    void UpdateMenu(string detectedObject)
    {
        objectNameText.text = detectedObject;

        string[] tasks = GetTasksForObject(detectedObject);
        if (tasks != null)
        {
            for (int i = 0; i < taskButtons.Length; i++)
            {
                if (i < tasks.Length)
                {
                    taskButtons[i].GetComponentInChildren<Text>().text = tasks[i];
                    taskButtons[i].gameObject.SetActive(true);

                    string selectedTask = tasks[i];
                    taskButtons[i].onClick.RemoveAllListeners();
                    taskButtons[i].onClick.AddListener(() => OnTaskSelected(detectedObject, selectedTask));
                }
                else
                {
                    taskButtons[i].gameObject.SetActive(false);
                }
            }
            menuPanel.SetActive(true);
        }
        else
        {
            menuPanel.SetActive(false);
        }
    }

    string[] GetTasksForObject(string detectedObject)
    {
        switch (detectedObject.ToLower())
        {
            case "cell phone": return cellPhoneTasks;
            case "remote": return remoteTasks;
            case "keyboard": return keyboardTasks;
            default: return null;
        }
    }

    void OnTaskSelected(string objectName, string task)
    {
        Debug.Log($"Task '{task}' selected for '{objectName}'!");
        SaveTaskToFile(objectName, task);
    }

    void SaveTaskToFile(string objectName, string task)
    {
        string filePath = Application.dataPath + "/selected_tasks.csv";
        try
        {
            System.IO.File.AppendAllText(filePath, $"{objectName},{task}\n");
            Debug.Log($"Saved task: {objectName}, {task} to {filePath}");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Error saving task to file: " + ex.Message);
        }
    }

    void CleanupConnection()
    {
        isRunning = false;

        if (stream != null)
        {
            stream.Close();
            stream = null;
        }

        if (tcpClient != null)
        {
            tcpClient.Close();
            tcpClient = null;
        }

        if (receiveThread != null && receiveThread.IsAlive)
        {
            receiveThread.Join();
            receiveThread = null;
        }
    }

    void OnDestroy()
    {
        CleanupConnection();
    }

    // Add a method to handle ImageTarget detection
    public void OnDetectedObject(string detectedObject)
    {
        // This function will be called when a Vuforia ImageTarget is detected.
        UpdateMenu(detectedObject);
    }
}
