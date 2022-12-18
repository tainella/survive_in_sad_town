using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static UnityEditor.FilePathAttribute;
using static UnityEditor.IMGUI.Controls.PrimitiveBoundsHandle;

public class Player : MonoBehaviour
{
    public GameObject health_manager;

    private Vector3 moveDirection;
    public float speed = 0.3f;
    private Rigidbody rb;

    public Inventory inventory;
    [SerializeField] public List<RawImage> icons = new List<RawImage>(new RawImage[4]);

    //для вращения
    Vector2 rotation = Vector2.zero;
    const string xAxis = "Mouse X";
    const string yAxis = "Mouse Y";
    public float Sensitivity
    {
        get { return sensitivity; }
        set { sensitivity = value; }
    }
    [Range(0.1f, 9f)][SerializeField] float sensitivity = 2f;
    [Tooltip("Limits vertical camera rotation. Prevents the flipping that happens when rotation goes above 90.")]
    [Range(0f, 90f)][SerializeField] float yRotationLimit = 88f;

    Camera c;

    void Start()
    {
        inventory = gameObject.AddComponent<Inventory>();
        rb = gameObject.GetComponent<Rigidbody>();
        c = Camera.main;

        //загрузка иконок инвентаря
    }

    public void UpdateUI()
    {
        for (int i = 0; i < inventory.GetSize(); i++)
        {
            icons[inventory.GetItem(i).id].enabled = true;
        }
    }

    void SimpleMovement()
    {
        moveDirection = Vector3.zero;
        if (Input.GetKeyDown("s"))
        {
            moveDirection += -transform.forward;
        }

        if (Input.GetKeyDown("w"))
        {
            moveDirection += transform.forward;
        }

        if (Input.GetKeyDown("a"))
        {
            moveDirection += -transform.right;
        }

        if (Input.GetKeyDown("d"))
        {
            moveDirection += transform.right;
        }
        //print(moveDirection);
        moveDirection *= speed * Time.fixedDeltaTime;
        //transform.position += moveDirection;
        if (moveDirection != Vector3.zero) {
           rb.MovePosition(transform.position + moveDirection);
        }
    }

    void Rotate()
    {
        rotation.x += Input.GetAxis(xAxis) * sensitivity;
        rotation.y += Input.GetAxis(yAxis) * sensitivity;
        rotation.y = Mathf.Clamp(rotation.y, -yRotationLimit, yRotationLimit);
        var xQuat = Quaternion.AngleAxis(rotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(rotation.y, Vector3.left);

        transform.localRotation = xQuat * yQuat;

        c.transform.rotation = transform.rotation;
    }


    void Update()
    {
        //перемещение
        SimpleMovement();
        Rotate();

        UpdateUI();
    }
}
