using UnityEngine;
using UnityEngine.Networking;

public class LineController : NetworkBehaviour
{
    private Collider target;
    public GameObject alphaCube;
    private GameObject CubeController;
    private bool makingBool = false;
    private int cubeType;

    private float blockHP = 60f;
    private float blockHPMax = 60f;




    //거리저장
    private float distance = 0;

    // Use this for initialization
    void Start()
    {
        CubeController = Instantiate(alphaCube, new Vector3(1, -10000001, 1), Quaternion.Euler(Vector3.zero));
        
    }


    // Update is called once per frame
    private void Update()
    {
        //블록만드는 공 컨트롤
        var spd = Time.deltaTime * 40;
        transform.Translate(new Vector3(spd, 0, 0));
        distance += spd;
        if(transform.localPosition.x >= 10)
        {
            transform.position = transform.parent.position;
            blockHP = blockHPMax;
            if (target != null)
            {
                target.transform.Find("Cube").localScale = new Vector3(100f, 100f, 100f);
                target = null;
            }
        }
        //
        //블럭 만들기
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            cubeType = 0; // 초원블럭
            if (makingBool)
            {
                makingBool = false;
                CubeController.transform.position = new Vector3(1, -10000000, 1);
            }
            else
            {
                makingBool = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            cubeType = 1; // 돌벽블럭
            if (makingBool)
            {
                makingBool = false;
                CubeController.transform.position = new Vector3(1, -10000000, 1);
            }
            else
            {
                makingBool = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            cubeType = 2; // 부쉬블럭
            if (makingBool)
            {
                makingBool = false;
                CubeController.transform.position = new Vector3(1, -10000000, 1);
            }
            else
            {
                makingBool = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            cubeType = 3; // 모래블럭
            if (makingBool)
            {
                makingBool = false;
                CubeController.transform.position = new Vector3(1, -10000000, 1);
            }
            else
            {
                makingBool = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            cubeType = 4; // 벽돌블럭
            if (makingBool)
            {
                makingBool = false;
                CubeController.transform.position = new Vector3(1, -10000000, 1);
            }
            else
            {
                makingBool = true;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (makingBool)
            {
                if (alphaCube.GetComponent<AlphaCube>().apply){
                    if (CubeController.transform.position.y >= -100000)
                    {
                        makingBool = false;
                        transform.parent.parent.GetComponent<PlayerController>().CmdMakeCube(cubeType,CubeController.transform.position);
                        CubeController.transform.position = new Vector3(1, -10000000, 1);
                    }
                }
            }
        }
        BlockDestroy();
    }

    private void BlockDestroy()
    {
        //블럭제거
        if (Input.GetMouseButton(0))
        {
            if (makingBool == false)
            {
                if (target != null)
                {
                    blockHP--;
                    if (blockHP <= blockHPMax*0.7f)
                    {
                        target.transform.Find("Cube").localScale -= new Vector3(100f / (blockHPMax * 0.7f), 100f / (blockHPMax * 0.7f), 100f / (blockHPMax * 0.7f));
                        transform.parent.parent.GetComponent<Animator>().SetBool("Destroy", true);
                    }
                    if (blockHP < 0)
                    {
                        blockHP = blockHPMax;
                        transform.parent.parent.GetComponent<PlayerController>().CmdDestroy(target.gameObject);
                        target = null;
                    }
                }
            }
        }
        else
        {
            blockHP = blockHPMax;
            //transform.parent.parent.GetComponent<Animator>().SetBool("Destroy", false); 이부분 고쳐야합니다.
            if (target != null)
            {
                target.transform.Find("Cube").localScale = new Vector3(100f, 100f, 100f);
            }
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.name.Split('_')[0] == "Cube")
        {
            if (target != other)
            { 
                if (target != null)
                {
                    target.transform.Find("Cube").localScale = new Vector3(100f, 100f, 100f);
                }
                blockHPMax = other.transform.GetComponent<Cube>().blockHPMax;
                blockHP = blockHPMax;
                target = other;
            }
            if (makingBool)
            {
                var position = target.transform.position;
                var distance = transform.position - other.transform.position;
                var max = Mathf.Max(Mathf.Abs(distance.x), Mathf.Abs(distance.y), Mathf.Abs(distance.z));
                if (max == Mathf.Abs(distance.x))
                {
                    if (distance.x > 0)
                    {
                       
                        position.x += 2;
                    }
                    else
                    {
                        position.x -= 2;
                    }
                }
                else if (max == Mathf.Abs(distance.y))
                {
                    if (distance.y > 0)
                    {
                        position.y += 2;
                    }
                    else
                    {
                        position.y -= 2;
                    }
                }
                else if (max == Mathf.Abs(distance.z))
                {
                    if (distance.z > 0)
                    {
                        position.z += 2;
                    }
                    else
                    {
                        position.z -= 2;
                    }
                }
                CubeController.GetComponent<AlphaCube>().SetPosition(position);
            }
            transform.position = transform.parent.position;
        }
    }
    
}
