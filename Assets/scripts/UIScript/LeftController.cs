using UnityEngine;
using System.Collections;
using MyLib;

public class LeftBack : MonoBehaviour
{
    public enum LeftBackState
    {
        Idle,
        Move,
    }

    public LeftController con;
    public GUITexture tex;
    private LeftBackState state = LeftBackState.Idle;

    void Update()
    {
        if (state == LeftBackState.Idle)
        {
            var pos = new Vector2((Screen.width / 3) / 2, Screen.height / 6);
            SetPos(pos);
        }
    }

    void SetPos(Vector2 p)
    {
        var rsz = this.con.GetRealSize();
        tex.pixelInset = new Rect(p.x - rsz.x / 2, p.y - rsz.y / 2, rsz.x, rsz.y);
    }

    private bool first = false;
    private Vector2 lastPos = Vector2.zero;
    private Vector2 initPos = Vector2.zero;


    public void EnterMove()
    {
        state = LeftBackState.Move;
        first = true;
    }

    public void ExitMove()
    {
        state = LeftBackState.Idle;
    }

    public Vector2 GetPos()
    {
        return initPos;
    }

    public void SetFingerPos(Vector2 pos)
    {
        if (first)
        {
            first = false;
            lastPos = pos;
            initPos = pos;
            SetPos(pos);
        } else
        {
            //根据手指位置调整
            var diff = pos - initPos;
            var dir = pos - lastPos;
            lastPos = pos;

            //手指在中心半径范围内 50半径 相反运动
            var mag = diff.magnitude;
            var external = this.con.ExternalRadius * LeftController.GetRate();
            //手指在圆环外面 跟随手指移动
            if (mag > external)
            {
                //initPos += dir;
                var fingerDir = pos-initPos;
                //保守力
                var ex = fingerDir.normalized*external;
                initPos = pos-ex;
                SetPos(initPos);
            }

        }
    }
}


public class LeftFinger : MonoBehaviour
{
    public LeftController con;
    public GUITexture tex;

    public enum LeftFingerState
    {
        Idle,
        Move,
    }

    private LeftFingerState state = LeftFingerState.Idle;
    private Vector2 fingerPos = Vector2.zero;


    void Update()
    {
        if (state == LeftFingerState.Idle)
        {
            var pos = new Vector2((Screen.width / 3) / 2, Screen.height / 6);
            SetPos(pos);
        } else
        {
            SetPos(fingerPos);
        }
    }

    void SetPos(Vector2 p)
    {
        var rsz = this.con.GetFingerSize();
        tex.pixelInset = new Rect(p.x - rsz.x / 2, p.y - rsz.y / 2, rsz.x, rsz.y);
    }


    public void EnterMove()
    {
        state = LeftFingerState.Move;
    }

    public void ExitMove()
    {
        state = LeftFingerState.Idle;
    }

    public void SetFingerPos(Vector2 pos)
    {
        fingerPos = pos;
    }

    public Vector2 GetPos()
    {
        return fingerPos;
    }
}



public class LeftController : MonoBehaviour
{
    public Color activeColor;
    public Color inactiveColor;

    public Texture2D joyStick;
    public Texture2D background2D;


    public float joyWidth = 350;
    public float joyHeight = 350;

    public Vector2 fingerSize = new Vector2(100, 100);

    private float size;
    private GUITexture joyStickTexture;
    private GUITexture backObj;

    private LeftBack rb;
    private LeftFinger rf;

    public Vector2 MoveDir = Vector2.zero;

    public float InnerRadius = 80;
    public float ExternalRadius = 130;
    public float CancelRadius = 50;


    public enum LeftState
    {
        Idle,
        Move,
    }

    private LeftState state = LeftState.Idle;
    public static LeftController Instance;

    void Awake()
    {
        Instance = this;

        if (Screen.width > Screen.height)
        {
            size = Screen.height;
        } else
        {
            size = Screen.width;
        }

        joyStickTexture = gameObject.AddComponent<GUITexture>();
        joyStickTexture.texture = joyStick;
        joyStickTexture.color = inactiveColor;


        var bo = new GameObject("LeftBack");
        bo.transform.localScale = Vector3.zero;
        backObj = bo.gameObject.AddComponent<GUITexture>();
        backObj.texture = background2D;
        backObj.color = inactiveColor;

        rb = gameObject.AddComponent<LeftBack>();
        rb.con = this;
        rb.tex = backObj;

        rf = gameObject.AddComponent<LeftFinger>();
        gameObject.transform.localPosition = Vector3.zero;
        gameObject.transform.localScale = Vector3.zero;
        rf.con = this;
        rf.tex = joyStickTexture;
    }

