using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public GameObject health_manager;

    private Vector3 moveDirection;
    public float speed = 5f;
    private CharacterController charController;

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
        charController = gameObject.GetComponent<CharacterController>();
        c = Camera.main;

        JsonLoad json = new JsonLoad();
        json.LoadData();
        //подгрузка числа жизней
        health_manager.GetComponent<Health>().Set_health(json.playerInfo.lifes);

        //загрузка положения и поворота персонажа
        transform.position = new Vector3(json.playerInfo.x, json.playerInfo.y, json.playerInfo.z);
        transform.eulerAngles = new Vector3(json.playerInfo.rotateX, json.playerInfo.rotateY, json.playerInfo.rotateZ);
        c.transform.position = transform.position;
        c.transform.eulerAngles = transform.eulerAngles;

        //загрузка иконок инвентаря
        if (json.playerInfo.book == true)
        {
            Item item1 = new Item();
            item1.id = 0;
            inventory.AddItems(item1);
        }
        if (json.playerInfo.boots == true)
        {
            Item item1 = new Item();
            item1.id = 1;
            inventory.AddItems(item1);
        }
        if (json.playerInfo.present == true)
        {
            Item item1 = new Item();
            item1.id = 2;
            inventory.AddItems(item1);
        }
        if (json.playerInfo.food == true)
        {
            Item item1 = new Item();
            item1.id = 3;
            inventory.AddItems(item1);
        }

        UpdateUI();
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
        Vector3 moveDirection = Vector3.zero;
        if (Input.GetKeyDown(KeyCode.W))
        {
            moveDirection += transform.forward;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            moveDirection -= transform.forward;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            moveDirection += transform.right;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            moveDirection -= transform.right;
        }

        moveDirection.y = 0;
        charController.Move(moveDirection * speed);
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

    public void Check()
    {
        if (inventory.GetSize() == 4)
        {
            SceneManager.LoadScene("Win");
        }
    }

    void Update()
    {
        //перемещение
        SimpleMovement();
        Rotate();
    }
}
