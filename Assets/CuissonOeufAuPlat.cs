using UnityEngine;

public class CuissonOeufAuPlat : MonoBehaviour
{
    public Transform dropPoint;                 /*déclaration de référence d'objets*/
    public GameObject fondJaunePrefab;
    public GameObject oeufPasCuitPrefab;
    public GameObject oeufCuitPrefab;
    public GameObject oeufBrouillesPrefab; 
    public float tempsBeurre = 5f;              /*de timer (en s)*/
    public float tempsCuisson = 10f;
    private float timerBeurre = 0f;
    private float timerCuisson = 0f;
    private bool fondCree = false;              /*d'états*/
    private bool cuissonEnCours = false;
    private bool brouille = false;
    private GameObject fondActif;               /*et d'objets actif*/
    private GameObject oeufPasCuitActif;

    void Update()
    {
        Collider[] objets = Physics.OverlapSphere(dropPoint.position, 0.1f);        /*cherche tous les objets autour du dropPoint dans un rayon de 0.1f (m) et stocke dans ma liste objets*/
        bool beurrePresent = false;                                                 /*je déclare ses variable en false et null pour qu'elle soit true si détecté dans le foreach*/
        bool oeufHandPresent = false;
        GameObject oeufPose = null;

        foreach (var obj in objets)                                                 /*pour chaque objet de ma liste*/
        {
            if (obj.CompareTag("Butter")) beurrePresent = true;                      /*si beurre est détecté, alors je le flag*/
            if (obj.CompareTag("EggHand"))                                           /*si un objet a le tag EggHand*/
            {
                oeufHandPresent = true;                                             /*je flag sa présence*/
                oeufPose = obj.gameObject;                                          /*je garde une trace du gameObject pour le détruire après*/
            }
        }

        if (beurrePresent && !fondCree)                                             /*si du beurre est présent et que le fond n'a pas encore été créé alors on lance le timer de fonte*/
        {
            timerBeurre += Time.deltaTime;                                                          /*j'incrémente mon timer à chaque frame*/
            if (timerBeurre >= tempsBeurre)                                                         /*si le timer est écoulé*/
            {
                fondActif = Instantiate(fondJaunePrefab, dropPoint.position, dropPoint.rotation);   /*j'instancie le beurre fondu sur le drop point*/
                fondCree = true;                                                                    /*et le flag*/

                foreach (var obj in objets)
                    if (obj.CompareTag("Butter"))                                                   /*et détruit le beurre*/
                        Destroy(obj.gameObject);
            }
        }

        if (!beurrePresent && !fondCree)                                                            /*si jamais le joueur enlève le beurre avant qu'il fonde*/
            timerBeurre = 0f;                                                                       /*je reset le timer*/

        if (fondCree && oeufHandPresent && !cuissonEnCours && Input.GetKeyDown(KeyCode.C))          /*s'il y a du beurre fondu, et un oeuf à la main, et l'input "c"*/
        {
            if (oeufPose != null)                                                                   
            {
                Destroy(oeufPose);                                                                  /*je supprime l'œuf tenu (EggHand)*/
                Destroy(fondActif);                                                                 /*et le beurre fondu*/

                Vector3 pos = dropPoint.position + new Vector3(0f, 0.12f, 0f);                      /* je modifie la position de l'instanciation sur l'axe y */
                oeufPasCuitActif = Instantiate(oeufPasCuitPrefab, pos, dropPoint.rotation * Quaternion.Euler(90f, 0f, 0f)); /*et instancie l'oeuf au plat pas encore cuit*/

                cuissonEnCours = true;                                                              
                timerCuisson = 0f;
                brouille = false;                                                                   /*je flag que ce n'est pas brouillé encore*/
            }
        }

        if (cuissonEnCours)
        {
            timerCuisson += Time.deltaTime;
            if (timerCuisson >= tempsCuisson)
            {
                if (oeufPasCuitActif != null) Destroy(oeufPasCuitActif);
                if (fondActif != null) Destroy(fondActif);

                Vector3 pos = dropPoint.position + new Vector3(0f, 0.12f, 0f);

                if (brouille)                                                                   /*si brouille = true, alors j'instancie les oeufs brouillés*/
                {
                    Instantiate(oeufBrouillesPrefab, pos, dropPoint.rotation);
                }
                else
                {
                    Instantiate(oeufCuitPrefab, pos, dropPoint.rotation * Quaternion.Euler(90f, 0f, 0f));   /*sinon oeuf au plat (tourné sur l'axe X)*/
                }

                fondCree = false;           /*reset all*/
                cuissonEnCours = false;
                timerBeurre = 0f;
                timerCuisson = 0f;
                brouille = false;
            }
        }
    }

    public void ActiverModeBrouille()                   /*public car elle est appelée par un autre script (RamasseUstensile) et void car la fonction ne fait qu'executer*/
    {
        if (cuissonEnCours && oeufPasCuitActif != null)
        {
            brouille = true;
            Debug.Log("Mode œuf brouillé activé !");
        }
    }
}