    public Vector2 GetRealSize()
    {
        var rate = Mathf.Min(Screen.width / 1024.0f, Screen.height / 768.0f);
        var v = new Vector2(joyWidth, joyHeight) * rate;
        return v;
    }

    public Vector2 GetFingerSize()
    {
        var rate = Mathf.Min(Screen.width / 1024.0f, Screen.height / 768.0f);
        var v = fingerSize * rate;
        return v;
    }

    public static float GetRate()
    {
        var rate = Mathf.Min(Screen.width / 1024.0f, Screen.height / 768.0f);
        return rate;
    }

    private bool useMouse = false;

    private int fingerId = -1;

    void HandleIdle()
    {
        MoveDir = Vector3.zero;
        #if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            var mousePos = Input.mousePosition;
            if (mousePos.x < Screen.width / 2)
            {
                state = LeftState.Move;
                rb.EnterMove();
                rf.EnterMove();
                useMouse = true;

                rb.SetFingerPos(mousePos);
                rf.SetFingerPos(mousePos);

                CalculateShootDir();
                MyEventSystem.myEventSystem.PushEvent(MyEvent.EventType.EnterMove);
            }
        } else
        #endif
        {
            foreach (var touch in Input.touches)
            {
                fingerId = touch.fingerId;
                var phase = touch.phase;
                if (phase == TouchPhase.Began)
                {
                    if (touch.position.x < Screen.width / 2)
                    {
                        state = LeftState.Move;

                        var fp = touch.position;
                        rb.EnterMove();
                        rf.EnterMove();
                        useMouse = false;
                        rb.SetFingerPos(fp);
                        rf.SetFingerPos(fp);
                        CalculateShootDir();
                        MyEventSystem.myEventSystem.PushEvent(MyEvent.EventType.EnterMove);
                        break;
                    }
                }
            }
        }
    }

    void HandleMove()
    {
        #if UNITY_EDITOR
        if (useMouse)
        {
            if (!Input.GetMouseButton(0))
            {
                state = LeftState.Idle;
                rb.ExitMove();
                rf.ExitMove();
                useMouse = false;
                MyEventSystem.PushEventStatic(MyEvent.EventType.ExitMove);

            } else
            {
                var mousePos = Input.mousePosition;
                rb.SetFingerPos(mousePos);
                rf.SetFingerPos(mousePos);
                CalculateShootDir();
            }
        } else
        #endif
        {

            var find = false;
            Touch touch = new Touch();
            var getTouch = false;
            foreach (var t in Input.touches)
            {
                if (t.fingerId == fingerId)
                {
                    touch = t;
                    getTouch = true;
                    break;
                }
            }
            if (getTouch)
            {
                var phase = touch.phase;
                find = phase == TouchPhase.Ended || phase == TouchPhase.Canceled; 
            } else
            {
                find = true;
            }

            /*
                if (fingerId < Input.touchCount)
                {
                    var touch = Input.GetTouch(fingerId);
                    var phase = touch.phase; 
                    find = phase == TouchPhase.Ended || phase == TouchPhase.Canceled; 
                } else
                {
                    find = true;
                }
                */

            if (find)
            {
                state = LeftState.Idle;
                rb.ExitMove();
                rf.ExitMove();
                MyEventSystem.PushEventStatic(MyEvent.EventType.ExitMove);
            } else
            {
                //var touch = Input.GetTouch(fingerId);
                var pos = touch.position;
                rb.SetFingerPos(pos);
                rf.SetFingerPos(pos);
                CalculateShootDir();
            }
        }
    }

    void Update()
    {
        if (state == LeftState.Idle)
        {
            HandleIdle();
        } else
        {
            HandleMove();
        }
    }

    private void CalculateShootDir()
    {
        var dir = rf.GetPos() - rb.GetPos();
        var mag = dir.magnitude;

        MoveDir = new Vector2(dir.x / CancelRadius, dir.y / CancelRadius);
        if (mag > CancelRadius)
        {
            MoveDir.Normalize();
        }

        MyEventSystem.myEventSystem.PushEvent(new MyEvent()
        {
            type = MyEvent.EventType.MoveDir,
            vec2 = MoveDir,
        });
    }
}
