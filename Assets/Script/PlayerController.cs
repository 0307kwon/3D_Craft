using UnityEngine.Networking;
using UnityEngine;
using TMPro;

public class PlayerController : NetworkBehaviour {
    private Animator animator;
    private new GameObject camera;
    public GameObject visor;
    public GameObject HEAD;
    public GameObject view;
    public GameObject view3; // 3인칭에 쓸 카메라 위치 오브젝트
    public GameObject targetSphere;
    public float spd = 5f;

    public GameObject line;

    public GameObject grossCubePrefab;
    public GameObject greyCubePrefab;
    public GameObject bushCubePrefab;
    public GameObject sandCubePrefab;
    public GameObject brickCubePrefab;


    public GameObject networkManager;



    //점프
    private bool canJump = false;

    private bool manView = false; // false = 1인칭 , true = 3인칭


    private float mouseSensivility;
    private Vector2 mouseInput = Vector2.zero; // 마우스로입력된 각도를 저장해서 움직임의 한계를 만들수있음
    private float headMaxX = 60f; // 양방향으로 headMayX(단위: 도) 만큼 회전가능 한계를 둚
    private float headMaxY = 60f; // 양방향으로 headMayY(단위: 도) 만큼 회전가능 한계를 둚


    private Transform inputText;
    
