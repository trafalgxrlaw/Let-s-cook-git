using UnityEngine;

public class RamasseUstensile : MonoBehaviour
{
    public float distanceMax = 3f;
    public Transform main;
    private GameObject ustensileTenu;
    private Vector3 echelleOriginale;
    public string[] tagsUstensiles = new string[] { "Spatula", "Spoon" };

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (ustensileTenu == null)
            {
                Ray ray = new Ray(transform.position, transform.forward);
                if (Physics.Raycast(ray, out RaycastHit hit, distanceMax))
                {
                    GameObject cible = hit.collider.gameObject;
                    Debug.Log("Ustensile d�tect� : " + cible.name + " / Tag : " + cible.tag);

                    foreach (string tag in tagsUstensiles)
                    {
                        if (cible.CompareTag(tag))
                        {
                            echelleOriginale = cible.transform.localScale;

                            Collider col = cible.GetComponent<Collider>();
                            if (col != null) col.enabled = false;

                            Rigidbody rb = cible.GetComponent<Rigidbody>();
                            if (rb != null) rb.isKinematic = true;

                            cible.transform.SetParent(main);
                            cible.transform.localPosition = new Vector3(0, 0.6f, 0f);  

                            ustensileTenu = cible;
                            break;
                        }
                    }
                }
            }
            else
            {
                ustensileTenu.transform.SetParent(null);
                ustensileTenu.transform.localScale = echelleOriginale;
                ustensileTenu.transform.rotation = Quaternion.identity;

                Rigidbody rb = ustensileTenu.GetComponent<Rigidbody>();
                if (rb != null) rb.isKinematic = false;

                Collider col = ustensileTenu.GetComponent<Collider>();
                if (col != null) col.enabled = true;

                ustensileTenu = null;
            }
        }

        if (Input.GetKeyDown(KeyCode.B) && ustensileTenu != null && ustensileTenu.CompareTag("Spatula"))    /*si j�appuie sur B et que je tiens une spatule*/
        {
            Debug.Log("touche b d�tect�e + spatule en main");
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit[] hits = Physics.RaycastAll(ray, distanceMax);

            foreach (RaycastHit hit in hits)                                                                 /*je parcours chaque objet touch� par le raycast*/
            {
                GameObject cible = hit.collider.gameObject;
                Debug.Log("Objet touch� : " + cible.name + " / Tag : " + cible.tag);            

                if (cible.name.Contains("OeufPasCuit 1") || cible.CompareTag("OeufPasCuit"))                /*si c�est un �uf pas encore cuit*/
                {
                    CuissonOeufAuPlat cuisson = FindObjectOfType<CuissonOeufAuPlat>();                      /*je cherche le script qui g�re la cuisson (CuissonOeufAuPlat.cs)*/
                    if (cuisson != null)
                    {
                        cuisson.ActiverModeBrouille();                                                      /*je set brouille = true avec la fonction qui est dans mon script CuissonOeufAuPlat.cs*/
                    }
                    break;
                }
            }
        }
    }
}
