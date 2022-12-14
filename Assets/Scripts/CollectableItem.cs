using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.UIElements.UxmlAttributeDescription;

[RequireComponent(typeof(SphereCollider))]
public class CollectableItem : MonoBehaviour
{

    public RawImage prefabUI;
    private RawImage uiUse;
    Camera cam;
    private Vector3 offset = new Vector3(0, 0.5f, 0);
    private bool treeHit = false;
    private Inventory inventory;
    private Player player;
    private Vector3 item_pos;

    [SerializeField] public Item item;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Player")
        {
            uiUse.enabled = true;
            treeHit = true;
            inventory = other.gameObject.GetComponent<Inventory>();
            player = other.gameObject.GetComponent<Player>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        uiUse.enabled = false;
        treeHit = false;
    }

    void Start()
    {
        uiUse = Instantiate(prefabUI, FindObjectOfType<Canvas>().transform.Find("Inventory_panel").transform).GetComponent<RawImage>();
        cam = Camera.main;

        item_pos = transform.position;
        item_pos.y += GetComponent<MeshRenderer>().bounds.size.y + 0.6f;
        //если он уже есть в инвентаре: сразу уничтожить
        JsonLoad json = new JsonLoad();
        json.LoadData();
        if (item.id == 0 && json.playerInfo.book == true)
        {
            Destroy(gameObject);
        }
        else if (item.id == 1 && json.playerInfo.boots == true)
        {
            Destroy(gameObject);
        }
        else if (item.id == 2 && json.playerInfo.present == true)
        {
            Destroy(gameObject);
        }
        else if (json.playerInfo.food == true)
        {
            Destroy(gameObject);
        }

    }

    void Update()
    {
        uiUse.transform.position = Camera.main.WorldToScreenPoint(item_pos);
        if (treeHit && Input.GetKeyDown(KeyCode.E))
        {
            uiUse.enabled = false;
            inventory.AddItems(item);
            player.UpdateUI();
            player.Check();
            Destroy(gameObject);
        }
    }
}

