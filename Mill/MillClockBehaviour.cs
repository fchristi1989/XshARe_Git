using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MillClockBehaviour : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCLick(Button b)
    {
        string task = b.GetComponentInChildren<Text>().text;
        string entry = Time.time + " " + task + "\n";

        //InputField field = FindObjectOfType(typeof(InputField)) as InputField;

        GameObject log = GameObject.Find("Log");
        log.GetComponentInChildren<Text>().text += entry;
    }
}
