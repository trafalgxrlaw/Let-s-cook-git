using UnityEngine;

public class OuvrePorteDroite : MonoBehaviour
{
    public float angleOuverture = -90f; /*sens antihoraire*/
    public float vitesse = 3f;
    private bool ouverte = false;

    private Quaternion rotationInitiale;
    private Quaternion rotationCible;

    void Start()
    {
        rotationInitiale = transform.rotation;
        rotationCible = Quaternion.Euler(transform.eulerAngles + new Vector3(0, angleOuverture, 0));
    }

    public void Toggle()
    {
        ouverte = !ouverte;
    }

    void Update()
    {
        Quaternion cible = ouverte ? rotationCible : rotationInitiale;
        transform.rotation = Quaternion.Lerp(transform.rotation, cible, Time.deltaTime * vitesse);
    }
}
