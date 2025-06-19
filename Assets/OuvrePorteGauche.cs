using UnityEngine;

public class OuvrePorteGauche : MonoBehaviour
{
    public float angleOuverture = 90f;
    public float vitesse = 3f;
    private bool ouverte = false;
    private Quaternion rotationInitiale;
    private Quaternion rotationCible;

    void Start()
    {
        rotationInitiale = transform.rotation;                                                          /*je garde la rotation actuelle comme référence fermée*/
        rotationCible = Quaternion.Euler(transform.eulerAngles + new Vector3(0, angleOuverture, 0));    /*je définis la rotation cible (j'ajoute l’angle d’ouverture sur l’axe Y)*/
    }

    public void Toggle()
    {
        ouverte = !ouverte;         /*à chaque appel j’inverse l’état (ouvert ou fermé)*/
    }
    void Update()
    {
        Quaternion cible = ouverte ? rotationCible : rotationInitiale;                          /*je choisis la rotation à atteindre en fonction de l’état de la porte (avec l'opérateur ternaire)*/
        transform.rotation = Quaternion.Lerp(transform.rotation, cible, Time.deltaTime * vitesse);  /*rotation fluide vers la cible avec la vitesse 3f*/
    }
}
