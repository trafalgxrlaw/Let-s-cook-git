using UnityEngine;

public class CuissonOeufAuPlat : MonoBehaviour
{
    public Transform dropPoint;                 /*d�claration de r�f�rence d'objets*/
    public GameObject fondJaunePrefab;
    public GameObject oeufPasCuitPrefab;
    public GameObject oeufCuitPrefab;
    public GameObject oeufBrouillesPrefab; 
    public float tempsBeurre = 5f;              /*de timer (en s)*/
    public float tempsCuisson = 10f;
    private float timerBeurre = 0f;
    private float timerCuisson = 0f;
    private bool fondCree = false;              /*d'�tats*/
    private bool cuissonEnCours = false;
    private bool brouille = false;
    private GameObject fondActif;               /*et d'objets actif*/
    private GameObject oeufPasCuitActif;

    void Update()
    {
        Collider[] objets = Physics.OverlapSphere(dropPoint.position, 0.1f);        /*cherche tous les objets autour du dropPoint dans un rayon de 0.1f (m) et stocke dans ma liste objets*/
        bool beurrePresent = false;                                                 /*je d�clare ses variable en false et null pour qu'elle soit true si d�tect� dans le foreach*/
        bool oeufHandPresent = false;
        GameObject oeufPose = null;

        foreach (var obj in objets)                                                 /*pour chaque objet de ma liste*/
        {
            if (obj.CompareTag("Butter")) beurrePresent = true;                      /*si beurre est d�tect�, alors je le flag*/
            if (obj.CompareTag("EggHand"))                                           /*si un objet a le tag EggHand*/
            {
                oeufHandPresent = true;                                             /*je flag sa pr�sence*/
                oeufPose = obj.gameObject;                                          /*je garde une trace du gameObject pour le d�truire apr�s*/
            }
        }

        if (beurrePresent && !fondCree)                                             /*si du beurre est pr�sent et que le fond n'a pas encore �t� cr�� alors on lance le timer de fonte*/
        {
            timerBeurre += Time.deltaTime;                                                          /*j'incr�mente mon timer � chaque frame*/
            if (timerBeurre >= tempsBeurre)                                                         /*si le timer est �coul�*/
            {
                fondActif = Instantiate(fondJaunePrefab, dropPoint.position, dropPoint.rotation);   /*j'instancie le beurre fondu sur le drop point*/
                fondCree = true;                                                                    /*et le flag*/

                foreach (var obj in objets)
                    if (obj.CompareTag("Butter"))                                                   /*et d�truit le beurre*/
                        Destroy(obj.gameObject);
            }
        }

        if (!beurrePresent && !fondCree)                                                            /*si jamais le joueur enl�ve le beurre avant qu'il fonde*/
            timerBeurre = 0f;                                                                       /*je reset le timer*/

        if (fondCree && oeufHandPresent && !cuissonEnCours && Input.GetKeyDown(KeyCode.C))          /*s'il y a du beurre fondu, et un oeuf � la main, et l'input "c"*/
        {
            if (oeufPose != null)                                                                   
            {
                Destroy(oeufPose);                                                                  /*je supprime l'�uf tenu (EggHand)*/
                Destroy(fondActif);                                                                 /*et le beurre fondu*/

                Vector3 pos = dropPoint.position + new Vector3(0f, 0.12f, 0f);                      /* je modifie la position de l'instanciation sur l'axe y */
                oeufPasCuitActif = Instantiate(oeufPasCuitPrefab, pos, dropPoint.rotation * Quaternion.Euler(90f, 0f, 0f)); /*et instancie l'oeuf au plat pas encore cuit*/

                cuissonEnCours = true;                                                              
                timerCuisson = 0f;
                brouille = false;                                                                   /*je flag que ce n'est pas brouill� encore*/
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

                if (brouille)                                                                   /*si brouille = true, alors j'instancie les oeufs brouill�s*/
                {
                    Instantiate(oeufBrouillesPrefab, pos, dropPoint.rotation);
                }
                else
                {
                    Instantiate(oeufCuitPrefab, pos, dropPoint.rotation * Quaternion.Euler(90f, 0f, 0f));   /*sinon oeuf au plat (tourn� sur l'axe X)*/
                }

                fondCree = false;           /*reset all*/
                cuissonEnCours = false;
                timerBeurre = 0f;
                timerCuisson = 0f;
                brouille = false;
            }
        }
    }

    public void ActiverModeBrouille()                   /*public car elle est appel�e par un autre script (RamasseUstensile) et void car la fonction ne fait qu'executer*/
    {
        if (cuissonEnCours && oeufPasCuitActif != null)
        {
            brouille = true;
            Debug.Log("Mode �uf brouill� activ� !");
        }
    }
}
