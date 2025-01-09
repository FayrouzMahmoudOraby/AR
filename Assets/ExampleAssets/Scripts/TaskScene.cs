//using UnityEngine;
//using UnityEngine.SceneManagement;
//using System.IO;

//public class TaskScene : MonoBehaviour
//{
//    // File path for saving tasks
//    private string filePath;

//    // Temporarily store task data between scenes
//    private static string currentTaskName = "";
//    private static string assignedEngineer = "";

//    void Start()
//    {
//        // Define the main file path
//        filePath = Path.Combine(Directory.GetCurrentDirectory(), "task.txt");

//        // Optionally, log the file path for debugging
//        Debug.Log("File Path: " + filePath);
//    }

//    // Function to navigate to the Engineers Scene
//    public void GoToEngineersScene(string taskName)
//    {
//        // Store the selected task name
//        currentTaskName = taskName;

//        // Load the Engineers Scene
//        SceneManager.LoadScene("Engineers Scene");
//    }

//    // Function to go back to the Main Scene
//    public void GoBackToMainScene()
//    {
//        SceneManager.LoadScene("CRUD Scene");
//    }

//    // Function to go back to the Tasks Scene
//    public void GoToTasksScene()
//    {
//        SceneManager.LoadScene("Tasks Scene");
//    }

//    // Function to assign an engineer's name
//    public void AssignEngineer(string engineerName)
//    {
//        // Store the engineer's name
//        assignedEngineer = engineerName;

//        // Save the task with the assigned engineer
//        SaveTask(currentTaskName, assignedEngineer);

//        // Navigate back to the Tasks Scene
//        SceneManager.LoadScene("Tasks Scene");
//    }

//    // Function to determine the next task number
//    private int GetNextTaskNumber()
//    {
//        int taskNumber = 1;

//        if (File.Exists(filePath))
//        {
//            string[] lines = File.ReadAllLines(filePath);
//            if (lines.Length > 2) // Skipping header and separator lines
//            {
//                string lastLine = lines[lines.Length - 1];
//                string[] parts = lastLine.Split('|');
//                if (parts.Length > 0 && parts[0].Trim().StartsWith("Task"))
//                {
//                    string numberString = parts[0].Replace("Task", "").Trim();
//                    if (int.TryParse(numberString, out int lastNumber))
//                    {
//                        taskNumber = lastNumber + 1;
//                    }
//                }
//            }
//        }

//        return taskNumber;
//    }

//    // Function to save task to both the main file and engineer-specific file
//    private void SaveTask(string taskName, string engineerName)
//    {
//        int taskNumber = GetNextTaskNumber();

//        // If the main task file does not exist, create it with a header
//        if (!File.Exists(filePath))
//        {
//            File.WriteAllText(filePath, $"{"Task Number",-15} | {"Task Name",-15} | {"Engineer",-15} | {"Task Description",-40}\n");
//            File.AppendAllText(filePath, new string('-', 90) + "\n");
//        }

//        // Append the task details to the main file
//        using (StreamWriter writer = new StreamWriter(filePath, true))
//        {
//            writer.WriteLine($"{"Task " + taskNumber,-15} | {taskName,-15} | {engineerName,-15} | {"created task to ENG " + engineerName + " to change " + taskName,-40}");
//        }

//        // Engineer-specific file path
//        string engineerFilePath = Path.Combine(Directory.GetCurrentDirectory(), $"{engineerName}.txt");

//        // If the engineer-specific file does not exist, create it with a header
//        if (!File.Exists(engineerFilePath))
//        {
//            File.WriteAllText(engineerFilePath, $"{"Task Number",-15} | {"Task Name",-15} | {"Engineer",-15} | {"Task Description",-40}\n");
//            File.AppendAllText(engineerFilePath, new string('-', 90) + "\n");
//        }

//        // Append the task details to the engineer-specific file
//        using (StreamWriter writer = new StreamWriter(engineerFilePath, true))
//        {
//            writer.WriteLine($"{"Task " + taskNumber,-15} | {taskName,-15} | {engineerName,-15} | {"created task to ENG " + engineerName + " to change " + taskName,-40}");
//        }

//        Debug.Log($"Task {taskNumber} ('{taskName}') assigned to '{engineerName}' saved successfully.");


//        Debug.Log("Main Task File Path: " + filePath);
//        Debug.Log("Engineer-Specific File Path: " + engineerFilePath);

//    }

//    // Functions for each button to navigate to the Engineers Scene
//    public void SaveCellPhoneTask()
//    {
//        GoToEngineersScene("cell phone");
//    }

//    public void SaveKeyboardTask()
//    {
//        GoToEngineersScene("keyboard");
//    }

//    public void SaveRemoteTask()
//    {
//        GoToEngineersScene("remote");
//    }
//}



