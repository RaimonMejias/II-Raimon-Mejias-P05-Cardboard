# II-Raimon-Mejias-P05-Cardboard
Repositorio que contiene los scripts realizados y un README con la descripción del trabajo de la práctica

## DuckBehaviour

El script DuckBehaviour se encarga de indicar que hará el pato a lo largo del programa, comprobando los eventos recibidos durante la ejecución

```C#
    public Mesh inactiveMesh;
    public Mesh gazedAtMesh;
    AudioSource source;
    private bool gazed = false;
    public float impulse = 5.0f;
    private Transform cameraTransform;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
        cameraTransform = GameObject.FindWithTag("MainCamera").transform;
        SetMesh(false);
    }

    void FixedUpdate() {
        if (gazed && Input.GetAxis("Jump") > 0) { Impulse(); }
    }

    void OnCollisionEnter(Collision other) {
        if (other.collider.gameObject.tag == "Player") { return; }
        if (source != null) { source.Play(); }
        ScoreController.scoreValue++;
    }

    public void OnPointerEnter() {
        Debug.Log($"Ha mirado a {name}");
        gazed = true;
        SetMesh(true);
    }

    public void OnPointerExit() {
        Debug.Log($"Ha dejado de mirar a {name}");
        gazed = false;
        SetMesh(false);
    }

    public void OnPointerClick() {
        Impulse();
    }

    private void Impulse() {
        GetComponent<Rigidbody>().AddForce(
            (cameraTransform.forward + cameraTransform.up) * impulse,
            ForceMode.Impulse
        );
    }

    private void SetMesh(bool gazedAt)
    {
        if (inactiveMesh != null && gazedAtMesh != null)
        {
            GetComponent<MeshFilter>().mesh = gazedAt ? gazedAtMesh : inactiveMesh;
        }
    }
```

## ScoreController

Se encarga de controlar la puntuación del juego y de instanciar los siguientes objetos a lo largo de la ejecución

```C#
    TMP_Text scoreText;
    public static int scoreValue;
    public GameObject score25Duck; 
    public GameObject score50Duck;
    public GameObject score100Duck; 
    private int state;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GameObject.FindWithTag("Score").GetComponent<TMP_Text>();
        scoreValue = 0;
        state = 0;
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = $"Score: {scoreValue}"; 
        switch(state) {
            case 0: 
                if (scoreValue == 25) { 
                    Instantiate(score25Duck);
                    state++;
                }
                break;
            
            case 1:
                if (scoreValue == 50) {
                    Instantiate(score50Duck);
                    state++;
                }
                break;
            case 2:
                if (scoreValue == 100) {
                    Instantiate(score100Duck);
                    state++;
                }    
                break;            
            default:
                break;
        }
    }
```

## PlayerMovement

Controla el movimiento la camara

```C#
    public float speed = 1.0f;
    public float mouseSensity = 5.0f;
    private Vector2 rotation = new Vector2();
    private Vector3 movement = new Vector3(); 
    private GameObject playerCamera;
    private Rigidbody parentBody;

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerCamera = transform.parent.transform.GetChild(0).gameObject;
        parentBody = transform.parent.gameObject.GetComponent<Rigidbody>();
    }

    void Update() {
        rotateCamera();
    }

    // Update is called once per frame
    void FixedUpdate() {
        MovePlayer();
        Debug.DrawRay(playerCamera.transform.position, playerCamera.transform.forward * 2 , Color.red);
    }

    void MovePlayer() {
        movement = (
            transform.forward * Input.GetAxis("Vertical") + 
            transform.right * Input.GetAxis("Horizontal")
        );
        movement = movement * speed;
        parentBody.AddForce(movement, ForceMode.Force);
        parentBody.velocity = Vector3.zero;
        parentBody.angularVelocity = Vector3.zero;  // Parar al objeto cuando no se usan las flechas 
        playerCamera.transform.position = transform.position + new Vector3(0, 0.5f, 0);  // Hacer que la posición de la camara no sea independiente del objeto   
    }

    void rotateCamera() {
        rotation.x += Input.GetAxisRaw("Mouse X") * mouseSensity * Time.deltaTime;
        rotation.y -= Input.GetAxisRaw("Mouse Y") * mouseSensity * Time.deltaTime;
        rotation.y = Mathf.Clamp(rotation.y, -90.0f, 90.0f); // Limitar la rotación horizontal a 90º maximo 
        transform.rotation = Quaternion.Euler(rotation.y, rotation.x, 0);
        playerCamera.transform.rotation = Quaternion.Euler(rotation.y, rotation.x, 0); // Rotar tanto camara como objeto
    }
```
