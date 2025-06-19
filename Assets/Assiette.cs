using UnityEngine;

public class RamasseAssiette : MonoBehaviour
{
    public float distanceMax = 3f;
    public Transform main2;                  
    public GameObject platePrefab;           
    public GameObject friedEggPrefab;        
    private GameObject objetTenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (objetTenu == null)              /*si aucune assiette n’est tenue*/
            {
                RamasserAssiette();             /*je tente d’en ramasser une*/
            }
            else                                /*cas ou j'en tiens déjà une*/
            {
                Ray ray = new Ray(transform.position, transform.forward);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, distanceMax))
                {
                    GameObject cible = hit.collider.gameObject;

                    Debug.Log("objetTenu.tag = " + objetTenu.tag + " / cible.tag = " + cible.tag);
                    if (objetTenu.CompareTag("Plate") && cible.CompareTag("FriedEgg"))                  /*si je tiens une assiette et je vise un œuf au plat*/
                    {
                        Debug.Log("ajout de l'œuf au plat dans l'assiette");

                        GameObject eggDansAssiette = Instantiate(friedEggPrefab, objetTenu.transform);  /*j'instancie un clone de l'œuf dans l’assiette*/
                        eggDansAssiette.transform.localPosition = new Vector3(0f, 0f, 0f);              /*positionné au centre*/
                        eggDansAssiette.transform.localRotation = Quaternion.Euler(0f,0f, 0f);

                        SetGlobalScale(eggDansAssiette, friedEggPrefab.transform.lossyScale); /*source SetGlobalScale:chatGPT, voir en fin de code*/

                        Collider colEgg = eggDansAssiette.GetComponent<Collider>();
                        if (colEgg)
                        {
                            colEgg.enabled = false;                                                     /*je désactive les collisions de l'œuf*/
                            Physics.IgnoreCollision(colEgg, objetTenu.GetComponent<Collider>());        /*ignore les collisions avec l’assiette*/
                        }
                        if (Camera.main && Camera.main.transform.root.TryGetComponent(out Collider playerCollider))
                        {
                            Physics.IgnoreCollision(colEgg, playerCollider);                            /*et avec le joueur*/
                        }


                        Rigidbody rbEgg = eggDansAssiette.GetComponent<Rigidbody>();                    
                        if (rbEgg) rbEgg.isKinematic = true;                                            /*pas de cinétique (gravité, etc..)*/

                        Destroy(cible);                                                                 /*détruis l'oeuf sur la poele de la scène*/
                        return;
                    }
                }
                PoserObjet();                                                                           /*sinon je pose l’objet (l'assiette) tenu*/
            }
        }
    }
    void RamasserAssiette()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, distanceMax))
        {
            GameObject cible = hit.collider.gameObject;

            if (cible.CompareTag("Plate"))                                                              /*si c’est une assiette*/
            {
                Debug.Log("assiette détectée, instanciation..");

                objetTenu = Instantiate(platePrefab, main2.position + new Vector3(0.5f, 0.3f, 0f), Quaternion.Euler(-90f, 0f, 0f), main2);  /*instanciation de l'assiette*/
                objetTenu.transform.localScale = platePrefab.transform.localScale;
                objetTenu.tag = "Plate";                                                                                                    /*lui réattribue le bon tag à la copie*/

                Collider col = objetTenu.GetComponent<Collider>();
                if (col) col.enabled = false;       /*je désactive les collisions tant qu’elle est tenue*/

                Rigidbody rb = objetTenu.GetComponent<Rigidbody>();
                if (rb) rb.isKinematic = true;       /*aucune cinétique dans la main*/
            }
        }
    }
    void PoserObjet()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        Vector3 positionDepot;
        Quaternion rotationDepot = Quaternion.identity;

        if (Physics.Raycast(ray, out hit, distanceMax) && hit.collider.CompareTag("Meuble"))
        {
            positionDepot = hit.point + new Vector3(0.2f, 0.5f, 0f);
            rotationDepot = Quaternion.Euler(-90f, 0f, 0f);
            Debug.Log("déposé sur meuble : " + hit.collider.name);
        }
        else
        {
            Debug.Log("pas de meuble détecté, pose annulée");
            return;
        }
        objetTenu.transform.SetParent(null);                                                    /*je détache de la main*/
        objetTenu.transform.SetPositionAndRotation(positionDepot, rotationDepot);               /*positionne et oriente*/
        /*la physique reprend*/
        Rigidbody rb = objetTenu.GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = false;                                                         

        Collider col = objetTenu.GetComponent<Collider>();
        if (col) col.enabled = true;
        /*je vide la main*/
        objetTenu = null;
    }

    //Conversion de la taille globale en une échelle locale relative au parent
    void SetGlobalScale(GameObject obj, Vector3 globalScale)     /*source SetGlobalScale:chatGPT*/
    {
        Transform t = obj.transform;
        t.localScale = new Vector3(
            globalScale.x / t.parent.lossyScale.x,
            globalScale.y / t.parent.lossyScale.y,
            globalScale.z / t.parent.lossyScale.z
        );
    }
}
