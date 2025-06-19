using UnityEngine;

public class OuvreFour : MonoBehaviour
{
    public float angleOuverture = 90f;
    public float vitesse = 2f;
    private bool ouverte = false;

    private Quaternion rotationFerm�e;
    private Quaternion rotationOuverte;

    void Start()
    {
        rotationFerm�e = transform.localRotation;

        rotationOuverte = rotationFerm�e * Quaternion.Euler(-angleOuverture, 0f, 0f); /*ouverture sur l'axe x*/
    }

    public void Toggle()
    {
        ouverte = !ouverte;
    }

    void Update()
    {
        Quaternion cible = ouverte ? rotationOuverte : rotationFerm�e;
        transform.localRotation = Quaternion.Lerp(transform.localRotation, cible, Time.deltaTime * vitesse);
    }
}
