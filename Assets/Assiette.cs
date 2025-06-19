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
            if (objetTenu == null)              /*si aucune assiette n�est tenue*/
            {
                RamasserAssiette();             /*je tente d�en ramasser une*/
            }
            else                                /*cas ou j'en tiens d�j� une*/
            {
                Ray ray = new Ray(transform.position, transform.forward);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, distanceMax))
                {
                    GameObject cible = hit.collider.gameObject;

                    Debug.Log("objetTenu.tag = " + objetTenu.tag + " / cible.tag = " + cible.tag);
                    if (objetTenu.CompareTag("Plate") && cible.CompareTag("FriedEgg"))                  /*si je tiens une assiette et je vise un �uf au plat*/
                    {
                        Debug.Log("ajout de l'�uf au plat dans l'assiette");

                        GameObject eggDansAssiette = Instantiate(friedEggPrefab, objetTenu.transform);  /*j'instancie un clone de l'�uf dans l�assiette*/
                        eggDansAssiette.transform.localPosition = new Vector3(0f, 0f, 0f);              /*positionn� au centre*/
                        eggDansAssiette.transform.localRotation = Quaternion.Euler(0f,0f, 0f);

                        SetGlobalScale(eggDansAssiette, friedEggPrefab.transform.lossyScale); /*source SetGlobalScale:chatGPT, voir en fin de code*/

                        Collider colEgg = eggDansAssiette.GetComponent<Collider>();
                        if (colEgg)
                        {
                            colEgg.enabled = false;                                                     /*je d�sactive les collisions de l'�uf*/
                            Physics.IgnoreCollision(colEgg, objetTenu.GetComponent<Collider>());        /*ignore les collisions avec l�assiette*/
                        }
                        if (Camera.main && Camera.main.transform.root.TryGetComponent(out Collider playerCollider))
                        {
                            Physics.IgnoreCollision(colEgg, playerCollider);                            /*et avec le joueur*/
                        }


                        Rigidbody rbEgg = eggDansAssiette.GetComponent<Rigidbody>();                    
                        if (rbEgg) rbEgg.isKinematic = true;                                            /*pas de cin�tique (gravit�, etc..)*/

                        Destroy(cible);                                                                 /*d�truis l'oeuf sur la poele de la sc�ne*/
                        return;
                    }
                }
                PoserObjet();                                                                           /*sinon je pose l�objet (l'assiette) tenu*/
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

            if (cible.CompareTag("Plate"))                                                              /*si c�est une assiette*/
            {
                Debug.Log("assiette d�tect�e, instanciation..");

                objetTenu = Instantiate(platePrefab, main2.position + new Vector3(0.5f, 0.3f, 0f), Quaternion.Euler(-90f, 0f, 0f), main2);  /*instanciation de l'assiette*/
                objetTenu.transform.localScale = platePrefab.transform.localScale;
                objetTenu.tag = "Plate";                                                                                                    /*lui r�attribue le bon tag � la copie*/

                Collider col = objetTenu.GetComponent<Collider>();
                if (col) col.enabled = false;       /*je d�sactive les collisions tant qu�elle est tenue*/

                Rigidbody rb = objetTenu.GetComponent<Rigidbody>();
                if (rb) rb.isKinematic = true;       /*aucune cin�tique dans la main*/
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
            Debug.Log("d�pos� sur meuble : " + hit.collider.name);
        }
        else
        {
            Debug.Log("pas de meuble d�tect�, pose annul�e");
            return;
        }
        objetTenu.transform.SetParent(null);                                                    /*je d�tache de la main*/
        objetTenu.transform.SetPositionAndRotation(positionDepot, rotationDepot);               /*positionne et oriente*/
        /*la physique reprend*/
        Rigidbody rb = objetTenu.GetComponent<Rigidbody>();
        if (rb) rb.isKinematic = false;                                                         

        Collider col = objetTenu.GetComponent<Collider>();
        if (col) col.enabled = true;
        /*je vide la main*/
        objetTenu = null;
    }

    //Conversion de la taille globale en une �chelle locale relative au parent
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
