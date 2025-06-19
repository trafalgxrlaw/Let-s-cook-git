using UnityEngine;

public class InteractionPorte : MonoBehaviour
{
    public float distanceMax = 3f;
    public Material matSurbrillance;         /*matériau de surbrillance quand je regarde une porte ou le four*/
    private Material matInitial;             /*matériau d’origine de l’objet visé*/
    private Renderer rendActuel;             /*renderer de l’objet actuellement visé*/
    private GameObject porteActuelle;        /*référence à la porte/four actuellement visé*/

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distanceMax))
        {
            GameObject cible = hit.collider.gameObject;

            if (cible != porteActuelle) /*si je vise un nouvel objet*/
            {
                if (rendActuel != null && matInitial != null) /*je retire l’effet de surbrillance de l’ancienne cible*/
                    rendActuel.material = matInitial;

                if (cible.CompareTag("Porte") || cible.CompareTag("Four"))  /*si la nouvelle cible est une porte ou le four*/
                {
                    Renderer r = cible.GetComponent<Renderer>();            /*je cherche le composant "renderer" de l'objet cible*/
                    if (r != null)                                          /*s'il y a bien un renderer*/
                    {
                        matInitial = r.material;        /*je stocke le mat d’origine*/
                        r.material = matSurbrillance;   /*je mets le mat de surbrillance*/
                        rendActuel = r;                 /*trace du renderer actif pour pouvoir le reset plus tard*/
                        porteActuelle = cible;
                    }
                }
                else    /*si ce n’est ni une porte ni le four je reset tout*/
                {
                    porteActuelle = null;   
                    rendActuel = null;
                    matInitial = null;
                }

            }

            if (Input.GetKeyDown(KeyCode.E))    /*si "e" est appuyé*/
            {
                if (cible.CompareTag("Porte"))
                {
                    var script = cible.GetComponentInParent<OuvrePorteGauche>();          /*je cherche le script OuvrePorte qui est sur le parent (du pivot de rotation)*/
                    if (script != null) script.Toggle();                    

                    var scriptD = cible.GetComponentInParent<OuvrePorteDroite>();   /*si c'est la porte de droite le parent aura le script OuvrePorteDroite*/
                    if (scriptD != null) scriptD.Toggle();
                }

                if (cible.CompareTag("Four"))                                       /*si c'est le tag four*/
                {
                    var scriptFour = cible.GetComponent<OuvreFour>();               /*on va chercher le script OuvreFour*/
                    if (scriptFour != null) scriptFour.Toggle();
                }
            }
        }
        else
        {
            if (porteActuelle != null && rendActuel != null && matInitial != null)  /*si je ne vise rien je retire la surbrillance*/
                rendActuel.material = matInitial;

            porteActuelle = null;
            rendActuel = null;
            matInitial = null;
        }
    }
}