    public override void OnStartLocalPlayer()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //body.GetComponent<MeshRenderer>().material.color = Color.green;
        var obj = Instantiate(line,HEAD.transform);
        obj.transform.position = HEAD.transform.position;
        var obj2 = Instantiate(targetSphere, HEAD.transform);
        obj2.transform.localScale = new Vector3(0.1f, 0.06f, 0.1f);
        obj2.transform.localPosition = new Vector3(2.138f, 0.063f, 0);
        inputText = GameObject.Find("PlayerUI").transform.Find("InputField");
    }   

    // Use this for initialization
    void Start () {
        animator = transform.Find("Man").GetComponent<Animator>();
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        mouseSensivility = 3.0f;
        transform.GetComponent<NetworkAnimator>().animator = animator;
    }



    // Update is called once per frame
    void Update() {
        if (!isLocalPlayer)
        {
            return;
        }

        if (inputText.GetComponent<InputText>().GetWriting())
        {
            animator.SetBool("Moving", false);
        }
        else
        {
            //애니메이션
            if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
            {
                animator.SetBool("Moving", true);
            }
            else
            {
                animator.SetBool("Moving", false);
            }
            //
            transform.Translate(new Vector3(Input.GetAxis("Vertical") * Time.deltaTime * spd, 0, -Input.GetAxis("Horizontal") * Time.deltaTime * spd));

            if (Input.GetKeyDown("space"))
            {
                if (canJump)
                {
                    transform.GetComponent<Rigidbody>().AddForce(new Vector3(0, 500, 0));
                    canJump = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.F2))
            {
                networkManager.GetComponent<Manager>().StopHost();
            }
        }


        Chat();
       
        CameraMoving();

        

    }

    void LateUpdate()
    {
        if (!isLocalPlayer)
        {
            return;
        }

        HeadMoving(); // 머리 움직이는건 LateUpdate로 해야됨 애니메이션이 끝나고 머리 상태를 설정해야되기때문인듯
    }


    //채팅 함수
    private void Chat() // update에서 동작
    {
        if (inputText.GetComponent<InputText>().GetWriting())
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                CmdSendMassage(networkManager.GetComponent<Manager>().myNick, inputText.GetComponent<TMP_InputField>().text);
                inputText.GetComponent<TMP_InputField>().text = null;
                inputText.GetComponent<InputText>().WritingBool(false);
            }
        }
        else
        {  
            if (Input.GetKeyDown(KeyCode.Return))
            {
                inputText.GetComponent<TMP_InputField>().Select();
                inputText.GetComponent<TMP_InputField>().ActivateInputField();
                inputText.GetComponent<InputText>().WritingBool(true);
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            inputText.GetComponent<InputText>().WritingBool(false);
        }
        
    }
    //
    

    public void CanJump(bool jump)
    {
        canJump = jump;
    }

    private void CameraMoving()
    {
        if (Input.GetKeyDown("left ctrl"))
        {
            if (manView)
            {
                manView = false;
            }
            else
            {
                manView = true;
            }
        }

        if (manView)
        {
            //3인칭
            camera.transform.position = view3.transform.position;
            camera.transform.LookAt(view.transform);

        }
        else
        {
            //1인칭
            camera.transform.position = visor.transform.position;
            camera.transform.LookAt(view.transform);
        }
    }

    [Command]
    public void CmdMakeCube(int type, Vector3 position)
    {
        if (type == 0)
        {
            var Cube = Instantiate(grossCubePrefab, position, Quaternion.Euler(Vector3.zero));
            NetworkServer.Spawn(Cube);
        }
        else if (type == 1)
        {
            var Cube = Instantiate(greyCubePrefab, position, Quaternion.Euler(Vector3.zero));
            NetworkServer.Spawn(Cube);
        }
        else if (type == 2)
        {
            var Cube = Instantiate(bushCubePrefab, position, Quaternion.Euler(Vector3.zero));
            NetworkServer.Spawn(Cube);
        }
        else if (type == 3)
        {
            var Cube = Instantiate(sandCubePrefab, position, Quaternion.Euler(Vector3.zero));
            NetworkServer.Spawn(Cube);

        }
        else if (type == 4)
        {
            var Cube = Instantiate(brickCubePrefab, position, Quaternion.Euler(Vector3.zero));
            NetworkServer.Spawn(Cube);
        }

        }

    [Command]
    public void CmdDestroy(GameObject obj)
    {
        NetworkServer.Destroy(obj);
    }

    //채팅
    public class MyMsgType
    {
        public static short Chat = MsgType.Highest + 1; // 메세지 타입이 47개가 있는데 유저 커스텀 타입은 Highest보다 높아야함
    }
    private class ChatMessage : MessageBase
    {
        public string nickname;
        public string message;
    }
    [Command]
    public void CmdSendMassage(string nickname, string message) 
    {
        ChatMessage msg = new ChatMessage();
        msg.nickname = nickname;
        msg.message = message;
        NetworkServer.SendToAll(MyMsgType.Chat, msg);
    }
    //채팅끝


    private void HeadMoving()
    {
        //마우스이동으로 캐릭터 머리 움직이기
        //alt누른채로 마우스 좌우 = 머리 움직임 , alt안누른채로 마우스 좌우 = 캐릭터 회전
        var horizontal = Input.GetAxis("Mouse X") * mouseSensivility;//*마우스감도;
        var vertical = Input.GetAxis("Mouse Y") * mouseSensivility;

        if (horizontal < 0)
        {
            if (mouseInput.x + horizontal < -headMaxX)
            {
                mouseInput.x = -headMaxX;
            }
            else
            {
                mouseInput.x += horizontal;
            }
        }
        else if (horizontal > 0)
        {
            if (mouseInput.x + horizontal > headMaxX)
            {
                mouseInput.x = headMaxX;
            }
            else
            {
                mouseInput.x += horizontal;
            }
        }


        if (vertical < 0)
        {
            if (mouseInput.y + vertical < -headMaxY)
            {
                mouseInput.y = -headMaxY;
            }
            else
            {
                mouseInput.y += vertical;
            }
        }
        else if (vertical > 0)
        {
            if (mouseInput.y + vertical > headMaxY)
            {
                mouseInput.y = headMaxY;
            }
            else
            {
                mouseInput.y += vertical;
            }
        }
        if (Input.GetKey("left alt"))
        {
            HEAD.transform.localRotation = Quaternion.Euler(0, mouseInput.x, mouseInput.y);
        }
        else
        {
            HEAD.transform.localRotation = Quaternion.Euler(0, 0, mouseInput.y);
            animator.GetBoneTransform(HumanBodyBones.Neck).localRotation = Quaternion.Euler(-180, -1.525879e-05f, -mouseInput.y);
            transform.Rotate(new Vector3(0, horizontal * Time.deltaTime * 120.0f, 0));
        }
    }
}
