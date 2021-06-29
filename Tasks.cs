using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tasks : MonoBehaviour
{
    [SerializeField] public List<Sprite> _letters;
    [SerializeField] public List<Sprite> _numbers;
    public List<Sprite> _choosedList;
    public int[] _randomedAnswers;
    public List<char> _lettersNames = new List<char>();
    public List<char> _numbersNames = new List<char>();
    public List<char> _choosedNames;

    public Sprite _choosedSprite;
    public char _choosedChar;
    public int _objectsNum;
    public int _right;
    public int _lvl;
    public bool _firstBank;
    public bool _restart;

    public int[,] _rememberRights = new int[2,3];

    private void Start()
    {
        char[] alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
        _lettersNames.AddRange(alphabet);
        char[] numbers = "123456789".ToCharArray();
        _numbersNames.AddRange(numbers);
        NewGame();
    }

    public void NewGame()
    {
        _lvl = 1;
        for (int i = 0; i < 3; i++)
        {
            _rememberRights[0, i] = -1;
            _rememberRights[1, i] = -1;
        }
    }

    public void ChooseBank()
    {
        _objectsNum = 3 * _lvl;
        if (Random.Range(0, 2) == 0)
        {
            _choosedList = _letters;
            _choosedNames = _lettersNames;
            _firstBank = true;
        }
        else
        {
            _choosedList = _numbers;
            _choosedNames = _numbersNames;
            _firstBank = false;
        }
        _randomedAnswers = new int[_choosedList.Count];
        SelectAnswers();
        SelectTask();
    }

    public void SelectAnswers()
    {
        List<int> allNumbers = new List<int>();
        for (int i = 0; i < _choosedList.Count;i++)
            allNumbers.Add(i);
        for (int i = 0; i < _objectsNum; i++)
        {            
            int index = Random.Range(0, allNumbers.Count - 1);
            _randomedAnswers[i] = allNumbers[index];
            allNumbers.RemoveAt(index);
        }        
    }

    public void SelectTask()
    {
        int index = 0;
        do
        {
            index = _randomedAnswers[Random.Range(0, _objectsNum)];
        }
        while (CheckUsed(index));
        _choosedChar = _choosedNames[index];
        _choosedSprite = _choosedList[index];
        _right = index;
        AddUsed(index);
    }

    public bool CheckUsed(int comparier)
    {
        int row = 0;
        bool compare = false;
        if (!_firstBank)
        {
            row = 1;
        }
        for (int i = 0; i < 3; i++)
        {
            Debug.Log($"used {_rememberRights[row, i]}");
            Debug.Log($"compare with {comparier}");
            if (_rememberRights[row, i] == comparier)
            {
                compare = true;
                break;             
            }
            else if (_rememberRights[row, i] < 0)
            {
                break;
            }
        }
        return compare;
    }

    public void AddUsed(int index)
    {
        int row = 0;
        if (!_firstBank)
        {
            row = 1;
        }
        for (int i = 0; i < 3; i++)
        {
            if (_rememberRights[row, i] < 0)
            {
                _rememberRights[row, i] = index;
                break;
            }
        }
    }
}
