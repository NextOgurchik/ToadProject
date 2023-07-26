using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class Move : MonoBehaviour, IPunObservable
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private TextMeshPro title;
    [SerializeField] private Sprite spritePing0;
    [SerializeField] private RuntimeAnimatorController animPing0;
    [SerializeField] private Sprite spritePing;
    [SerializeField] private RuntimeAnimatorController animPing;

    private int mode;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private PhotonView photonView;
    private float horizontalMove;
    private float[] rgb = new float[3];

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
 
    }
   
    void Start()
    {
        mode = PlayerPrefs.GetInt("mode");
        animator = GetComponent<Animator>();
        photonView = GetComponent<PhotonView>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = gameObject.GetComponent<Rigidbody2D>();
        for (int i = 0; i < rgb.Length; i++)
        {
            rgb[i] = Random.Range(0f, 1f);
        }
        title.color = new Color(rgb[0], rgb[1], rgb[2]);
        title.text = PhotonNetwork.NickName;
    }

    void Update()
    {
        if (!photonView.IsMine) return;
        photonView.RPC("SendData", RpcTarget.All, spriteRenderer.flipX, title.text, rgb, mode);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        animator.SetFloat("HorizontalMove", 0);
        if (Chat.isChating) return;
        horizontalMove = Input.GetAxisRaw("Horizontal") * speed;
        animator.SetFloat("HorizontalMove", Mathf.Abs(horizontalMove));
        if (Input.GetButtonDown("Jump"))
        {
            rb.AddForce(Vector3.up * jumpSpeed, ForceMode2D.Impulse);
        }
        if (horizontalMove > 0)
        {
            spriteRenderer.flipX = true;
            transform.position += new Vector3(speed, 0, 0) * Time.deltaTime;
        }
        else if (horizontalMove < 0)
        {
            spriteRenderer.flipX = false;
            transform.position += new Vector3(-speed, 0, 0) * Time.deltaTime;
        }
    }
    [PunRPC]
    private void SendData(bool flipX, string titleText, float[] rgb, int mode)
    {
        if (!spriteRenderer) return;
        spriteRenderer.flipX = flipX;
        title.text = titleText;
        title.color = new Color(rgb[0],rgb[1],rgb[2]);

        if (mode == 0)
        {
            spriteRenderer.sprite = spritePing0;
            animator.runtimeAnimatorController = animPing0;
        }
        else
        {
            spriteRenderer.sprite = spritePing;
            animator.runtimeAnimatorController = animPing;
        }
    }
}
