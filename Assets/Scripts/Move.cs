using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using TMPro;

public class Move : MonoBehaviour, IPunObservable
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject camera;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private TextMeshPro title;
    [SerializeField] private Sprite spritePing0;
    [SerializeField] private RuntimeAnimatorController animPing0;
    [SerializeField] private Sprite spritePing;
    [SerializeField] private RuntimeAnimatorController animPing;

    private int mode;
    private int jumpCount;
    private float dir;
    private bool isGrounded;
    private bool isWallDetected;
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
        isGrounded = false;
        isWallDetected = true;
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
        if (!photonView.IsMine)
        {
            camera.SetActive(false);
        }
    }

    void Update()
    {
        if (!photonView.IsMine) return;
        photonView.RPC("SendData", RpcTarget.All, spriteRenderer.flipX, title.text, rgb, mode);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (transform.position.y < -15)
        {
            Respawn();
            Finish.timer = 0;
        }
        if (Chat.isChating) return;
        Movement();
    }
    private void Respawn()
    {
        transform.position = Vector3.zero;
        rb.velocity = Vector3.zero;
        jumpCount = 0;
        animator.SetTrigger("isFall");
    }
    private void Movement()
    {
        dir = spriteRenderer.flipX ? 1 : -1;
        //if (Input.GetButtonDown("Jump") && jumpCount<2)
        if (Input.GetMouseButtonDown(0) && jumpCount<2)
        {
            Jump();
        }

        if (isWallDetected) return;
        horizontalMove = speed * dir;

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
    void Jump()
    {
        isWallDetected = false;
        jumpCount++;
        if (jumpCount == 1)
        {
            animator.SetTrigger("isFall");
        }
        if (jumpCount == 2)
        {
            animator.SetTrigger("isJump");
            rb.velocity = Vector3.zero;
            spriteRenderer.flipX = !spriteRenderer.flipX;
            dir = spriteRenderer.flipX ? 1 : -1;
            isGrounded = false;
        }
        rb.gravityScale = 1;
        rb.AddForce(new Vector3(dir, 1f) * jumpSpeed, ForceMode2D.Impulse);
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
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            animator.SetTrigger("isMove");
            rb.velocity = Vector3.zero;
            jumpCount = 0;
            isGrounded = true;
            isWallDetected = false;
            rb.gravityScale = 1;
        }
        if (collision.gameObject.tag == "Finish")
        {
            Respawn();
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 3)
        {

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            animator.SetTrigger("isWall");
            rb.velocity = Vector3.zero;
            jumpCount = 0;
            isWallDetected = true;
            rb.gravityScale = 0.01f;
            spriteRenderer.flipX = !spriteRenderer.flipX;
            if (collision.gameObject.tag == "AutoJump")
            {
                dir = spriteRenderer.flipX ? 1 : -1;
                collision.gameObject.GetComponent<Animator>().SetTrigger("isWork");
                Jump();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            rb.gravityScale = 1f;
            isWallDetected = false;
        }
    }

}
