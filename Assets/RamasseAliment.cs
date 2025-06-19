using UnityEngine;

public class RamasseAliment : MonoBehaviour
{
    public float distanceMax = 3f;
    public Transform main;
    public GameObject[] alimentsRamassables;       /*liste de tous les aliment qu'on peut ramasser*/
    private GameObject objetTenu;                   
    private GameObject bowlTenu; 
    public GameObject omelettePrefab;
    public Transform dropPointPoele;
    public string[] tagsRamassables = new string[] { "Food", "EggBox", "Butter" };  /*énumération des tags valides pour ramassage*/
    private Vector3 echelleOriginale;                                               /*pour restaurer la taille originale à la pose*/

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(transform.position, transform.forward);               /*raycast devant*/
            if (Physics.Raycast(ray, out RaycastHit hit, distanceMax))
            {
                GameObject cible = hit.collider.gameObject;
                Debug.Log("Objet détecté : " + cible.name + " / Tag : " + cible.tag);

                if (objetTenu == null)                                              /*si rien n'est tenu*/
                {
                    if (cible.CompareTag("Bowl"))                                   /*cas spécial si je détecte bowl*/
                    {
                        Transform dropPoint = cible.transform.Find("DropPointBowl");    /*il faut que le DropPointBowl contienne qqch*/
                        if (dropPoint != null)
                        {
                            Collider[] objets = Physics.OverlapSphere(dropPoint.position, 0.3f);    /*j'overlap*/
                            GameObject eggFondTrouve = null;

                            foreach (var obj in objets)
                            {
                                if (obj.CompareTag("EggFond"))                                      /*si EggFond est détécté*/
                                {
                                    eggFondTrouve = obj.gameObject;                                 /*flag en pointant l'objet de la scène*/
                                    break; /*je quitte la boucle, plus besoin*/
                                }
                            }

                            if (eggFondTrouve == null)                                              /*si pas de EggFond alors on peut pas prendre le bol*/
                            {
                                Debug.Log("le bol ne peut pas être pris car EggFond pas détecté");
                                return;
                            }
                            /*je ramasse le bol*/
                            echelleOriginale = cible.transform.localScale;
                            cible.transform.SetParent(main);
                            cible.transform.localPosition = new Vector3(0f, 0.3f, -1f);
                            cible.transform.localRotation = Quaternion.Euler(30f, 0f, 0f);
                            cible.transform.localScale = echelleOriginale;
                            /*j'essaie de prendre le contenu avec son contenant*/
                            eggFondTrouve.transform.SetParent(cible.transform);                  
                            eggFondTrouve.transform.position = dropPoint.position;              
                            eggFondTrouve.transform.rotation = dropPoint.rotation;              
                            Rigidbody rbFond = eggFondTrouve.GetComponent<Rigidbody>();
                            if (rbFond != null)
                            {
                                rbFond.isKinematic = true;
                                rbFond.Sleep();             /*je bloque les calculs physique de l'objet*/
                            }

                            Collider col = cible.GetComponent<Collider>();
                            if (col != null) col.enabled = false;       /*pas collisions*/

                            Rigidbody rb = cible.GetComponent<Rigidbody>();
                            if (rb != null) rb.isKinematic = true;      

                            Collider[] colliders = cible.GetComponentsInChildren<Collider>();
                            foreach (Collider c in colliders)
                            {
                                c.enabled = false;                  /*je désactive aussi toutes les collisions internes (EggFond)*/
                            }

                            bowlTenu = cible;
                            objetTenu = cible;
                            return;
                        }
                    }

                    string prefabCible = null;                      /*ma variable pour savoir quel prefab instancier*/

                    if (cible.CompareTag("EggBox") || cible.name.Contains("chicken_eggs_in_the_tray"))  /*si c’est la boîte d’œufs*/
                    {
                        prefabCible = "EggHand";  /*j'instancie un oeuf (qui est dans mon prefab)*/
                    }
                    else
                    {
                        foreach (string tag in tagsRamassables)     /*sinon je regarde si le tag est autorisé*/
                        {
                            if (cible.CompareTag(tag))
                            {
                                prefabCible = cible.name.Replace("(Clone)", "");    /*je nettoie le nom du clone pour le comparer au prefab*/
                                break;
                            }
                        }
                    }

                    if (prefabCible != null)                                        /*si j’ai trouvé un nom de prefab valide*/
                    {
                        foreach (GameObject prefab in alimentsRamassables)          /*je cherche ce prefab dans ma liste de prefabs*/
                        {
                            if (prefab.name == prefabCible)
                            {
                                GameObject copie = Instantiate(prefab, main.position, main.rotation);   /*j'instancie la copie à la position de main*/
                                echelleOriginale = prefab.transform.localScale;
                                copie.transform.SetParent(main);
                                copie.transform.localRotation = Quaternion.identity;
                                objetTenu = copie;

                                Collider col = copie.GetComponent<Collider>();
                                if (col != null) col.enabled = false;

                                Rigidbody rb = copie.GetComponent<Rigidbody>();
                                if (rb != null) rb.isKinematic = true;

                                return;
                            }
                        }
                    }
                }
                else if (objetTenu != null)                                             /*si je tiens quelque chose*/
                {
                    if (cible.CompareTag("Poele"))                                      /*que je vise la poele*/  
                    {
                        if (objetTenu.CompareTag("Bowl") && bowlTenu != null)           /*que je tiens un bol (rempli par conséquent)*/
                        {
                            Transform dropPoint = cible.transform.Find("DropPoint_Poele");
                            if (dropPoint != null)
                            {
                                Destroy(bowlTenu);                                      /*je détruis le bol*/
                                bowlTenu = null;

                                Vector3 pos = dropPoint.position + new Vector3(0.15f, 0.1f, 0.26f);
                                Instantiate(omelettePrefab, pos, dropPoint.rotation);   /*instancie omelette*/

                                Debug.Log("omelette créée dans la poêle");
                                objetTenu = null; 
                                return;
                            }
                        }
                        else
                        {
                            Transform dropPoint = cible.transform.Find("DropPoint_Poele");  /*sinon je mets l'oeuf EggHand*/
                            if (dropPoint != null)
                            {
                                objetTenu.transform.SetParent(null);
                                objetTenu.transform.position = dropPoint.position +
                                    (objetTenu.CompareTag("EggHand") ? new Vector3(0f, 0.3f, 0f) : Vector3.zero);   /*operateur ternaire pour raccourcir, si c'est EggHand que je pose, il sera un peu au dessus de la poele: sinon vector3.zero*/
                                objetTenu.transform.rotation = dropPoint.rotation;
                                objetTenu.transform.localScale = echelleOriginale;

                                Rigidbody rb = objetTenu.GetComponent<Rigidbody>();
                                if (rb != null) rb.isKinematic = true;

                                Collider col = objetTenu.GetComponent<Collider>();
                                if (col != null) col.enabled = true;

                                objetTenu = null;
                                return;
                            }
                        }
                    }
                    if (cible.CompareTag("Bowl"))               /*même procédé que pour la poelle si j'ai un EggHand, mais pour le bol*/
                    {
                        Transform dropPoint = cible.transform.Find("DropPointBowl");
                        if (dropPoint != null)
                        {
                            objetTenu.transform.SetParent(null);
                            objetTenu.transform.position = dropPoint.position + (objetTenu.CompareTag("EggHand") ? new Vector3(0f, 0.2f, 0f) : Vector3.zero);
                            objetTenu.transform.rotation = dropPoint.rotation;
                            objetTenu.transform.localScale = echelleOriginale;

                            Rigidbody rb = objetTenu.GetComponent<Rigidbody>();
                            if (rb != null) rb.isKinematic = true;

                            Collider col = objetTenu.GetComponent<Collider>();
                            if (col != null) col.enabled = true;

                            objetTenu = null;
                            return;
                        }
                    }
                    objetTenu.transform.SetParent(null);
                    objetTenu.transform.rotation = Quaternion.identity;
                    objetTenu.transform.localScale = echelleOriginale;

                    Rigidbody rbd = objetTenu.GetComponent<Rigidbody>();
                    if (rbd != null) rbd.isKinematic = false;

                    Collider cld = objetTenu.GetComponent<Collider>();
                    if (cld != null) cld.enabled = true;

                    objetTenu = null;
                }
            }
        }
    }
}
