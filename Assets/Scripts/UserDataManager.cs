using System.Collections.Generic;
using UnityEngine;


public class UserDataManager : MonoBehaviour
{
    // Singleton instance
    public static UserDataManager Instance { get; private set; }

    // User input data
    public string SelectedMonth { get; set; }
    public string SelectedDay { get; set; }
    public string SelectedYear { get; set; }
    public string SelectedGender { get; set; }
    public string Q3SelectedOption { get; set; }
    public string Q4SelectedOption { get; set; }
    public string Q5SelectedOption { get; set; }
    public string Q6SelectedOption { get; set; }
    public string Q7SelectedOption { get; set; }
    public string Q8SelectedOption { get; set; }
    public string Q9SelectedOption { get; set; }
    public string Q11SelectedOption { get; set; }
    public string Q12SelectedOption { get; set; }
    public string Q13SelectedOption { get; set; }
    public string Q14SelectedOption { get; set; }
    public string Q15SelectedOption { get; set; }
    public string Q16SelectedOption { get; set; }
    public string Q17SelectedOption { get; set; }
    public string Q18SelectedOption { get; set; }
    public string Q19SelectedOption { get; set; }
    public string Q20SelectedOption { get; set; }
    public string Q21SelectedOption { get; set; }
    public string Q22SelectedOption { get; set; }
    public string Q23SelectedOption { get; set; }
    public string Q24SelectedOption { get; set; }
    public string Q25SelectedOption { get; set; }
    public string Q26SelectedOption { get; set; }
    public string Q27SelectedOption { get; set; }
    public string Q28SelectedOption { get; set; }
    public string Q29SelectedOption { get; set; }
    public string Q30SelectedOption { get; set; }



    private void Awake()
    {
        // Ensure there's only one instance of this class
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make this persist across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method to log all data to the console
 
    public void LogData()
    {
        Debug.Log($"DOB: {SelectedMonth} {SelectedDay}, {SelectedYear}");
        
        // Add logs for other questions as needed   
    }
    public void LogDataQ2()
    {
        Debug.Log("Hi,I'm from user data manager");
        Debug.Log($"Gender: {SelectedGender}");
    }
    public void LogDataQ3()
    {
        Debug.Log("Hi,I'm from user data manager Q3");
        Debug.Log($"Q3: {Q3SelectedOption}");
    }
    public void LogDataQ4()
    {
        Debug.Log("Hi,I'm from user data manager Q4");
        Debug.Log($"Q4: {Q4SelectedOption}");
    }
    public void LogDataQ5()
    {
        Debug.Log("Hi,I'm from user data manager Q5");
        Debug.Log($"Q5: {Q5SelectedOption}");
    }
    public void LogDataQ6()
    {
        Debug.Log("Hi,I'm from user data manager Q6");
        Debug.Log($"Q6: {Q6SelectedOption}");
    }
    public void LogDataQ7()
    {
        Debug.Log("Hi,I'm from user data manager Q7");
        Debug.Log($"Q7: {Q7SelectedOption}");
    }
    public void LogDataQ8()
    {
        Debug.Log("Hi,I'm from user data manager Q8");
        Debug.Log($"Q8: {Q8SelectedOption}");
    }
    public void LogDataQ9()
    {
        Debug.Log("Hi,I'm from user data manager Q9");
        Debug.Log($"Q9: {Q9SelectedOption}");
    }
    public void LogDataQ11()
    {
        Debug.Log("Hi,I'm from user data manager Q11");
        Debug.Log($"Q11: {Q11SelectedOption}");
    }
    public void LogDataQ12()
    {
        Debug.Log("Hi,I'm from user data manager Q12");
        Debug.Log($"Q12: {Q12SelectedOption}");
    }
    public void LogDataQ13()
    {
        Debug.Log("Hi,I'm from user data manager Q13");
        Debug.Log($"Q13: {Q13SelectedOption}");
    }
    public void LogDataQ14()
    {
        Debug.Log("Hi,I'm from user data manager Q14");
        Debug.Log($"Q14: {Q14SelectedOption}");
    }
    public void LogDataQ15()
    {
        Debug.Log("Hi,I'm from user data manager Q15");
        Debug.Log($"Q15: {Q15SelectedOption}");
    }
    public void LogDataQ16()
    {
        Debug.Log("Hi,I'm from user data manager Q16");
        Debug.Log($"Q16: {Q16SelectedOption}");
    }
    public void LogDataQ17()
    {
        Debug.Log("Hi,I'm from user data manager Q17");
        Debug.Log($"Q17: {Q17SelectedOption}");
    }
    public void LogDataQ18()
    {
        Debug.Log("Hi,I'm from user data manager Q18");
        Debug.Log($"Q18: {Q18SelectedOption}");
    }
    public void LogDataQ19()
    {
        Debug.Log("Hi,I'm from user data manager Q19");
        Debug.Log($"Q19: {Q19SelectedOption}");
    }
    public void LogDataQ20()
    {
        Debug.Log("Hi,I'm from user data manager Q20");
        Debug.Log($"Q20: {Q20SelectedOption}");
    }
    public void LogDataQ21()
    {
        Debug.Log("Hi,I'm from user data manager Q21");
        Debug.Log($"Q21: {Q21SelectedOption}");
    }

    public void LogDataQ22()
    {
        Debug.Log("Hi,I'm from user data manager Q22");
        Debug.Log($"Q22: {Q22SelectedOption}");
    }
    public void LogDataQ23()
    {
        Debug.Log("Hi,I'm from user data manager Q23");
        Debug.Log($"Q23: {Q23SelectedOption}");
    }
    public void LogDataQ24()
    {
        Debug.Log("Hi,I'm from user data manager Q24");
        Debug.Log($"Q24: {Q24SelectedOption}");
    }
    public void LogDataQ25()
    {
        Debug.Log("Hi,I'm from user data manager Q25");
        Debug.Log($"Q25: {Q25SelectedOption}");
    }
    public void LogDataQ26()
    {
        Debug.Log("Hi,I'm from user data manager Q26");
        Debug.Log($"Q26: {Q26SelectedOption}");
    }
    public void LogDataQ27()
    {
        Debug.Log("Hi,I'm from user data manager Q27");
        Debug.Log($"Q27: {Q27SelectedOption}");
    }
    public void LogDataQ28()
    {
        Debug.Log("Hi,I'm from user data manager Q28");
        Debug.Log($"Q28: {Q28SelectedOption}");
    }
    public void LogDataQ29()
    {
        Debug.Log("Hi,I'm from user data manager Q29");
        Debug.Log($"Q29: {Q29SelectedOption}");
    }

}
