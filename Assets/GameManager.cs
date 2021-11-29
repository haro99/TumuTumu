using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject[] Fruits;

    [SerializeField]
    private List<GameObject> list = new List<GameObject>();
    public GameObject cam, selectojbect, Drop;
    [SerializeField]
    private LayerMask layerMask;
    public AudioSource Audio;
    public string name;
    public int earnings;
    public string[] names;
    public int[] norma;
    public Text[] texts;
    public Text Score;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < norma.Length;i++)
        {
            norma[i] = Random.Range(3, 10);
        }

        StartCoroutine(FruitSet(15));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Debug.Log("押されてるよ");
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            pos.z = 0;
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 10, layerMask);
            Debug.DrawRay(pos, Vector2.zero, Color.blue, 10);

            if (hit)
            {           
                Debug.Log(hit.collider.gameObject);
                GameObject obj = hit.collider.gameObject;
                if (name == "")
                {
                    list.Add(obj);
                    selectojbect = obj;
                    name = obj.GetComponent<Fruit>().fruitname;
                    obj.transform.localScale = new Vector3(3f, 3f, 3f);
                }
                else if(CheckDistance(obj))
                {
                    string getname = obj.GetComponent<Fruit>().fruitname;
                    if (name == getname && !list.Contains(obj))
                    {
                        list.Add(obj);
                        obj.transform.localScale = new Vector3(3f, 3f, 3f);
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("押されてないよ");

            if (list.Count > 2)
            {
                Debug.Log("3個異常揃ってるよ");
                for (int i =0; i<list.Count;i++)
                {
                    if (i < 2)
                    {
                        Destroy(list[i]);
                    }
                    else 
                    {
                        list[i].GetComponent<Fruit>().Moveing();
                    }
                }
                StartCoroutine(FruitSet(list.Count));
                int number = list.Count - 2;
                for (int i = 0; i < 3; i++)
                {
                    if (name == names[i])
                    {
                        //stocks[i] += number;
                        break;
                    }
                }
                list.Clear();
            }
            else
            {
                Debug.Log("3個未満だよ");
                list.Clear();
            }
            name = "";
        }
    }

    bool CheckDistance(GameObject target)
    {
        float distance = Vector2.Distance(selectojbect.transform.position, target.transform.position);
        Debug.Log(distance);
        if (distance < 4f)
        {
            return true;
        }
        return false;
    }

    IEnumerator FruitSet(int number)
    {
        for (int i = 0; i < number; i++)
        {
            GameObject obj = Instantiate(Fruits[Random.Range(0, Fruits.Length)], Drop.transform.position, Quaternion.identity);
            obj.name = i.ToString();
            yield return null;
        }
    }
}
