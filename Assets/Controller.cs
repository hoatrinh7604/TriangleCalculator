using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class Controller : MonoBehaviour
{
    [SerializeField] GameObject inputField;
    [SerializeField] Button addButton;
    [SerializeField] Button continueButton;

    [SerializeField] TMP_InputField[] listInputAngle;
    [SerializeField] TMP_InputField[] listInputSide;



    [SerializeField] GameObject[] listItemResults;

    [SerializeField] TextMeshProUGUI SN;
    [SerializeField] TextMeshProUGUI EN;
    [SerializeField] TextMeshProUGUI EngiN;
    [SerializeField] TextMeshProUGUI RN;

    [SerializeField] GameObject listFieldParent;

    [SerializeField] GameObject numberPrefab;
    [SerializeField] GameObject GCFObj;

    [SerializeField] Button calButton;

    private List<GameObject> list = new List<GameObject>();
    private List<double> listInt = new List<double>();
    private int amount = 0;

    public string CURRENCY_FORMAT = "#,##0.00";
    public NumberFormatInfo NFI = new NumberFormatInfo { NumberDecimalSeparator = ",", NumberGroupSeparator = "." };

    private int type = 1;

    [SerializeField] Color[] listColor;

    //Singleton
    public static Controller Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    private void Start()
    {
        Clear();
    }

    private void DropdownitemSelected()
    {
        
    }

    private void DropdownitemAgeSelected()
    {
        
    }


    public void OnValueChanged()
    {
        if(CheckValidate())
        {
            calButton.interactable = true;
        }
        else
        {
            calButton.interactable= false;
        }
    }

    List<int> listNumbers = new List<int>();
    private bool CheckValidate()
    {
        int countAngle = 0;
        int countSide = 0;
        for (int i = 0; i < listInputAngle.Length; i++)
        {
            if (listInputAngle[i].text != "")
            {
                countAngle++;
            }

            if (listInputSide[i].text != "")
            {
                countSide++;
            }
        }

        if(countSide == 3 && countAngle == 0)
            return true;

        if(countAngle + countSide == 3 && countSide > 0 && countAngle > 0)
            return true;

        return false;
    }


    public void Sum()
    {
        knownAngle.Clear();
        knownSide.Clear();
        CalWithAdult();
        //listFieldParent.SetActive(true);
    }

    private void CalWithAdult()
    {
        List<float> listAngle = new List<float>();
        for(int i = 0; i < listInputAngle.Length; i++)
        {
            if (listInputAngle[i].text != "")
            {
                listAngle.Add(float.Parse(listInputAngle[i].text));
                knownAngle.Add(i);
            }
            else
            {
                listAngle.Add(0);
            }
        }

        List<float> listSide = new List<float>();
        for (int i = 0; i < listInputSide.Length; i++)
        {
            if (listInputSide[i].text != "")
            {
                listSide.Add(float.Parse(listInputSide[i].text));
                knownSide.Add(i);
            }
            else
            {
                listSide.Add(0);
            }
        }

        if(knownSide.Count == 3)
        {
            listAngle[0] = Mathf.Acos((listSide[1] * listSide[1] + listSide[2] * listSide[2] - listSide[0] * listSide[0]) / (2 * listSide[1] * listSide[2])) * 180 / Mathf.PI;
            listAngle[1] = Mathf.Acos((listSide[0] * listSide[0] + listSide[2] * listSide[2] - listSide[1] * listSide[1]) / (2 * listSide[0] * listSide[2])) * 180 / Mathf.PI;
            listAngle[2] = Mathf.Acos((listSide[1] * listSide[1] + listSide[0] * listSide[0] - listSide[2] * listSide[2]) / (2 * listSide[1] * listSide[0])) * 180 / Mathf.PI;

            UpdateResult(listAngle, listSide);
            return;
        }

        // Known 1 angle
        if(knownAngle.Count == 1)//A
        {
            // known opposite
            int index = knownSide.IndexOf(knownAngle[0]);//a
            if (index >= 0)
            {
                // Find an angle has opposite
                for(int i = 0; i < knownSide.Count; i++)
                {
                    if (index == i) continue;
                    float sinAnge = Mathf.Sin((listAngle[knownAngle[0]] * Mathf.PI)/180) / listSide[knownSide[index]] * listSide[knownSide[i]];
                    listAngle[knownSide[i]] = Mathf.Asin(sinAnge) * 180 / Mathf.PI;
                }

                listAngle[3 - knownSide[0] - knownSide[1]] = 180 - listAngle[knownSide[0]] - listAngle[knownSide[1]];

                listSide[3 - knownSide[0] - knownSide[1]] = listSide[knownAngle[0]] * Mathf.Sin((listAngle[3 - knownSide[0] - knownSide[1]] * Mathf.PI) / 180) / Mathf.Sin((listAngle[knownAngle[0]] * Mathf.PI) / 180);
            }
            else
            {
                // Find side of known angle
                listSide[knownAngle[0]] = Mathf.Sqrt((listSide[knownSide[0]] * listSide[knownSide[0]] + listSide[knownSide[1]] * listSide[knownSide[1]] - 2 * listSide[knownSide[0]] * listSide[knownSide[1]] * Mathf.Cos((listAngle[knownAngle[0]] * Mathf.PI) / 180)));

                if (knownAngle[0] == 0)
                {
                    listAngle[1] = Mathf.Acos((listSide[0] * listSide[0] + listSide[2] * listSide[2] - listSide[1] * listSide[1]) / (2 * listSide[0] * listSide[2])) * 180 / Mathf.PI;
                    listAngle[2] = Mathf.Acos((listSide[1] * listSide[1] + listSide[0] * listSide[0] - listSide[2] * listSide[2]) / (2 * listSide[1] * listSide[0])) * 180 / Mathf.PI;
                }
                else if(knownAngle[0] == 1)
                {
                    listAngle[0] = Mathf.Acos((listSide[1] * listSide[1] + listSide[2] * listSide[2] - listSide[0] * listSide[0]) / (2 * listSide[1] * listSide[2])) * 180 / Mathf.PI;
                    listAngle[2] = Mathf.Acos((listSide[1] * listSide[1] + listSide[0] * listSide[0] - listSide[2] * listSide[2]) / (2 * listSide[1] * listSide[0])) * 180 / Mathf.PI;
                }else
                {
                    listAngle[0] = Mathf.Acos((listSide[1] * listSide[1] + listSide[2] * listSide[2] - listSide[0] * listSide[0]) / (2 * listSide[1] * listSide[2])) * 180 / Mathf.PI;
                    listAngle[1] = Mathf.Acos((listSide[0] * listSide[0] + listSide[2] * listSide[2] - listSide[1] * listSide[1]) / (2 * listSide[0] * listSide[2])) * 180 / Mathf.PI;
                }
            }

            
        }
        else if(knownAngle.Count == 2)
        {
            listAngle[3 - knownAngle[0] - knownAngle[1]] = 180 - listAngle[knownAngle[0]] - listAngle[knownAngle[1]];

            // Find the angle had opposite
            int index = knownSide[0];

            for(int i = 0; i < listSide.Count; i++)
            {
                if (i == index) continue;
                listSide[i] = listSide[index] * Mathf.Sin((listAngle[i] * Mathf.PI)/180) / Mathf.Sin((listAngle[index]* Mathf.PI)/180);
            }
        }

        UpdateResult(listAngle, listSide);
    }

    List<int> knownSide = new List<int>();
    List<int> knownAngle = new List<int>();

    void UpdateResult(List<float> angle, List<float> side)
    {
        for(int i = 0; i< angle.Count; i++)
        {
            listInputAngle[i].text = angle[i].ToString("0.00");
            listInputSide[i].text = side[i].ToString("0.00");
        }
    }

    public bool CalcIsPrime(int number)
    {

        if (number == 1) return false;
        if (number == 2) return true;

        if (number % 2 == 0) return false; // Even number     

        for (int i = 2; i < number; i++)
        { // Advance from two to include correct calculation for '4'
            if (number % i == 0) return false;
        }

        return true;

    }

    public void Continue()
    {
        if(amount==0) return;
        Clear();
    }

    public void Clear()
    {
        listFieldParent.SetActive(false);

        for(int i = 0; i < listInputAngle.Length; i++)
        {
            listInputAngle[i].text = "";
            listInputSide[i].text = "";
        }

        knownAngle.Clear();
        knownSide.Clear();
        calButton.interactable = false;
    }

    public void Quit()
    {
        Clear();
        Application.Quit();
    }
}
