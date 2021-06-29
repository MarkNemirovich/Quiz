using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI : MonoBehaviour
{
    [SerializeField] private Text _taskText;
    [SerializeField] private GameObject _objectsBase;
    [SerializeField] private GameObject[] _sells;
    private GameObject[] _objects;
    private ObjectClick[] _objectsData;
    private const float _scale = 2f;
    private Tasks _tasks;
    [SerializeField] private Image _panel;
    [SerializeField] private GameObject _restart;
    [SerializeField] private ParticleSystem _ps;

    void Start()
    {
        _tasks = GetComponent<Tasks>();
        _restart.GetComponent<SpriteRenderer>().DOFade(0, 0);
        _panel.DOFade(0, 0);
        _taskText.DOFade(0, 0f);
        StartCoroutine(CreateLvl());
        for (int i = 0; i < _sells.Length; i++)
        {
            _sells[i].GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    public IEnumerator CreateLvl()
    {
        yield return new WaitForEndOfFrame();
        _objectsBase.transform.position = new Vector3(0, -3 + _tasks._lvl, 0);
        _tasks.ChooseBank();
        StartCoroutine(Bounce());
    }

    public IEnumerator Bounce()
    {
        yield return new WaitUntil(() => _tasks._choosedSprite != null);
        _objects = new GameObject[_tasks._objectsNum];
        _objectsData = new ObjectClick[_tasks._objectsNum];
        for (int i = 0; i < _tasks._objectsNum; i++)
        {
            var s = _sells[i].GetComponent<SpriteRenderer>();
            s.enabled = true;
            _objects[i] = Instantiate(Resources.Load("OneObject"), _sells[i].transform, false) as GameObject;
            _objects[i].transform.localScale = new Vector3(0.4f, 0.4f, 1);
            _objects[i].GetComponent<SpriteRenderer>().sprite = _tasks._choosedList[_tasks._randomedAnswers[i]];
            _objectsData[i] = _objects[i].GetComponent<ObjectClick>();
            _objectsData[i]._myNumber = _tasks._randomedAnswers[i];
        }
        yield return new WaitForEndOfFrame();
        FadeIn();
    }

    public void FadeIn()
    {
        _taskText.text = $"Find {_tasks._choosedChar}";
        _taskText.DOFade(1, 1f);
        StartCoroutine(CheckAnswers());
    }
    public IEnumerator CheckAnswers()
    {
        bool findRight = false;
        foreach (ObjectClick oc in _objectsData)
        {
            if (oc._clicked)
            {
                if (oc._myNumber == _tasks._right)
                {
                    _ps.transform.position = oc.transform.position - new Vector3(0, 0, 3);
                    _ps.Play();
                    findRight = true;
                    oc.Right();
                    StartCoroutine(NewLvl());
                    break;
                }
                else
                {
                    oc.Wrong();
                    yield return new WaitUntil(() => !oc._clicked);
                    break;
                }
            }
        }
        yield return new WaitForEndOfFrame();
        if (!findRight) StartCoroutine(CheckAnswers());
    }
    public IEnumerator NewLvl()
    {
        _taskText.DOFade(0, 0.5f);
        yield return new WaitForSeconds(1);
        var objs = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in objs)
            if (obj.name == "[DOTween]")
            {
                Destroy(obj);
            }
        for (int i = 0; i < _tasks._objectsNum; i++)
        {
            Destroy(_objects[i]);
            _sells[i].GetComponent<SpriteRenderer>().enabled = false;
        }
        yield return new WaitForEndOfFrame();
        _tasks._lvl++;
        if (_tasks._lvl > 3)
        {
            var seq = DOTween.Sequence();
            seq.Append(_restart.GetComponent<SpriteRenderer>().DOFade(1, 0.1f));
            seq.Append(_panel.DOFade(1, 0.5f));
            _tasks.NewGame();
            seq.AppendInterval(0.5f);
            yield return new WaitUntil(() => _restart.GetComponent<Restart>()._clicked);
            seq.Append(_restart.GetComponent<SpriteRenderer>().DOFade(0, 0.1f));
            seq.Append(_panel.DOFade(0, 0.5f));         
        }
        StartCoroutine(CreateLvl());
    }
}