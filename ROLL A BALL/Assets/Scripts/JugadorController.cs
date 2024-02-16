using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class JugadorController : MonoBehaviour
{
    new AudioSource audio; 
    public float velocidad;
    private int contador;
    public float time;
    public Text textoContador, textoGanar, textoTimer;
    public float jumpForce = 0.0f;
    public int t_monedas = 12;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        contador = 0;

        // Inicializar el tiempo a 60 segundos en cualquier nivel
        time = 60f;
    }

    private void FixedUpdate()
    {
        float movimientoH = Input.GetAxis("Horizontal");
        float movimientoV = Input.GetAxis("Vertical");
        Vector3 movimiento = new Vector3(movimientoH, 0.0f, movimientoV);
        rb.AddForce(movimiento);

        // Actualizar el tiempo restante
        if (time > 0)
        {
            time -= Time.deltaTime;
            int seconds = Mathf.CeilToInt(time);
            textoTimer.text = "Tiempo restante: " + seconds.ToString() + "s";
        }
        else
        {
            textoGanar.text = "PERDISTE";
            CambiarEscena("MenuInicio");
        }
    }

    void Update()
    {
        float movimientoH = Input.GetAxis("Horizontal");
        float movimientoV = Input.GetAxis("Vertical");
        Vector3 movimiento = new Vector3(movimientoH, 0.0f, movimientoV);
        rb.AddForce(movimiento * velocidad);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Coleccionable"))
        {
            other.gameObject.SetActive(false);
            contador++;
            audio.Play();
            setTextoContador();
        }
    }

    void setTextoContador()
    {
        textoContador.text = " Puntaje: " + contador.ToString();
        if (contador >= t_monedas)
        {
            textoGanar.text = "GANASTE";
            string scene;
            if (SceneManager.GetActiveScene().name == "Nivel1") 
                scene = "Nivel2";
            else if (SceneManager.GetActiveScene().name == "Nivel2") 
                scene = "Nivel3";
            else if (SceneManager.GetActiveScene().name == "Nivel3") 
                scene = "Nivel4";
            else
                scene = "MenuInicio";
            CambiarEscena(scene);
        }
    }

    void CambiarEscena(string escena)
    {
        StartCoroutine(CambiarEscena(escena, 5f));
    }

    IEnumerator CambiarEscena(string escena, float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(escena);
    }
}