using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class TaskScene : MonoBehaviour
{
    // File path for saving tasks
    private string filePath;

    // Temporarily store task data between scenes
    private static string currentTaskName = "";
    private static string assignedEngineer = "";

    void Start()
    {
        // Define the main file path
        filePath = Path.Combine(Directory.GetCurrentDirectory(), "task.txt");

        // Optionally, log the file path for debugging
        Debug.Log("File Path: " + filePath);
    }

    // Function to navigate to the Engineers Scene
    public void GoToEngineersScene(string taskName)
    {
        // Store the selected task name
        currentTaskName = taskName;

        // Load the Engineers Scene
        SceneManager.LoadScene("Engineers Scene");
    }

    // Function to go back to the Main Scene
    public void GoBackToMainScene()
    {
        SceneManager.LoadScene("CRUD Scene");
    }

    // Function to go back to the Tasks Scene
    public void GoToTasksScene()
    {
        SceneManager.LoadScene("Tasks Scene");
    }

    // Function to navigate to the Engineers Scene and display tasks from file
    public void ReadTaskButtonClicked()
    {
        // Ensure the file exists before attempting to open
        if (File.Exists(filePath))
        {
            // Open the file using the default text editor
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
            {
                FileName = filePath,
                UseShellExecute = true // Ensures it opens with the default associated application
            });
        }
        else
        {
            Debug.LogWarning("The file task.txt does not exist.");
        }
    }






    // New Method: Delete the last task from the engineer-specific file
    public void DeleteTaskButtonClicked()
    {
        string engineerFilePath = Path.Combine(Directory.GetCurrentDirectory(), $"{assignedEngineer}.txt");

        if (File.Exists(engineerFilePath))
        {
            string[] lines = File.ReadAllLines(engineerFilePath);

            if (lines.Length > 2) // Ensure there are tasks to delete
            {
                using (StreamWriter writer = new StreamWriter(engineerFilePath))
                {
                    // Write all lines except the last task
                    for (int i = 0; i < lines.Length - 1; i++)
                    {
                        writer.WriteLine(lines[i]);
                    }
                }
                Debug.Log("Last task deleted from engineer-specific file: " + engineerFilePath);
            }
            else
            {
                Debug.LogWarning("No tasks available to delete in the engineer file.");
            }
        }
        else
        {
            Debug.LogWarning("The engineer file does not exist: " + engineerFilePath);
        }
    }




    // Function to assign an engineer's name
    public void AssignEngineer(string engineerName)
    {
        // Store the engineer's name
        assignedEngineer = engineerName;

        // Save the task with the assigned engineer
        SaveTask(currentTaskName, assignedEngineer);

        // Navigate back to the Tasks Scene
        SceneManager.LoadScene("Tasks Scene");
    }

    // Function to determine the next task number
    private int GetNextTaskNumber()
    {
        int taskNumber = 1;

        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            if (lines.Length > 2) // Skipping header and separator lines
            {
                string lastLine = lines[lines.Length - 1];
                string[] parts = lastLine.Split('|');
                if (parts.Length > 0 && parts[0].Trim().StartsWith("Task"))
                {
                    string numberString = parts[0].Replace("Task", "").Trim();
                    if (int.TryParse(numberString, out int lastNumber))
                    {
                        taskNumber = lastNumber + 1;
                    }
                }
            }
        }

        return taskNumber;
    }

    // Function to save task to both the main file and engineer-specific file
    private void SaveTask(string taskName, string engineerName)
    {
        int taskNumber = GetNextTaskNumber();

        // If the main task file does not exist, create it with a header
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, $"{"Task Number",-15} | {"Task Name",-15} | {"Engineer",-15} | {"Task Description",-40}\n");
            File.AppendAllText(filePath, new string('-', 90) + "\n");
        }

        // Append the task details to the main file
        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            writer.WriteLine($"{"Task " + taskNumber,-15} | {taskName,-15} | {engineerName,-15} | {"created task to ENG " + engineerName + " to change " + taskName,-40}");
        }

        // Engineer-specific file path
        string engineerFilePath = Path.Combine(Directory.GetCurrentDirectory(), $"{engineerName}.txt");

        // If the engineer-specific file does not exist, create it with a header
        if (!File.Exists(engineerFilePath))
        {
            File.WriteAllText(engineerFilePath, $"{"Task Number",-15} | {"Task Name",-15} | {"Engineer",-15} | {"Task Description",-40}\n");
            File.AppendAllText(engineerFilePath, new string('-', 90) + "\n");
        }

        // Append the task details to the engineer-specific file
        using (StreamWriter writer = new StreamWriter(engineerFilePath, true))
        {
            writer.WriteLine($"{"Task " + taskNumber,-15} | {taskName,-15} | {engineerName,-15} | {"created task to ENG " + engineerName + " to change " + taskName,-40}");
        }

        Debug.Log($"Task {taskNumber} ('{taskName}') assigned to '{engineerName}' saved successfully.");


        Debug.Log("Main Task File Path: " + filePath);
        Debug.Log("Engineer-Specific File Path: " + engineerFilePath);

    }

    // Functions for each button to navigate to the Engineers Scene
    public void SaveCellPhoneTask()
    {
        GoToEngineersScene("cell phone");
    }

    public void SaveKeyboardTask()
    {
        GoToEngineersScene("keyboard");
    }

    public void SaveRemoteTask()
    {
        GoToEngineersScene("remote");
    }
}